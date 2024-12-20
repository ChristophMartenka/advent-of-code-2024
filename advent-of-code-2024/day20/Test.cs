using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day20;

[TestFixture]
internal class Test {

    private const int Day = 20;

    [TestCase(Day, "testInput.txt", 1, TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = 44)]
    [TestCase(Day, "input.txt", 100, TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = 1393)]
    public int Task1_Test(int day, string fileName, int minSpeedGain) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName), minSpeedGain);
    }

    [TestCase(Day, "testInput.txt", 50, TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = 285)]
    [TestCase(Day, "input.txt", 100, TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = 990096)]
    public int Task2_Test(int day, string fileName, int minSpeedGain) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName), minSpeedGain);
    }
}