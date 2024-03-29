﻿using System.Collections.Generic;

namespace RedLife.Core.Statistics.DonorStatistics
{
    public interface IDonorStatisticsManager
    {
        public int GetNrOfDonations(long donorId);
        public int GetNrOfTransfusions(long donorId);
        public double GetDonatedQuantity(long donorId);
        public double GetTransfusionsQuantity(long donorId);
        public List<int> GetDonationTypes(long donorId);
        public List<int> GetTransfusionsPerMonth(long donorId);
        public int GetNrOfTotalAppTransfusions();

    }
}
