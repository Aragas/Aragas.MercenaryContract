﻿using Aragas.CampaignSystem;
using Aragas.CampaignSystem.LogEntries;
using Aragas.CampaignSystem.MapNotificationTypes;

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace Aragas.SaveSystem
{
    public class AragasSaveDefiner : SaveableTypeDefiner
	{
		// Nice.
		public AragasSaveDefiner() : base(1_690_000) { }

		protected override void DefineClassTypes()
		{
			AddClassDefinition(typeof(MercenaryContractMapNotification), 1);
			AddClassDefinition(typeof(MercenaryContractExpiredLogEntry), 2);
			AddClassDefinition(typeof(MercenaryContractRenewedLogEntry), 3);
            AddClassDefinition(typeof(MercenaryContractEndedLogEntry), 4);
            AddClassDefinition(typeof(BattleHistoryEntry), 5);
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(List<BattleHistoryEntry>));
			ConstructContainerDefinition(typeof(Dictionary<MapEvent, List<PartyBase>>));
        }
	}
}