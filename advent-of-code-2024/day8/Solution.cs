namespace advent.of.code.day8;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        var map = ReadMap(reader);
        var antennaLocationsByFrequency = ReadAntennaLocations(map);
        return FindAntinodes(antennaLocationsByFrequency, map.Count, map[0].Length, false).Count;
    }

    internal static int Task2(StreamReader reader) {
        var map = ReadMap(reader);
        var antennaLocationsByFrequency = ReadAntennaLocations(map);
        var antinodeLocations = FindAntinodes(antennaLocationsByFrequency, map.Count, map[0].Length, true);

        // Add all antenna locations to the antinode locations
        antinodeLocations.UnionWith(antennaLocationsByFrequency.Values.SelectMany(antennas => antennas));

        return antinodeLocations.Count;
    }

    private static List<char[]> ReadMap(StreamReader reader) {
        var map = new List<char[]>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            map.Add(line.ToCharArray());
        }

        return map;
    }

    private static Dictionary<char, List<Point>> ReadAntennaLocations(List<char[]> map) {
        var frequencies = new Dictionary<char, List<Point>>();
        for (var y = 0; y < map.Count; y++) {
            for (var x = 0; x < map[y].Length; x++) {
                var frequency = map[y][x];
                if (frequency == '.') continue;
                if (!frequencies.ContainsKey(frequency)) {
                    frequencies.Add(frequency, []);
                }

                frequencies[frequency].Add(new Point(y, x));
            }
        }

        return frequencies;
    }

    private record Point(int Y, int X) {
        public static Point operator +(Point a, Point b) => new(a.Y + b.Y, a.X + b.X);
        public static Point operator -(Point a, Point b) => new(a.Y - b.Y, a.X - b.X);
        public static Point operator *(Point a, int val) => new(a.Y * val, a.X * val);
    }

    private static HashSet<Point> FindAntinodes(
        Dictionary<char, List<Point>> antennaLocationsByFrequency,
        int height,
        int width,
        bool resonant
    ) {
        var locations = new HashSet<Point>();
        foreach (var antennas in antennaLocationsByFrequency.Values) {
            for (var i = 0; i < antennas.Count; i++) {
                var antennaA = antennas[i];
                for (var j = i + 1; j < antennas.Count; j++) {
                    var antennaB = antennas[j];
                    var direction = antennaA - antennaB;
                    locations.UnionWith(GetAntinodes(antennaA, direction, height, width, resonant));
                    locations.UnionWith(GetAntinodes(antennaB, direction * -1, height, width, resonant));
                }
            }
        }

        return locations;
    }

    private static IEnumerable<Point> GetAntinodes(Point start, Point direction, int height, int width, bool resonant) {
        var antinode = start + direction;
        while (antinode.Y >= 0 && antinode.Y < height && antinode.X >= 0 && antinode.X < width) {
            yield return antinode;
            if (!resonant) break;
            antinode += direction;
        }
    }
}