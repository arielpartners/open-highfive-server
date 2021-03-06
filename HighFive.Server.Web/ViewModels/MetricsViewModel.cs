﻿using HighFive.Server.Services.Utils;
using System.Collections.Generic;

namespace HighFive.Server.Web.ViewModels
{
    public class MetricsViewModel
    {
        public IList<GroupedMetric> Week { get; set; }
        public IList<GroupedMetric> Month { get; set; }
        public IList<GroupedMetric> Year { get; set; }
        public IList<GroupedMetric> ToDate { get; set; }

        public MetricsViewModel()
        {
            Week = new List<GroupedMetric>();
            Month = new List<GroupedMetric>();
            Year = new List<GroupedMetric>();
            ToDate = new List<GroupedMetric>();
        }
    }
}
