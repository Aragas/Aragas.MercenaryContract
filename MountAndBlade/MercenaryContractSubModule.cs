using Aragas.CampaignSystem;

using HarmonyLib;

using System;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Aragas.MountAndBlade
{
	public class MercenaryContractSubModule : MBSubModuleBase
	{
		protected override void OnGameStart(Game game, IGameStarter gameStarter)
		{
			try
			{
				new Harmony("org.aragas.bannerlord.mercenarycontract").PatchAll(typeof(MercenaryContractSubModule).Assembly);
			}
			catch (Exception ex)
			{
				// TODO: Find a logger
			}

			if (game.GameType is Campaign && gameStarter is CampaignGameStarter campaignGameStarter)
			{
				campaignGameStarter.LoadGameTexts(BasePath.Name + "Modules/Aragas.MercenaryContract/ModuleData/global_strings.xml");
				campaignGameStarter.AddBehavior(new MercenaryContractBehavior());
			}
		}
	}
}