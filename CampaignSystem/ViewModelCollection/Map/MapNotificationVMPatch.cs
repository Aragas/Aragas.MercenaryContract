using System;
using System.Reflection;

using Aragas.CampaignSystem.MapNotificationTypes;

using HarmonyLib;

using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem.ViewModelCollection.Map
{
    // The good thing about this patch is that even if they create a good implementation of adding custom
	// MapNotificationVM classes, this should still work and should not break the game
	[HarmonyPatch(typeof(MapNotificationVM))]
    [HarmonyPatch("DetermineNotificationType")]
	public class MapNotificationVMPatch
	{
        private static MethodInfo RemoveNotificationItemMethod { get; } =
            typeof(MapNotificationVM).GetMethod("RemoveNotificationItem", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Postfix(MapNotificationVM __instance, ref MapNotificationItemBaseVM __result, InformationData data)
		{
			// Vanilla didn't found the right type and returned a default one
			if (__result == null || __result.GetType() == typeof(MapNotificationItemBaseVM))
			{
				if (data is MercenaryContractMapNotification)
				{
					var removeNotificationItemDelegate = RemoveNotificationItemMethod.CreateDelegate(typeof(Action<MapNotificationItemBaseVM>), __instance);
					__result = new MercenaryContractExpiredNotificationItemVM(data, (Action<MapNotificationItemBaseVM>) removeNotificationItemDelegate);
				}
			}
		}
	}
}