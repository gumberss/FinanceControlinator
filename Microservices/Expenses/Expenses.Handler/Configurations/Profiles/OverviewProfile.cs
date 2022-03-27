﻿using AutoMapper;
using Expenses.Domain.Models.Expenses;
using Expenses.Handler.Domain.Cqrs.ExpenseOverviews;

namespace Expenses.Handler.Configurations.Profiles
{
    internal class OverviewProfile : Profile
    {
        public OverviewProfile()
        {
            CreateMap<ExpenseOverview, ExpenseOverviewDTO>();
        }
    }
}