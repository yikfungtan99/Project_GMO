// Copyright (C) 2020 - 2021 Seeley Studio. All Rights Reserved.

using Animatext.Effects;
using UnityEngine;

namespace Animatext.Examples
{
    public class FadeModeScript : BaseExampleScript
    {
        public GameObject titleA;
        public GameObject titleB;
        public GameObject titleC;
        public GameObject titleD;

        private void Start()
        {
            SetExample(titleA, ColorMode.Add);
            SetExample(titleB, ColorMode.Difference);
            SetExample(titleC, ColorMode.Multiply);
            SetExample(titleD, ColorMode.Replace);
        }

        private void SetExample(GameObject gameObject, ColorMode colorMode)
        {
            if (gameObject == null) return;

            TRFadeA01 preset = ScriptableObject.CreateInstance<TRFadeA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 0.5f;
            preset.singleTime = 2;
            preset.easingType = EasingType.Linear;
            preset.fadeMode = colorMode;

            AddAnimatext(gameObject, preset);
        }
    }
}