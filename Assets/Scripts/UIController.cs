using TMPro;
using UnityEngine;
using VideoCreator;
using UnityEngine.UI;
using System.Collections;
using UnityAtoms.BaseAtoms;
using VoxelBusters.ReplayKit;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject HomeScreen;
    public GameObject LoginPanel;
    [SerializeField]
    GameObject ARScenePanel;
    [SerializeField]
    GameObject ARSceneUIElements;
    [SerializeField]
    GameObject PasswordPanel;
   
    [SerializeField]
    ToggleGroup toggleGroup;
    [SerializeField]
    IntVariable ApplicationMode;
    [SerializeField]
    GameObject RecorderImage;
    [SerializeField]
    GameObject RecordButon;
    [SerializeField]
    GameObject MeetingModePanel;
    [SerializeField]
    GameObject VideoRecordingMode;

    private bool isrecording = false;
    private bool isDoubleTap = false;

    private void Start()
    {
        Application.targetFrameRate = 30;
        ApplicationMode.Value = 3;
        toggleGroup.SetAllTogglesOff();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void StartApplication()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("KEY")))
            HomeScreen.SetActive(true);
        else
            LoginPanel.SetActive(true);
       
    }

    public void OnToggleChange()
    {        
        Debug.Log("Toggle Change");
        Debug.Log(toggleGroup.GetFirstActiveToggle().gameObject.name);
        if(toggleGroup.GetFirstActiveToggle().gameObject.name.ToLower() == "picture")
        {
            ApplicationMode.Value = 1;
        }
        else if(toggleGroup.GetFirstActiveToggle().gameObject.name.ToLower() == "meeting")
        {
            ApplicationMode.Value = 2;
            MeetingModePanel.SetActive(true);
        }
        else if(toggleGroup.GetFirstActiveToggle().gameObject.name.ToLower() == "video")
        {
            
            ApplicationMode.Value = 3;
        }
        else
        {
            ApplicationMode.Value = 3;
        }
        
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0 && ARScenePanel.activeInHierarchy && !isrecording)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began )
            {
                StartCoroutine("StartCountdown");
            }
            if (touch.phase == TouchPhase.Ended )
            {
                StopCoroutine("StartCountdown");
            }
        }
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0) && !ARSceneUIElements.activeInHierarchy && isrecording)
        {
            StartCoroutine("DoubleTap");
            StartRecording();
        }
        if (Input.GetMouseButtonDown(0) && !ARSceneUIElements.activeInHierarchy && isrecording && isDoubleTap)
        {
            Debug.Log("its a double tap");
            StopRecording();
            //StartCoroutine("DoubleTap");
        }
        if (Input.GetMouseButtonUp(0) && !ARScenePanel.activeInHierarchy && isrecording)
        {
            //StopCoroutine("StartCountdown");
        }
#endif
        if(Input.touchCount>0 && !ARSceneUIElements.activeInHierarchy && isrecording)
        {
            Debug.Log("You Touched the Screen After stating the Recording");
            StartCoroutine("DoubleTap");
            
        }
        if(Input.touchCount>0 && !ARSceneUIElements.activeInHierarchy && isrecording && isDoubleTap)
        {
            Debug.Log("its a double Tap");
            StopRecording();
        }
    }

    IEnumerator DoubleTap()
    {
        Debug.Log("Starting Double Tap");
        isDoubleTap = true;
        yield return new WaitForSeconds(0.3f);
        isDoubleTap = false;
        Debug.Log("Stop Double Tap");
        //StopCoroutine("DoubleTap");
    }

    IEnumerator StartCountdown()
    {
        Debug.Log("Staring Coroutine");
        yield return new WaitForSeconds(3);
        ARSceneUIElements.SetActive(true);
        ApplicationMode.Value = 3;
    }

    IEnumerator TakeScreenshot()
    {
        ARSceneUIElements.SetActive(false);
        yield return new WaitForEndOfFrame();
        var screenshot =  ScreenCapture.CaptureScreenshotAsTexture();
        MediaSaver.SaveImage(screenshot,"jpg");
        ARSceneUIElements.SetActive(true);
    }

    public void SetupRecording()
    {
        Debug.Log("Setting Up Recording");
        VideoRecordingMode.SetActive(true);
        ReplayKitManager.Initialise();
    }

    public void StartRecording()
    {
        Debug.Log("recording Started");
        VideoRecordingMode.SetActive(false);

    if (ReplayKitManager.IsRecordingAPIAvailable())
            {
                isrecording = true;
                ReplayKitManager.SetMicrophoneStatus(true);
                ReplayKitManager.PrepareRecording();
                ARSceneUIElements.SetActive(false);
                ReplayKitManager.StartRecording();
            }
        isrecording = true;
        ARSceneUIElements.SetActive(false);
    }
    public void StopRecording()
    {
        Debug.Log("Recording Stopped");
        if (ReplayKitManager.IsRecording())
            ReplayKitManager.StopRecording();

        ARScenePanel.GetComponent<ARScenePanelController>().ShowSaveVideo();
        ARScenePanel.GetComponent<ARScenePanelController>().SetVideoSavedText("Saving Video",true);

        ReplayKitManager.DidRecordingStateChange += statusChanged;
        ARSceneUIElements.SetActive(true);
        isrecording = false;
        ReplayKitManager.SetMicrophoneStatus(false);
    }


    private void statusChanged(ReplayKitRecordingState state, string message)
    {
        if (state == ReplayKitRecordingState.Available)
        {
            ReplayKitManager.SavePreview((error) => {
                Debug.Log("Recording Saved with Error");
                ARScenePanel.GetComponent<ARScenePanelController>().SetVideoSavedText("Err Video",false);
            });
            ReplayKitManager.DidRecordingStateChange -= statusChanged;
            ARScenePanel.GetComponent<ARScenePanelController>().SetVideoSavedText("Video Saved",false);
            ARScenePanel.GetComponent<ARScenePanelController>().HideSaveVideo();


            // ReplayKitManager.Discard();
        }
    }
    

    public void Action()
    {
        switch(ApplicationMode.Value)
        {
            case 1:
                StartCoroutine("TakeScreenshot");
                break;
            case 2:
                //SetupMeetingMode();
                break;
            case 3:
                VideoRecordingMode.SetActive(true);
                break;
        }
    }

    

    public void SetupMeetingMode()
    {
        MeetingModePanel.SetActive(true);
    }
    public void StartMeetingMode()
    {
        ARSceneUIElements.SetActive(false);
        MeetingModePanel.SetActive(false);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
