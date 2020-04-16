using System.Linq;

using Aragas.CampaignSystem.LogEntries;

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
                if (!MercenaryContractOptions.ApplyRelationshipRulesToNPC && clan != Clan.PlayerClan)
                    continue;

                if (MercenaryManager.DaysBeforeContractEnds(clan) < 1f)
                {
                    LogEntry.AddLogEntry(new MercenaryContractExpiredLogEntry(clan.Leader));
                }
            }
        }
    }
}