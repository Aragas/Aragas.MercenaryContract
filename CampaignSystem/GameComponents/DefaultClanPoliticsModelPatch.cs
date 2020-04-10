using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;

namespace Aragas.CampaignSystem.GameComponents
{
    [HarmonyPatch(typeof(DefaultClanPoliticsModel))]
    [HarmonyPatch("CalculateInfluenceChange")]
    public class DefaultClanPoliticsModelPatch
    {
        public static void Postfix(ref float __result, Clan clan, StatExplainer explanation)
        {
            var refContainer = new RefContainer<float>(__result);
            MercenaryContractCampaignEvents.Instance.OnCalculateInfluenceChange(refContainer, clan, explanation);
            __result = refContainer.Value;
        }
    }
}