using System.Text.RegularExpressions;

namespace advent.of.code.day13;

internal static class Solution {
    internal static long Task1(StreamReader reader) {
        return ReadClawMachines(reader)
            .Select(CalculateButtonPressesToWin)
            .Where(win => win >= 0)
            .Sum();
    }

    internal static long Task2(StreamReader reader) {
        return ReadClawMachines(reader)
            .Select(machine => machine with { Target = machine.Target + 10000000000000L })
            .Select(CalculateButtonPressesToWin)
            .Where(win => win >= 0)
            .Sum();
    }

    private record ClawMachine(Vector2 ButtonA, Vector2 ButtonB, Vector2 Target);

    private record Vector2(long X, long Y) {
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator +(Vector2 v, long val) => new(v.X + val, v.Y + val);
        public static Vector2 operator *(Vector2 v, long val) => new(v.X * val, v.Y * val);

        internal long CrossProduct(Vector2 b) => X * b.Y - Y * b.X;
    }

    private static IEnumerable<ClawMachine> ReadClawMachines(StreamReader reader) {
        while (!reader.EndOfStream) {
            var buttonA = ParseLine(reader.ReadLine() ?? throw new Exception());
            var buttonB = ParseLine(reader.ReadLine() ?? throw new Exception());
            var prize = ParseLine(reader.ReadLine() ?? throw new Exception());
            yield return new ClawMachine(buttonA, buttonB, prize);

            // skip empty line
            if (!reader.EndOfStream) reader.ReadLine();
        }
    }

    private static Vector2 ParseLine(string line) {
        var match = Regex.Match(line, @"X=?\+?(?<x>\d+), Y=?\+?(?<y>\d+)");
        return new Vector2(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
    }

    private static long CalculateButtonPressesToWin(ClawMachine machine) {
        var pressesA = machine.Target.CrossProduct(machine.ButtonB) / machine.ButtonA.CrossProduct(machine.ButtonB);
        var pressesB = (machine.Target.X - machine.ButtonA.X * pressesA) / machine.ButtonB.X;

        if (machine.ButtonA * pressesA + machine.ButtonB * pressesB != machine.Target) return -1;

        return pressesA * 3 + pressesB;
    }
}