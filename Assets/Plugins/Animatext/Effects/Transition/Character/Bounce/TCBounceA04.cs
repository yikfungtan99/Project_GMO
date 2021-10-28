﻿// Copyright (C) 2020 - 2021 Seeley Studio. All Rights Reserved.

#pragma warning disable IDE0090

using UnityEngine;

namespace Animatext.Effects
{
    [CreateAssetMenu(menuName = "Animatext Preset/Transition - Character/Bounce/Bounce - A04", fileName = "New TCBounceA04 Preset", order = 369)]
    public sealed class TCBounceA04 : DefaultTemplateEffect
    {
        public float singleTime = 1;
        public SortType sortType;
        public Vector2 skew = new Vector2(0, 45);
        public AnchorType anchorType = AnchorType.Center;
        public Vector2 anchorOffset = Vector2.zero;
        public int bounces = 2;
        public float bounciness = 0.5f;
        public EasingType easingType;
        public ColorMode fadeMode = ColorMode.Multiply;
        public FloatRange fadeRange = new FloatRange(0, 0.25f);

        public override InfoFlags infoFlags
        {
            get { return InfoFlags.Character; }
        }

        protected override int unitCount
        {
            get { return characterCount; }
        }

        protected override float unitTime
        {
            get { return singleTime; }
        }

        protected override void Animate()
        {
            for (int i = 0; i < characterCount; i++)
            {
                float progress = GetCurrentProgress(SortUtility.Rank(i, characterCount, sortType));

                if (progress <= fadeRange.start)
                {
                    characters[i].Opacify(0, fadeMode);
                }
                else
                {
                    if (progress >= fadeRange.end)
                    {
                        characters[i].Opacify(1, fadeMode);
                    }
                    else
                    {
                        characters[i].Opacify(Mathf.InverseLerp(fadeRange.start, fadeRange.end, progress), fadeMode);
                    }

                    progress = 1 - EasingUtility.Ease(progress, easingType);
                    progress = EasingUtility.EaseBounce(progress, bounces, bounciness);

                    characters[i].Skew(skew * progress, characters[i].GetAnchorPoint(anchorType) + anchorOffset);
                }
            }
        }
    }
}