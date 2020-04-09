using HarmonyLib;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem
{
    [HarmonyPatch(typeof(MapEvent), "FinalizeEvent")]
    public class MapEventPatch
    {
        public static void Prefix(MapEvent __instance)
        {
            if(__instance.IsFinalized || __instance.EventType == MapEvent.BattleTypes.None)
                return;

            MercenaryContractCampaignEvents.Instance.OnBattleEnded(__instance, CampaignTime.Now);
        }
    }
}