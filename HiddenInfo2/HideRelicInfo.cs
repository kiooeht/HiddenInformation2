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
        if (ModInitializer.Config.RelicNames) {
            __result.Title = "";
        }

        if (ModInitializer.Config.RelicDescriptions) {
            __result.Description = "";
        }
    }
    
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.HoverTips), MethodType.Getter)]
    [HarmonyPrefix]
    static bool HideExtraTooltips(RelicModel __instance, ref IEnumerable<IHoverTip> __result) {
        if (ModInitializer.Config.RelicDescriptions) {
            __result = [__instance.HoverTip];
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(NInspectRelicScreen), nameof(NInspectRelicScreen.UpdateRelicDisplay))]
    [HarmonyPostfix]
    static void HideInspect(NInspectRelicScreen __instance) {
        var hideNames = ModInitializer.Config.RelicNames;
        var hideDescriptions = ModInitializer.Config.RelicDescriptions;

        __instance._nameLabel.Visible = !hideNames;
        
        __instance._description.Visible = !hideDescriptions;
        __instance._flavor.Visible = !hideDescriptions;
        if (hideDescriptions) {
            NHoverTipSet.Clear();
        }
    }
}
