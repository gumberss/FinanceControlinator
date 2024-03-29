﻿using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Payments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        Task<Result<List<Invoice>, BusinessException>> Change(List<Invoice> changedInvoices);
    }
}
