using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Repositories;
using PiggyBanks.Domain.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PiggyBanks.Application.AppServices
{
    public class TransferAppService : ITransferAppService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IPiggyBankRepository _piggyBankRepository;

        public TransferAppService(
            ITransferRepository transferRepository,
            IPiggyBankRepository piggyBankRepository
            )
        {
            _transferRepository = transferRepository;
            _piggyBankRepository = piggyBankRepository;
        }

        public async Task<Result<Transfer, BusinessException>> RegisterTransfer(Transfer transfer)
        {
            var (destinationId, sourceId, amount) = transfer;

            var piggyBanksIds = new[] { destinationId, sourceId };

            var piggyBanks = await _piggyBankRepository.GetAllAsync(null, x => piggyBanksIds.Contains(x.Id));

            if (piggyBanks.IsFailure) return piggyBanks.Error;

            var source = piggyBanks.Value.Find(x => x.Id == transfer.SourceId);
            var destination = piggyBanks.Value.Find(x => x.Id == transfer.DestinationId);

            if (source is null || destination is null)
            {
                return new BusinessException(HttpStatusCode.BadRequest, "From or to is null"); //todo: change error message, please! (use localization)
            }

            if (!source.CanWithdraw(amount))
            {
                return new BusinessException(HttpStatusCode.BadRequest, "There is no much money in the Piggy Bank to withdraw"); //todo: change error message, please! (use localization)
            }

            source.Withdraw(amount);
            destination.AddMoney(amount);

            await _piggyBankRepository.UpdateAsync(source);
            await _piggyBankRepository.UpdateAsync(destination);

            transfer.CreatedDate = DateTime.Now;

            return await _transferRepository.AddAsync(transfer);
        }
    }
}
