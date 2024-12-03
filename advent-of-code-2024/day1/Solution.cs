using System.Text.RegularExpressions;

namespace advent.of.code.day1;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        ReadLists(reader, out var left, out var right);

        left.Sort();
        right.Sort();

        return left.Select((value, index) => Math.Abs(value - right[index]))
            .Sum();
    }

    internal static int Task2(StreamReader reader) {
        ReadLists(reader, out var left, out var right);

        return (
            from leftNumber in left
            let count = right.Count(number => number == leftNumber)
            select leftNumber * count
        ).Sum();
    }

    private static void ReadLists(StreamReader reader, out List<int> left, out List<int> right) {
        left = [];
        right = [];

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var match = Regex.Match(line, @"(?<left>\d+)\s+(?<right>\d+)");
            left.Add(int.Parse(match.Groups["left"].Value));
            right.Add(int.Parse(match.Groups["right"].Value));
        }
    }
}