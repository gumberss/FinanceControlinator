using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Data.Repositories;
using PiggyBanks.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PiggyBanks.Domain.Localizations;

namespace PiggyBanks.Application.AppServices
{
    public class PiggyBankAppService : IPiggyBankAppService
    {
        private readonly IPiggyBankDbContext _piggyBankDbContext;
        private readonly IPiggyBankRepository _piggyBankRepository;
        private readonly ILocalization _localization;
        private readonly ILogger<IPiggyBankAppService> _logger;

        public PiggyBankAppService(
                PiggyBankDbContext piggyBankDbContext
                , IPiggyBankRepository piggyBankRepository
                , ILocalization localization
                , ILogger<IPiggyBankAppService> logger
            )
        {
            _piggyBankDbContext = piggyBankDbContext;
            _piggyBankRepository = piggyBankRepository;
            _localization = localization;
            _logger = logger;
        }

        public async Task<Result<List<PiggyBank>, BusinessException>> GetAllPiggyBanks()
        {
            return Result.From(null as List<PiggyBank>);
        }
        
        public async Task<Result<List<PiggyBank>, BusinessException>> GetMonthPiggyBanks()
        {
            return Result.From(null as List<PiggyBank>);
        }

        public async Task<Result<List<PiggyBank>, BusinessException>> GetLastMonthPiggyBanks()
        {
            return Result.From(null as List<PiggyBank>);
        }

        public async Task<Result<PiggyBank, BusinessException>> RegisterPiggyBank(PiggyBank piggyBank)
        {
            return Result.From(null as PiggyBank);
        }
    }
}
