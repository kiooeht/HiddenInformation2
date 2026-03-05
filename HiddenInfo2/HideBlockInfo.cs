using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace HiddenInfo2;

[HarmonyPatch]
static class HideBlockInfo {
    [HarmonyPatch(typeof(NHealthBar), nameof(NHealthBar.RefreshText))]
    [HarmonyPostfix]
    static void HideBlockOnBar(NHealthBar __instance) {
        if (__instance._creature.IsPlayer || __instance._creature.IsPet) {
            __instance._blockLabel.Visible = !ModInitializer.Config.PlayerBlock;
        }
    }
}
