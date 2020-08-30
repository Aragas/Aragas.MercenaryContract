using Aragas.CampaignSystem.LogEntries;
using Aragas.CampaignSystem.MapNotificationTypes;

using HarmonyLib;

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
        private static readonly MethodInfo? OnMercenaryClanChangedKingdomMethod =
            AccessTools.Method(typeof(CampaignEventReceiver), "OnMercenaryClanChangedKingdom");

		private static readonly MethodInfo? CheckIfPartyIconIsDirtyMethod =
                AccessTools.Method(typeof(ChangeKingdomAction), "CheckIfPartyIconIsDirty");


		private MercenaryContractMapNotification MercenaryContractMapNotification { get; }

		public MercenaryContractExpiredNotificationItemVM(InformationData data, Action<MapNotificationItemBaseVM> onRemove)
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
						GameTexts.FindText("str_mercenary_contract_expired").ToString(),
						GameTexts.FindText("str_mercenary_contract_expired_desc").ToString(), true,
						true,
						GameTexts.FindText("str_accept").ToString(),
						GameTexts.FindText("str_reject").ToString(),
						RenewContract,
						EndContract,
						""));
			}
		}
		private void RenewContract()
		{
			MercenaryContractMapNotification.Mercenary.Clan.LastFactionChangeTime = CampaignTime.Now;

			if (MercenaryContractMapNotification.Mercenary.IsHumanPlayerCharacter)
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_renewed").ToString()));
			MercenaryContractMapNotification.IsHandled = true;

			LogEntry.AddLogEntry(new MercenaryContractRenewedLogEntry(MercenaryContractMapNotification.Mercenary, MercenaryContractMapNotification.Mercenary.MapFaction));
		}
		private void EndContract()
		{
			var mercenary = MercenaryContractMapNotification.Mercenary;
			var mercenaryClan = mercenary.Clan;
			var mercenaryKingdom = mercenaryClan.Kingdom;
			var mercenaryFaction = mercenary.MapFaction;

			StatisticsDataLogHelper.AddLog(StatisticsDataLogHelper.LogAction.ChangeKingdomAction, 
                mercenaryClan,
                mercenaryKingdom,
                null,
                true);
			mercenaryClan.ClanLeaveKingdom(false);

			OnMercenaryClanChangedKingdomMethod!.Invoke(CampaignEventDispatcher.Instance, new object[] { mercenaryClan, mercenaryKingdom, null });
            mercenaryClan.IsUnderMercenaryService = false;

			CheckIfPartyIconIsDirtyMethod!.Invoke(null, new object[] { mercenaryClan, mercenaryKingdom });

			if (mercenary.IsHumanPlayerCharacter)
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_ended").ToString()));

			MercenaryContractMapNotification.IsHandled = true;

			LogEntry.AddLogEntry(new MercenaryContractEndedLogEntry(mercenary, mercenaryFaction));
		}

		public override void ManualRefreshRelevantStatus()
		{
			base.ManualRefreshRelevantStatus();

			// Auto renew contract if no decivion was made ater one day
			// This may happen in siege I guess
			if (MercenaryContractMapNotification.Mercenary.Clan.IsUnderMercenaryService &&
				MercenaryContractMapNotification.Mercenary.Clan.LastFactionChangeTime.ElapsedDaysUntilNow - MercenaryContractMapNotification.CreatedDay >= 1f)
			{
				MercenaryContractMapNotification.Mercenary.Clan.LastFactionChangeTime = CampaignTime.Now;
				MercenaryContractMapNotification.IsHandled = true;

				ExecuteRemove();
			}
		}
	}
}