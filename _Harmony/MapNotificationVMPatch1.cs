using Aragas.CampaignSystem.ViewModelCollection.Map;
using Aragas.Core;

using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Core;

namespace Aragas
{
	// The good thing about this patch is that even if they create a good implementation of adding custom
	// MapNotificationVM classes, this should still work and should not break the game
	[HarmonyPatch(typeof(MapNotificationVM), "DetermineNotificationType")]
	public class MapNotificationVMPatch1
	{
		public static bool IsEnabled { get; set; } = true;

		private static MethodInfo RemoveNotificationItemMethod {get;}= typeof(MapNotificationVM).GetMethod("RemoveNotificationItem", BindingFlags.Instance | BindingFlags.NonPublic);
		private static void Postfix(MapNotificationVM __instance, ref MapNotificationItemBaseVM __result, InformationData data)
		{
			// Vanilla didn't found the right type and returned a default one
			if (__result == null || __result.GetType() == typeof(MapNotificationItemBaseVM))
			{
				if (data is MercenaryContractMapNotification mercenaryContractMapNotification)
				{
					var removeNotificationItemDelegate = RemoveNotificationItemMethod.CreateDelegate(typeof(Action<MapNotificationItemBaseVM>), __instance);
					__result = new MercenaryContractExpiredNotificationItemVM(data, (Action<MapNotificationItemBaseVM>) removeNotificationItemDelegate);
				}
			}
		}
	}
}