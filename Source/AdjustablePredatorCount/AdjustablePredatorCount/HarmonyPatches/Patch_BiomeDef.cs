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

    public static class Patch_BiomeDef
    {

        [HarmonyPatch(typeof(BiomeDef))]
        [HarmonyPatch(nameof(BiomeDef.CommonalityOfAnimal))]
        public static class Patch_CommonalityOfAnimal
        {

            public static void Postfix(PawnKindDef animalDef, ref float __result)
            {
                if (animalDef.RaceProps.predator)
                    __result *= AdjustablePredatorCountSettings.PredatorCommonalityFactor;
            }

        }

    }

}
