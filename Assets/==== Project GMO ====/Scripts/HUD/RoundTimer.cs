using System;
using TMPro;
using UnityEngine;
using Animatext;

public class RoundTimer : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector;

    [SerializeField] private TextMeshProUGUI roundStatusText;

    [SerializeField] private TextMeshProUGUI roundNumberText;

    [SerializeField] private float textWarningTime;

    private float curPrepTime = 0;

    bool roundNumberEffectFired = false;

    private void OnEnable()
    {
        gameDirector.OnStartGame += StartRound;
        gameDirector.OnEnterPrepPhase += PrepPhase;
        gameDirector.OnEnterRoundPhase += RoundPhase;
    }

    private void Awake()
    {
        HideRoundTimer();
    }

    // Update is called once per frame
    void Update()
    {
        bool prepPhase = gameDirector.isPrepPhase && gameDirector.gameStarted;

        if (prepPhase)
        {
            curPrepTime = gameDirector.waitTime - Time.time;

            if (curPrepTime < 0) curPrepTime = 0;

            if(curPrepTime <= textWarningTime && !roundNumberEffectFired)
            {
                
                roundNumberEffectFired = true;
            }

            int showPrepTIme = (int)curPrepTime;

            roundNumberText.text = showPrepTIme.ToString();
        }
    }

    private void StartRound(object sender, EventArgs e)
    {
        ShowRoundTimer();
    }

    private void PrepPhase(object sender, EventArgs e)
    {
        PrepPhaseText();
    }

    private void RoundPhase(object sender, EventArgs e)
    {
        RoundPhaseText();
    }

    private void PrepPhaseText()
    {
        roundStatusText.text = "PREP TIME ";
        roundNumberText.text = curPrepTime.ToString();
        roundNumberEffectFired = false;
    }

    private void RoundPhaseText()
    {
        roundStatusText.text = "ROUND ";
        roundNumberText.text = gameDirector.currentWave.ToString();
    }

    private void ShowRoundTimer()
    {
        roundStatusText.gameObject.SetActive(true);
        roundNumberText.gameObject.SetActive(true);
    }
    private void HideRoundTimer()
    {
        roundStatusText.gameObject.SetActive(false);
        roundNumberText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        gameDirector.OnStartGame -= StartRound;
        gameDirector.OnEnterPrepPhase -= PrepPhase;
        gameDirector.OnEnterRoundPhase -= RoundPhase;
    }
}
