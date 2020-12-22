using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020.Day6
{
    public static class Solution
    {
        public static void Run()
        {
            Console.WriteLine("Part 1:");
            Console.WriteLine("Challenge: Maximize Speed & Minimize Big O Complexity");

            var input = MultiLineInputReader.ReadInputAsync<string>("Day6/Input.txt").Result;

            var stopWatchPart1 = new Stopwatch();
            stopWatchPart1.Start();
            var customsCounter = new CustomsCounter();
            Console.WriteLine($"Items Declared: {customsCounter.GetTotalDeclarations(input)}");
            stopWatchPart1.Stop();
            Console.WriteLine($"Part 1a Run Time: {stopWatchPart1.ElapsedMilliseconds}");
            // Average run time of 4msec

            // Big O analysis:
            // O(m + n)
            // m = #lines 
            // n = total number of declarations
            // m <= 2n worst case so can be omitted 
            // therefore O(n) where n is number of declarations

            //Another approach to see what sort of time savings were made:
            var stopWatchPart2 = new Stopwatch();
            stopWatchPart2.Start();
            var customsCounter2 = new LessEfficientCustomsCounter();
            Console.WriteLine($"Items Declared: {customsCounter2.GetTotalDeclarations(input)}");
            stopWatchPart2.Stop();
            Console.WriteLine($"Part 1b Run Time: {stopWatchPart2.ElapsedMilliseconds}");
            // Average run time of 7msec


            Console.WriteLine("Part 2:");
            var customsCounterPart2 = new CustomsCounterPart2();
            Console.WriteLine($"Items Declared: {customsCounterPart2.GetTotalDeclarations(input)}");
        }
    }

    public class CustomsCounter
    {
        private int _grandTotal = 0;
        private int _groupTotal = 0;
        private bool[] _groupDeclarations = new bool[26];

        public int GetTotalDeclarations(List<string> input)
        {
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    AddToGrandTotalAndResetGroup();
                }
                else
                {
                    EvaluateDeclarationsForPerson(line);
                }
            }

            // Handle last one
            AddToGrandTotalAndResetGroup();

            return _grandTotal;
        }

        private void AddToGrandTotalAndResetGroup()
        {
            _grandTotal += _groupTotal;
            _groupTotal = 0;
            _groupDeclarations = new bool[26];
        }

        private void EvaluateDeclarationsForPerson(string line)
        {
            foreach (var zeroBasedCharacterIndex in line.Select(ShiftCharacterToZeroIndex))
            {
                AddToGroupTotalIfNotAlreadyCounted(zeroBasedCharacterIndex);
            }
        }

        private void AddToGroupTotalIfNotAlreadyCounted(int zeroBasedCharacterIndex)
        {
            if (_groupDeclarations[zeroBasedCharacterIndex] == false)
            {
                _groupDeclarations[zeroBasedCharacterIndex] = true;
                _groupTotal++;
            }
        }

        private int ShiftCharacterToZeroIndex(char character)
        {
            return character - 97;
        }
    }

    public class LessEfficientCustomsCounter
    {
        public int GetTotalDeclarations(List<string> input)
        {
            var total = 0;

            var groups = GetGroups(input);
            foreach (var group in groups)
            {
                total += GetGroupDeclarationCount(group);
            }

            return total;
        }
        
        private IEnumerable<List<string>> GetGroups(List<string> input)
        {
            var groups = new List<List<string>>();
            var groupDeclarations = new List<string>();
            
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    groups.Add(groupDeclarations);
                    groupDeclarations = new List<string>();
                }
                else
                {
                    groupDeclarations.Add(line);
                }
            }

            // Add the last one
            groups.Add(groupDeclarations);

            return groups;
        }

        private int GetGroupDeclarationCount(List<string> group)
        {
            var groupDeclarations = new Dictionary<char, bool>();
            foreach (var individualDeclarations in group)
            {
                foreach (var declaration in individualDeclarations)
                {
                    if (!groupDeclarations.ContainsKey(declaration))
                    {
                        groupDeclarations.Add(declaration, true);
                    }
                }
            }

            return groupDeclarations.Count;
        }
    }

    public class CustomsCounterPart2
    {
        private int _grandTotal = 0;
        private int _groupMembers = 0;
        private int[] _groupDeclarations = new int[26];

        public int GetTotalDeclarations(List<string> input)
        {
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    AddToGrandTotalAndResetGroup();
                }
                else
                {
                    EvaluateDeclarationsForPerson(line);
                }
            }

            // Handle last one
            AddToGrandTotalAndResetGroup();

            return _grandTotal;
        }

        private void AddToGrandTotalAndResetGroup()
        {

            var groupTotal = CalculateGroupTotal();
            _grandTotal += groupTotal;
            _groupDeclarations = new int[26];
            _groupMembers = 0;
        }

        private int CalculateGroupTotal()
        {
            var groupTotal = 0;
            foreach (var groupDeclaration in _groupDeclarations)
            {
                if (groupDeclaration == _groupMembers)
                {
                    groupTotal++;
                }
            }

            return groupTotal;
        }

        private void EvaluateDeclarationsForPerson(string line)
        {
            _groupMembers++;

            foreach (var zeroBasedCharacterIndex in line.Select(ShiftCharacterToZeroIndex))
            {
                AddToGroupTotal(zeroBasedCharacterIndex);
            }
        }

        private void AddToGroupTotal(int zeroBasedCharacterIndex)
        {
            _groupDeclarations[zeroBasedCharacterIndex]++;
        }

        private int ShiftCharacterToZeroIndex(char character)
        {
            return character - 97;
        }
    }
}
