using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day14;

[TestFixture]
internal class Test {

    private const int Day = 14;

    [TestCase(Day, "testInput.txt", 11, 7, TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = 12)]
    [TestCase(Day, "input.txt", 101, 103, TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = 229069152)]
    public int Task1_Test(int day, string fileName, int width, int height) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName), width, height);
    }

    // Test input is not applicable for this task
    // [TestCase(Day, "testInput.txt", 11, 7, TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = 0)]
    [TestCase(Day, "input.txt", 101, 103, TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = 7383)]
    public int Task2_Test(int day, string fileName, int width, int height) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName), width, height);
    }
}