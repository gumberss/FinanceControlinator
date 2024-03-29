using CleanHandling;
using Microsoft.Extensions.Logging;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Repositories;
using PiggyBanks.Domain.Interfaces.Validators;
using PiggyBanks.Domain.Localizations;
using PiggyBanks.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PiggyBanks.Application.AppServices
{
    public class PiggyBankAppService : IPiggyBankAppService
    {
        private readonly IPiggyBankRepository _piggyBankRepository;
        private readonly ILocalization _localization;
        private readonly ILogger<IPiggyBankAppService> _logger;
        private readonly IPiggyBankValidator _piggyBankValidator;

        public PiggyBankAppService(
                IPiggyBankRepository piggyBankRepository
                , ILocalization localization
                , ILogger<IPiggyBankAppService> logger
                , IPiggyBankValidator piggyBankValidator
            )
        {
            _piggyBankRepository = piggyBankRepository;
            _localization = localization;
            _logger = logger;
            _piggyBankValidator = piggyBankValidator;
        }

        public async Task<Result<List<PiggyBank>, BusinessException>> GetAllPiggyBanks()
        {
            return await _piggyBankRepository.GetAllAsync();
        }

        public async Task<Result<List<PiggyBank>, BusinessException>> RegisterPayment(Invoice invoice)
        {
            var invoiceItemIds = invoice.Items.Select(x => x.ExpenseId).ToList();

            var piggyBanks = await _piggyBankRepository.GetAllAsync(where: x => invoiceItemIds.Contains(x.Id));

            if (piggyBanks.IsFailure) return piggyBanks.Error;

            foreach (var item in invoice.Items)
            {
                var piggyBankPaid = piggyBanks.Value.Find(x => x.Id == item.ExpenseId);

                if (piggyBankPaid is null) continue;

                piggyBankPaid.InstallmentPaid(item.InstallmentCost);
            }

            return piggyBanks;
        }

        public async Task<Result<PiggyBank, BusinessException>> RegisterPiggyBank(PiggyBank piggyBank)
        {
            var validationResult = await _piggyBankValidator.ValidateAsync(piggyBank);

            if (!validationResult.IsValid)
            {
                var errorDatas = validationResult.Errors.Select(x => new ErrorData(x.ErrorMessage, x.PropertyName));
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorDatas);
                _logger.LogInformation(exception.Log());
                return exception;
            }

            var existsByTitle = await _piggyBankRepository.ExistsPiggyBankByTitle(piggyBank.Title);

            if (existsByTitle.IsFailure) return existsByTitle.Error;

            if (existsByTitle)
                return new BusinessException(HttpStatusCode.BadRequest, _localization.PIGGY_BANK_ALREADY_EXISTS_BY_TITLE);

            return await _piggyBankRepository.AddAsync(piggyBank);
        }

        public async Task<Result<PiggyBank, BusinessException>> Save(decimal value)
        {
            var defaultPiggyBankResult = await _piggyBankRepository.GetAsync(where: x => x.Default);

            if (defaultPiggyBankResult.IsFailure) return defaultPiggyBankResult.Error;

            var defaultPiggyBank = defaultPiggyBankResult.Value;

            if (defaultPiggyBank is null)
            {
                defaultPiggyBank = new PiggyBank()
                    .AsDefault()
                    .AddMoney(value);

                return await _piggyBankRepository.AddAsync(defaultPiggyBank);
            }
            else
            {
                defaultPiggyBank.AddMoney(value);
                return await _piggyBankRepository.UpdateAsync(defaultPiggyBank);
            }
        }
    }
}
