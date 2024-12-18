namespace advent.of.code.day18;

internal static class Solution {
    internal static int Task1(StreamReader reader, int width, int height, int target) {
        var bytes = ReadFallingBytes(reader);

        var memoryMap = PrepareMap(width, height, bytes, target);
        var start = new Vector2(0, 0);
        var end = new Vector2(width - 1, height - 1);

        return CalculateShortestDistance(memoryMap, start, end);
    }

    internal static string Task2(StreamReader reader, int width, int height) {
        var bytes = ReadFallingBytes(reader);

        var start = new Vector2(0, 0);
        var end = new Vector2(width - 1, height - 1);

        var left = 0;
        var right = bytes.Count / 2;

        do {
            var memoryMap = PrepareMap(width, height, bytes, right);

            var window = right - left;
            try {
                CalculateShortestDistance(memoryMap, start, end);
                // Path found, offset and reduce window
                left = right;
                right += window / 2;
            }
            catch (Exception) {
                // No path found, reduce window
                right -= window / 2;
            }
        } while (right - left > 1);

        return $"{bytes[left].X},{bytes[left].Y}";
    }

    private record Vector2(int X, int Y) {
        public Vector2 Offset(Direction direction) {
            return direction switch {
                Direction.Up => this with { Y = Y - 1 },
                Direction.Down => this with { Y = Y + 1 },
                Direction.Left => this with { X = X - 1 },
                Direction.Right => this with { X = X + 1 },
                _ => this
            };
        }

        public int ManhattenDistance(Vector2 other) {
            // Manhatten distance as heuristic, no tie-breaker needed here
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
    }

    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    private static List<Vector2> ReadFallingBytes(StreamReader reader) {
        var bytes = new List<Vector2>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            var split = line.Split(',').Select(int.Parse).ToArray();
            bytes.Add(new Vector2(split[0], split[1]));
        }

        return bytes;
    }

    private static char[][] PrepareMap(int width, int height, List<Vector2> bytes, int steps) {
        var memory = new char[height][];

        for (var i = 0; i < memory.Length; i++) {
            memory[i] = new char[width];
            for (var j = 0; j < memory[i].Length; j++) {
                memory[i][j] = '.';
            }
        }

        for (var i = 0; i < steps; i++) {
            memory[bytes[i].Y][bytes[i].X] = '#';
        }

        return memory;
    }

    private static int CalculateShortestDistance(char[][] map, Vector2 start, Vector2 end) {
        var distances = new Dictionary<Vector2, int> { { start, 0 } };

        var queue = new PriorityQueue<Vector2, int>();
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var current, out _)) {
            // Smallest distance found
            if (current == end) break;

            foreach (var direction in Enum.GetValues<Direction>()) {
                var next = current.Offset(direction);
                if (next.Y < 0 || next.Y >= map.Length) continue;
                if (next.X < 0 || next.X >= map[0].Length) continue;
                if (map[next.Y][next.X] == '#') continue;

                var nextDistance = distances[current] + 1;

                if (nextDistance >= distances.GetValueOrDefault(next, int.MaxValue)) continue;

                distances[next] = nextDistance;
                queue.Enqueue(next, -next.ManhattenDistance(end));
            }
        }

        return distances[end];
    }
}