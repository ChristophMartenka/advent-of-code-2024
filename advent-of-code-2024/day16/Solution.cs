namespace advent.of.code.day16;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        ReadMap(reader, out var map, out var start, out var end);
        var distances = CalculateShortestDistance(map, start, end);
        return distances.Where(pair => pair.Key.Pos == end).Select(pair => pair.Value).Min();
    }

    internal static int Task2(StreamReader reader) {
        ReadMap(reader, out var map, out var start, out var end);
        var distances = CalculateShortestDistance(map, start, end);

        var startStep = new Step(start, Direction.Right);
        var endStep = distances.First(pair => pair.Key.Pos == end).Key;

        var queue = new PriorityQueue<Step, int>();
        queue.Enqueue(endStep, distances[endStep]);

        var partOfBestPath = new HashSet<Step> { startStep, endStep };

        while (queue.TryDequeue(out var currentStep, out var remainingDistance)) {
            if (currentStep == startStep) break;
            
            foreach (var direction in Enum.GetValues<Direction>()) {
                if (currentStep.Direction == direction.Opposite()) continue;
                
                var next = new Step(currentStep.Pos.Offset(currentStep.Direction.Opposite()), direction);
                if (partOfBestPath.Contains(next)) continue;
                
                var nextDistance = remainingDistance - (currentStep.Direction == direction ? 1 : 1001);
                if (!distances.TryGetValue(next, out var distance) || distance != nextDistance) continue;

                partOfBestPath.Add(next);
                queue.Enqueue(next, nextDistance);
            }
        }

        return partOfBestPath.DistinctBy(step => step.Pos).Count();
    }

    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    private static Direction Opposite(this Direction direction) {
        return direction switch {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new Exception()
        };
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
    }

    private record Step(Vector2 Pos, Direction Direction);

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

    private static Dictionary<Step, int> CalculateShortestDistance(List<char[]> map, Vector2 start, Vector2 end) {
        var distances = new Dictionary<Step, int> { { new Step(start, Direction.Right), 0 } };

        var queue = new PriorityQueue<Step, int>();
        queue.Enqueue(new Step(start, Direction.Right), 0);

        while (queue.TryDequeue(out var current, out var distance)) {
            // Smallest distance found
            if (current.Pos == end) break;

            foreach (var direction in Enum.GetValues<Direction>()) {
                if (direction == current.Direction.Opposite()) continue;

                var next = new Step(current.Pos.Offset(direction), direction);
                if (map[next.Pos.Y][next.Pos.X] == '#') continue;

                var nextDistance = distance + 1;
                if (current.Direction != direction) {
                    nextDistance += 1000;
                }

                if (nextDistance >= distances.GetValueOrDefault(next, int.MaxValue)) continue;

                distances[next] = nextDistance;
                queue.Enqueue(next, nextDistance);
            }
        }

        return distances;
    }
}