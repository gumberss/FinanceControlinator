using Invoices.Domain.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Invoices.Tests.Domain
{
    [TestClass]
    public class InvoicesTest
    {

        [TestMethod]
        public void Deveria_possuir_o_custo_total_da_despesa_valida_quando_for_igual_a_soma_dos_custos_dos_itens()
        {
            var invoice = new Invoice()
            {
                TotalCost = 100,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem(){ Cost = 25, Amount = 2 },
                    new InvoiceItem(){ Cost = 50, Amount = 1 }
                }
            };

            invoice.TotalCostIsValid().Should().BeTrue();
        }

        [TestMethod]
        public void Deveria_possuir_o_custo_total_da_despesa_invalido_quando_for_diferente_da_soma_dos_custos_dos_itens()
        {
            var invoice = new Invoice()
            {
                TotalCost = 100,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem(){ Cost = 10, Amount = 2 },
                    new InvoiceItem(){ Cost = 10, Amount = 1 }
                }
            };

            invoice.TotalCostIsValid().Should().BeFalse();
        }
    }
}
