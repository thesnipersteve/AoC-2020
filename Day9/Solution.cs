using System;
using System.Collections.Generic;
using System.Text;

namespace AoC_2020.Day9
{
    public class Solution
    {
        private const int PreambleLength = 25;
        private long[] input;


        public void Run()
        {
            input = MultiLineInputReader.ReadInput<long>("Day9/Input.txt").Result.ToArray();

            Console.WriteLine("Part 1");

            long firstNonSummedNumber = FindFirstNonSummedNumber();

            Console.WriteLine($"First Number that is not the sum of the prior 25: {firstNonSummedNumber}");
            
            
            Console.WriteLine("Part 2");

            var (smallestNumber, largestNumber) = FindExtremeValuesInContiguousRangeAddingUpToValue(firstNonSummedNumber);
            Console.WriteLine($"Encryption Weakness Number: {smallestNumber + largestNumber}");


        }

        private (long, long) FindExtremeValuesInContiguousRangeAddingUpToValue(long firstNonSummedNumber)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var rangeResult = FindSetWithLengthAddingUpToNumber(i, firstNonSummedNumber);
                if ((rangeResult?.SetLength ?? -1) > 2)
                {
                    return (rangeResult.SmallestNumber, rangeResult.LargestNumber);
                }
            }

            throw new Exception("No set found");
        }

        private RangeResult FindSetWithLengthAddingUpToNumber(int startingIndex, long firstNonSummedNumber)
        {
            long setSum = 0;
            var rangeResult = new RangeResult()
            {
                SmallestNumber = input[startingIndex] //Set this otherwise smallest number will be default(int)
            };
            
            for (int i = startingIndex; (i < input.Length && setSum < firstNonSummedNumber); i++)
            {
                rangeResult.SetLength++;
                setSum += input[i];

                if (input[i] < rangeResult.SmallestNumber)
                {
                    rangeResult.SmallestNumber = input[i];
                }

                if (input[i] > rangeResult.LargestNumber)
                {
                    rangeResult.LargestNumber = input[i];
                }

                if (setSum == firstNonSummedNumber)
                {
                    return rangeResult;
                }
            }

            return null;
        }

        private long FindFirstNonSummedNumber()
        {
            for (int i = PreambleLength; i < input.Length; i++)
            {
                var isSummed = IsNumberSummed(i);
                if (!isSummed)
                {
                    return input[i];
                }
            }

            throw new Exception("All numbers summed");
        }

        private bool IsNumberSummed(int currentIndex)
        {
            for (int j = currentIndex-PreambleLength; j < currentIndex; j++)
            {
                for (int k = currentIndex-1; k > j; k--)
                {
                    var sumsToNumber = input.SumsToNumber(j, k, currentIndex);
                    if (sumsToNumber)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }

    public class RangeResult
    {
        public int SetLength { get; set; }
        public long SmallestNumber { get; set; }
        public long LargestNumber { get; set; }
    }

    public static class Day9Extensions {
        public static bool SumsToNumber(this long[] input, int index1, int index2, int currentIndex)
        {
            if (index1 == index2) return false;

            return input[index1] + input[index2] == input[currentIndex];
        }
    }
}
