using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Harmony;
using UnityEngine;

namespace AdjustablePredatorCount
{

    public class AdjustablePredatorCount : Mod
    {
        public static AdjustablePredatorCountSettings settings;

        public AdjustablePredatorCount(ModContentPack content) : base(content)
        {
            settings = GetSettings<AdjustablePredatorCountSettings>();
            HarmonyInstance = HarmonyInstance.Create("XeoNovaDan.AdjustablePredatorCount");
        }

        public override string SettingsCategory() => "AdjustablePredatorCount.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }

        public static HarmonyInstance HarmonyInstance;

    }
}
