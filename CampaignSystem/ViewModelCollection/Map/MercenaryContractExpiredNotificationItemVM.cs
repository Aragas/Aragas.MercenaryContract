using System;
using System.Reflection;

using Aragas.CampaignSystem.MapNotificationTypes;

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
                var textObject = GameTexts.FindText("str_mercenary_contract_expired_desc", null);
                textObject.SetTextVariable("DAYS", MercenaryContractOptions.ContractLengthInDays);

				InformationManager.ShowInquiry(
					new InquiryData(
						GameTexts.FindText("str_mercenary_contract_expired", null).ToString(),
                        textObject.ToString(), true,
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
            MercenaryManager.RenewContract(MercenaryContractMapNotification.Mercenary);
            MercenaryContractMapNotification.IsHandled = true;
        }
		private void EndContract()
		{
            MercenaryManager.EndContract(MercenaryContractMapNotification.Mercenary);
            MercenaryContractMapNotification.IsHandled = true;
        }

		public override void ManualRefreshRelevantStatus()
		{
			base.ManualRefreshRelevantStatus();

            if (MercenaryContractMapNotification.Mercenary == null)
            {
                ExecuteRemove();
				return;
            }

            // Auto renew contract if no decision was made after one day
			// This may happen in siege I guess
			if (MercenaryContractMapNotification.Mercenary.Clan.IsUnderMercenaryService &&
                MercenaryContractMapNotification.CreationTime.ElapsedDaysUntilNow >= 1f)
            {
                RenewContract();
                ExecuteRemove();
			}
        }
    }
}