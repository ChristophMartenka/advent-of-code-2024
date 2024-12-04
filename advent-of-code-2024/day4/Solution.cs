namespace advent.of.code.day4;

internal static class Solution {
    internal static int Task1(StreamReader reader) {
        var wordSearch = ReadWordSearch(reader);
        return AllPoints(wordSearch)
            .Where(point => wordSearch[point.Y][point.X] == 'X')
            .SelectMany(point => CountXmas(wordSearch, point))
            .Count();
    }

    internal static int Task2(StreamReader reader) {
        var wordSearch = ReadWordSearch(reader);
        return AllPoints(wordSearch)
            .Where(point => wordSearch[point.Y][point.X] == 'A')
            .Where(point => wordSearch.Count > point.Y + 1 && point.Y - 1 >= 0)
            .Where(point => wordSearch[point.Y].Length > point.X + 1 && point.X - 1 >= 0)
            .Where(point =>
                (wordSearch[point.Y - 1][point.X - 1] == 'M' && wordSearch[point.Y + 1][point.X + 1] == 'S') ||
                (wordSearch[point.Y - 1][point.X - 1] == 'S' && wordSearch[point.Y + 1][point.X + 1] == 'M'))
            .Count(point =>
                (wordSearch[point.Y - 1][point.X + 1] == 'S' && wordSearch[point.Y + 1][point.X - 1] == 'M') ||
                (wordSearch[point.Y - 1][point.X + 1] == 'M' && wordSearch[point.Y + 1][point.X - 1] == 'S'));
    }

    private static List<string> ReadWordSearch(StreamReader reader) {
        var wordSearch = new List<string>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            wordSearch.Add(line);
        }

        return wordSearch;
    }

    private record Point(int Y, int X);

    private static IEnumerable<Point> AllPoints(List<string> map) {
        for (var y = 0; y < map.Count; y++) {
            for (var x = 0; x < map[y].Length; x++) {
                yield return new Point(y, x);
            }
        }
    }

    private static IEnumerable<bool> CountXmas(List<string> wordSearch, Point p) {
        for (var i = -1; i <= 1; i++) {
            if (p.Y + i * 3 < 0 || p.Y + i * 3 >= wordSearch.Count) continue;

            for (var j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) continue;

                if (p.X + j * 3 < 0 || p.X + j * 3 >= wordSearch[p.Y + i].Length) continue;

                if (
                    wordSearch[p.Y + i][p.X + j] == 'M' &&
                    wordSearch[p.Y + i * 2][p.X + j * 2] == 'A' &&
                    wordSearch[p.Y + i * 3][p.X + j * 3] == 'S'
                ) {
                    yield return true;
                }
            }
        }
    }
}