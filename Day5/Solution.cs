using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2020.Day5
{
    public static class Solution
    {
        public static void Run()
        {
            var input = MultiLineInputReader.ReadInputAsync<string>("Day5/Input.txt").Result;


            Console.WriteLine($"Part 1");
            var maxSeatId = input.Select(GetSeatId).Max();
            Console.WriteLine($"Max Seat Id: {maxSeatId}");

            Console.WriteLine("\r\nPart 2: ");
            var sortedSeats = input.Select(GetSeatId).OrderBy(x => x).ToList();
            var firstMissingSeat = FindFirstMissingSeat(sortedSeats);
            Console.WriteLine($"First Missing Seat: {firstMissingSeat}");
        }

        private static int GetSeatId(string seatingPattern)
        {
            var uniformSeatingPattern = ConvertSeatingPatternIntoLowerUpperNotation(seatingPattern);

            var row = FindRowPosition(GetRowSeatingString(uniformSeatingPattern));
            var column = FindColumnPosition(GetColumnSeatingString(uniformSeatingPattern));

            return SeatIdCalculation(row, column);
        }

        private static string ConvertSeatingPatternIntoLowerUpperNotation(string seatingPattern)
        {
            // Converts patterns like `FBFBBFFRLR` into `LULUULLULU`
            return seatingPattern
                .Replace("F", "L")
                .Replace("L", "L")
                .Replace("B", "U")
                .Replace("R", "U");
        }

        private static string GetRowSeatingString(string uniformSeatingPattern)
        {
            return uniformSeatingPattern.Substring(0, 7);
        }

        private static int FindRowPosition(string uniformSeatingPattern)
        {
            return FindPosition(uniformSeatingPattern, 128);
        }

        private static string GetColumnSeatingString(string uniformSeatingPattern)
        {
            return uniformSeatingPattern.Substring(7, 3);
        }

        private static int FindColumnPosition(string uniformSeatingPattern)
        {

            return FindPosition(uniformSeatingPattern, 8);
        }

        private static int FindPosition(string uniformSeatingPattern, int size)
        {
            var lowerBound = 0;
            var upperBound = size - 1;

            foreach (var spaceIdentifier in uniformSeatingPattern)
            {
                var midpoint = FindMidpoint(lowerBound, upperBound);
                if (spaceIdentifier == 'L')
                {
                    upperBound = midpoint - 1;
                }
                else
                {
                    lowerBound = midpoint;
                }
            }

            // At this point lower and upper bound should be equal
            return lowerBound;
        }

        private static int FindMidpoint(int lowerBound, int upperBound)
        {
            // `upperBound + 1` : deal with 0-based indexing
            // `- lowerBound` : get the difference (or range)
            // ` / 2` : get the mid-point
            // `+ lowerBound` : add the lowerBound back to get the index
            return (((upperBound + 1) - lowerBound) / 2) + lowerBound;
        }

        private static int SeatIdCalculation(int rowNumber, int columnNumber)
        {
            return rowNumber * 8 + columnNumber;
        }

        private static int FindFirstMissingSeat(List<int> sortedSeats)
        {
            var priorSeatNumber = sortedSeats.First() - 1;
            foreach (var seatNumber in sortedSeats)
            {
                if (seatNumber != ++priorSeatNumber)
                {
                    return seatNumber - 1;
                }
            }

            return -1;
        }
    }

}
