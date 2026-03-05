using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace HiddenInfo2;

[HarmonyPatch(typeof(NEnergyCounter), nameof(NEnergyCounter.RefreshLabel))]
static class HideEnergyInfo {
    [HarmonyPostfix]
    static void HideEnergy(NEnergyCounter __instance) {
        __instance._label.Visible = !ModInitializer.Config.PlayerEnergy;
    }
}
