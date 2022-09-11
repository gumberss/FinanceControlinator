using CleanHandling;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Repositories;
using PiggyBanks.Domain.Models;
using System.Threading.Tasks;

namespace PiggyBanks.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        public InvoiceAppService(
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository
            )
        {
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task<Result<Invoice, BusinessException>> RegisterPaid(Invoice paidInvoice)
        {
            var registeredInvoice = await _invoiceRepository.GetByIdAsync(paidInvoice.Id);

            if (registeredInvoice.IsFailure) return registeredInvoice.Error;

            if (registeredInvoice.Value is not null)
            {
                var deleteItemsResult = await _invoiceItemRepository.DeleteAsync(registeredInvoice.Value.Items);

                if (deleteItemsResult.IsFailure) return deleteItemsResult.Error;

                var addNewItemsResult = await _invoiceItemRepository.AddAsync(paidInvoice.Items);

                if (addNewItemsResult.IsFailure) return addNewItemsResult.Error;

                registeredInvoice.Value
                    .ChangeDueDate(paidInvoice.DueDate)
                    .ReplaceItems(paidInvoice.Items);

                return await _invoiceRepository.UpdateAsync(registeredInvoice.Value);
            }

            return await _invoiceRepository.AddAsync(paidInvoice);
        }
    }
}
