namespace advent.of.code.day11;

internal static class Solution {
    internal static long Task1(StreamReader reader) {
        var stones = ReadStones(reader);

        var blinks = 25;
        while (blinks > 0) {
            stones = Blink(stones);
            blinks--;
        }

        return stones.Values.Sum();
    }

    internal static long Task2(StreamReader reader) {
        var stones = ReadStones(reader);

        var blinks = 75;
        while (blinks > 0) {
            stones = Blink(stones);
            blinks--;
        }

        return stones.Values.Sum();
    }

    private static Dictionary<long, long> ReadStones(StreamReader reader) {
        var stones = new Dictionary<long, long>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            foreach (var stone in line.Split(" ").Select(long.Parse).Where(s => !stones.TryAdd(s, 1))) {
                stones[stone]++;
            }
        }

        return stones;
    }

    private static Dictionary<long, long> Blink(Dictionary<long, long> stones) {
        var newMappings = new Dictionary<long, long>();
        foreach (var (stone, count) in stones) {
            var newStones = stone switch {
                0 => [1],
                _ when stone.ToString().Length % 2 == 0 => new List<long> {
                    long.Parse(stone.ToString()[..(stone.ToString().Length / 2)]),
                    long.Parse(stone.ToString()[(stone.ToString().Length / 2)..])
                },
                _ => [stone * 2024]
            };

            foreach (var newStone in newStones.Where(s => !newMappings.TryAdd(s, count))) {
                newMappings[newStone] += count;
            }
        }

        return newMappings;
    }
}