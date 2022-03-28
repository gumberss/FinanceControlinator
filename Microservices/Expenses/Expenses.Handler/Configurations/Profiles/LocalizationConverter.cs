using AutoMapper;
using Expenses.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Handler.Configurations.Profiles
{
    internal class LocalizationConverter : IValueConverter<ExpenseType, string>
    {
        public LocalizationConverter()
        {

        }
        public string Convert(ExpenseType sourceMember, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
