namespace advent.of.code.day15;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        var map = ReadInput(reader, out var startPos, out var instructions, false);

        foreach (var instruction in instructions) {
            var direction = instruction.Vector();

            var nextPos = startPos + direction;
            if (!Move(map, nextPos, direction)) continue;

            map.Set(startPos, '.');
            startPos = nextPos;
        }

        return CalculateGpsCoordinateSum(map, startPos);
    }

    internal static int Task2(StreamReader reader) {
        var map = ReadInput(reader, out var startPos, out var instructions, true);

        foreach (var instruction in instructions) {
            var nextPos = startPos + instruction.Vector();
            if (map.Get(nextPos) == '#') {
                continue;
            }

            if (map.Get(nextPos) == '.') {
                startPos = nextPos;
                continue;
            }

            var encounteredBox = new Box(nextPos, nextPos with { X = nextPos.X + 1 });
            if (map.Get(nextPos) == ']') {
                encounteredBox += Direction.Left.Vector();
            }

            var boxesToBeMoved = new List<Box> { encounteredBox };

            if (!MoveBox(map, boxesToBeMoved, instruction)) continue;

            startPos = nextPos;

            // remove moved boxes
            foreach (var box in boxesToBeMoved) {
                map.Set(box.Left, '.');
                map.Set(box.Right, '.');
            }

            // place moved boxes
            foreach (var box in boxesToBeMoved.Select(b => b + instruction.Vector())) {
                map.Set(box.Left, '[');
                map.Set(box.Right, ']');
            }
        }

        return CalculateGpsCoordinateSum(map, startPos);
    }

    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    private record Vector2(int X, int Y) {
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    }

    private static Vector2 Vector(this Direction direction) {
        return direction switch {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            Direction.Right => new Vector2(1, 0),
            _ => throw new Exception()
        };
    }

    private record Box(Vector2 Left, Vector2 Right) {
        public static Box operator +(Box box, Vector2 offset) => new(box.Left + offset, box.Right + offset);
    }

    private static List<char[]> ReadInput(
        StreamReader reader,
        out Vector2 startPos,
        out List<Direction> instructions,
        bool doubleWidth
    ) {
        var map = new List<char[]>();
        startPos = new Vector2(0, 0);

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            if (string.IsNullOrEmpty(line)) break;

            if (doubleWidth) {
                line = line.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");
            }

            if (line.Contains('@')) {
                startPos = new Vector2(line.IndexOf('@'), map.Count);
                line = line.Replace('@', '.');
            }

            map.Add(line.ToCharArray());
        }

        instructions = [];

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            instructions.AddRange(line.Select(c => c switch {
                '^' => Direction.Up,
                'v' => Direction.Down,
                '<' => Direction.Left,
                '>' => Direction.Right,
                _ => throw new Exception()
            }));
        }

        return map;
    }

    private static bool Move(List<char[]> map, Vector2 nextPos, Vector2 direction) {
        if (map.Get(nextPos) == '#') {
            return false;
        }

        if (map.Get(nextPos) == '.') {
            map.Set(nextPos, 'O');
            return true;
        }

        if (!Move(map, nextPos + direction, direction)) return false;

        map.Set(nextPos, 'O');
        return true;
    }

    private static bool MoveBox(List<char[]> map, List<Box> boxesToBeMoved, Direction instruction) {
        var movedBoxes = new List<Box>(boxesToBeMoved);
        while (movedBoxes.Count > 0) {
            if (!MoveBoxes(map, movedBoxes, instruction, out movedBoxes)) return false;
            boxesToBeMoved.AddRange(movedBoxes);
        }

        return true;
    }

    private static bool MoveBoxes(List<char[]> map, List<Box> boxes, Direction direction, out List<Box> nextBoxes) {
        nextBoxes = [];

        foreach (var nextBox in boxes.Select(box => box + direction.Vector())) {
            if (map.Get(nextBox.Left) == '#' || map.Get(nextBox.Right) == '#') {
                return false;
            }

            switch (direction) {
                case Direction.Left when map.Get(nextBox.Left) == '.':
                case Direction.Right when map.Get(nextBox.Right) == '.':
                case Direction.Up or Direction.Down when map.Get(nextBox.Left) == '.' && map.Get(nextBox.Right) == '.':
                    continue;
            }

            if (map.Get(nextBox.Left) == '[' || map.Get(nextBox.Right) == ']') {
                nextBoxes.Add(nextBox);
                continue;
            }

            if (direction != Direction.Right && map.Get(nextBox.Left) == ']') {
                nextBoxes.Add(new Box(nextBox.Left with { X = nextBox.Left.X - 1 }, nextBox.Left));
            }

            if (direction != Direction.Left && map.Get(nextBox.Right) == '[') {
                nextBoxes.Add(new Box(nextBox.Right, nextBox.Right with { X = nextBox.Right.X + 1 }));
            }
        }

        return true;
    }

    private static char Get(this List<char[]> map, Vector2 pos) => map[pos.Y][pos.X];
    private static void Set(this List<char[]> map, Vector2 pos, char c) => map[pos.Y][pos.X] = c;

    private static int CalculateGpsCoordinateSum(List<char[]> map, Vector2 startPos) {
        var total = 0;
        for (var y = 0; y < map.Count; y++) {
            for (var x = 0; x < map[y].Length; x++) {
                if (y == startPos.Y && x == startPos.X) map.Set(startPos, '@');

                if (map[y][x] == 'O' || map[y][x] == '[') {
                    total += 100 * y + x;
                }
            }
        }

        return total;
    }
}