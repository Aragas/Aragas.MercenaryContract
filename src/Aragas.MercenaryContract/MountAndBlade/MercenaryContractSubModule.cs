using Aragas.CampaignSystem.Sandbox.CampaignBehaviors;

using HarmonyLib;

using System;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Aragas.MountAndBlade
{
    public class MercenaryContractSubModule : MBSubModuleBase
	{
		public MercenaryContractSubModule()
		{
			try
			{
				new Harmony("org.aragas.bannerlord.mercenarycontract").PatchAll(typeof(MercenaryContractSubModule).Assembly);
			}
			catch (Exception ex)
			{
				// TODO: Find a logger
			}
		}

		protected override void OnSubModuleLoad()
		{
			var mercenarycontractSpriteData = SpriteDataFactory.CreateNewFromModule(
				"mercenarycontractSpriteData",
				UIResourceManager.UIResourceDepot);
			UIResourceManager.SpriteData.AppendFrom(mercenarycontractSpriteData);

			UIResourceManager.BrushFactory.ImportAndAppend(
				"Map.Notification.Type.Circle.Image",
				"MercenacyContractMapNotification",
				"Aragas.MercenaryContract.Map.Notification.Type.Circle.Image");
		}

		protected override void OnGameStart(Game game, IGameStarter gameStarter)
		{
			if (game.GameType is Campaign campaign && gameStarter is CampaignGameStarter campaignGameStarter)
			{
				campaignGameStarter.LoadGameTexts($"{BasePath.Name}Modules/Aragas.MercenaryContract/ModuleData/global_strings.xml");
				campaignGameStarter.AddBehavior(new MercenaryContractBehavior());

				// When creating new Campaign the value is null
				if (Clan.PlayerClan != null)
				{
					// Keep this fix for a few versions
					if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.Kingdom == null)
					{
						Clan.PlayerClan.IsUnderMercenaryService = false;
                    }
				}
			}
		}
	}
}