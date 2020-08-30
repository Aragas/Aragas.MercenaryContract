using Aragas.CampaignSystem.LogEntries;
using Aragas.CampaignSystem.MapNotificationTypes;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem.Sandbox.CampaignBehaviors
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
                var mercenaryContractExpiredLogEntry = new MercenaryContractExpiredLogEntry(Clan.PlayerClan.Leader);
                LogEntry.AddLogEntry(mercenaryContractExpiredLogEntry);
                Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new MercenaryContractMapNotification(Clan.PlayerClan.Leader, mercenaryContractExpiredLogEntry.GetEncyclopediaText()));
			}
		}
	}
}