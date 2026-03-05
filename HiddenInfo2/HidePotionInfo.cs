using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace HiddenInfo2;

[HarmonyPatch]
static class HidePotionInfo {
    [HarmonyPatch(typeof(PotionModel), nameof(PotionModel.HoverTip), MethodType.Getter)]
    [HarmonyPostfix]
    static void HideTitleDescription(PotionModel __instance, ref HoverTip __result) {
        if (ModInitializer.Config.PotionNames) {
            __result.Title = "";
        }

        if (ModInitializer.Config.PotionDescriptions) {
            __result.Description = "";
        }
    }
    
    [HarmonyPatch(typeof(PotionModel), nameof(PotionModel.HoverTips), MethodType.Getter)]
    [HarmonyPrefix]
    static bool HideExtraTooltips(PotionModel __instance, ref IEnumerable<IHoverTip> __result) {
        if (ModInitializer.Config.PotionDescriptions) {
            __result = [__instance.HoverTip];
            return false;
        }

        return true;
    }
}
