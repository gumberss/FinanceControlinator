using FinanceControlinator.Common.Localizations;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;
using Payments.Data.Repositories;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILocalization _localization;
        private readonly ILogger<IPaymentAppService> _logger;
        private readonly IAsyncDocumentSession _documentSession;

        public InvoiceAppService(
                IAsyncDocumentSession documentSession
                , IInvoiceRepository invoiceRepository
                , ILocalization localization
                , ILogger<IPaymentAppService> logger
            )
        {
            _documentSession = documentSession;
            _invoiceRepository = invoiceRepository;
            _localization = localization;
            _logger = logger;
        }
        //Registar no injetor de dependencia
    }
}
