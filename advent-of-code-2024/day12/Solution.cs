namespace advent.of.code.day12;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        return ReadPositionsByType(reader)
            .SelectMany(FindAreas)
            .Select(area => area.Count * area.Sum(point => GetNeighborsOutsideOfArea(area, point).Count()))
            .Sum();
    }

    internal static int Task2(StreamReader reader) {
        return ReadPositionsByType(reader)
            .SelectMany(FindAreas)
            .Select(area => area.Count * CalculatePerimeter(GetBorder(area)))
            .Sum();
    }

    private record Vector2(int Y, int X) {
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.Y + b.Y, a.X + b.X);
        public static Vector2 operator *(Vector2 a, int val) => new(a.Y * val, a.X * val);
        
        internal Vector2 Flip() => new(X, Y);
        
        internal Direction DirectionTowards(Vector2 b) {
            if (Y == b.Y) return X < b.X ? Direction.Left : Direction.Right;
            return Y < b.Y ? Direction.Up : Direction.Down;
        }
    }

    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    private static Vector2 Vector(this Direction direction) {
        return direction switch {
            Direction.Up => new Vector2(-1, 0),
            Direction.Down => new Vector2(1, 0),
            Direction.Left => new Vector2(0, -1),
            Direction.Right => new Vector2(0, 1),
            _ => throw new Exception()
        };
    }

    private static IEnumerable<HashSet<Vector2>> ReadPositionsByType(StreamReader reader) {
        var positions = new Dictionary<char, HashSet<Vector2>>();

        var y = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            for (var x = 0; x < line.Length; x++) {
                var c = line[x];
                positions.TryAdd(c, []);
                positions[c].Add(new Vector2(y, x));
            }

            y++;
        }

        return positions.Values;
    }

    private static IEnumerable<HashSet<Vector2>> FindAreas(HashSet<Vector2> remainingPoints) {
        while (remainingPoints.Count > 0) {
            yield return FindArea(ref remainingPoints, remainingPoints.First());
        }
    }

    private static HashSet<Vector2> FindArea(ref HashSet<Vector2> remainingPoints, Vector2 start) {
        var area = new HashSet<Vector2> { start };

        remainingPoints.Remove(start);

        foreach (var direction in Enum.GetValues<Direction>()) {
            var next = start + direction.Vector();
            if (!remainingPoints.Contains(next)) continue;

            foreach (var position in FindArea(ref remainingPoints, next)) {
                area.Add(position);
            }
        }

        return area;
    }

    private static IEnumerable<Vector2> GetNeighborsOutsideOfArea(HashSet<Vector2> area, Vector2 vector2) {
        for (var i = -1; i < 2; i += 2) {
            if (!area.Contains(vector2 with { Y = vector2.Y + i })) yield return vector2 with { Y = vector2.Y + i };
            if (!area.Contains(vector2 with { X = vector2.X + i })) yield return vector2 with { X = vector2.X + i };
        }
    }

    private static Dictionary<Vector2, List<Direction>> GetBorder(HashSet<Vector2> area) {
        var border = new Dictionary<Vector2, List<Direction>>();

        foreach (var position in area) {
            foreach (var neighbor in GetNeighborsOutsideOfArea(area, position)) {
                border.TryAdd(position, []);
                border[position].Add(neighbor.DirectionTowards(position));
            }
        }

        return border;
    }

    private static int CalculatePerimeter(Dictionary<Vector2, List<Direction>> border) {
        var perimeter = 0;

        while (border.Count > 0) {
            perimeter++;

            var (start, startDirections) = border.First();
            var direction = startDirections.First();

            startDirections.Remove(direction);
            if (startDirections.Count == 0) border.Remove(start);

            for (var i = -1; i < 2; i += 2) {
                var current = start;
                while (true) {
                    var next = current + direction.Vector().Flip() * i;

                    if (border.TryGetValue(next, out var directions) && directions.Remove(direction)) {
                        if (directions.Count == 0) border.Remove(next);
                        current = next;
                        continue;
                    }

                    break;
                }
            }
        }

        return perimeter;
    }
}