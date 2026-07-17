using UnityEngine;
[System.Serializable]
public class FinishResult
{
    public string playerName;
    public float finishTime;
    public bool didFinish;
    public float speed;

    public FinishResult(string name, float time, bool finished , float distance)
    {
        playerName = name;
        finishTime = time;
        didFinish = finished;
        speed = distance / time ;
    }

    public string GetFormattedTime()
    {
        if (!didFinish)
            return "00:00";

        int totalSeconds = Mathf.FloorToInt(finishTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes:00}:{seconds:00}";
    }

    public string GetFormattedSpeed()
    {
        if (!didFinish)
            return "0M/HR";
        float roundedSpeed = Mathf.Round(speed * 10f) / 10f;
        return  roundedSpeed+"M/HR";
    }
}