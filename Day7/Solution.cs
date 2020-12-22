using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC_2020.Day7
{
    public class Solution
    {
        // <nodeName, pointersToOuterBags>
        Dictionary<string, List<string>> _parentGraph = new Dictionary<string, List<string>>();
        Dictionary<string, BagNode> _graph = new Dictionary<string, BagNode>();

        public void Run()
        {
            var input = MultiLineInputReader.ReadInputAsync<string>("Day7/Input.txt").Result;
            var bagRules = input.Select(DeserializeBagRule);

            Console.WriteLine("Part 1");
            foreach (var bagRule in bagRules)
            {
                AddBagRuleToParentGraph(bagRule);
            }

            var allParentBags = new List<string>();
            FindAllParentBags("shiny gold", allParentBags);
            Console.WriteLine($"Number of Parent Bags: {allParentBags.Count}"); // First Time Right!

            Console.WriteLine("Part 2");
            foreach (var bagRule in bagRules)
            {
                AddNewBagNode(bagRule);
            }

            int numberNestedBags = CountAllNestedBags("shiny gold");
            Console.WriteLine($"Number of Nested Bags: {numberNestedBags - 1}");
        }

        private int CountAllNestedBags(string bagColor)
        {
            var bagNode = GetBagNode(bagColor);

            if (!bagNode.Edges.Any())
            {
                return 1;
            }

            // Todo: Think about tail recursive
            return bagNode.Edges.Sum(e => CountAllNestedBags(e.BagColor) * e.Quantity) + 1;
        }

        private void AddNewBagNode(BagRule bagRule)
        {
            //use the color as an index so we don't need to link objects
            var newBagNode = new BagNode()
            {
                BagColor = bagRule.BagColor,
                Edges = bagRule.Contains
            };
            _graph.Add(bagRule.BagColor, newBagNode);
        }

        private BagNode GetBagNode(string bagColor)
        {
            if (_graph.TryGetValue(bagColor, out var bagNode))
            {
                return bagNode;
            }

            return null;
        }

        private BagRule DeserializeBagRule(string inputLine)
        {
            // Matches `(drab tan) bags contain (4 clear gold bags, 3 plaid aqua bags).`
            var regex = new Regex(@"^(?<OuterBagColor>[\w\s]+) bags contain (?<InnerBags>([\d]+ [\w\s]+ bags?[, ]*)+|no other bags)+.$");
            var matches = regex.Matches(inputLine);

            var outerBagColor = matches.First().Groups.Values.First(g => g.Name.Equals("OuterBagColor")).Value;
            var innerBagsSubstring = matches.First().Groups.Values.First(g => g.Name.Equals("InnerBags")).Value;

            var innerBags = GetInnerBagsFromRuleSubString(innerBagsSubstring);

            return new BagRule()
            {
                BagColor = outerBagColor,
                Contains = innerBags.ToList()
            };
        }

        private IEnumerable<QuantityOfColoredBag> GetInnerBagsFromRuleSubString(string innerBagsSubstring)
        {
            if (innerBagsSubstring == "no other bags") return new List<QuantityOfColoredBag>();

            return innerBagsSubstring
                .Split(",")
                .Select(innerBag =>
                {
                    // Matches: ` (3) (faded gold) bags`
                    var regex = new Regex(@"^\s?(?<Quantity>[\d]+) (?<BagColor>[\w\s]+) bags?$");
                    var matches = regex.Matches(innerBag);

                    return new QuantityOfColoredBag()
                    {
                        Quantity = int.Parse(matches.First().Groups.Values.First(g => g.Name.Equals("Quantity")).Value),
                        BagColor = matches.First().Groups.Values.First(g => g.Name.Equals("BagColor")).Value
                    };
                });
        }
        
        private void AddBagRuleToParentGraph(BagRule bagRule)
        {
            foreach (var innerBag in bagRule.Contains)
            {
                AddInnerBagToGraphWithParent(innerBag, bagRule.BagColor);
            }
        }

        private void AddInnerBagToGraphWithParent(QuantityOfColoredBag innerBag, string parentBag)
        {
            var innerBagParents = GetParentBagsForNodeAddIfNotFound(innerBag.BagColor);

            if (!innerBagParents.Contains(parentBag))
            {
                innerBagParents.Add(parentBag);
            }
        }

        private List<string> GetParentBagsForNodeAddIfNotFound(string bagColor)
        {
            return GetParentBagsForNode(bagColor) ?? AddBagToGraph(bagColor);
        }

        private List<string> GetParentBagsForNode(string bagColor)
        {
            return _parentGraph.TryGetValue(bagColor, out var parentBags) ? parentBags : null;
        }

        private List<string> AddBagToGraph(string bagColor)
        {
            var newParentBagList = new List<string>();
            _parentGraph.Add(bagColor, newParentBagList);
            return newParentBagList;
        }

        private void FindAllParentBags(string currentBag, List<string> foundBags)
        {
            var parentBagsForNode = GetParentBagsForNode(currentBag);

            if (parentBagsForNode == null)
            {
                return;
            }

            var newlyDiscoveredBags = parentBagsForNode.Except(foundBags).ToList();
            foundBags.AddRange(newlyDiscoveredBags);

            foreach (var newlyDiscoveredBag in newlyDiscoveredBags)
            {
                FindAllParentBags(newlyDiscoveredBag, foundBags);
            }
        }
    }

    public class BagRule
    {
        public string BagColor { get; set; }
        public List<QuantityOfColoredBag> Contains { get; set; }
    }

    public class QuantityOfColoredBag
    {
        public string BagColor { get; set; }
        public int Quantity { get; set; }
    }

    public class BagNode
    {
        public string BagColor { get; set; }
        public List<QuantityOfColoredBag> Edges { get; set; }
    }

}
