using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.sts2.Core.Nodes.TopBar;

namespace HiddenInfo2;

[HarmonyPatch]
static class HideHpInfo {
    [HarmonyPatch(typeof(NCharacterSelectScreen), nameof(NCharacterSelectScreen.SelectCharacter))]
    [HarmonyPostfix]
    static void HideCharacterSelect(NCharacterSelectScreen __instance) {
        if (ModInitializer.Config.PlayerHp) {
            __instance._hp.SetTextAutoSize("??/??");
        }
    }
    
    [HarmonyPatch(typeof(NHealthBar), nameof(NHealthBar.RefreshText))]
    [HarmonyPostfix]
    static void HideHpOnBar(NHealthBar __instance) {
        if (__instance._creature.IsPlayer || __instance._creature.IsPet) {
            if (ModInitializer.Config.PlayerHp) {
                __instance._hpLabel.Visible = false;
            }
        }
        else {
            if (ModInitializer.Config.EnemyHp) {
                __instance._hpLabel.Visible = false;
            }
        }
    }

    [HarmonyPatch(typeof(NTopBarHp), nameof(NTopBarHp.UpdateHealth))]
    [HarmonyPostfix]
    static void HideHpTopBar(NTopBarHp __instance) {
        // __instance._hpLabel.Visible = !ModInitializer.Config.PlayerHp;
        if (ModInitializer.Config.PlayerHp) {
            __instance._hpLabel.SetTextAutoSize("??/??");
        }
    }

    [HarmonyPatch(typeof(NContinueRunInfo), nameof(NContinueRunInfo.ShowInfo))]
    [HarmonyPostfix]
    static void HideHpContinueRun(NContinueRunInfo __instance) {
        if (ModInitializer.Config.PlayerHp) {
            __instance._healthLabel.Text = "[red]??/??[/red]";
        }
    }
}
