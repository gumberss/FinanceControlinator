namespace Accounts.Domain.Localizations
{
    public class Ptbr : ILocalization
    {
        public string PAYMENT_ITEM_NOT_CLOSED => "O item não pode ser pago pois ainda não está fechado";

        public string PAYMENT_ALREADY_IN_PROCESS => "O pagamento do item já está em processamento";

        public string ITEM_ALREADY_WAS_PAID => "O Item já foi pago";

        public string ITEM_NOT_FOUND => "Item não encontrado";
    }
}
