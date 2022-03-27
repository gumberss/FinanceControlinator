
using Expenses.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.Localizations
{
    public class Ptbr : ILocalization
    {
        public string DATE_INCORRECT => "A 'Data' informada não é permitida";

        public string PURCHASE_LOCATION_MUST_HAVE_VALUE => "O 'Local da Compra' deve ser informado";

        public string TITLE_MUST_HAVE_VALUE => "O 'Título' deve ser informado";

        public string EXPENSE_TYPE_MUST_BE_VALID => "O 'Tipo da despesa' deve ser um dos valores válidos informados";

        public string ITEM_NAME_MUST_BE_VALID => "O 'Nome do Item' deve ser informado";

        public string ITEM_AMOUNT_MUST_BE_GREATER_THAN_ZERO => "A 'Quantidade de Itens' informada deve ser maior que zero";

        public string ITEM_COST_MUST_BE_GREATER_THAN_ZERO => "O 'Valor do Item' deve ser maior que zero";

        public string TOTAL_COST_DOES_NOT_MATCH_WITH_ITEMS => "O 'Valor Total' do custo não está igual com a soma dos valors dos items";

        public string AN_ERROR_OCCURRED_ON_THE_SERVER => "Ocorreu um erro no servidor";

        public string EXPENSES_NOT_FOUND => "Nenhuma despesa foi encontrada";

        public string EXPENSE_NOT_FOUND => "Despesa não encontrada";

        public string EXPENSE_COST_IS_LESS_THAN_WHAT_WAS_PAID => "O valor da despesa está menor que o valor dela já pago";

        public string EXPENSE_INSTALLMENTS_IS_LESS_THAN_TIMES_PAID => "A quantidade de parcelas da despesa é inferior a quantidade de parcelas já pagas";

        public string INSTALLMENTS_QUANTITY_IS_NOT_VALID => "A quantidade de parcelas não é válida";

        public string MOST_EXPENT_TYPE_TEMPLATE => "Maior gasto esse mês foi com [[MOST_SPENT_TYPE]]";

        public String TOTAL_SPENT_MONEY_IN_THE_PLACE_TEMPLATE => "O lugar que você mais gastou foi em [[MOST_SPENT_PLACE]] R$[[TOTAL_VALUE]]";

        public String TOTAL_SPENT_IN_THE_MONTH_TEMPLATE => "Você gastou R$[[TOTAL_SPENT_IN_THE_MONTH]] esse mês";

        public string EXPENSE_TYPE(ExpenseType expenseType) => new Dictionary<ExpenseType, String>
        {
            { ExpenseType.Market, "Mercado" },
            { ExpenseType.Bill, "Contas" },
            { ExpenseType.Investment, "Investimento" },
            { ExpenseType.Health, "Saúde" },
            { ExpenseType.Leisure, "Lazer" },
            { ExpenseType.Other, "Outros" },

        }[expenseType];
    }
}
