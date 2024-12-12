using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day12;

[TestFixture]
internal class Test {

    private const int Day = 12;

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = 1930)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = 1363682)]
    public int Task1_Test(int day, string fileName) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName));
    }

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = 1206)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = 787680)]
    public int Task2_Test(int day, string fileName) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName));
    }
}