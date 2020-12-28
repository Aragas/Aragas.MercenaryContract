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
    internal class EndCaptivityActionPatch
    {
        // If you let leaders of the parties you defeat go, same thing with the [Merciful] trait.
        public static void Prefix(Hero prisoner, EndCaptivityDetail detail, Hero facilitatior)
        {
            if (prisoner == null || facilitatior == null || detail != EndCaptivityDetail.ReleasedAfterBattle)
                return;

            var multiplier = facilitatior.Clan.IsUnderMercenaryService
                ? MercenarySettings.Instance!.MercenaryMultiplier
                : MercenarySettings.Instance!.VassalMultiplier;

            if (prisoner.GetTraitLevel(DefaultTraits.Mercy) > 0)
            {
                AragasChangeRelationAction.ApplyRelation(
                    facilitatior,
                    prisoner,
                    2 * multiplier,
                    MercenarySettings.Instance.EnemyCap,
                    true);
            }
            else
            {
                AragasChangeRelationAction.ApplyRelation(
                    facilitatior,
                    prisoner,
                    1 * multiplier,
                    MercenarySettings.Instance.EnemyCap,
                    true);
            }
        }
    }
}