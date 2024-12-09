namespace advent.of.code.day9;

internal static class Solution {
    internal static long Task1(StreamReader reader) {
        var blocks = ReadBlocks(reader);
        CompactFiles(blocks, true);
        return CalculateChecksum(blocks);
    }

    internal static long Task2(StreamReader reader) {
        var blocks = ReadBlocks(reader);
        CompactFiles(blocks, false);
        return CalculateChecksum(blocks);
    }

    private record Block(int Id, int Size);

    private static List<Block> ReadBlocks(StreamReader reader) {
        var blocks = new List<Block>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            // Every even index is a file, odd index is an empty space
            blocks.AddRange(line.Select((c, i) => new Block(i % 2 == 0 ? i / 2 : -1, int.Parse(c.ToString()))));
        }

        return blocks;
    }

    private static void CompactFiles(List<Block> blocks, bool splitFiles) {
        /*
         * Performance optimization when splitting files is allowed. There is no need to check the whole list, therefor
         * store the last right pointer to avoid unnecessary iterations
         */
        var lastRightElement = blocks.Count - 1;

        for (var leftPointer = 0; leftPointer < blocks.Count; leftPointer++) {
            // Skip files, they are already in correct position
            if (blocks[leftPointer].Id >= 0) continue;

            for (var rightPointer = lastRightElement; rightPointer > leftPointer; rightPointer--) {
                // Skip empty blocks, as we are looking for files
                if (blocks[rightPointer].Id < 0) continue;

                var remainingSize = blocks[leftPointer].Size - blocks[rightPointer].Size;
                if (remainingSize < 0) {
                    // If file splitting is disabled, skip this file as it is too big
                    if (!splitFiles) continue;

                    blocks[leftPointer] = blocks[rightPointer] with { Size = blocks[leftPointer].Size };
                    blocks[rightPointer] = blocks[rightPointer] with { Size = Math.Abs(remainingSize) };

                    lastRightElement = rightPointer;
                    break;
                }

                // Move file from right to left
                blocks[leftPointer] = blocks[rightPointer];
                blocks[rightPointer] = blocks[rightPointer] with { Id = -1 };

                // If block was not completely filled, add new empty block with remaining size
                if (remainingSize > 0) {
                    blocks.Insert(leftPointer + 1, new Block(-1, remainingSize));
                    lastRightElement++;
                }

                break;
            }
        }
    }

    private static long CalculateChecksum(List<Block> blocks) {
        var checksum = 0L;
        var totalIndex = 0;

        foreach (var block in blocks) {
            if (block.Id >= 0) {
                for (var i = 0; i < block.Size; i++) {
                    checksum += (totalIndex + i) * block.Id;
                }
            }

            totalIndex += block.Size;
        }

        return checksum;
    }
}