using PiggyBanks.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;


namespace PiggyBanks.Handler.Domain.Cqrs.Events
{
    public class GetMonthPiggyBanksQuery : IRequest<Result<List<PiggyBank>, BusinessException>>
    {
    }
}
