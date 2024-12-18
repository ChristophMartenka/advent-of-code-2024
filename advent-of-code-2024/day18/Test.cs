using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day18;

[TestFixture]
internal class Test {

    private const int Day = 18;

    [TestCase(Day, "testInput.txt", 7, 7, 12, TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = 22)]
    [TestCase(Day, "input.txt", 71, 71, 1024, TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = 336)]
    public int Task1_Test(int day, string fileName, int width, int height, int target) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName), width, height, target);
    }

    [TestCase(Day, "testInput.txt", 7, 7, TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = "6,1")]
    [TestCase(Day, "input.txt", 71, 71, TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = "24,30")]
    public string Task2_Test(int day, string fileName, int width, int height) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName), width, height);
    }
}