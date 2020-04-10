using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Aragas.CampaignSystem.Actions
{
    [HarmonyPatch(typeof(BeHostileAction))]
    [HarmonyPatch("ApplyInternal")]
    public class BeHostileActionPatch
    {
        // Reduce/remove? the relationship loss when attacking an enemy of the Kingdom (MelissaSanctum)
        public static void Prefix(PartyBase attackerParty, PartyBase defenderParty, ref float value)
        {
            if (attackerParty == null || defenderParty == null)
                return;

            if (attackerParty.LeaderHero.Clan.IsUnderMercenaryService && value > 2f)
            {
                value -= 2f;
            }
        }
    }
}