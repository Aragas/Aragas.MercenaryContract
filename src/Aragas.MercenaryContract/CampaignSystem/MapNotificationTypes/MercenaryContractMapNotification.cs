using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem.MapNotificationTypes
{
    public class MercenaryContractMapNotification : InformationData
    {
        [SaveableProperty(1)]
        public Hero Mercenary { get; set; }

        [SaveableProperty(2)]
        public bool IsHandled { get; set; }

        [SaveableProperty(3)]
        public float CreatedDay { get; set; }

        public override TextObject TitleText { get; } = GameTexts.FindText("str_mercenary_contract_expired");

        public override string SoundEventPath { get; } = "";

        public MercenaryContractMapNotification(Hero mercenary, TextObject descriptionText) : base(descriptionText)
        {
            Mercenary = mercenary;
            CreatedDay = mercenary.Clan.LastFactionChangeTime.ElapsedDaysUntilNow;
        }
    }
}