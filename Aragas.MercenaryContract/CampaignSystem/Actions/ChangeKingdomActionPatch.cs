using System.Linq;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Aragas.CampaignSystem.Actions
{
    [HarmonyPatch(typeof(ChangeKingdomAction))]
    [HarmonyPatch("ApplyByLeaveKingdomAsMercenaryForNoPayment")]
    internal class ChangeKingdomActionPatch
    {
        public static void Postfix(Clan mercenaryClan, Kingdom kingdom, bool showNotification)
        {
            // Let NPC's logic control leaving for now.
            if (/*!MercenaryContractOptions.ApplyRelationshipRulesToNPC && */mercenaryClan != Clan.PlayerClan)
                return;

            var isAtWar = FactionManager.GetEnemyFactions(kingdom).Any();
            foreach (var contractorClan in kingdom.Clans)
            {
                ChangeRelationAction.ApplyRelationChangeBetweenHeroes(
                    mercenaryClan.Leader,
                    contractorClan.Leader,
                    isAtWar ? MercenarySettings.Instance.LeavingLossWar : MercenarySettings.Instance.LeavingLossPeace,
                    true);
            }
        }
    }
}