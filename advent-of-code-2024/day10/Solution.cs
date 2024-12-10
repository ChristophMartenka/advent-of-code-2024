namespace advent.of.code.day10;

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var map = new List<List<int>>();

        var positions = new List<List<Node>>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            for (var i = 0; i < line.Length; i++) {
                if (line[i] == '0') {
                    positions.Add([new Node(map.Count, i)]);
                }
            }

            map.Add(line.Select(c => int.Parse(c.ToString())).ToList());
        }

        Console.WriteLine(positions.Count);

        var toFind = 1;
        while (toFind <= 9) {
            var newPoints = new List<List<Node>>();
            foreach (var points in positions) {
                var point = points[^1];
                
                if (point.Y - 1 >= 0 && map[point.Y - 1][point.X] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y - 1, point.X));
                    newPoints.Add(p);
                }

                if (point.Y + 1 < map.Count && map[point.Y + 1][point.X] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y + 1, point.X));
                    newPoints.Add(p);
                }

                if (point.X - 1 >= 0 && map[point.Y][point.X - 1] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y, point.X - 1));
                    newPoints.Add(p);
                }

                if (point.X + 1 < map[point.Y].Count && map[point.Y][point.X + 1] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y, point.X + 1));
                    newPoints.Add(p);
                }
            }

            positions = newPoints;

            toFind++;
        }

        var pointsSave = positions.ToList();

        foreach (var point in pointsSave) {

            if (positions.Count(p => p[0] == point[0] && p[^1] == point[^1]) > 1) {
                positions.Remove(point);
            }
        }
        

        return positions.Count;
    }

    private record Node(int Y, int X);

    internal static int Task2(StreamReader reader) {
        var map = new List<List<int>>();

        var positions = new List<List<Node>>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            for (var i = 0; i < line.Length; i++) {
                if (line[i] == '0') {
                    positions.Add([new Node(map.Count, i)]);
                }
            }

            map.Add(line.Select(c => int.Parse(c.ToString())).ToList());
        }

        Console.WriteLine(positions.Count);

        var toFind = 1;
        while (toFind <= 9) {
            var newPoints = new List<List<Node>>();
            foreach (var points in positions) {
                var point = points[^1];
                
                if (point.Y - 1 >= 0 && map[point.Y - 1][point.X] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y - 1, point.X));
                    newPoints.Add(p);
                }

                if (point.Y + 1 < map.Count && map[point.Y + 1][point.X] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y + 1, point.X));
                    newPoints.Add(p);
                }

                if (point.X - 1 >= 0 && map[point.Y][point.X - 1] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y, point.X - 1));
                    newPoints.Add(p);
                }

                if (point.X + 1 < map[point.Y].Count && map[point.Y][point.X + 1] == toFind) {
                    var p = points.ToList();
                    p.Add(new Node(point.Y, point.X + 1));
                    newPoints.Add(p);
                }
            }

            positions = newPoints;

            toFind++;
        }

        return positions.Count;
    }
}