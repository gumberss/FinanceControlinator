using System;

namespace PiggyBanks.Domain.Localizations
{
    public interface ILocalization
    {
        String PIGGY_BANK_ALREADY_EXISTS_BY_TITLE { get; }
        string PIGGY_BANK_INVALID_GOAL_VALUE { get; }
        string PIGGY_BANK_INVALID_SAVED_VALUE { get; }
        string PIGGY_BANK_INVALID_START_DATE { get; }
        string PIGGY_BANK_WITHOUT_TITLE { get; }
        string PIGGY_BANK_INVALID_GOAL_DATE { get; }
        string PIGGY_BANK_WITH_BIG_DESCRIPTION { get; }
    }
}
