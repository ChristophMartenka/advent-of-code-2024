namespace advent.of.code.day17;

internal static class Solution {
    internal static string Task1(StreamReader reader) {
        ReadInput(reader, out var regA, out var regB, out var regC, out var program);
        return string.Join(",", ExecuteProgram(program, regA, regB, regC));
    }

    internal static long Task2(StreamReader reader, bool isTest) {
        ReadInput(reader, out _, out _, out _, out var program);

        var validValues = new List<long>();
        for (var i = 0; i < 8; i++) {
            Find(program, validValues, i, isTest ? IterateTestProgram : IterateProgram, program.Count - 1);
        }

        return validValues.Min();

        long IterateTestProgram(long registerA) {
            registerA = (long)(registerA / Math.Pow(2, 3));
            return registerA % 8;
        }

        long IterateProgram(long regA) {
            var regB = regA % 8;
            regB ^= 2;
            var regC = (long)(regA / Math.Pow(2, regB));
            // register A is never returned or used, so the step in the program can be ignored
            // regA = (long)(registerA / Math.Pow(2, 3));
            regB ^= regC;
            regB ^= 7;
            return regB % 8;
        }
    }

    private static void Find(List<int> target, List<long> validA, long registerA, Func<long, long> step, int index) {
        if (step(registerA) != target[index]) return;

        if (index == 0) {
            validA.Add(registerA);
            return;
        }

        for (var i = 0; i < 8; i++) {
            Find(target, validA, registerA * 8 + i, step, index - 1);
        }
    }

    private static void ReadInput(StreamReader reader, out long regA, out long regB, out long regC,
        out List<int> program) {
        regA = ReadRegister(reader.ReadLine() ?? throw new Exception());
        regB = ReadRegister(reader.ReadLine() ?? throw new Exception());
        regC = ReadRegister(reader.ReadLine() ?? throw new Exception());

        reader.ReadLine();

        var programLine = reader.ReadLine() ?? throw new Exception();
        program = programLine[programLine.LastIndexOf(' ')..].Split(',').Select(int.Parse).ToList();
    }

    private static long ReadRegister(string line) {
        return long.Parse(line[line.LastIndexOf(' ')..]);
    }

    private static List<long> ExecuteProgram(List<int> program, long regA, long regB, long regC) {
        var output = new List<long>();

        for (var i = 0; i < program.Count; i += 2) {
            var opcode = program[i];
            switch (opcode) {
                case 0:
                    regA = (long)(regA / Math.Pow(2, ComboOperator(program[i + 1], regA, regB, regC)));
                    break;
                case 1:
                    regB ^= program[i + 1];
                    break;
                case 2:
                    regB = ComboOperator(program[i + 1], regA, regB, regC) % 8;
                    break;
                case 3:
                    // jump to index - 2 as index is incremented at the end of the loop
                    if (regA != 0) i = program[i + 1] - 2;
                    break;
                case 4:
                    regB ^= regC;
                    break;
                case 5:
                    output.Add(ComboOperator(program[i + 1], regA, regB, regC) % 8);
                    break;
                case 6:
                    regB = (long)(regA / Math.Pow(2, ComboOperator(program[i + 1], regA, regB, regC)));
                    break;
                case 7:
                    regC = (long)(regA / Math.Pow(2, ComboOperator(program[i + 1], regA, regB, regC)));
                    break;
            }
        }

        return output;
    }

    private static long ComboOperator(int operand, long regA, long regB, long regC) {
        return operand switch {
            < 4 => operand,
            4 => regA,
            5 => regB,
            6 => regC,
            _ => throw new Exception()
        };
    }
}