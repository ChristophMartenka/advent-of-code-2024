using System.Text.RegularExpressions;

namespace advent.of.code.day5;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        ReadRulesAndPages(reader, out var rules, out var listOfPages);

        return listOfPages.Where(page => IsCorrect(page, rules))
            .Sum(page => int.Parse(page[page.Count / 2]));
    }

    internal static int Task2(StreamReader reader) {
        ReadRulesAndPages(reader, out var rules, out var listOfPages);

        var total = 0;

        foreach (var pages in listOfPages.Where(pages => !IsCorrect(pages, rules))) {
            for (var i = pages.Count - 1; i >= 0; i--) {
                while (true) {
                    var wrongIndex = rules.Where(rule => rule[0] == pages[i])
                        .Select(rule => pages.IndexOf(rule[1]))
                        .Where(index => index >= 0 && index < i)
                        .FirstOrDefault(-1);

                    if (wrongIndex < 0) break;

                    (pages[i], pages[wrongIndex]) = (pages[wrongIndex], pages[i]);
                }
            }

            total += int.Parse(pages[pages.Count / 2]);
        }

        return total;
    }

    private static void ReadRulesAndPages(
        StreamReader reader,
        out List<List<string>> rules,
        out List<List<string>> listOfPages
    ) {
        rules = [];
        listOfPages = [];
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            if (Regex.IsMatch(line, @"\d+\|\d+")) {
                rules.Add(line.Split("|").ToList());
            }
            else if (!string.IsNullOrEmpty(line)) {
                listOfPages.Add(line.Split(",").ToList());
            }
        }
    }

    private static bool IsCorrect(List<string> pages, List<List<string>> rules) {
        return !pages.Where((page, i) =>
                rules.Any(rule => rule[1] == page && pages.IndexOf(rule[0]) >= 0 && pages.IndexOf(rule[0]) > i))
            .Any();
    }
}