﻿using System;

namespace FinanceControlinator.Events.PiggyBanks.DTOs
{
    public class PiggyBankDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Version { get; }

        public String Title { get; set; }

        public String Description { get; set; }

        public int Type { get; set; }

        public DateTime GoalDate { get; set; }

        public decimal GoalValue { get; set; }

        public decimal SavedValue { get; set; }

        public DateTime StartDate { get; set; }

        public bool Default { get; set; }
    }
}
