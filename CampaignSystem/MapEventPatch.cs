using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem
{
    [HarmonyPatch(typeof(MapEvent))]
    [HarmonyPatch("AddInvolvedParty")]
    public class MapEventPatch
    {
        public static void Postfix(MapEvent __instance, PartyBase involvedParty, BattleSideEnum side, bool notFromInit)
        {
            if(!notFromInit)
                return;

            MercenaryContractCampaignEvents.Instance.OnPartyJoinedMapEvent(__instance, involvedParty);
        }
    }
}
