namespace advent.of.code.day6;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        var map = ReadMap(reader, out var start);
        return GetVisitedPoints(map, start).Count;
    }

    internal static int Task2(StreamReader reader) {
        var map = ReadMap(reader, out var start);

        return GetVisitedPoints(map, start).Where(point => {
            if (point == start) return false;
                
            var newMap = map.Select(array => array.ToArray()).ToList();
            newMap[point.Y][point.X] = '#';

            return HasCircle(newMap, start);
        }).Count();
    }

    private static List<char[]> ReadMap(StreamReader reader, out Point start) {
        var map = new List<char[]>();
        start = new Point(0, 0);
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            if (line.IndexOf('^') > -1) {
                start = new Point(map.Count, line.IndexOf('^'));
            }

            map.Add(line.ToCharArray());
        }

        return map;
    }

    private static HashSet<Point> GetVisitedPoints(List<char[]> map, Point start) {
        var points = new HashSet<Point>();
        
        var direction = Direction.Up;

        while (true) {
            points.Add(start);

            var next = Next(start, direction);
            if (!IsInBounds(map, next)) {
                break;
            }

            if (map[next.Y][next.X] == '#') {
                direction = Next(direction);
                continue;
            }

            start = next;
        }

        return points;
    }

    private static bool HasCircle(List<char[]> map, Point start) {
        var seenDirections = new HashSet<Direction>[map.Count][];
        for (var i = 0; i < map.Count; i++) {
            seenDirections[i] = new HashSet<Direction>[map[i].Length];
            for (var j = 0; j < map[i].Length; j++) {
                seenDirections[i][j] = [];
            }
        }

        var direction = Direction.Up;

        while (true) {
            if (seenDirections[start.Y][start.X].Contains(direction)) {
                return true;
            }

            seenDirections[start.Y][start.X].Add(direction);

            var next = Next(start, direction);

            if (!IsInBounds(map, next)) {
                break;
            }

            if (map[next.Y][next.X] == '#') {
                direction = Next(direction);
                continue;
            }

            start = next;
        }

        return false;
    }

    private enum Direction {
        Up,
        Right,
        Down,
        Left
    }

    private record Point(int Y, int X);

    private static bool IsInBounds(List<char[]> map, Point point) {
        return point is { X: >= 0, Y: >= 0 } && point.Y < map.Count && point.X < map[0].Length;
    }

    private static Point Next(Point point, Direction direction) {
        return direction switch {
            Direction.Up => point with { Y = point.Y - 1 },
            Direction.Right => point with { X = point.X + 1 },
            Direction.Down => point with { Y = point.Y + 1 },
            Direction.Left => point with { X = point.X - 1 },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Direction Next(Direction direction) {
        return direction switch {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}