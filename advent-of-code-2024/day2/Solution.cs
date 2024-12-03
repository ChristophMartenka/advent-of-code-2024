namespace advent.of.code.day2;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        var reports = ReadReports(reader);
        return reports.Count(x => IsSafe(x, out _));
    }

    internal static int Task2(StreamReader reader) {
        var reports = ReadReports(reader);
        return reports.Count(IsSafeWithTolerance);
    }

    private static List<List<int>> ReadReports(StreamReader reader) {
        var reports = new List<List<int>>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            reports.Add(line.Split(" ").Select(int.Parse).ToList());
        }

        return reports;
    }

    private static bool IsSafe(List<int> report, out int unsafeIndex) {
        var increasing = report[0] - report[1] > 0;
        for (var i = 0; i < report.Count - 1; i++) {
            var increase = report[i] - report[i + 1];
            if (increasing == increase > 0 && Math.Abs(increase) > 0 && Math.Abs(increase) <= 3) {
                continue;
            }

            unsafeIndex = i;
            return false;
        }

        unsafeIndex = -1;
        return true;
    }

    private static bool IsSafeWithTolerance(List<int> report) {
        if (IsSafe(report, out var index)) {
            return true;
        }

        // check around unsafe index to see if it's safe when removing the number or one of the adjacent numbers
        for (var i = -1; i < 2; i++) {
            if (index == 0 && i == -1) {
                continue;
            }

            if (index == report.Count && i == 1) {
                continue;
            }

            var reportWithTolerance = report.ToList();
            reportWithTolerance.RemoveAt(index + i);

            if (IsSafe(reportWithTolerance, out _)) {
                return true;
            }
        }

        return false;
    }
}