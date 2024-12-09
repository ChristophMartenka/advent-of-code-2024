namespace advent.of.code.day9;

internal static class Solution {

    internal static long Task1(StreamReader reader) {
        var final = new List<int>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var index = 0;
            for (var i = 0; i < line.Length; i += 2) {
                for (var j = 0; j < int.Parse(line[i].ToString()); j++) {
                    final.Add(index);
                }

                if (i + 1 < line.Length) {
                    for (var j = 0; j < int.Parse(line[i + 1].ToString()); j++) {
                        final.Add(-1);
                    }
                }

                index++;
            }
        }

        var left = 0;
        var right = final.Count - 1;

        while (true) {
            while (final[left] >= 0) {
                left++;
            }

            while (final[right] < 0) {
                right--;
            }

            if (left >= right) break;

            final[left] = final[right];
            final[right] = -1;
        }

        var total = 0L;

        for (var i = 0; i < final.Count; i++) {
            if (final[i] == -1) break;

            total += i * final[i];
        }

        return total;
    }

    private record Block(int ID, int Size);

    private static List<Block> ReadBlocks(StreamReader reader) {
        var blocks = new List<Block>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var fileId = 0;
            for (var i = 0; i < line.Length; i += 2) {
                blocks.Add(new Block(fileId, int.Parse(line[i].ToString())));

                if (i + 1 < line.Length) {
                    blocks.Add(new Block(-1, int.Parse(line[i + 1].ToString())));
                }

                fileId++;
            }
        }

        return blocks;
    }

    internal static long Task2(StreamReader reader) {
        var blocks = ReadBlocks(reader);

        for (var i = 0; i < blocks.Count; i++) {
            // Skip non-empty blocks
            if (blocks[i].ID >= 0) continue;

            for (var j = blocks.Count - 1; j > i; j--) {
                if (blocks[j].ID < 0 || blocks[j].Size > blocks[i].Size) continue;

                var remainingSize = blocks[i].Size - blocks[j].Size;
                blocks[i] = blocks[j];
                blocks[j] = blocks[j] with { ID = -1 };
                if (remainingSize <= 0) break;

                // Insert empty block with remaining size and continue search with index set to the new empty block
                blocks.Insert(++i, new Block(-1, remainingSize));
            }
        }

        var total = 0L;
        var index = 0;

        foreach (var block in blocks) {
            if (block.ID >= 0) {
                for (var i = 0; i < block.Size; i++) {
                    total += (index + i) * block.ID;
                }
            }

            index += block.Size;
        }

        return total;
    }
}