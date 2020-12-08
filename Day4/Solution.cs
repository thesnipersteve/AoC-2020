using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC_2020.Day4
{
    public static class Solution
    {
        public static void Run()
        {
            // did some notepad++ find-and-replace magic to get it into json
            var input = MultiLineInputReader.ReadJsonInput<List<Passport>>("Day4/Input.json").Result;

            Console.WriteLine("Part 1");
            Console.WriteLine($"Valid Passports: {input.Count(p => p.HasAllRequiredFields())}");

            Console.WriteLine("\r\nPart 2");
            Console.WriteLine($"Valid Passports: {input.Count(p => p.HasAllRequiredFields() && p.AllFieldsValid())}");
        }
    }


    public class Passport
    {
        public bool HasAllRequiredFields()
        {
            return !string.IsNullOrWhiteSpace(this.byr)
                   && !string.IsNullOrWhiteSpace(this.iyr)
                   && !string.IsNullOrWhiteSpace(this.eyr)
                   && !string.IsNullOrWhiteSpace(this.hgt)
                   && !string.IsNullOrWhiteSpace(this.hcl)
                   && !string.IsNullOrWhiteSpace(this.ecl)
                   && !string.IsNullOrWhiteSpace(this.pid);
        }

        public bool AllFieldsValid()
        {
            return IsValidBirthYear()
                && IsValidIssueYear()
                && IsValidExpirationYear()
                && IsValidHeight()
                && IsValidHairColor()
                && IsValidEyeColor()
                && IsValidPassportId();
        }

        private bool IsValidBirthYear()
        {
            var intYear = int.Parse(byr);
            return (intYear >= 1920 && intYear <= 2002);
        }

        private bool IsValidIssueYear()
        {
            var intYear = int.Parse(iyr);
            return (intYear >= 2010 && intYear <= 2020);
        }
        private bool IsValidExpirationYear()
        {
            var intYear = int.Parse(eyr);
            return (intYear >= 2020 && intYear <= 2030);
        }

        private bool IsValidHeight()
        {
            if (hgt.Contains("cm"))
            {
                return IsValidCmHeight();
            }
            else if (hgt.Contains("in"))
            {
                return IsValidInchHeight();
            }
            return false;
        }

        private bool IsValidCmHeight()
        {
            var intCmHeight = int.Parse(hgt.Replace("cm", "", true, CultureInfo.CurrentCulture).Trim());
            return intCmHeight >= 150 && intCmHeight <= 193;
        }

        private bool IsValidInchHeight()
        {
            var intCmHeight = int.Parse(hgt.Replace("in", "", true, CultureInfo.CurrentCulture).Trim());
            return intCmHeight >= 59 && intCmHeight <= 76;
        }

        private bool IsValidHairColor()
        {
            var pattern = new Regex("^#[0-9a-f]{6}$");
            return pattern.IsMatch(hcl);
        }

        private bool IsValidEyeColor()
        {
            var validColors = new List<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            return validColors.Any(c => c.Equals(ecl));
        }

        private bool IsValidPassportId()
        {
            var pattern = new Regex("^[0-9]{9}$");
            return pattern.IsMatch(pid);
        }


        public string byr { get; set; }
        public string iyr { get; set; }
        public string eyr { get; set; }
        public string hgt { get; set; }
        public string hcl { get; set; }
        public string ecl { get; set; }
        public string pid { get; set; }
        public string cid { get; set; }
    }
}
