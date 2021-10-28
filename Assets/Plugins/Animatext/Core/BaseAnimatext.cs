// Copyright (C) 2020 - 2021 Seeley Studio. All Rights Reserved.

#pragma warning disable IDE0066, IDE0090, IDE1006

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animatext
{
    [DefaultExecutionOrder(999)]
    [DisallowMultipleComponent]
    public abstract class BaseAnimatext : MonoBehaviour
    {
        private readonly static string _version = "1.0.0";

        private TextInfo _textInfo;
        private Func<string, StringInfo> _outputTextInfoFunc;
        [SerializeField] private string _inputText = string.Empty;
        [SerializeField] private string _parsedText = string.Empty;
        [SerializeField] private string _outputText = string.Empty;
        [SerializeField] private string _effectText = string.Empty;
        [SerializeField] private string _retainedText = string.Empty;
        [SerializeField] private Settings _settings = new Settings();
        [SerializeField] private List<Effect> _effects = new List<Effect>();

        /// <summary>
        /// The version of Animatext.
        /// </summary>
        public static string version
        {
            get { return _version; }
        }

        /// <summary>
        /// The effect list.
        /// </summary>
        public List<Effect> effects
        {
            get { return _effects; }
        }

        /// <summary>
        /// The text containing the effect.
        /// </summary>
        public string effectText
        {
            get { return _effectText; }
        }

        /// <summary>
        /// The text initially entered in the text component.
        /// </summary>
        public string inputText
        {
            get { return _inputText; }
        }

        /// <summary>
        /// The parsed text of the text component.
        /// </summary>
        public string outputText
        {
            get { return _outputText; }
        }

        /// <summary>
        /// The parsed text of the Animatext component.
        /// </summary>
        public string parsedText
        {
            get { return _parsedText; }
        }

        /// <summary>
        /// The retained text of the text component at runtime.
        /// </summary>
        public string retainedText
        {
            get { return _retainedText; }
        }

        /// <summary>
        /// The settings of the Animatext component.
        /// </summary>
        public Settings settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// The text of the text component.
        /// </summary>
        public abstract string text { get; set; }
        
        #region <- MonoBehaviour Methods ->

        protected virtual void Awake() { }

        protected virtual void OnEnable()
        {
            if (_textInfo == null)
            {
                _textInfo = new TextInfo();
            }

            if (_outputTextInfoFunc == null)
            {
                _outputTextInfoFunc = new Func<string, StringInfo>(GetOutputTextInfo);
            }

            _inputText = text ?? string.Empty;
        }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void LateUpdate() { }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                SetComponentDirty();
                return;
            }
#endif
            switch (settings.disabledText)
            {
                case DisabledText.InputText:
                    text = _inputText;
                    SetComponentDirty();
                    break;

                case DisabledText.ParsedText:
                    text = _parsedText;
                    SetComponentDirty();
                    break;

                case DisabledText.OutputText:
                    text = _outputText;
                    SetComponentDirty();
                    break;

                case DisabledText.EffectText:
                    text = _effectText;
                    SetComponentDirty();
                    break;

                case DisabledText.BlankText:
                    text = string.Empty;
                    SetComponentDirty();
                    break;

                default:
                    break;
            }

            switch (settings.disabledEffects)
            {
                case DisabledEffects.Stop:
                    StopEffects();
                    break;

                case DisabledEffects.Refresh:
                    RefreshEffects();
                    break;

                case DisabledEffects.Clear:
                    _effects.Clear();
                    break;

                default:
                    break;
            }
        }

        protected virtual void OnDestroy() { }

        #endregion

        /// <summary>
        /// Method to check whether the text in the text component has been modified.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckTextDirty();

        /// <summary>
        /// Method to execute the text animations.
        /// </summary>
        protected void Execute()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Execute();
            }

            _textInfo.Execute();

            if (_textInfo.dataDirty)
            {
                if (_textInfo.positionsDirty)
                {
                    for (int i = 0; i < _textInfo.charCount; i++)
                    {
                        if (_textInfo.charInfo[i].positionGroup.isDirty)
                        {
                            SetCharPositions(_textInfo.charInfo[i].outputIndex, _textInfo.charInfo[i].positionGroup.currentPositions);
                        }
                    }
                }

                if (_textInfo.colorsDirty)
                {
                    for (int i = 0; i < _textInfo.charCount; i++)
                    {
                        if (_textInfo.charInfo[i].colorGroup.isDirty)
                        {
                            SetCharColors(_textInfo.charInfo[i].outputIndex, _textInfo.charInfo[i].colorGroup.currentColors);
                        }
                    }
                }

                UpdateComponentData();
            }
        }

        /// <summary>
        /// Method to additionally execute the text animations. This method is called when the text animations will be executed again in the same frame.
        /// </summary>
        protected void ExtraExecute()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].ExtraExecute();
            }

            _textInfo.Execute();

            if (_textInfo.dataDirty)
            {
                if (_textInfo.positionsDirty)
                {
                    for (int i = 0; i < _textInfo.charCount; i++)
                    {
                        if (_textInfo.charInfo[i].positionGroup.isDirty)
                        {
                            SetCharPositions(_textInfo.charInfo[i].outputIndex, _textInfo.charInfo[i].positionGroup.currentPositions);
                        }
                    }
                }

                if (_textInfo.colorsDirty)
                {
                    for (int i = 0; i < _textInfo.charCount; i++)
                    {
                        if (_textInfo.charInfo[i].colorGroup.isDirty)
                        {
                            SetCharColors(_textInfo.charInfo[i].outputIndex, _textInfo.charInfo[i].colorGroup.currentColors);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to get the color array of the character vertices.
        /// </summary>
        /// <param name="outputIndex">The character index of the output text.</param>
        /// <returns></returns>
        protected abstract Color[] GetCharColors(int outputIndex);

        /// <summary>
        /// Method to get the position array of the character vertices.
        /// </summary>
        /// <param name="outputIndex">The character index of the output text.</param>
        /// <returns></returns>
        protected abstract Vector3[] GetCharPositions(int outputIndex);

        /// <summary>
        /// Method to get the origin text of the text component.
        /// </summary>
        /// <param name="inputText">The text initially entered in the text component.</param>
        /// <returns></returns>
        protected virtual string GetOriginText(string inputText)
        {
            return inputText;
        }

        /// <summary>
        /// Method to get the parsed string information of the output text.
        /// </summary>
        /// <param name="parsedText">The parsed text of the Animatext component.</param>
        /// <returns></returns>
        protected virtual StringInfo GetOutputTextInfo(string parsedText)
        {
            return StringInfo.GetStringInfo(parsedText);
        }

        /// <summary>
        /// Method to set the color array of the character vertices.
        /// </summary>
        /// <param name="outputIndex">The character index of the output text.</param>
        /// <param name="colors">The color array of the character vertices.</param>
        protected abstract void SetCharColors(int outputIndex, Color[] colors);

        /// <summary>
        /// Method to set the position array of the character vertices.
        /// </summary>
        /// <param name="outputIndex">The character index of the output text.</param>
        /// <param name="positions">The position array of the character vertices.</param>
        protected abstract void SetCharPositions(int outputIndex, Vector3[] positions);

        /// <summary>
        /// Method to mark the text component as dirty.
        /// </summary>
        protected abstract void SetComponentDirty();

        /// <summary>
        /// Method to update the text vertex data in the text component.
        /// </summary>
        protected abstract void UpdateComponentData();

        /// <summary>
        /// Method to update the text vertex data in the text information.
        /// </summary>
        protected virtual void UpdateData()
        {
            for (int i = 0; i < _textInfo.charCount; i++)
            {
                _textInfo.charInfo[i].SetPositions(GetCharPositions(_textInfo.charInfo[i].outputIndex));
            }

            for (int i = 0; i < _textInfo.charCount; i++)
            {
                _textInfo.charInfo[i].SetColors(GetCharColors(_textInfo.charInfo[i].outputIndex));
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                _inputText = string.Empty;
                _parsedText = string.Empty;
                _outputText = string.Empty;
                _effectText = string.Empty;
                _retainedText = string.Empty;
            }
#endif
        }

        /// <summary>
        /// Method to update the text infomation in the Animatext component.
        /// </summary>
        protected virtual void UpdateInfo()
        {
#if UNITY_EDITOR
            if (_textInfo == null)
            {
                _textInfo = new TextInfo();
            }

            if (Application.isPlaying)
            {
                HashSet<string> effectTags = new HashSet<string>();

                for (int i = 0; i < _effects.Count; i++)
                {
                    effectTags.UnionWith(_effects[i].GetTags());
                }

                List<char> symbolList = new List<char>();

                switch (settings.tagSymbols)
                {
                    case TagSymbols.AngleBrackets:
                        symbolList.Add('<');
                        symbolList.Add('>');
                        break;

                    case TagSymbols.RoundBrackets:
                        symbolList.Add('(');
                        symbolList.Add(')');
                        break;

                    case TagSymbols.SquareBrackets:
                        symbolList.Add('[');
                        symbolList.Add(']');
                        break;

                    case TagSymbols.CurlyBrackets:
                        symbolList.Add('{');
                        symbolList.Add('}');
                        break;

                    default:
                        symbolList.Add('<');
                        symbolList.Add('>');
                        break;
                }

                switch (settings.markerSymbols)
                {
                    case MarkerSymbols.Slashes:
                        symbolList.Add('/');
                        break;

                    case MarkerSymbols.Backslashes:
                        symbolList.Add('\\');
                        break;

                    case MarkerSymbols.VerticalBars:
                        symbolList.Add('|');
                        break;

                    default:
                        symbolList.Add('/');
                        break;
                }

                foreach (var tag in effectTags)
                {
                    int symbolIndex = -1;

                    for (int i = 0; i < symbolList.Count; i++)
                    {
                        symbolIndex = Mathf.Max(symbolIndex, tag.IndexOf(symbolList[i]));
                    }

                    if (symbolIndex != -1)
                    {
                        Debug.LogError("<Animatext> The tag name \"" + tag + "\" can't contain tag or marker symbols.");
                    }

                    if (tag == "c" || tag == "w" || tag == "l" || tag == "g")
                    {
                        Debug.LogError("<Animatext> The tag name \"" + tag + "\" can't be the same as the name of a unit tag, including 'c', 'w', 'l', 'g'.");
                    }

                    if (IsComponentRichTextTag(tag))
                    {
                        Debug.LogWarning("<Animatext> The tag name \"" + tag + "\" shouldn't be the same as the rich text tag name of the text component.");
                    }
                }
            }
#endif
            _textInfo.Update(GetOriginText(_inputText), _effects, _settings, _outputTextInfoFunc);

            _parsedText = _textInfo.parsedText;
            _outputText = _textInfo.outputText;
            _effectText = _textInfo.effectText;

            switch (_settings.retainedText)
            {
                case RetainedText.InputText:
                    _retainedText = _inputText;
                    break;

                case RetainedText.ParsedText:
                    _retainedText = _parsedText;
                    break;

                case RetainedText.OutputText:
                    _retainedText = _outputText;
                    break;

                case RetainedText.EffectText:
                    _retainedText = _effectText;
                    break;

                case RetainedText.BlankText:
                    _retainedText = string.Empty;
                    break;

                default:
                    _retainedText = _inputText;
                    break;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Update(_textInfo, this);
            }

            text = _retainedText;
        }

        /// <summary>
        /// Method to update all the texts in the Animatext component.
        /// </summary>
        protected virtual void UpdateText()
        {
            _inputText = text ?? string.Empty;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UpdateInfo();

                return;
            }
#endif
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Refresh();
            }

            UpdateInfo();
        }

        #region <- Effect Methods ->

        public void EndEffect(int index)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].End();
        }

        public void EndEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].End();
            }
        }

        public void PauseEffect(int index)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].Pause();
        }

        public void PauseEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Pause();
            }
        }

        public void PlayEffect(int index)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].Play();
        }

        public void PlayEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Play();
            }
        }

        public void RefreshEffect(int index)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].Refresh();
        }

        public void RefreshEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Refresh();
            }
        }

        public void StartEffect(int index)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].Start();
        }

        public void StartEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Start();
            }
        }

        public void StopEffect(int index)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].Stop();
        }

        public void StopEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Stop();
            }
        }

        public void SetEffectAutoPlay(int index, bool autoPlay)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].autoPlay = autoPlay;
        }

        public void SetEffectsAutoPlay(bool autoPlay)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].autoPlay = autoPlay;
            }
        }

        public void SetEffectAutoStart(int index, bool autoStart)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].autoStart = autoStart;
        }

        public void SetEffectsAutoStart(bool autoStart)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].autoStart = autoStart;
            }
        }

        public void SetEffectRefreshMode(int index, RefreshMode refreshMode)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].refreshMode = refreshMode;
        }

        public void SetEffectsRefreshMode(RefreshMode refreshMode)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].refreshMode = refreshMode;
            }
        }

        public void SetEffectSpeed(int index, float speed)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].speed = speed;
        }

        public void SetEffectsSpeed(float speed)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].speed = speed;
            }
        }

        public void SetEffectTime(int index, float time)
        {
            if (index < 0 || index >= _effects.Count) return;

            _effects[index].time = time;
        }

        public void SetEffectsTime(float time)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].time = time;
            }
        }

        #endregion

        /// <summary>
        /// Method to refresh the Animatext component. This method is used after adding new effects or modifying preset tags.
        /// </summary>
        /// <param name="refreshEffects">Whether to refresh the effects.</param>
        public abstract void Refresh(bool refreshEffects);

        /// <summary>
        /// Method to set the text of the text component.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            if (text == null)
            {
                text = string.Empty;
            }

            if (this.text == text)
            {
                _inputText = text;

                Refresh(true);
            }
            else
            {
                this.text = text;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Method to log the origin text of the Animatext.
        /// </summary>
        [ContextMenu("Log Origin Text")]
        private void LogOriginText()
        {
            if (_textInfo != null)
            {
                Debug.Log("<Animatext> Origin Text - " + _textInfo.originText.Replace(">", "\u001E>"));
            }
            else
            {
                Debug.Log("<Animatext> Origin Text - (Null)");
            }
        }

        /// <summary>
        /// Method to determine whether the tag is a rich text tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected virtual bool IsComponentRichTextTag(string tag)
        {
            return false;
        }
#endif
    }
}