using HarmonyLib;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Aragas.CampaignSystem.Actions
{
    [HarmonyPatch(typeof(StartBattleAction))]
    [HarmonyPatch("ApplyInternal")]
    internal class StartBattleActionPatch
    {
        private static MethodInfo AddInvolvedPartyMethod { get; } = typeof(MapEvent).GetMethod("AddInvolvedParty");

        // Every PartyBase added in ApplyInternal() should be marked as notFromInit = true
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