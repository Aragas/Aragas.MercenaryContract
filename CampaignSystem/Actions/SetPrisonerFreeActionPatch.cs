using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Aragas.CampaignSystem.Actions
{
    /// <summary>
    /// While there exists OnPrisonerReleased, we can't capture facilitator Hero.
    /// </summary>
    [HarmonyPatch(typeof(EndCaptivityAction))]
    [HarmonyPatch("ApplyInternal")]
    public class SetPrisonerFreeActionPatch
    {
        // If you let leaders of the parties you defeat go, same thing with the [Merciful] trait.
        public static void Prefix(Hero prisoner, EndCaptivityDetail detail, Hero facilitator)
        {
            if (prisoner == null || facilitator == null || detail != EndCaptivityDetail.ReleasedAfterBattle)
                return;


            var isPlayer = facilitator == Clan.PlayerClan.Leader;

            var multiplier = facilitator.Clan.IsUnderMercenaryService
                ? MercenaryContractOptions.Instance.MercenaryMultiplier
                : MercenaryContractOptions.Instance.VassalMultiplier;

            if (prisoner.GetTraitLevel(DefaultTraits.Mercy) > 0)
            {
                AragasChangeRelationAction.ApplyRelation(
                    facilitator,
                    prisoner,
                    2 * multiplier,
                    MercenaryContractOptions.Instance.EnemyCap,
                    true && isPlayer);
            }
            else
            {
                AragasChangeRelationAction.ApplyRelation(
                    facilitator,
                    prisoner,
                    1 * multiplier,
                    MercenaryContractOptions.Instance.EnemyCap,
                    true && isPlayer);
            }
        }
    }
}