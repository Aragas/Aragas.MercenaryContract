﻿using Aragas.CampaignSystem.MapNotificationTypes;

using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem.LogEntries
{
	public class MercenaryContractEndedLogEntry : LogEntry, IEncyclopediaLog
	{
		[SaveableField(5)]
		private readonly CharacterObject _mercenary;

		[SaveableField(6)]
		private readonly IFaction _hiringFaction;

		public override CampaignTime KeepInHistoryTime => CampaignTime.Weeks(40f);
        public override ChatNotificationType NotificationType => ChatNotificationType.Neutral;

		public MercenaryContractEndedLogEntry(Hero mercenary, IFaction hiringFaction)
		{
			_mercenary = mercenary.CharacterObject;
			_hiringFaction = hiringFaction;
        }

		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase => obj == _mercenary?.HeroObject;

		public TextObject GetEncyclopediaText()
		{
			if (_mercenary == null || _hiringFaction == null)
				return TextObject.Empty;

			var textObject = GameTexts.FindText("str_mercenary_contract_encyclopedia_ended");
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