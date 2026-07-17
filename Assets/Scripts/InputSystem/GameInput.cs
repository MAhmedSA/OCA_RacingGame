using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    
    IInputProvider _inputProvider;
    UIVideoController _uiVideoController;
    IGameProcess _gameProcess;
    
    public void SetDependencies(IInputProvider inputProvider , UIVideoController uiVideoController ,IGameProcess gameProcess)
    {
        this._inputProvider = inputProvider;
        this._uiVideoController = uiVideoController;
        this._gameProcess= gameProcess;
    }
    
    private void Update()
    {
        if (_inputProvider != null)
        {
            if (_inputProvider.PlayButton)
            {
                //Fire Game
                _gameProcess.IsGameRunning = true;
               
            }
            if (_inputProvider.BackButton)
            {
                //Close Game and Back To MainMenu
                _gameProcess.IsGameRunning = false;
               
            }
        }
    }
}
