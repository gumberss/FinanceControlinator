
using Invoices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Invoices.Domain.Localizations
{
    public class Ptbr : ILocalization
    {
        public CultureInfo CULTURE => new CultureInfo("pt-BR");

        public string DATE_INCORRECT => "A 'Data' informada não é permitida";

        public string PAYMENT_ALREADY_EXISTENT => "O pagamento já foi registrado";

        public string OVERVIEW_FUTURE_PURCHASE_PERCENT => "[[PERCENT]]% do valor dessa fatura foi guardado para compras futuras";

        public string INVOICE_OVERVIEW_INVESTMENT_PERCENT => "[[PERCENT]]% do valor dessa fatura foi em investimento";

        public string INVOICE_OVERVIEW_BILL_PERCENT_INCREASE_COMPARED_LAST_MONTHES => "O gasto com contas aumentou [[PERCENT]]% comparado com a média das últimas [[INVOICES_QUANTITIES]] faturas";
        public string INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_MONTHES => "O gasto com contas diminuiu [[PERCENT]]% comparado com a média das últimas [[INVOICES_QUANTITIES]] faturas";
        public string INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_INVOICES => "O gasto com contas se manteve comparado com a média das últimas [[INVOICES_QUANTITIES]] faturas";

        public string INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICE => "O valor da fatura aumentou [[PERCENT]]% comparado com a anterior";
        public string INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_INVOICE => "O valor da fatura diminuiu [[PERCENT]]% comparado com a anterior";
        public string INVOICE_COST_PERCENT_NOT_CHANGE_COMPARED_WITH_LAST_INVOICE => "O valor da fatura não mudou comparado com a anterior";

        public string INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICES => "O valor da fatura aumentou [[PERCENT]]% comparado com as ultimas [[INVOICES_QUANTITIES]] faturas";
        public string INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_INVOICES => "O valor da fatura diminuiu [[PERCENT]]% comparado com as ultimas [[INVOICES_QUANTITIES]] faturas";
        public string INVOICE_COST_PERCENT_NOT_CHANGE_COMPARED_WITH_LAST_INVOICES => "O valor da fatura não mudou comparado com as ultimas [[INVOICES_QUANTITIES]] faturas";

        public string OVERDUE => "Fatura atrasada";

        public string PAID => "Paga";

        public string OPEN => "Fatura aberta";

        public string CLOSED => "Fatura fechada";

        public string INVOICE_SYNC_NAME => "Faturas";

        public string INVOICE_INSTALLMENT_NUMBER => "Parcela: [[INSTALLMENT_NUMBER]]";

        public string FORMAT_MONEY(decimal value) => value.ToString("C2", CULTURE);

        public string INVOICE_ITEM_TYPE(InvoiceItemType invoiceItemType) => new Dictionary<InvoiceItemType, String>
        {
            { InvoiceItemType.Market, "Mercado" },
            { InvoiceItemType.Bill, "Contas" },
            { InvoiceItemType.Investment, "Investimento" },
            { InvoiceItemType.Health, "Saúde" },
            { InvoiceItemType.Leisure, "Lazer" },
            { InvoiceItemType.PiggyBank, "Cofrinho" },
            { InvoiceItemType.Other, "Outros" },

        }[invoiceItemType];
    }
}