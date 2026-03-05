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
        if (ModInitializer.Config.RelicName) {
            __result.Title = "";
        }

        if (ModInitializer.Config.RelicDescription) {
            __result.Description = "";
        }
    }
    
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.HoverTips), MethodType.Getter)]
    [HarmonyPrefix]
    static bool HideExtraTooltips(RelicModel __instance, ref IEnumerable<IHoverTip> __result) {
        if (ModInitializer.Config.RelicDescription) {
            __result = [__instance.HoverTip];
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(NInspectRelicScreen), nameof(NInspectRelicScreen.UpdateRelicDisplay))]
    [HarmonyPostfix]
    static void HideInspect(NInspectRelicScreen __instance) {
        __instance._nameLabel.Visible = !ModInitializer.Config.RelicName;
        __instance._description.Visible = !ModInitializer.Config.RelicDescription;
        __instance._flavor.Visible = !ModInitializer.Config.RelicFlavor;
        if (ModInitializer.Config.RelicDescription) {
            NHoverTipSet.Clear();
        }
    }
}
