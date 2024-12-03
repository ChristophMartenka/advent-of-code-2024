using System.Text.RegularExpressions;

namespace advent.of.code.day3;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        var total = 0;

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            total += Regex.Matches(line, @"mul\((?<mul>\d{1,3},\d{1,3})\)")
                .Select(match => match.Groups["mul"].Value.Split(",").Select(int.Parse).ToList())
                .Select(toMultiply => toMultiply[0] * toMultiply[1])
                .Sum();
        }

        return total;
    }

    internal static int Task2(StreamReader reader) {
        var total = 0;
        var enabled = true;

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            total += Regex.Matches(line, @"(?<instruction>mul\((?<mul>\d{1,3},\d{1,3})\)|don't\(\)|do\(\))")
                .Where(match => {
                    switch (match.Groups["instruction"].Value) {
                        case "do()":
                            enabled = true;
                            return false;
                        case "don't()":
                            enabled = false;
                            return false;
                        default:
                            return enabled;
                    }
                })
                .Select(match => match.Groups["mul"].Value.Split(",").Select(int.Parse).ToList())
                .Select(toMultiply => toMultiply[0] * toMultiply[1])
                .Sum();
        }

        return total;
    }
}