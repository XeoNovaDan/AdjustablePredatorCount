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
    static class HarmonyPatches
    {
        private static float predatorCommonalityFactor = AdjustablePredatorSettings.predatorCommonalityFactor;

        static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            HarmonyInstance h = HarmonyInstance.Create("XeoNovaDan.AdjustablePredatorCount");

            // The following line was the real MVP for helping debug part 2 of the transpiler:
            // HarmonyInstance.DEBUG = true;

            h.Patch(AccessTools.Method(typeof(BiomeDef), "CommonalityOfAnimal"), null, null,
                new HarmonyMethod(patchType, "TranspileCommonalityOfAnimal"));
        }

        public static IEnumerable<CodeInstruction> TranspileCommonalityOfAnimal(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();
            MethodInfo commonalityGetterInfo = AccessTools.Method(patchType, "AdjustCommonalityForPredator");
            MethodInfo defCommonalityGetterInfo = AccessTools.Method(patchType, "AdjustCommonalityForPredatorFromPawn");
            MethodInfo racePropsGetter = AccessTools.Property(typeof(PawnKindDef), "RaceProps").GetGetMethod();

            int varIndexesFound = 0;
            int varIndexesFound2 = 0;
            bool doneFirstOperation = false;
            bool doneSecondOperation = false;

            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];

                // Part 1

                if (!doneFirstOperation && instruction.opcode == OpCodes.Callvirt && varIndexesFound == 1)
                {
                    instructionList[(i + 1)].opcode = OpCodes.Call;
                    instructionList[(i + 1)].operand = commonalityGetterInfo;
                    doneFirstOperation = true;
                }

                if (!doneFirstOperation && instruction.opcode == OpCodes.Callvirt && instructionList[(i - 1)].opcode == OpCodes.Ldloc_0)
                {
                    varIndexesFound++;
                }

                // Part 2

                if (!doneSecondOperation && instruction.opcode == OpCodes.Callvirt && instruction.operand == racePropsGetter && varIndexesFound2 == 2)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_3);
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = defCommonalityGetterInfo;
                    for (int x = 1; x < 5; x++)
                    {
                        instructionList[(i + x)].opcode = OpCodes.Nop;
                    }
                    doneSecondOperation = true;
                }

                if (!doneSecondOperation && instruction.opcode == OpCodes.Callvirt && instruction.operand == racePropsGetter)
                {
                    varIndexesFound2++;
                }

                yield return instruction;
            }
        }

        private static float AdjustCommonalityForPredator(BiomeAnimalRecord currentRecord)
        {
            float commonality = currentRecord.commonality;
            if (currentRecord.animal.RaceProps.predator)
            {
                commonality *= predatorCommonalityFactor;
            }
            return commonality;
        }

        private static float AdjustCommonalityForPredatorFromPawn(PawnKindDef animalDef, int j)
        {
            float commonality = animalDef.RaceProps.wildBiomes[j].commonality;
            if (animalDef.RaceProps.predator)
            {
                commonality *= predatorCommonalityFactor;
            }
            return commonality;
        }

    }
}
