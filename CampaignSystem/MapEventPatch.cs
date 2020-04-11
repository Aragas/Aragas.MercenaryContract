using HarmonyLib;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem
{
    [HarmonyPatch(typeof(MapEvent))]
    [HarmonyPatch("AddInvolvedParty")]
    internal class MapEventPatch1
    {
        public static void Postfix(MapEvent __instance, PartyBase involvedParty, BattleSideEnum side, bool notFromInit)
        {
            if(!notFromInit)
                return;

            MercenaryContractCampaignEvents.Instance.OnPartyJoinedMapEvent(__instance, involvedParty);
        }
    }

    [HarmonyPatch(typeof(MapEvent))]
    [HarmonyPatch("Initialize")]
    internal class MapEventPatch2
    {
        private static MethodInfo AddInvolvedPartyMethod { get; } = typeof(MapEvent).GetMethod("AddInvolvedParty");

        // Every PartyBase added in Initialize() should be marked as notFromInit = true
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = instructions.ToList();

            for (var index = 0; index < list.Count; index++)
            {
                if (list[index].Calls(AddInvolvedPartyMethod) && list[index - 1].opcode == OpCodes.Ldc_I4_1)
                    list[index - 1].opcode = OpCodes.Ldc_I4_0;
            }

            return list;
        }
    }

    [HarmonyPatch(typeof(MapEvent))]
    [HarmonyPatch("AddInsideSettlementParties")]
    internal class MapEventPatch3
    {
        private static MethodInfo AddInvolvedPartyMethod { get; } = typeof(MapEvent).GetMethod("AddInvolvedParty");

        // Every PartyBase added in Initialize() should be marked as notFromInit = true
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = instructions.ToList();

            for (var index = 0; index < list.Count; index++)
            {
                if (list[index].Calls(AddInvolvedPartyMethod) && list[index - 1].opcode == OpCodes.Ldc_I4_1)
                    list[index - 1].opcode = OpCodes.Ldc_I4_0;
            }

            return list;
        }
    }
}