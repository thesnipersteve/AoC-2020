using System;
using System.Collections.Generic;
using System.Text;

namespace AoC_2020.Day1
{
    public static class Solution
    {
        public static void Run()
        {
            var expenseItems = MultiLineInputReader.ReadInput<int>("Day1/Input.txt").Result;

            Console.WriteLine("Part 1:");
            Console.WriteLine($"Total {FindTwoNumbersThatAddUpTo2020(expenseItems)}");

            Console.WriteLine("\r\nPart 2:");
            Console.WriteLine($"Total {FindThreeNumbersThatAddUpTo2020(expenseItems)}");
        }

        private static int FindTwoNumbersThatAddUpTo2020(List<int> expenseItems)
        {
            for (int i = 0; i < expenseItems.Count; i++)
            {
                for (int j = 0; j < expenseItems.Count; j++)
                {
                    if (j == i) continue; //don't compare against self
                    if (expenseItems[i] + expenseItems[j] == 2020)
                    {
                        return expenseItems[i] * expenseItems[j];
                    }
                }
            }

            return -1;
        }

        private static int FindThreeNumbersThatAddUpTo2020(List<int> expenseItems)
        {
            for (int i = 0; i < expenseItems.Count; i++)
            {
                for (int j = 0; j < expenseItems.Count; j++)
                {
                    for (int k = 0; k < expenseItems.Count; k++)
                    {
                        if (k == i) continue; //don't compare against self
                        if (j == i) continue; //don't compare against self
                        if (j == k) continue; //don't compare against self
                        if (expenseItems[i] + expenseItems[j] + expenseItems[k] == 2020)
                        {
                            return expenseItems[i] * expenseItems[j] * expenseItems[k];
                        }
                    }
                }
            }

            return -1;
        }
    }
}
