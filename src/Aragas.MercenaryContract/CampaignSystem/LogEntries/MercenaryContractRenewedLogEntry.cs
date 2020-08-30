using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem.LogEntries
{
	public class MercenaryContractRenewedLogEntry : LogEntry, IEncyclopediaLog
	{
		[SaveableField(1)]
		private readonly CharacterObject _mercenary;

		[SaveableField(2)]
		private readonly IFaction _hiringFaction;

		public override CampaignTime KeepInHistoryTime => CampaignTime.Weeks(40f);

		public MercenaryContractRenewedLogEntry(Hero mercenary, IFaction hiringFaction)
		{
			_mercenary = mercenary.CharacterObject;
			_hiringFaction = hiringFaction;
		}

		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase => obj == _mercenary.HeroObject;

		public TextObject GetEncyclopediaText()
		{
			if (_mercenary == null)
				return TextObject.Empty;

			var textObject = GameTexts.FindText("str_mercenary_contract_encyclopedia_renewed");
			StringHelpers.SetCharacterProperties("HERO", _mercenary, null, textObject);
			textObject.SetTextVariable("FACTION", _hiringFaction.EncyclopediaLinkWithName);
			return textObject;
		}

		public override string ToString() => GetEncyclopediaText().ToString();
	}
}