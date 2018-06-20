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
    public class AdjustablePredatorSettings : ModSettings
    {

        public static float predatorCommonalityFactor = 1f;

        public static string predatorCommonalityFactorBuffer = predatorCommonalityFactor.ToString();


        public void DoWindowContents(Rect wrect)
        {

            Listing_Standard options = new Listing_Standard();
            Color defaultColor = GUI.color;
            options.Begin(wrect);

            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            options.Gap();
            Text.Font = GameFont.Medium;
            options.Label("SettingPredatorCommonalityFactorLabel".Translate());
            Text.Font = GameFont.Small;
            options.Gap(4);
            options.TextFieldNumeric(ref predatorCommonalityFactor, ref predatorCommonalityFactorBuffer);
            options.Gap(6);
            GUI.color = Color.grey;
            options.Label("SettingPredatorCommonalityFactorTooltip".Translate());
            GUI.color = defaultColor;

            options.End();

            Mod.GetSettings<AdjustablePredatorSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref predatorCommonalityFactor, "predatorCommonalityFactor", 1);
        }

    }

    public class AdjustablePredatorCount : Mod
    {
        public static AdjustablePredatorSettings settings;

        public AdjustablePredatorCount(ModContentPack content) : base(content)
        {
            settings = GetSettings<AdjustablePredatorSettings>();
        }

        public override string SettingsCategory() => "AdjustablePredatorCountTitle".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<AdjustablePredatorSettings>().DoWindowContents(inRect);
        }
    }
}
