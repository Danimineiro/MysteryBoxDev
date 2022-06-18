using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MysteryBoxFrameWork
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatcher
    {
        private static readonly Harmony harmony = new Harmony("dani.MysteryItemFrameWork");
        private static readonly Dictionary<ThingDef, ItemReplacerDef> toReplace = new Dictionary<ThingDef, ItemReplacerDef>();

        static HarmonyPatcher()
        {
            foreach (ItemReplacerDef itemReplacerDef in DefDatabase<ItemReplacerDef>.AllDefsListForReading)
            {
                if (!toReplace.TryAdd(itemReplacerDef.targetItem, itemReplacerDef))
                {
                    Log.Error($"ThingDef: {itemReplacerDef.targetItem} has two ItemReplacerDefs targeting it, when at most one will work!");
                }
            }

            Log.Message($"<color=orange>[MysteryBoxFrameWork]</color> Hello world!");
            harmony.Patch(typeof(GenRecipe).GetMethod(nameof(GenRecipe.MakeRecipeProducts)), postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(MakeRecipeProductsPatch)));
        }

        private static void MakeRecipeProductsPatch(ref IEnumerable<Thing> __result)
        {
            __result = ReplaceResult(__result);
        }

        private static IEnumerable<Thing> ReplaceResult(IEnumerable<Thing> result)
        {
            foreach (Thing thing in result)
            {
                if (toReplace.ContainsKey(thing.def))
                {
                    yield return toReplace[thing.def].GenRandomThing;
                    continue;
                }

                yield return thing;
            }
        }
    }
}
