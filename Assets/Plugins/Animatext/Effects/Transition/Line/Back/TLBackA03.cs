﻿// Copyright (C) 2020 - 2021 Seeley Studio. All Rights Reserved.

#pragma warning disable IDE0090

using UnityEngine;

namespace Animatext.Effects
{
    [CreateAssetMenu(menuName = "Animatext Preset/Transition - Line/Back/Back - A03", fileName = "New TLBackA03 Preset", order = 369)]
    public sealed class TLBackA03 : DefaultTemplateEffect
    {
        public float singleTime = 1;
        public SortType sortType;
        public Vector2 scale = Vector2.zero;
        public AnchorType anchorType = AnchorType.Center;
        public Vector2 anchorOffset = Vector2.zero;
        public float amplitude = 2;
        public EasingType easingType;
        public ColorMode fadeMode = ColorMode.Multiply;
        public FloatRange fadeRange = new FloatRange(0, 0.25f);

        public override InfoFlags infoFlags
        {
            get { return InfoFlags.Line; }
        }

        protected override int unitCount
        {
            get { return lineCount; }
        }

        protected override float unitTime
        {
            get { return singleTime; }
        }

        protected override void Animate()
        {
            for (int i = 0; i < lineCount; i++)
            {
                float progress = GetCurrentProgress(SortUtility.Rank(i, lineCount, sortType));

                if (progress <= fadeRange.start)
                {
                    lines[i].Opacify(0, fadeMode);
                }
                else
                {
                    if (progress >= fadeRange.end)
                    {
                        lines[i].Opacify(1, fadeMode);
                    }
                    else
                    {
                        lines[i].Opacify(Mathf.InverseLerp(fadeRange.start, fadeRange.end, progress), fadeMode);
                    }

                    progress = 1 - EasingUtility.Ease(progress, easingType);
                    progress = EasingUtility.EaseBack(progress, amplitude);

                    lines[i].Scale(Vector2.LerpUnclamped(Vector2.one, scale, progress), lines[i].GetAnchorPoint(anchorType) + anchorOffset);
                }
            }
        }
    }
}