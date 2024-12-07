using System.Text.RegularExpressions;

namespace advent.of.code.day7;

internal static class Solution {
    internal static long Task1(StreamReader reader) {
        return TotalCalibration(reader, false);
    }

    internal static long Task2(StreamReader reader) {
        return TotalCalibration(reader, true);
    }

    private static long TotalCalibration(StreamReader reader, bool allowConcat) {
        var total = 0L;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            var blocks = line.Split(":");

            var target = long.Parse(blocks[0]);
            var numbers = Regex.Matches(blocks[1], @"(\d+)")
                .Select(m => long.Parse(m.Value))
                .ToList();

            if (CanCalibrate(target, numbers[0], numbers, 1, allowConcat)) {
                total += target;
            }
        }

        return total;
    }

    private static bool CanCalibrate(long target, long current, List<long> numbers, int index, bool allowConcat) {
        if (index >= numbers.Count || current > target || current == 0) {
            return current == target;
        }

        var next = numbers[index];
        return CanCalibrate(target, current + next, numbers, index + 1, allowConcat) ||
               CanCalibrate(target, current * next, numbers, index + 1, allowConcat) ||
               allowConcat &&
               CanCalibrate(target, long.Parse($"{current}{next}"), numbers, index + 1, allowConcat);
    }
}