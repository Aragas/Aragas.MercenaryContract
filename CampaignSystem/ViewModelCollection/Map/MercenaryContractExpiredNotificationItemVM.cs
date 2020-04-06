using Aragas.CampaignSystem.LogEntries;
using Aragas.Core;

using System;
using System.Reflection;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem.ViewModelCollection.Map
{
	public class MercenaryContractExpiredNotificationItemVM : MapNotificationItemBaseVM
	{
		private static readonly MethodInfo OnMercenaryClanChangedKingdomMethod =
			typeof(CampaignEventReceiver).GetMethod("OnMercenaryClanChangedKingdom", BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly MethodInfo CheckIfPartyIconIsDirtyMethod =
			typeof(ChangeKingdomAction).GetMethod("CheckIfPartyIconIsDirty", BindingFlags.NonPublic | BindingFlags.Static);


		private MercenaryContractMapNotification MercenaryContractMapNotification { get; }

		public MercenaryContractExpiredNotificationItemVM(
			InformationData data,
			Action<MapNotificationItemBaseVM> onRemove)
			: base(data, null, onRemove)
		{
			NotificationIdentifier = "mercenarycontractexpired";

			if (data is MercenaryContractMapNotification mercenaryContractMapNotification)
			{
				MercenaryContractMapNotification = mercenaryContractMapNotification;
				_onInspect = OnMercenaryContractExpiredNotificationInspect;
			}
		}

		private void OnMercenaryContractExpiredNotificationInspect()
		{
			if (!MercenaryContractMapNotification.IsHandled)
			{
				InformationManager.ShowInquiry(
					new InquiryData(
						GameTexts.FindText("str_mercenary_contract_expired", null).ToString(),
						GameTexts.FindText("str_mercenary_contract_expired_desc", null).ToString(), true,
						true,
						GameTexts.FindText("str_accept", null).ToString(),
						GameTexts.FindText("str_reject", null).ToString(),
						RenewContract,
						EndContract,
						""),
					false);
			}
		}
		private void RenewContract()
		{
			Clan.PlayerClan.LastFactionChangeTime = CampaignTime.Now;
			InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_renewed", null).ToString()));
			MercenaryContractMapNotification.IsHandled = true;
			LogEntry.AddLogEntry(new MercenaryContractRenewedLogEntry(MercenaryContractMapNotification.Mercenary));
		}
		private void EndContract()
		{
			var mercenaryClan = MercenaryContractMapNotification.Mercenary.Clan;
			var mercenaryKingdom = MercenaryContractMapNotification.Mercenary.Clan.Kingdom;

			mercenaryClan.ClanLeaveKingdom(false);

			OnMercenaryClanChangedKingdomMethod.Invoke(CampaignEventDispatcher.Instance, new object[] { mercenaryClan, mercenaryKingdom, null });
			//CampaignEventDispatcher.Instance.OnMercenaryClanChangedKingdom(clan, kingdom2, null);
			mercenaryClan.IsUnderMercenaryService = false;

			if (mercenaryClan == Clan.PlayerClan)
			{
				Campaign.Current.UpdateDecisions();
			}
			CheckIfPartyIconIsDirtyMethod.Invoke(null, new object[] { mercenaryClan, mercenaryKingdom });
			//ChangeKingdomAction.CheckIfPartyIconIsDirty(clan, kingdom2);

			InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_ended", null).ToString()));
			MercenaryContractMapNotification.IsHandled = true;
			LogEntry.AddLogEntry(new MercenaryContractEndedLogEntry(MercenaryContractMapNotification.Mercenary));
		}

		public override void ManualRefreshRelevantStatus()
		{
			base.ManualRefreshRelevantStatus();

			// Auto renew contract if no decivion was made ater one day
			// This may happen in siege I guess
			if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.LastFactionChangeTime.ElapsedDaysUntilNow - MercenaryContractMapNotification.CreatedDay >= 1f)
			{
				Clan.PlayerClan.LastFactionChangeTime = CampaignTime.Now;
				MercenaryContractMapNotification.IsHandled = true;

				ExecuteRemove();
			}
		}
	}
}