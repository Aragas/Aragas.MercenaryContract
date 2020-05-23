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

		// TODO
		public CampaignTime CreationTime { get; } //((LogEntry)InformationDataHolder).GameTime;

        public override TextObject TitleText { get; } = GameTexts.FindText("str_mercenary_contract_expired");
        public override string SoundEventPath { get; } = "";

        public MercenaryContractMapNotification(Hero mercenary, TextObject descriptionText) : base(descriptionText)
        {
            Mercenary = mercenary;
            //CreationTime = mercenary.Clan.LastFactionChangeTime.ElapsedDaysUntilNow;
        }
	}
}