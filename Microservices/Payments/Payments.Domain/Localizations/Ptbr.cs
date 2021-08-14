using FinanceControlinator.Common.Localizations;

namespace Payments.Domain.Localizations
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
    }
}
