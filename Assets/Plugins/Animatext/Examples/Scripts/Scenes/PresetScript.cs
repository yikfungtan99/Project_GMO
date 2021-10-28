// Copyright (C) 2020 - 2021 Seeley Studio. All Rights Reserved.

using Animatext.Effects;
using UnityEngine;

namespace Animatext.Examples
{
    public class PresetScript : BaseExampleScript
    {
        public GameObject titleA1;
        public GameObject titleA2;
        public GameObject titleB1;
        public GameObject titleB2;
        public GameObject titleB3;
        public GameObject titleB4;
        public GameObject titleB5;
        public GameObject titleC1;
        public GameObject titleC2;
        public GameObject titleC3;
        public GameObject titleC4;
        public GameObject titleC5;

        private void Start()
        {
            SetExampleA1(titleA1);
            SetExampleA2(titleA2);
            SetExampleB1(titleB1);
            SetExampleB2(titleB2);
            SetExampleB3(titleB3);
            SetExampleB4(titleB4);
            SetExampleB5(titleB5);
            SetExampleC1(titleC1);
            SetExampleC2(titleC2);
            SetExampleC3(titleC3);
            SetExampleC4(titleC4);
            SetExampleC5(titleC5);
        }

        private void SetExampleA1(GameObject gameObject)
        {
            if (gameObject == null) return;

            TRBasicA01 preset = ScriptableObject.CreateInstance<TRBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1;
            preset.singleTime = 1;
            preset.position = new Vector2(0, 30);
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleA2(GameObject gameObject)
        {
            if (gameObject == null) return;

            CRBasicA01 preset = ScriptableObject.CreateInstance<CRBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1;
            preset.singleTime = 1;
            preset.startPosition = Vector2.zero;
            preset.position = new Vector2(0, 30);
            preset.easingType = EasingType.Linear;
            preset.continuousEasing = true;

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleB1(GameObject gameObject)
        {
            if (gameObject == null) return;

            TRBasicA01 preset = ScriptableObject.CreateInstance<TRBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1;
            preset.singleTime = 1;
            preset.position = new Vector2(0, 30);
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleB2(GameObject gameObject)
        {
            if (gameObject == null) return;

            TRBounceA03 preset = ScriptableObject.CreateInstance<TRBounceA03>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1;
            preset.singleTime = 1;
            preset.scale = Vector2.zero;
            preset.anchorType = AnchorType.Center;
            preset.anchorOffset = Vector2.zero;
            preset.bounces = 2;
            preset.bounciness = 0.5f;
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleB3(GameObject gameObject)
        {
            if (gameObject == null) return;

            TRElasticA02 preset = ScriptableObject.CreateInstance<TRElasticA02>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1;
            preset.singleTime = 1;
            preset.rotation = 30;
            preset.anchorType = AnchorType.Center;
            preset.anchorOffset = Vector2.zero;
            preset.oscillations = 2;
            preset.stiffness = 5;
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleB4(GameObject gameObject)
        {
            if (gameObject == null) return;

            TRBackA03 preset = ScriptableObject.CreateInstance<TRBackA03>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1;
            preset.singleTime = 1;
            preset.scale = Vector2.zero;
            preset.anchorType = AnchorType.Center;
            preset.anchorOffset = Vector2.zero;
            preset.amplitude = 2;
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleB5(GameObject gameObject)
        {
            if (gameObject == null) return;

            CRWaveA02 preset = ScriptableObject.CreateInstance<CRWaveA02>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 1;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1;
            preset.singleTime = 1;
            preset.startRotation = 0;
            preset.rotation = 9;
            preset.anchorType = AnchorType.Center;
            preset.anchorOffset = Vector2.zero;
            preset.waves = 1;
            preset.easingType = EasingType.Linear;

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleC1(GameObject gameObject)
        {
            if (gameObject == null) return;

            TCBasicA01 preset = ScriptableObject.CreateInstance<TCBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 0.5f;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 0.0625f;
            preset.singleTime = 0.0625f;
            preset.sortType = SortType.FrontToBack;
            preset.position = new Vector2(9, 0);
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleC2(GameObject gameObject)
        {
            if (gameObject == null) return;

            TWBasicA01 preset = ScriptableObject.CreateInstance<TWBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 0.5f;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 0.375f;
            preset.singleTime = 0.375f;
            preset.sortType = SortType.FrontToBack;
            preset.position = new Vector2(15, 0);
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleC3(GameObject gameObject)
        {
            if (gameObject == null) return;

            TLBasicA01 preset = ScriptableObject.CreateInstance<TLBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 0.5f;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 0.75f;
            preset.singleTime = 0.75f;
            preset.sortType = SortType.FrontToBack;
            preset.position = new Vector2(15, 0);
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }

        private void SetExampleC4(GameObject gameObject)
        {
            if (gameObject == null) return;

            TGBasicA01 preset = ScriptableObject.CreateInstance<TGBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 0.5f;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 0.125f;
            preset.singleTime = 0.125f;
            preset.sortType = SortType.FrontToBack;
            preset.position = new Vector2(9, 0);
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, "<g>A</g><g>nim</g><g>a</g><g>ted</g> <color=\"#3087db\"><g>Te</g><g>xt</g></color>\n<g>An</g><g>i</g><g>mat</g><g>ed</g> <color=\"#3087db\"><g>Tex</g><g>t</g></color>", preset);
        }

        private void SetExampleC5(GameObject gameObject)
        {
            if (gameObject == null) return;

            TRBasicA01 preset = ScriptableObject.CreateInstance<TRBasicA01>();

            preset.tag = string.Empty;
            preset.startInterval = 0;
            preset.reverse = false;
            preset.loopCount = 0;
            preset.loopInterval = 0.5f;
            preset.loopBackInterval = 0;
            preset.pingpongLoop = false;
            preset.continuousLoop = false;
            preset.interval = 1.5f;
            preset.singleTime = 1.5f;
            preset.position = new Vector2(15, 0);
            preset.easingType = EasingType.Linear;
            preset.fadeMode = ColorMode.Multiply;
            preset.fadeRange = new FloatRange(0, 0.5f);

            AddAnimatext(gameObject, preset);
        }
    }
}