namespace advent.of.code.day19;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        ReadInput(reader, out var towels, out var patterns);
        return patterns.AsParallel()
            .Count(pattern => CountPossibleCombinations(towels, pattern, new Dictionary<string, long>()) > 0);
    }

    internal static long Task2(StreamReader reader) {
        ReadInput(reader, out var towels, out var patterns);
        return patterns.AsParallel()
            .Select(pattern => CountPossibleCombinations(towels, pattern, new Dictionary<string, long>()))
            .Sum();
    }

    private static void ReadInput(StreamReader reader, out List<string> towels, out List<string> patterns) {
        var line = reader.ReadLine() ?? throw new Exception();
        towels = line.Split(", ").ToList();

        reader.ReadLine();

        patterns = [];
        while (!reader.EndOfStream) {
            patterns.Add(reader.ReadLine() ?? throw new Exception());
        }
    }

    private static long CountPossibleCombinations(List<string> towels, string pattern, Dictionary<string, long> cache) {
        if (cache.TryGetValue(pattern, out var cachedCombinations)) {
            return cachedCombinations;
        }

        var combinations = 0L;

        foreach (var towel in towels) {
            if (towel.Length < pattern.Length && pattern[..towel.Length] == towel) {
                combinations += CountPossibleCombinations(towels, pattern[towel.Length..], cache);
            }
            else if (towel == pattern) {
                combinations++;
            }
        }

        cache[pattern] = combinations;
        return combinations;
    }
}