using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Harmony;
using UnityEngine;
using SettingsHelper;

namespace AdjustablePredatorCount
{
    public class AdjustablePredatorCountSettings : ModSettings
    {

        private static float _basePredatorCommonalityFactor = 1;
        public static float PredatorCommonalityFactor
        {
            get
            {
                float offsetBase = _basePredatorCommonalityFactor - 1;
                if (offsetBase < 0)
                    return 1 + offsetBase;
                return Mathf.Pow(10, offsetBase);
            }
            set => _basePredatorCommonalityFactor = value;
        }

        private string PredatorCommonalityFactorFormatString
        {
            get
            {
                if (PredatorCommonalityFactor < 10)
                    return "F2";
                if (PredatorCommonalityFactor < 100)
                    return "F1";
                return "F0";
            }
        }

        public void DoWindowContents(Rect wrect)
        {

            Listing_Standard options = new Listing_Standard();
            Color defaultColor = GUI.color;
            options.Begin(wrect);

            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            // Predator commonality multiplier
            options.Gap();
            options.AddLabeledSlider("AdjustablePredatorCount.PredatorCommonalityMultiplier".Translate(), ref _basePredatorCommonalityFactor, 0, 4,
                rightAlignedLabel: PredatorCommonalityFactor.ToString(PredatorCommonalityFactorFormatString), roundTo: 0.005f);

            // Finish writing
            options.End();
            Mod.GetSettings<AdjustablePredatorCountSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref _basePredatorCommonalityFactor, "basePredatorCommonalityFactor", 1);
        }

    }

}
