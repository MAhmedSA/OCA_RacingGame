using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public class VideoTimeEvent{
    public double time;   // seconds
    public Action action;
}
public class UIVideoController : MonoBehaviour
{
    [Header("Video GamePlay Parameters")]
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] RawImage videoImage;
    [SerializeField] VideoClip _startVideoClip;
    private List<VideoTimeEvent> events = new();
    private int currentIndex = 0;

    [Header("Canvas Groups")]
    [SerializeField] CanvasGroup mainCanvas;
    [SerializeField] CanvasGroup videoCanvas;
    [SerializeField] CanvasGroup _gamePlayUICanvas;
    
    [SerializeField] CanvasGroup resultCanvas;
    [SerializeField] Button playButton;
    [SerializeField] Button BackButton;

    [SerializeField] float fadeDuration =5f;
    
    IGameProcess _iGameProcess;
    [SerializeField] private UIResultController uiResultController;
    [SerializeField] TimerCount timerController;
    bool firstFrameShown = false;
    
    private Coroutine timerCoroutine;
 
    public void SetGameProcess(IGameProcess gameProcess)
    {
        _iGameProcess = gameProcess;
        _iGameProcess.OnGameFinished += OnGameFinishedHandler;
    }
    void OnGameFinishedHandler(List<FinishResult> results)
    {
        uiResultController.UpdateResults(results);
    }


    void OnEnable()
    {
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += OnFrameReady;
    }

    void OnDisable() {
        videoPlayer.frameReady -= OnFrameReady;
    }

    void Awake(){
        IntiDependences();
        IntializeActionsBasedTime();
        IntializeUIOrder();
    }
    void IntializeVideo(){
        ResetVideoState();
        videoPlayer.Play();
    }

    void OnFrameReady(VideoPlayer vp, long frame) {
        if (currentIndex >= events.Count)
            return;

        if (vp.frameRate <= 0)
            return;

        double currentTime = frame / vp.frameRate;

        while (currentIndex < events.Count &&
               currentTime >= events[currentIndex].time)
        {
            events[currentIndex].action.Invoke();
            currentIndex++;
        }
    }

    void ShowGamePlayUI(){
       
        mainCanvas.Hide(0.5f);
    }
    void SecondAction(){
        _gamePlayUICanvas.Show(0.5f);

        timerController.ResetTime();
        timerController.ConvertTime();

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = CoroutineRunner.Instance.StartCoroutine(UpdateTimerWhileSecondAction());
    }

    IEnumerator UpdateTimerWhileSecondAction() {
        // Count only while video is playing
        while (videoPlayer.isPlaying)
        {
            timerController.time += Time.deltaTime;  // Increment timer
            timerController.ConvertTime();           // Update UI text
            yield return null;
        }
    }
    void ThirdAction() {
        CoroutineRunner.Instance.StartCoroutine(HandleThirdAction());
    }

    IEnumerator HandleThirdAction() {
        yield return null; // wait one frame (CRITICAL)

        videoCanvas.Hide(1f);
        _gamePlayUICanvas.Hide(0.2f);
        resultCanvas.Show();

        yield return new WaitForSeconds(0.1f);

        videoPlayer.Stop(); // SAFE now
        CoroutineRunner.Instance.StartCoroutine(ReturnToMain());
    }
    void IntializeActionsBasedTime() {
        // Register events in time order
        if(events.Count >0)
            return;
        events.Add(new VideoTimeEvent { time = 28, action = ShowGamePlayUI });
        events.Add(new VideoTimeEvent { time =34, action = SecondAction });
        events.Add(new VideoTimeEvent { time = 50, action = ThirdAction });
    }
    void IntializeUIOrder(){
        mainCanvas.Show();
        resultCanvas.Hide();
        _gamePlayUICanvas.Hide();
    }
    IEnumerator ReturnToMain() {
        CleanupVideo();
        IntializeActionsBasedTime();
        yield return new WaitForSeconds(20f);
        IntializeVideo();
        mainCanvas.Show(0.2f);
        videoCanvas.Show(0.2f);
        resultCanvas.Hide(0.3f);
    }
    void CleanupVideo() {
        ResetVideoState();
        videoPlayer.Stop();
    }

    void ResetVideoState(){
        currentIndex = 0;
        videoPlayer.time = 0;
        videoPlayer.frame = 0;
    }
    
    void IntiDependences() {
        if(uiResultController == null)
            uiResultController = GetComponent<UIResultController>();
    }
}