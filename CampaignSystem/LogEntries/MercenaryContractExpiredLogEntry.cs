using Aragas.Core;

using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem.LogEntries
{
	public class MercenaryContractExpiredLogEntry : LogEntry, IEncyclopediaLog
	{
		[SaveableField(1)]
		private readonly CharacterObject _mercenary;

		[SaveableField(2)]
		private readonly IFaction _hiringFaction;

		public override CampaignTime KeepInHistoryTime => CampaignTime.Weeks(1f);

		public MercenaryContractExpiredLogEntry(Hero mercenary)
		{
			_mercenary = mercenary.CharacterObject;
			_hiringFaction = mercenary.MapFaction;

			if (mercenary.MapFaction == MobileParty.MainParty.MapFaction)
			{
				InteractiveNotificationData = new MercenaryContractMapNotification(
					mercenary,
					GameTexts.FindText("str_mercenary_contract_expired", null),
					GetEncyclopediaText(),
					false,
					this);
			}
		}

		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase => obj == _mercenary.HeroObject;

		public TextObject GetEncyclopediaText()
		{
			if (_mercenary == null)
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

		public override string ToString() => GetEncyclopediaText().ToString();
	}
}