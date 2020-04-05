using Aragas.CampaignSystem.LogEntries;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem
{
	public class MercenaryContractBehavior : CampaignBehaviorBase
	{
		public override void SyncData(IDataStore dataStore) { }

		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
		}
		private void OnDailyTick()
		{
			if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.LastFactionChangeTime.ElapsedDaysUntilNow >= 30f)
			{
				LogEntry.AddLogEntry(new MercenaryContractExpiredLogEntry(Clan.PlayerClan.Leader));
			}
		}
	}
}