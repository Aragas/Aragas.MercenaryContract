using Aragas.CampaignSystem;
using Aragas.CampaignSystem.CampaignBehaviors;
using Aragas.TextureImportingHack;

using CommunityPatch;

using HarmonyLib;

using System;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Aragas.MountAndBlade
{
    // TODO: * Introduce 'rewards' for various war actions, like joining armies and capturing towns/castles(filipegroh)

    public class MercenaryContractSubModule : MBSubModuleBase
    {
        public static MercenaryContractSubModule Current { get; private set; } = default!;

		public MercenaryManager MercenaryManager { get; }
		public MercenaryContractOptions Options { get; }
		public MercenaryContractCampaignEvents CampaignEvents { get; }

		public MercenaryContractSubModule()
        {
            Current = this;
            MercenaryManager = new MercenaryManager();
			Options = new MercenaryContractOptions();
            CampaignEvents = new MercenaryContractCampaignEvents();

			try
			{
				new Harmony("org.aragas.bannerlord.mercenarycontract").PatchAll(typeof(MercenaryContractSubModule).Assembly);
			}
			catch (Exception ex)
			{
                CommunityPatchSubModule.Error(ex, "[Aragas.MercenaryContract]: Error while trying to initialize Harmony!");
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
				campaignGameStarter.AddBehavior(new BattleHistoryBehavior());
				campaignGameStarter.AddBehavior(new MercenaryContractBehavior());

				// When creating new Campaign the value is null
				if (Clan.PlayerClan != null)
				{
					// Keep this fix for a few versions
					if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.Kingdom == null)
					{
						Clan.PlayerClan.IsUnderMercenaryService = false;
						campaign.UpdateDecisions();
					}
				}
			}
		}
    }
}