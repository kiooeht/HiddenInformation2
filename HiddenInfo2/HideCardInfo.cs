using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace HiddenInfo2;

[HarmonyPatch]
static class HideCardInfo {
    [HarmonyPatch(typeof(NCard), nameof(NCard.Reload))]
    [HarmonyPostfix]
    static void HideText(NCard __instance) {
        if (!__instance.IsNodeReady() || __instance.Model == null) {
            return;
        }
        
        __instance._titleLabel.Visible = !ModInitializer.Config.CardName;
        __instance._descriptionLabel.Visible = !ModInitializer.Config.CardDescription;
        __instance._typeLabel.Visible = false;
        __instance._energyLabel.Visible = !ModInitializer.Config.CardCost;
        __instance._starLabel.Visible = !ModInitializer.Config.CardCost;

        var flag = __instance.Model.Rarity == CardRarity.Ancient;
        __instance._typePlaque.Visible = !flag;
        if (flag) {
            __instance._ancientBanner.Visible = false;
            __instance._ancientTextBg.Visible = false;
        }
    }
    
    [HarmonyPatch(typeof(NCard), nameof(NCard.UpdateEnergyCostVisuals))]
    [HarmonyPostfix]
    static void HideEnergyIcon(NCard __instance) {
        if (__instance.Model is { Rarity: CardRarity.Ancient }) {
            // __instance._energyIcon.Visible = false;
            // __instance._unplayableEnergyIcon.Visible = false;
        }
    }
    
    [HarmonyPatch(typeof(NCard), nameof(NCard.UpdateStarCostVisuals))]
    [HarmonyPostfix]
    static void HideStarIcon(NCard __instance) {
        if (__instance.Model is { Rarity: CardRarity.Ancient }) {
            // __instance._starIcon.Visible = false;
            // __instance._unplayableStarIcon.Visible = false;
        }
    }

    [HarmonyPatch(typeof(NCardHolder), nameof(NCardHolder.CreateHoverTips))]
    [HarmonyPrefix]
    static bool HideTooltips() {
        return !ModInitializer.Config.CardDescription;
    }

    [HarmonyPatch(typeof(NPreviewCardHolder), nameof(NCardHolder.CreateHoverTips))]
    [HarmonyPrefix]
    static bool HidePreviewTooltips() {
        return !ModInitializer.Config.CardDescription;
    }
    
    [HarmonyPatch(typeof(NInspectCardScreen), nameof(NInspectCardScreen.UpdateCardDisplay))]
    [HarmonyPostfix]
    static void HideInspectTooltips(NInspectCardScreen __instance) {
        if (ModInitializer.Config.CardDescription) {
            NHoverTipSet.Clear();
        }
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.BannerMaterialPath), MethodType.Getter)]
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> HideRarity(IEnumerable<CodeInstruction> instructions) {
        var codeMatcher = new CodeMatcher(instructions);
        
        codeMatcher
            .MatchEndForward(
                new CodeMatch(OpCodes.Ldarg_0),
                CodeMatch.Calls(typeof(CardModel).Property("Rarity").GetMethod),
                CodeMatch.StoresLocal()
            )
            .ThrowIfInvalid("Failed to find get_Rarity")
            .InsertAndAdvance(
                CodeInstruction.Call<CardRarity, CardRarity>(rarity => GetHiddenRarity(rarity))
            );
        
        return codeMatcher.Instructions();
    }

    private static CardRarity GetHiddenRarity(CardRarity original) {
        if (ModInitializer.Config.CardRarity) {
            return CardRarity.Common;
        }

        return original;
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.PortraitBorderPath), MethodType.Getter)]
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> HideTypeBorder(IEnumerable<CodeInstruction> instructions) {
        return HideType("Failed to find card border string", instructions);
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.FramePath), MethodType.Getter)]
    [HarmonyTranspiler]
    [HarmonyDebug]
    static IEnumerable<CodeInstruction> HideTypeFrame(IEnumerable<CodeInstruction> instructions) {
        return HideType("Failed to find card frame string", instructions);
    }
    
    private static IEnumerable<CodeInstruction> HideType(string errorMsg, IEnumerable<CodeInstruction> instructions) {
        var codeMatcher = new CodeMatcher(instructions);

        codeMatcher
            .MatchEndForward(
                CodeMatch.LoadsConstant(),
                CodeMatch.StoresLocal()
            )
            .ThrowIfInvalid("Failed to find cardType local index");
        var index = codeMatcher.Instruction.LocalIndex();
        
        codeMatcher
            .MatchStartForward(
                new CodeMatch(OpCodes.Ldloca_S)
            )
            .ThrowIfInvalid(errorMsg)
            .InsertAndAdvance(
                CodeInstruction.LoadLocal(index),
                CodeInstruction.Call<CardType, CardType>(type => GetHiddenType(type)),
                CodeInstruction.StoreLocal(index)
            );
        
        return codeMatcher.Instructions();
    }

    private static CardType GetHiddenType(CardType original) {
        if (ModInitializer.Config.CardType) {
            return CardType.Skill;
        }

        return original;
    }
}
