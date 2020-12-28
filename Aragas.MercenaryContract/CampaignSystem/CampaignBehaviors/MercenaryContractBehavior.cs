using System.Linq;

using Aragas.CampaignSystem.LogEntries;
using Aragas.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem.CampaignBehaviors
{
    public class MercenaryContractBehavior : CampaignBehaviorBase
    {
        public override void SyncData(IDataStore dataStore) { }

        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
        }
        private static void OnDailyTick()
        {
            foreach (var clan in Clan.All.Where(c => c.IsUnderMercenaryService))
            {
                if (!MercenarySettings.Instance!.ApplyRelationshipRulesToNPC && clan != Clan.PlayerClan)
                    continue;

                if (MercenaryManager.DaysBeforeContractEnds(clan) < 1f)
                {
                    var mercenaryContractExpired = new MercenaryContractExpiredLogEntry(Clan.PlayerClan.Leader);
                    LogEntry.AddLogEntry(mercenaryContractExpired);
                    Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new MercenaryContractMapNotification(Clan.PlayerClan.Leader, mercenaryContractExpired.GetEncyclopediaText()));
                }
            }
        }
    }
}