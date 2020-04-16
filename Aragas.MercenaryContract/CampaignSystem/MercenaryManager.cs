using System.Reflection;

using Aragas.CampaignSystem.LogEntries;
using Aragas.MountAndBlade;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Aragas.CampaignSystem
{
    public class MercenaryManager
    {
        private static readonly MethodInfo OnMercenaryClanChangedKingdomMethod =
            typeof(CampaignEventReceiver).GetMethod("OnMercenaryClanChangedKingdom", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo CheckIfPartyIconIsDirtyMethod =
            typeof(ChangeKingdomAction).GetMethod("CheckIfPartyIconIsDirty", BindingFlags.NonPublic | BindingFlags.Static);

        public static MercenaryManager Instance => MercenaryContractSubModule.MercenaryManager;

        public static float DaysAfterContractStartedOrRenewed(Clan mercenaryClan)
        {
            var contractLength = MercenaryContractOptions.ContractLengthInDays;
            var elapsedDays = mercenaryClan.LastFactionChangeTime.ElapsedDaysUntilNow;
            return elapsedDays - (MathF.Floor(elapsedDays / contractLength) * contractLength);
        }

        public static float DaysBeforeContractEnds(Clan mercenaryClan) => MercenaryContractOptions.ContractLengthInDays - DaysAfterContractStartedOrRenewed(mercenaryClan);


        public static void RenewContract(Hero mercenary)
        {
            if (mercenary == Hero.MainHero)
                InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_renewed", null).ToString()));

            LogEntry.AddLogEntry(new MercenaryContractRenewedLogEntry(mercenary, mercenary.MapFaction));
        }

        public static void EndContract(Hero mercenary)
        {
            var mercenaryClan = mercenary.Clan;
            var mercenaryKingdom = mercenaryClan.Kingdom;
            var mercenaryFaction = mercenary.MapFaction;

            StatisticsDataLogHelper.AddLog(StatisticsDataLogHelper.LogAction.ChangeKingdomAction, new object[]
            {
                mercenaryClan,
                mercenaryKingdom,
                (Kingdom) null,
                true
            });
            mercenaryClan.ClanLeaveKingdom(false);

            OnMercenaryClanChangedKingdomMethod.Invoke(CampaignEventDispatcher.Instance, new object[]
            {
                mercenaryClan,
                mercenaryKingdom,
                (Kingdom) null
            });
            mercenaryClan.IsUnderMercenaryService = false;

            if (mercenary == Hero.MainHero)
                Campaign.Current.UpdateDecisions();

            CheckIfPartyIconIsDirtyMethod.Invoke(null, new object[] { mercenaryClan, mercenaryKingdom });

            if (mercenary == Hero.MainHero)
                InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_ended", null).ToString()));

            LogEntry.AddLogEntry(new MercenaryContractEndedLogEntry(mercenary, mercenaryFaction));
        }
    }
}