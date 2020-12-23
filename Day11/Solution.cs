using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Xml.Schema;

namespace AoC_2020.Day11
{
    public class Solution
    {
        private List<char[,]> _seatingMapHistory = new List<char[,]>();

        private char[,] _lastSeatingMap;
        private char[,] _nextSeatingMap;
        private string _mode = "AdjacentSeat";
        private int _numberOfOccupiedSeatsToVacate = 4;

        public void Run()
        {
            var input = MultiLineInputReader.ReadInputAsync<string>("Day11/Input.txt").Result;

            var seatingMap = ConvertInputToMultiDimensionalArray(input);

            Console.WriteLine("Part 1:");
            
            AddInitialSeatingMapStateToHistory(seatingMap);
            IterativelyApplyRulesUntilEquilibrium();
            var occupiedSeats = ReturnOccupiedSeatCount(_lastSeatingMap);

            Console.WriteLine($"Number of occupied seats after Equilibrium: {occupiedSeats} after {_seatingMapHistory.Count} iterations.");
            
            
            Console.WriteLine("Press Any Key To Continue with Part 2");
            Console.Read();
            
            // Reset for Part 2
            _seatingMapHistory.Clear();


            Console.WriteLine("Part 2");

            _mode = "NextVisibleSeat";
            _numberOfOccupiedSeatsToVacate = 5;

            AddInitialSeatingMapStateToHistory(seatingMap);
            IterativelyApplyRulesUntilEquilibrium();
            occupiedSeats = ReturnOccupiedSeatCount(_lastSeatingMap);

            Console.WriteLine($"Number of occupied seats after Equilibrium: {occupiedSeats} after {_seatingMapHistory.Count} iterations.");

        }

        private static char[,] ConvertInputToMultiDimensionalArray(List<string> input)
        {
            var inputWidth = input.First().Length;
            var mda = new char[inputWidth, input.Count];
            for (int columnIndex = 0; columnIndex < inputWidth; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < input.Count; rowIndex++)
                {
                    mda[columnIndex, rowIndex] = input[rowIndex][columnIndex];
                }
            }

            return mda;
        }

        private void AddInitialSeatingMapStateToHistory(char[,] seatingMap)
        {
            _seatingMapHistory.Add(seatingMap);
        }

        private void IterativelyApplyRulesUntilEquilibrium()
        {
            var isSeatingMapChanged = true;

            while (isSeatingMapChanged)
            {
                _lastSeatingMap = _seatingMapHistory.Last();
                _nextSeatingMap = (char[,])_seatingMapHistory.Last().Clone();

                WriteSeatingGridToConsole(_lastSeatingMap);

                ApplyRulesToAllSeatsInNextSeatingMap();

                if (IsSeatingMapChanged(_lastSeatingMap, _nextSeatingMap))
                {
                    _seatingMapHistory.Add(_nextSeatingMap);
                }
                else
                {
                    isSeatingMapChanged = false;
                }
            }
        }

        private void WriteSeatingGridToConsole(char[,] seatingMap)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < seatingMap.GetLength(1); y++)
            {
                for (int x = 0; x < seatingMap.GetLength(0); x++)
                {
                    sb.Append(seatingMap[x, y]);
                }

                sb.AppendLine();
            }

            Console.Clear();
            Console.Write(sb.ToString());
            Thread.Sleep(100);
        }

        private void ApplyRulesToAllSeatsInNextSeatingMap()
        {
            for (int x = 0; x < _lastSeatingMap.GetLength(0); x++)
            {
                for (int y = 0; y < _lastSeatingMap.GetLength(1); y++)
                {
                    if (!IsGridValueASeat(_lastSeatingMap[x, y]))
                    {
                        continue;
                    }
                    
                    ApplyRuleToSeatAt(x, y);
                }
            }
        }

        private bool IsGridValueASeat(char seatingMapGridValue)
        {
            return IsGridValueAnEmptySeat(seatingMapGridValue) || IsGridValueAnOccupiedSeat(seatingMapGridValue);
        }

        private bool IsGridValueAnEmptySeat(char seatingMapGridValue)
        {
            return seatingMapGridValue.Equals('L');
        }

        private bool IsGridValueAnOccupiedSeat(char seatingMapGridValue)
        {
            return seatingMapGridValue.Equals('#');
        }

        private void ApplyRuleToSeatAt(int x, int y)
        {
            if (IsGridValueAnEmptySeat(_lastSeatingMap[x, y]))
            {
                if (DoesSeatBecomeOccupied(x, y))
                {
                    _nextSeatingMap[x, y] = '#';
                }
            }

            if (IsGridValueAnOccupiedSeat(_lastSeatingMap[x, y]))
            {
                if (DoesSeatBecomeOpen(x, y))
                {
                    _nextSeatingMap[x, y] = 'L';
                }
            }
        }

        private bool DoesSeatBecomeOccupied(int x, int y)
        {
            return CountOccupiedSeats(x, y) == 0;
        }

        private bool DoesSeatBecomeOpen(int x, int y)
        {
            return CountOccupiedSeats(x, y) >= _numberOfOccupiedSeatsToVacate;
        }

        private int CountOccupiedSeats(int x, int y)
        {
            if (_mode == "AdjacentSeat")
            {
                return ValidSeatAndOccupiedReturnsOne(x - 1, y - 1)
                       + ValidSeatAndOccupiedReturnsOne(x, y - 1)
                       + ValidSeatAndOccupiedReturnsOne(x + 1, y - 1)
                       + ValidSeatAndOccupiedReturnsOne(x - 1, y)
                       + ValidSeatAndOccupiedReturnsOne(x + 1, y)
                       + ValidSeatAndOccupiedReturnsOne(x - 1, y + 1)
                       + ValidSeatAndOccupiedReturnsOne(x, y + 1)
                       + ValidSeatAndOccupiedReturnsOne(x + 1, y + 1);
            }
            else if(_mode == "NextVisibleSeat")
            {
                return NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(-1, -1))
                       + NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(0, -1))
                       + NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(1, -1))
                       + NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(-1, 0))
                       + NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(1, 0))
                       + NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(-1, 1))
                       + NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(0, 1))
                       + NextVisibleAndOccupiedSeatReturnsOne(x, y, new IntVector(1, 1));
            }

            return 0;
        }

        private int NextVisibleAndOccupiedSeatReturnsOne(int x, int y, IntVector vector)
        {
            // Apply Vector
            x += vector.X;
            y += vector.Y;
            
            if (!CoordsOnGrid(x, y))
            {
                return 0;
            }

            if (!IsGridValueASeat(_lastSeatingMap[x, y]))
            {
                return NextVisibleAndOccupiedSeatReturnsOne(x, y, vector);
            }

            return IsGridValueAnOccupiedSeat(_lastSeatingMap[x, y]) ? 1 : 0;
        }

        private int ValidSeatAndOccupiedReturnsOne(int x, int y)
        {
            if (CoordsOnGrid(x, y) && IsGridValueAnOccupiedSeat(_lastSeatingMap[x,y]))
            {
                return 1;
            }

            return 0;
        }

        private bool CoordsOnGrid(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }

            if (x >= _lastSeatingMap.GetLength(0))
            {
                return false;
            }

            if (y >= _lastSeatingMap.GetLength(1))
            {
                return false;
            }

            return true;
        }

        private bool IsSeatingMapChanged(char[,] seatingMapA, char[,] seatingMapB)
        {
            for (int x = 0; x < seatingMapA.GetLength(0); x++)
            {
                for (int y = 0; y < seatingMapA.GetLength(1); y++)
                {
                    if (seatingMapA[x,y] != seatingMapB[x,y])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private int ReturnOccupiedSeatCount(char[,] seatingMap)
        {
            var numOccupiedSeats = 0;
            for (int x = 0; x < seatingMap.GetLength(0); x++)
            {
                for (int y = 0; y < seatingMap.GetLength(1); y++)
                {
                    if (IsGridValueAnOccupiedSeat(seatingMap[x, y]))
                    {
                        numOccupiedSeats++;
                    }
                }
            }

            return numOccupiedSeats;
        }
    }

    internal class IntVector
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IntVector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
