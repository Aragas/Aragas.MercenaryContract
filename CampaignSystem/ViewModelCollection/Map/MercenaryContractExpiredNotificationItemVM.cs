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
	// Character icon on Map:
	// ButtonWidget with child ImageIdentifierWidget
	// ImageId = CharacterId data
	// ImageTypeCode = 5
	// AdditionalArgs = ""
	// Texture - character_tableaue0
	// TextureProvider TaleWorlds.MountAndBlade.GauntletUI.ImageIdentifierTextureProvider


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
			MercenaryContractMapNotification.Mercenary.Clan.LastFactionChangeTime = CampaignTime.Now;

			if (MercenaryContractMapNotification.Mercenary.IsHumanPlayerCharacter)
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_renewed", null).ToString()));
			MercenaryContractMapNotification.IsHandled = true;

			LogEntry.AddLogEntry(new MercenaryContractRenewedLogEntry(MercenaryContractMapNotification.Mercenary, MercenaryContractMapNotification.Mercenary.MapFaction));
		}
		private void EndContract()
		{
			var mercenary = MercenaryContractMapNotification.Mercenary;
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

			OnMercenaryClanChangedKingdomMethod.Invoke(CampaignEventDispatcher.Instance, new object[] { mercenaryClan, mercenaryKingdom, null });
			//CampaignEventDispatcher.Instance.OnMercenaryClanChangedKingdom(clan, kingdom2, null);
			mercenaryClan.IsUnderMercenaryService = false;

			if (mercenary.IsHumanPlayerCharacter)
				Campaign.Current.UpdateDecisions();

			CheckIfPartyIconIsDirtyMethod.Invoke(null, new object[] { mercenaryClan, mercenaryKingdom });
			//ChangeKingdomAction.CheckIfPartyIconIsDirty(clan, kingdom2);

			if (mercenary.IsHumanPlayerCharacter)
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_mercenary_contract_ended", null).ToString()));
			
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