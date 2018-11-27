using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Forum.Models
{
    public class Rate
    {
        public static List<Rate> rateList = new List<Rate>();

        public int rating;
        public string userId;

        public static double calculateAverageRating(List<Rate> rateList)
        {
            if (rateList.Count == 0)
            {
                return 0;
            }

            var totalNumberOfRatings = rateList.Count;
            var totalRating = 0;

            foreach (var item in rateList)
            {
                totalRating += item.rating;
            }

            var averageRating = (double)totalRating / totalNumberOfRatings;

            return Math.Round(averageRating, 2);
        }

        public static int getUserRating(List<Rate> rateList, string userId)
        {
            foreach (var item in rateList)
            {
                if (item.userId == userId)
                {
                    return item.rating;
                }
            }
            return 0;
        }
    }
}