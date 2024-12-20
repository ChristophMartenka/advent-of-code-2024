namespace advent.of.code.day20;

internal static class Solution {
    internal static int Task1(StreamReader reader, int minSpeedGain) {
        ReadMap(reader, out var map, out var start, out var end);
        var distances = CalculateShortestDistance(map, start, end);
        return CountValidCheats(distances.Keys.ToList(), 2, minSpeedGain);
    }

    internal static int Task2(StreamReader reader, int minSpeedGain) {
        ReadMap(reader, out var map, out var start, out var end);
        var distances = CalculateShortestDistance(map, start, end);
        return CountValidCheats(distances.Keys.ToList(), 20, minSpeedGain);
    }

    private enum Direction {
        Up,
        Down,
        Left,
        Right
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

    private static void ReadMap(StreamReader reader, out List<char[]> map, out Vector2 start, out Vector2 end) {
        map = [];
        start = new Vector2(0, 0);
        end = new Vector2(0, 0);
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            if (line.Contains('S')) start = new Vector2(line.IndexOf('S'), map.Count);
            if (line.Contains('E')) end = new Vector2(line.IndexOf('E'), map.Count);

            map.Add(line.ToCharArray());
        }
    }

    private static Dictionary<Vector2, int> CalculateShortestDistance(List<char[]> map, Vector2 start, Vector2 end) {
        var distances = new Dictionary<Vector2, int> { { start, 0 } };

        var queue = new PriorityQueue<Vector2, int>();
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var current, out _)) {
            // Smallest distance found
            if (current == end) break;

            foreach (var direction in Enum.GetValues<Direction>()) {
                var next = current.Offset(direction);
                if (next.Y < 0 || next.Y >= map.Count) continue;
                if (next.X < 0 || next.X >= map[0].Length) continue;
                if (map[next.Y][next.X] == '#') continue;

                var nextDistance = distances[current] + 1;

                if (nextDistance >= distances.GetValueOrDefault(next, int.MaxValue)) continue;

                distances[next] = nextDistance;
                queue.Enqueue(next, -next.ManhattenDistance(end));
            }
        }

        return distances;
    }

    private static int CountValidCheats(List<Vector2> path, int maxCheatLength, int minSpeedGain) {
        return path.AsParallel()
            .Select((p, i) => {
                var skips = 0;
                for (var j = i + 1; j < path.Count; j++) {
                    var distance = p.ManhattenDistance(path[j]);
                    if (distance > maxCheatLength) continue;
                    if (j - i - distance >= minSpeedGain) {
                        skips++;
                    }
                }

                return skips;
            })
            .Sum();
    }
}