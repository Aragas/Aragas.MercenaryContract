using Aragas.CampaignSystem;
using Aragas.CampaignSystem.CampaignBehaviors;
using Aragas.TextureImportingHack;

using CommunityPatch;

using HarmonyLib;

using StoryMode;

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
        private static MercenaryContractSubModule _instance;

        private MercenaryManager _mercenaryManager;
        private MercenaryContractCampaignEvents _campaignEvents;

        public static MercenaryManager MercenaryManager => _instance._mercenaryManager;
        public static MercenaryContractCampaignEvents CampaignEvents => _instance._campaignEvents;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            _instance = this;

            try
            {
                new Harmony("org.aragas.bannerlord.mercenarycontract").PatchAll(typeof(MercenaryContractSubModule).Assembly);
            }
            catch (Exception ex)
            {
                CommunityPatchSubModule.Error(ex, "[Aragas.MercenaryContract]: Error while trying to initialize Harmony!");
            }

            var mercenarycontractSpriteData = SpriteDataFactory.CreateNewFromModule(
				"mercenarycontractSpriteData",
				UIResourceManager.UIResourceDepot);
			UIResourceManager.SpriteData.AppendFrom(mercenarycontractSpriteData);

			UIResourceManager.BrushFactory.ImportAndAppend(
				"Map.Notification.Type.Circle.Image",
				"MercenaryContractMapNotification",
				"Aragas.MercenaryContract.Map.Notification.Type.Circle.Image");
        }
        protected override void OnSubModuleUnloaded()
        {
            _instance = null!;
            base.OnSubModuleUnloaded();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
		{
            _mercenaryManager = new MercenaryManager();
            _campaignEvents = new MercenaryContractCampaignEvents();

            if (game.GameType is CampaignStoryMode campaignStoryMode && gameStarter is CampaignGameStarter campaignGameStarter)
			{
                campaignGameStarter.LoadGameTexts($"{BasePath.Name}Modules/Aragas.MercenaryContract/ModuleData/global_strings.xml");
                campaignGameStarter.AddBehavior(new BattleBehavior());
				campaignGameStarter.AddBehavior(new BattleHistoryBehavior());
				campaignGameStarter.AddBehavior(new MercenaryContractBehavior());

                if (campaignStoryMode.CampaignGameLoadingType == Campaign.GameLoadingType.SavedCampaign)
                {
                    // Keep this fix for a few versions
                    if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.Kingdom == null)
                    {
                        Clan.PlayerClan.IsUnderMercenaryService = false;
                    }
				}
            }
		}

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
        }
    }
}