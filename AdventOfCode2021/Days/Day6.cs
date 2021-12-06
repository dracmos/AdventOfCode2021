using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
    public class Day6
    {
        //We group here the fishes by the day of life
        long[] mainFishNumberArrayGroupedByDayLife;

        //A new fish will rebord with 6 days of life
        private readonly int indexSameFishReborn = 6;

        //A new fish is born with 8 days of life
        private readonly int indexNewFishBorn = 8;
        
        public void GetResult()
        {
            GetPartOne();
            GetPartTwo();
        }

        private void GetPartOne()
        {
            PopulateInputList();
            var result = GetFishNumber(80);
        }

        private void GetPartTwo()
        {
            PopulateInputList();
            var result = GetFishNumber(256);
        }

        private void PopulateInputList()
        {
            mainFishNumberArrayGroupedByDayLife = new long[9];

            foreach (var fishDayLifeNumber in startingFishLifeDayList)
            {
                mainFishNumberArrayGroupedByDayLife[fishDayLifeNumber]++;
            }
        }

        private long GetFishNumber(int days)
        {
            for (var currentDay = 0; currentDay < days; currentDay++)
                ExecuteDayCalculation();

            long result = mainFishNumberArrayGroupedByDayLife.Sum();

            return result;
        }

        private void ExecuteDayCalculation()
        {
            var updatedFishNumberGroupedByDayLifeWithDataFromCurrentDay = new long[9];

            for (var currentIndexLifeFish = 0; currentIndexLifeFish <= 8; currentIndexLifeFish++)
                ExecuteCalculationOnDayOfLifeOfFish(updatedFishNumberGroupedByDayLifeWithDataFromCurrentDay, currentIndexLifeFish);

            mainFishNumberArrayGroupedByDayLife = updatedFishNumberGroupedByDayLifeWithDataFromCurrentDay;
        }

        private void ExecuteCalculationOnDayOfLifeOfFish(long[] updatedFishNumberGroupedByDayLifeWithDataFromCurrentDay, int currentIndexLifeFish)
        {
            //We get the number of existing fishing for the current day of life of the main
            long numberOffishForCurrentLifeDayIndex = mainFishNumberArrayGroupedByDayLife[currentIndexLifeFish];

            if (IsLastDayOfLifeForFish(currentIndexLifeFish))
                RebornAndDuplicateFishes(updatedFishNumberGroupedByDayLifeWithDataFromCurrentDay, numberOffishForCurrentLifeDayIndex);
            else
                RemoveOneDayOfLifeToFish(updatedFishNumberGroupedByDayLifeWithDataFromCurrentDay, currentIndexLifeFish, numberOffishForCurrentLifeDayIndex);
        }

        private static bool IsLastDayOfLifeForFish(int currentIndexFishLife)
        {
            return currentIndexFishLife == 0;
        }

        private void RebornAndDuplicateFishes(long[] fishNumberGroupedByDayLifeCurrentDay, long numberOffishForCurrentLifeDayIndex)
        {
            fishNumberGroupedByDayLifeCurrentDay[indexSameFishReborn] = numberOffishForCurrentLifeDayIndex;
            fishNumberGroupedByDayLifeCurrentDay[indexNewFishBorn] = numberOffishForCurrentLifeDayIndex;
        }

        private static void RemoveOneDayOfLifeToFish(long[] fishNumberGroupedByDayLifeCurrentDay, int currentIndexLifeFish, long numberOffishForCurrentLifeDayIndex)
        {
            //We shift the number of the fish removing a day of life
            fishNumberGroupedByDayLifeCurrentDay[currentIndexLifeFish - 1] += numberOffishForCurrentLifeDayIndex;
        }

        #region input

        List<int> startingFishLifeDayList = new List<int> {
5,1,1,1,3,5,1,1,1,1,5,3,1,1,3,1,1,1,4,1,1,1,1,1,2,4,3,4,1,5,3,4,1,1,5,1,2,1,1,2,1,1,2,1,1,4,2,3,2,1,4,1,1,4,2,1,4,5,5,1,1,1,1,1,2,1,1,1,2,1,5,5,1,1,4,4,5,1,1,1,3,1,5,1,2,1,5,1,4,1,3,2,4,2,1,1,4,1,1,1,1,4,1,1,1,1,1,3,5,4,1,1,3,1,1,1,2,1,1,1,1,5,1,1,1,4,1,4,1,1,1,1,1,2,1,1,5,1,2,1,1,2,1,1,2,4,1,1,5,1,3,4,1,2,4,1,1,1,1,1,4,1,1,4,2,2,1,5,1,4,1,1,5,1,1,5,5,1,1,1,1,1,5,2,1,3,3,1,1,1,3,2,4,5,1,2,1,5,1,4,1,5,1,1,1,1,1,1,4,3,1,1,3,3,1,4,5,1,1,4,1,4,3,4,1,1,1,2,2,1,2,5,1,1,3,5,2,1,1,1,1,1,1,1,4,4,1,5,4,1,1,1,1,1,2,1,2,1,5,1,1,3,1,1,1,1,1,1,1,1,1,1,2,1,3,1,5,3,3,1,1,2,4,4,1,1,2,1,1,3,1,1,1,1,2,3,4,1,1,2};

        #endregion
    }
}
