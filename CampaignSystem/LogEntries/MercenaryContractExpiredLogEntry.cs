using Aragas.CampaignSystem.MapNotificationTypes;

using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem.LogEntries
{
	public class MercenaryContractExpiredLogEntry : LogEntry
	{
		[SaveableField(5)]
		private CharacterObject _mercenary;

		[SaveableField(6)]
		private IFaction _hiringFaction;

        public override CampaignTime KeepInHistoryTime => CampaignTime.Weeks(1f);

		public MercenaryContractExpiredLogEntry(Hero mercenary)
		{
			_mercenary = mercenary.CharacterObject;
			_hiringFaction = mercenary.MapFaction;

			if (mercenary == Hero.MainHero && mercenary.MapFaction == MobileParty.MainParty.MapFaction)
			{
				InteractiveNotificationData = new MercenaryContractMapNotification(
					mercenary,
					GameTexts.FindText("str_mercenary_contract_expired", null),
                    GetDescriptionText(),
					false,
					this);
			}
		}

		public TextObject GetDescriptionText()
		{
            if (_mercenary == null || _hiringFaction == null)
                return TextObject.Empty;

            var textObject = GameTexts.FindText("str_mercenary_contract_encyclopedia", null);
			StringHelpers.SetCharacterProperties(
				"HERO",
				_mercenary,
				null,
				textObject);
			textObject.SetTextVariable("FACTION", _hiringFaction.EncyclopediaLinkWithName);
			return textObject;
		}
    }
}