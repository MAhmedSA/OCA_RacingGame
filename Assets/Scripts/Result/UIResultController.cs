using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIResultController : MonoBehaviour
{
    [Header("PlayersName")]
    [SerializeField] TMP_Text first;
    [SerializeField] TMP_Text second;
    [SerializeField] TMP_Text third;
    [SerializeField] TMP_Text fourth;
    [SerializeField] TMP_Text fifth;
    [Header("TimerText")]
    [SerializeField] TMP_Text timerText1;
    [SerializeField] TMP_Text timerText2;
    [SerializeField] TMP_Text timerText3;
    [SerializeField] TMP_Text timerText4;
    [SerializeField] TMP_Text timerText5;
    [Header("SpeedText")]
    [SerializeField] TMP_Text speedText1;
    [SerializeField] TMP_Text speedText2;
    [SerializeField] TMP_Text speedText3;
    [SerializeField] TMP_Text speedText4;
    [SerializeField] TMP_Text speedText5;
    
    public void UpdateResults(List<FinishResult> result)
    {
        first.text = result[0].playerName;
        timerText1.text = result[0].GetFormattedTime();
        speedText1.text  = result[0].GetFormattedSpeed();
        second.text =result[1].playerName;
        timerText2.text = result[1].GetFormattedTime();
        speedText2.text  = result[1].GetFormattedSpeed();
        third.text  =result[2].playerName;
        timerText3.text  = result[2].GetFormattedTime();
        speedText3.text  = result[2].GetFormattedSpeed();
        fourth.text=result[3].playerName;
        timerText4.text  = result[3].GetFormattedTime();
        speedText4.text  = result[3].GetFormattedSpeed();
        fifth.text= result[4].playerName;
        timerText5.text  = result[4].GetFormattedTime();
        speedText5.text  = result[4].GetFormattedSpeed();
    }
}