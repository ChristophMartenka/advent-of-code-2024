using System.Text.RegularExpressions;

namespace advent.of.code.day14;

internal static class Solution {
    internal static int Task1(StreamReader reader, int width, int height) {
        var robots = ReadRobots(reader);

        for (var step = 0; step < 100; step++) {
            PerformStep(ref robots, width, height);
        }

        return robots
            .Where(robot => !(robot.Pos.Y == height / 2 || robot.Pos.X == width / 2))
            .GroupBy(robot => (robot.Pos.Y < height / 2 ? 0 : 1) + (robot.Pos.X < width / 2 ? 0 : 2))
            .Aggregate(1, (current, quadrant) => current * quadrant.Count());
    }

    internal static int Task2(StreamReader reader, int width, int height) {
        var robots = ReadRobots(reader);

        var step = 0;
        do {
            step++;
            PerformStep(ref robots, width, height);
        } while (robots.GroupBy(r => r.Pos).Any(g => g.Count() > 1));

        // Print step displaying the tree
        // for (var y = 0; y < height; y++) {
        //     for (var x = 0; x < width; x++) {
        //         var count = robots.Count(r => r.X == x && r.Y == y);
        //         Console.Write(count > 0 ? count : ".");
        //     }
        //     Console.WriteLine();
        // }

        return step;
    }

    private record Robot(Vector2 Pos, Vector2 Speed);

    private static List<Robot> ReadRobots(StreamReader reader) {
        var robots = new List<Robot>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            var match = Regex.Match(line, @"^p=(?<X>\d+),(?<Y>\d+)\sv=(?<XSpeed>-?\d+),(?<YSpeed>-?\d+)$");

            robots.Add(new Robot(
                new Vector2(int.Parse(match.Groups["X"].Value), int.Parse(match.Groups["Y"].Value)),
                new Vector2(int.Parse(match.Groups["XSpeed"].Value), int.Parse(match.Groups["YSpeed"].Value))
            ));
        }

        return robots;
    }

    private static void PerformStep(ref List<Robot> robots, int width, int height) {
        for (var i = 0; i < robots.Count; i++) {
            var robot = robots[i];
            robots[i] = robot with { Pos = (robot.Pos + robot.Speed).WithinBounds(width, height) };
        }
    }

    private record Vector2(int X, int Y) {
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);

        internal Vector2 WithinBounds(int maxX, int maxY) => new(
            X < 0 ? X + maxX : X >= maxX ? X - maxX : X,
            Y < 0 ? Y + maxY : Y >= maxY ? Y - maxY : Y
        );
    }
}