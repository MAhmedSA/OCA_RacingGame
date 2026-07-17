using System;
using TMPro;
using UnityEngine;

public class TimerCount : MonoBehaviour
{
   [SerializeField]TextMeshProUGUI timerText;
   public float time;

    private void Awake()
    {
        time = 0;
    }
    public void ConvertTime()
    {
        int totalSeconds = Mathf.FloorToInt(time);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        DisplayTimer(hours, minutes, seconds);
    }

    void DisplayTimer(int hours, int minutes, int seconds)
    {
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

  public void ResetTime(){
      time = 0;
  }
}
