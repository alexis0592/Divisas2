using System;
using SQLite.Net.Attributes;

namespace Taller2Divisas.Models
{
    public class LastSearch
    {
        [PrimaryKey]
        public int LastSearchId { get; set; }

        public string CodeSourceRateSearch { get; set; }

        public string TargetRateSearch { get; set; }
    }
}
