using Aragas.CampaignSystem.LogEntries;
using Aragas.Core;

using TaleWorlds.SaveSystem;

namespace Aragas.SaveSystem
{
	public class AragasSaveDefiner : SaveableTypeDefiner
	{
		public AragasSaveDefiner() : base(1_690_000) { }

		protected override void DefineClassTypes()
		{
			AddClassDefinition(typeof(MercenaryContractMapNotification), 1);
			AddClassDefinition(typeof(MercenaryContractExpiredLogEntry), 2);
			AddClassDefinition(typeof(MercenaryContractRenewedLogEntry), 3);
			AddClassDefinition(typeof(MercenaryContractEndedLogEntry), 4);
		}
	}
}