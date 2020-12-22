using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace AoC_2020.Day10
{
    public class Solution
    {
        private List<int> _adapters;
        private int _deviceAdapterJoltage;
        private SolutionCache<int,long> _solutionCache;

        public void Run()
        {
            _solutionCache = new SolutionCache<int, long>();
            
            _adapters = MultiLineInputReader.ReadInputAsync<int>("Day10/Input.txt").Result;

            SortAndAddDevice();

            
            Console.WriteLine("Part 1:");

            var distribution = BuildAdapterChainUsingAllAdaptersInOrder();
            var puzzleAnswer = distribution[1] * distribution[3];
            
            Console.WriteLine($"Number of 1-jolt differences multiplied by the number of 3-jolt differences: {puzzleAnswer}");
            

            Console.WriteLine("\r\nPart 2:");

            _deviceAdapterJoltage = GetDeviceAdapterJoltsRatingFromSortedList();
            AddChargingOutletJoltage();
            var adapterArrangements = SupportedAdapterArrangements(0);
            
            Console.WriteLine($"Distinct arrangements: {adapterArrangements}");
        }
        
        private void SortAndAddDevice()
        {
            _adapters.Sort();

            AddDeviceToAdapterList();
        }

        private void AddDeviceToAdapterList()
        {
            var deviceAdapterJoltsRating = GetDeviceAdapterJoltsRatingFromSortedList();
            _adapters.Add(deviceAdapterJoltsRating);
        }

        private int GetDeviceAdapterJoltsRatingFromSortedList()
        {
            return _adapters.Last() + 3;
        }

        private int[] BuildAdapterChainUsingAllAdaptersInOrder()
        {
            var priorAdapterOrOutletJoltage = 0;
            var distribution = new int[4] {0, 0, 0, 0};

            foreach (var adapterJoltage in _adapters)
            {
                distribution[adapterJoltage - priorAdapterOrOutletJoltage]++;
                priorAdapterOrOutletJoltage = adapterJoltage;
            }

            return distribution;
        }
        
        private void AddChargingOutletJoltage()
        {
            _adapters = _adapters.Prepend(0).ToList();
        }

        private long SupportedAdapterArrangements(int inputJoltage)
        {
            if (_solutionCache.HasSolutionStored(inputJoltage))
            {
                return _solutionCache.GetSolutionFor(inputJoltage);
            }

            if (inputJoltage == _deviceAdapterJoltage)
            {
                return 1;
            }

            if (inputJoltage > _deviceAdapterJoltage)
            {
                return 0;
            }

            if (!AdapterExistsWithJoltageRating(inputJoltage))
            {
                return 0;
            }

            var supportedAdapterArrangements =
                SupportedAdapterArrangements(inputJoltage + 1) +
                SupportedAdapterArrangements(inputJoltage + 2) +
                SupportedAdapterArrangements(inputJoltage + 3);

            _solutionCache.AddSolutionFor(inputJoltage, supportedAdapterArrangements);

            return supportedAdapterArrangements;
        }

        private bool AdapterExistsWithJoltageRating(int adapterJoltage)
        {
            return _adapters.Any(a => a.Equals(adapterJoltage));
        }
    }
}
