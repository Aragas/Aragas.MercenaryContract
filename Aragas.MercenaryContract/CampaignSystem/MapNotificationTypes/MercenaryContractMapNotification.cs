using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem.MapNotificationTypes
{
	public class MercenaryContractMapNotification : InformationData
    {
        [SaveableProperty(6)]
        public Hero Mercenary { get; set; }

		[SaveableProperty(7)]
		public bool IsHandled { get; set; }

        public CampaignTime CreationTime => ((LogEntry)InformationDataHolder).GameTime;

		public MercenaryContractMapNotification(
			Hero mercenary,
			TextObject titleText,
			TextObject descriptionText,
			bool forceInspection,
			LogEntry logEntry,
			string soundEventPath = "")
			: base(titleText, descriptionText, forceInspection, logEntry, soundEventPath)
		{
			Mercenary = mercenary;
        }
	}
}