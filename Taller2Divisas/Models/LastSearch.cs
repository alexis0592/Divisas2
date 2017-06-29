using System;
using SQLite.Net.Attributes;

namespace Taller2Divisas.Models
{
    public class LastSearch
    {
        [PrimaryKey]
        public int LastSearchId { get; set; }

        public double CodeSourceRateSearch { get; set; }

        public double TargetRateSearch { get; set; }

        public override int GetHashCode()
        {
            return LastSearchId;
        }
    }
}
