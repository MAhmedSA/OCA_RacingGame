using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameProcess
{
    public event Action<List<FinishResult>> OnGameFinished;
    event Action OnGameStarted;
    bool IsGameRunning { get; set; }
}
