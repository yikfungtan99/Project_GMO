﻿// Copyright (C) 2020 - 2021 Seeley Studio. All Rights Reserved.

#pragma warning disable IDE0090

using UnityEngine;

namespace Animatext.Effects
{
    [CreateAssetMenu(menuName = "Animatext Preset/Coherence - Line/Wave/Wave - A04", fileName = "New CLWaveA04 Preset", order = 369)]
    public sealed class CLWaveA04 : DefaultTemplateEffect
    {
        public float singleTime = 1;
        public SortType sortType;
        public Vector2 startSkew = Vector2.zero;
        public Vector2 skew = new Vector2(0, 45);
        public AnchorType anchorType = AnchorType.Center;
        public Vector2 anchorOffset = Vector2.zero;
        public int waves = 1;
        public EasingType easingType;

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

                progress = EasingUtility.Ease(progress, easingType);
                progress = EasingUtility.Wave(progress, waves);

                lines[i].Skew(Vector2.LerpUnclamped(startSkew, skew, progress), lines[i].GetAnchorPoint(anchorType) + anchorOffset);
            }
        }
    }
}