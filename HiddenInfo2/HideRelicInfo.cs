using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;

namespace HiddenInfo2;

[HarmonyPatch]
static class HideRelicInfo {
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.HoverTip), MethodType.Getter)]
    [HarmonyPostfix]
    static void HideTitleDescription(RelicModel __instance, ref HoverTip __result) {
        // __result.Title = "";
        __result.Description = "";
    }
    
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.HoverTips), MethodType.Getter)]
    [HarmonyPrefix]
    static bool HideExtraTooltips(RelicModel __instance, ref IEnumerable<IHoverTip> __result) {
        __result = [__instance.HoverTip];
        return false;
    }

    [HarmonyPatch(typeof(NInspectRelicScreen), nameof(NInspectRelicScreen._Ready))]
    [HarmonyPostfix]
    static void HideInspectDescription(NInspectRelicScreen __instance) {
        __instance._description.Visible = false;
        __instance._flavor.Visible = false;
    }

    [HarmonyPatch(typeof(NInspectRelicScreen), nameof(NInspectRelicScreen.UpdateRelicDisplay))]
    [HarmonyPostfix]
    static void HideInspectTooltips(NInspectRelicScreen __instance) {
        NHoverTipSet.Clear();
    }
}
