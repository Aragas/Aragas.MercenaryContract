using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace Aragas.Core
{
	public class MercenaryContractMapNotification : InformationData
	{
		[SaveableProperty(1)]
		public Hero Mercenary { get; set; }

		[SaveableProperty(2)]
		public bool IsHandled { get; set; }

		[SaveableProperty(3)]
		public float CreatedDay { get; set; }

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
			CreatedDay = Clan.PlayerClan.LastFactionChangeTime.ElapsedDaysUntilNow;
		}
	}
}