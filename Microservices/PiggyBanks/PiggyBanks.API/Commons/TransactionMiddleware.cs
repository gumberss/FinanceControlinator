using FinanceControlinator.Common.Utils;
using Microsoft.AspNetCore.Http;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;

namespace PiggyBanks.API.Commons
{
    /// <summary>
    /// It is important to rollback something that happen in the process befere occured an error
    /// Ex: We update a data in database and when we try to send the message, an error occurs, we need to rollback the update...
    /// </summary>
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, IPiggyBankDbContext dbContext)
        {
            var methods = new[] {
                "POST",
                "PUT",
                "DELETE"
            };

            if (!methods.Contains(context.Request.Method))
            {
                _next(context).Wait();
                return Task.CompletedTask;
            }

            using (var transaction = (dbContext as PiggyBankDbContext).Database.BeginTransaction())
            {
                _next(context).Wait();

                if (IsSuccessStatusCode(context.Response.StatusCode))
                {
                    transaction.Commit();
                }
            }
            return Task.CompletedTask;
        }

        public static bool IsSuccessStatusCode(int statusCode)
            => statusCode >= 200 && statusCode <= 299;
    }
}
