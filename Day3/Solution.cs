using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2020.Day3
{
    public static class Solution
    {
        public static void Run()
        {
            var input = MultiLineInputReader.ReadInputAsync<string>("Day3/Input.txt").Result;

            char[,] map = ConvertInputToMultiDimensionalArray(input);

            PartOne(map);

            PartTwo(map);
        }

        private static void PartOne(char[,] map)
        {
            Console.WriteLine("Part 1");

            var treesEncountered = CheckSlope(map, 3, 1);

            Console.WriteLine($"Trees Encountered Part 1: {treesEncountered}"); 
        }

        private static void PartTwo(char[,] map)
        {
            Console.WriteLine("\r\nPart 2");

            long answer = 1;
            answer *= CheckSlope(map, 1, 1);
            answer *= CheckSlope(map, 3, 1);
            answer *= CheckSlope(map, 5, 1);
            answer *= CheckSlope(map, 7, 1);
            answer *= CheckSlope(map, 1, 2);

            Console.WriteLine($"Trees Encountered (multiplied) Part 2: {answer}");
        }

        private static int CheckSlope(char[,] map, int right, int down)
        {
            var point = new Point(0, 0);
            var treesEncountered = 0;

            while (map.CanMoveDown(point, down))
            {
                map.MoveRightWithLoopingForDistance(point, right);
                map.MoveDown(point, down);

                if (map.IsTreeAtLocation(point))
                {
                    treesEncountered++;
                }
            }

            return treesEncountered;
        }

        // TODO: Refactor as this is [y,x] rather than [x,y] 
        // See Day 11 for an example of better usage
        private static char[,] ConvertInputToMultiDimensionalArray(List<string> input)
        {
            var inputWidth = input.First().Length;
            var mda = new char[input.Count, inputWidth];
            for (int row = 0; row < input.Count; row++)
            {
                for (int column = 0; column < inputWidth; column++)
                {
                    mda[row, column] = input[row][column];
                }
            }

            return mda;
        }
    }

    public static class MultiDimensionalArrayExtensions
    {
        public static void MoveRightWithLoopingForDistance(this char[,] map, Point point, int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                MoveRightWithLooping(map, point);
            }
        }

        public static void MoveRightWithLooping(this char[,] map, Point point)
        {
            if (++point.Column >= map.GetLength(1))
            {
                point.Column = 0;
            }
        }

        public static bool CanMoveDown(this char[,] map, Point point, int distance = 1)
        {
            return (map.GetLength(0) > point.Row + distance);
        }

        public static void MoveDown(this char[,] map, Point point, int distance = 1)
        {
            if (!CanMoveDown(map, point, distance))
            {
                throw new Exception("Tried to move beyond end of map");
            }
            point.Row += distance;
        }

        public static bool IsTreeAtLocation(this char[,] map, Point point)
        {
            return map[point.Row, point.Column] == '#';
        }
    }

    public class Point
    {
        public Point(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }
        public int Column { get; set; }
    }
}
