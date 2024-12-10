namespace advent.of.code.day10;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        var map = ReadMapAndStartingPoints(reader, out var startingPoints);

        // Use HashSet to remove trails which start and end at the same location
        return FindTrails(map, startingPoints).ToHashSet().Count;
    }

    internal static int Task2(StreamReader reader) {
        var map = ReadMapAndStartingPoints(reader, out var startingPoints);
        return FindTrails(map, startingPoints).Count;
    }

    private record Point(int Y, int X);

    private record Trail(Point Start, Point End);

    private static List<List<int>> ReadMapAndStartingPoints(StreamReader reader, out List<Point> startingPoints) {
        var map = new List<List<int>>();
        startingPoints = [];

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            for (var i = 0; i < line.Length; i++) {
                if (line[i] != '0') continue;
                
                startingPoints.Add(new Point(map.Count, i));
            }

            map.Add(line.Select(c => int.Parse(c.ToString())).ToList());
        }

        return map;
    }

    private static List<Trail> FindTrails(List<List<int>> map, List<Point> startingPoints) {
        var trails = startingPoints.Select(p => new Trail(p, p)).ToList();

        var heightToFind = 0;
        while (++heightToFind <= 9) {
            var newTrails = new List<Trail>();
            foreach (var trail in trails) {
                var head = trail.End;

                for (var i = -1; i < 2; i += 2) {
                    if (head.Y + i >= 0 && head.Y + i < map.Count && map[head.Y + i][head.X] == heightToFind) {
                        newTrails.Add(trail with { End = head with { Y = head.Y + i } });
                    }

                    if (head.X + i >= 0 && head.X + i < map[head.Y].Count && map[head.Y][head.X + i] == heightToFind) {
                        newTrails.Add(trail with { End = head with { X = head.X + i } });
                    }
                }
            }

            trails = newTrails;
        }

        return trails;
    }
}