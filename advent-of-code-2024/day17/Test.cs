using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day17;

[TestFixture]
internal class Test {

    private const int Day = 17;

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = "4,6,3,5,6,3,5,2,1,0")]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = "7,1,3,7,5,1,0,3,4")]
    public string Task1_Test(int day, string fileName) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName));
    }

    [TestCase(Day, "testInput2.txt", TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = 117440)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = 190384113204239)]
    public long Task2_Test(int day, string fileName) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName), fileName.StartsWith("test"));
    }
}