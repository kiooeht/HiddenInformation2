using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace HiddenInfo2.Patches;

[HarmonyPatch]
static class HideEventInfo {
    [HarmonyPatch(typeof(NEventOptionButton), nameof(NEventOptionButton._Ready))]
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> HideEventOptionEffects(IEnumerable<CodeInstruction> instructions) {
        var codeMatcher = new CodeMatcher(instructions);
        
        codeMatcher
            .MatchEndForward(
                CodeMatch.Calls(typeof(EventOption).Property("Description").GetMethod),
                CodeMatch.Calls(typeof(LocString).Method("GetFormattedText")),
                CodeMatch.StoresLocal()
            )
            .ThrowIfInvalid("Failed to find Description.GetFormattedText()")
            .InsertAndAdvance(
                CodeInstruction.Call<string, string>(text => GetHiddenOptionEffect(text))
            );
        
        return codeMatcher.Instructions();
    }

    private static string GetHiddenOptionEffect(string original) {
        if (ModInitializer.Config.EventOptionEffect) {
            return "";
        }

        return original;
    }

    [HarmonyPatch(typeof(NEventOptionButton), nameof(NEventOptionButton.OnFocus))]
    [HarmonyPostfix]
    static void HideEventOptionTooltips(NEventOptionButton __instance) {
        if (ModInitializer.Config.EventOptionEffect) {
            NHoverTipSet.Remove(__instance);
        }
    }
}
