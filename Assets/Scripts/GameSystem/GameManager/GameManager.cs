using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour,IGameProcess
{
    [SerializeField] private int distance=20;
    
    [SerializeField] SerialCommunicationManager serialReader;
    [SerializeField] GameInput _gameInput; //reference for GameInput Script to set it Own Service input ;
    [SerializeField] UIVideoController _uiVideoController;
    KeyboardInput keyboardInput; //input Service
    [SerializeField] TimerCount timer;
    
    public event Action<List<FinishResult>> OnGameFinished;
    public event Action OnGameStarted;
    
    List<FinishResult> finishResults = new List<FinishResult>();
    
    bool isGameRunning;
    bool gameFinished = false;
    
    
    public bool IsGameRunning {
        get { return isGameRunning; }
        set { isGameRunning = value; }
    }
    void Awake(){
        EnsureAllReferences();
        IntiDependences();
        StartRace();
    }
    void StartRace()
    {
        gameFinished = false;
        finishResults.Clear();

        RegisterAI("Tigger", 8);
        RegisterAI("Panda", 12);
        RegisterAI("Elephant", 15);
    }
    void RegisterAI(string name, int time){
        finishResults.Add(new FinishResult(name, time,true,distance));
    }
    void OnEnable(){
        serialReader.OnMessageReceived += HandleSerial; //Handle action based on data recived from Reader (Which winner)
    }

    void OnDisable(){
        serialReader.OnMessageReceived -= HandleSerial;
    }
    void HandleSerial(string msg){
        if(gameFinished) return;
        if (msg == "Player1Finished") RegisterRealPlayer("Player1");
        if (msg == "Player2Finished") RegisterRealPlayer("Player2");
    }

    private void Update()
    {
        //// Keyboard testing
        //if (Input.GetKeyDown(KeyCode.A))
        //{
          
        //    RegisterRealPlayer("Player1");
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
           
        //    RegisterRealPlayer("Player2");
        //}

        if (timer.time >= 15 && !gameFinished)
        {
            gameFinished = true;
            
            MarkUnfinishedPlayers();
            DeterminePlaces();
            OnGameFinished?.Invoke(new List<FinishResult>(finishResults));
            StartCoroutine(ResetGame());
        }
    }

    void RegisterRealPlayer(string playerName){
        // prevent double finish
        if (finishResults.Exists(r => r.playerName == playerName))
            return;

        float finishTime = timer.time;

        finishResults.Add(new FinishResult(playerName, finishTime,true,distance));

        DeterminePlaces();
    }
    void MarkUnfinishedPlayers(){
        string[] allPlayers = { "Player1", "Player2" };

        foreach (var player in allPlayers)
        {
            if (!finishResults.Exists(r => r.playerName == player))
            {
                finishResults.Add(new FinishResult(player, 0f, false,distance));
            }
        }
    }
    void DeterminePlaces(){
        finishResults.Sort((a, b) =>
        {
            if (a.didFinish && !b.didFinish) return -1;
            if (!a.didFinish && b.didFinish) return 1;
            return a.finishTime.CompareTo(b.finishTime);
        });
    }
    IEnumerator ResetGame(){
        yield return new WaitForSeconds(2f);
        timer.ResetTime();
        StartRace();
        serialReader.Restart();
    }
    private void IntiDependences(){
        keyboardInput = new KeyboardInput();
        //Inject Services
        _uiVideoController.SetGameProcess(this);
        _gameInput.SetDependencies(keyboardInput,_uiVideoController,this);
       
    }
    void EnsureAllReferences(){
        if (serialReader == null)
            serialReader = FindObjectOfType<SerialCommunicationManager>();
        if (_gameInput == null)
            _gameInput= FindObjectOfType<GameInput>();
        if (_uiVideoController == null){}
            _uiVideoController = FindObjectOfType<UIVideoController>();
    }
}