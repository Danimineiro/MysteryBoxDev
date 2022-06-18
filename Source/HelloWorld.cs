using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MysteryBoxFrameWork
{
    [StaticConstructorOnStartup]
    public static class HelloWorld
    {
        static HelloWorld()
        {
            Log.Message($"<color=orange>[MysteryBoxFrameWork]</color> Hello world!");
        }
    }
}
