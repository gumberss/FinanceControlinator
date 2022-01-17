using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using PiggyBanks.Domain.Models;
using System.Collections.Generic;

namespace PiggyBanks.Handler.Domain.Cqrs.Events
{
    public class GetAllPiggyBanksQuery : IRequest<Result<List<PiggyBank>, BusinessException>>
    {
    }
}
