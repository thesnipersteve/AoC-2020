using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC_2020.Day2
{
    public static class Solution
    {
        public static void Run()
        {
            var passwordAndPolicyList = MultiLineInputReader.ReadInput<string>("Day2/Input.txt").Result;

            PartOne(passwordAndPolicyList);

            PartTwo(passwordAndPolicyList);
        }

        private static void PartOne(List<string> passwordAndPolicyList)
        {
            Console.WriteLine("Part 1");

            var totalValid = 0;

            foreach (var item in passwordAndPolicyList)
            {
                var minNumberOfTimes = PasswordPolicyGetFirstNumber(item);
                var maxNumberOfTimes = PasswordPolicyGetSecondNumber(item);
                var characterRestriction = PasswordPolicyGetCharacterRestriction(item);
                var passwordPart = PasswordGetPasswordPart(item);

                bool passwordIsValid = PartOnePasswordValidator(characterRestriction, minNumberOfTimes, maxNumberOfTimes, passwordPart);
                if (passwordIsValid)
                {
                    totalValid++;
                }
            }

            Console.WriteLine($"Valid Passwords: {totalValid}");
        }

        private static void PartTwo(List<string> passwordAndPolicyList)
        {
            Console.WriteLine("\r\nPart 2");

            var totalValid = 0;

            foreach (var item in passwordAndPolicyList)
            {
                var firstIndex = PasswordPolicyGetFirstNumber(item) - 1;
                var secondIndex = PasswordPolicyGetSecondNumber(item) - 1;
                var characterRestriction = PasswordPolicyGetCharacterRestriction(item);
                var passwordPart = PasswordGetPasswordPart(item);

                bool passwordIsValid = PartTwoPasswordValidator(characterRestriction, firstIndex, secondIndex, passwordPart);
                if (passwordIsValid)
                {
                    totalValid++;
                }
            }

            Console.WriteLine($"Valid Passwords: {totalValid}");
        }

        private static int PasswordPolicyGetFirstNumber(string passwordAndPolicy)
        {
            return int.Parse(passwordAndPolicy.Substring(0, passwordAndPolicy.IndexOf("-")));
        }
        
        private static int PasswordPolicyGetSecondNumber(string passwordAndPolicy)
        {
            var startIndexOfUpperBound = passwordAndPolicy.IndexOf("-") + 1;
            var endIndexOfUpperBound = passwordAndPolicy.IndexOf(" ");
            return int.Parse(passwordAndPolicy.Substring(startIndexOfUpperBound, endIndexOfUpperBound - startIndexOfUpperBound));
        }

        private static char PasswordPolicyGetCharacterRestriction(string passwordAndPolicy)
        {
            var startIndexOfUpperBound = passwordAndPolicy.IndexOf(" ") + 1;
            return passwordAndPolicy.Substring(startIndexOfUpperBound, 1)[0];
        }

        private static string PasswordGetPasswordPart(string passwordAndPolicy)
        {
            var startIndexOfUpperBound = passwordAndPolicy.IndexOf(":") + 2;
            return passwordAndPolicy.Substring(startIndexOfUpperBound);
        }

        private static bool PartOnePasswordValidator(char characterRestriction, int minNumberOfTimes, int maxNumberOfTimes, string password)
        {
            var numberOfTimes = password.Count(p => p.Equals(characterRestriction));

            return (numberOfTimes >= minNumberOfTimes && numberOfTimes <= maxNumberOfTimes);
        }

        private static bool PartTwoPasswordValidator(char characterRestriction, int firstIndex, int secondIndex, string passwordPart)
        {
            var firstContains = IndexContainsLetter(passwordPart, firstIndex, characterRestriction);
            var secondContains = IndexContainsLetter(passwordPart, secondIndex, characterRestriction);

            return firstContains ^ secondContains;
        }

        private static bool IndexContainsLetter(string passwordPart, in int index, in char characterRestriction)
        {
            if (index >= passwordPart.Length) return false;
            return passwordPart[index] == characterRestriction;
        }

    }
}
