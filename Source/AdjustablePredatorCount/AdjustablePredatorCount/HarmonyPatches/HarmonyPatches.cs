using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using Harmony;

namespace AdjustablePredatorCount
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            AdjustablePredatorCount.HarmonyInstance.PatchAll();
        }

    }
}
