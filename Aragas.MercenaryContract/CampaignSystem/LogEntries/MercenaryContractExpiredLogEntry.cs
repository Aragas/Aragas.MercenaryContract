using Aragas.CampaignSystem.MapNotificationTypes;

using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem.LogEntries
{
	public class MercenaryContractExpiredLogEntry : LogEntry, IEncyclopediaLog
    {
		[SaveableField(5)]
		private CharacterObject _mercenary;

		[SaveableField(6)]
		private IFaction _hiringFaction;

        public override CampaignTime KeepInHistoryTime => CampaignTime.Days(1f);
        public override ChatNotificationType NotificationType => ChatNotificationType.Neutral;

		public MercenaryContractExpiredLogEntry(Hero mercenary)
		{
			_mercenary = mercenary.CharacterObject;
			_hiringFaction = mercenary.MapFaction;
        }

        public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase => obj == _mercenary.HeroObject;

        public TextObject GetEncyclopediaText()
        {
            if (_mercenary == null)
                return TextObject.Empty;

            // Since our e1.0.5 did not include _hiringFaction, workaround.
            if (_hiringFaction == null)
                _hiringFaction = _mercenary.HeroObject.MapFaction;

            var textObject = GameTexts.FindText("str_mercenary_contract_encyclopedia");
            StringHelpers.SetCharacterProperties(
                "HERO",
                _mercenary,
                null,
                textObject);
            textObject.SetTextVariable("FACTION", _hiringFaction.EncyclopediaLinkWithName);
            return textObject;
        }

        public override string ToString() => GetEncyclopediaText().ToString();
	}
}