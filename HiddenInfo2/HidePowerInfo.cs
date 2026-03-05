using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace HiddenInfo2;

[HarmonyPatch]
static class HidePowerInfo {
    [HarmonyPatch(typeof(PowerModel), nameof(Creature.HoverTips), MethodType.Getter)]
    [HarmonyPostfix]
    static void HidePowerTooltips(ref IEnumerable<IHoverTip> __result) {
        var hoverTips = __result.ToList();
        if (hoverTips.Count == 0) return;

        if (hoverTips[0] is HoverTip tip) {
            if (ModInitializer.Config.PowerName) {
                tip.Title = "";
            }

            if (ModInitializer.Config.PowerDescription) {
                tip.Description = "";
            }

            if (ModInitializer.Config.PowerType) {
                tip.IsDebuff = false;
            }

            if (ModInitializer.Config.PowerName || ModInitializer.Config.PowerDescription || ModInitializer.Config.PowerType) {
                hoverTips[0] = tip;
                __result = hoverTips;
            }
        }
    }

    [HarmonyPatch(typeof(NPowerAppliedVfx), nameof(NPowerAppliedVfx.StartVfx))]
    [HarmonyPostfix]
    static void HideNamePowerAppliedVfx(NPowerAppliedVfx __instance) {
        if (ModInitializer.Config.PowerName) {
            __instance._powerField.SetTextAutoSize("???");
        }
    }

    [HarmonyPatch(typeof(NPowerRemovedVfx), nameof(NPowerRemovedVfx.StartVfx))]
    [HarmonyPostfix]
    static void HideNamePowerRemovedVfx(NPowerRemovedVfx __instance) {
        if (ModInitializer.Config.PowerName) {
            __instance._powerField.SetTextAutoSize("???");
        }
    }
}
