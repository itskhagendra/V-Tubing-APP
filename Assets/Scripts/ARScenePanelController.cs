using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ARScenePanelController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject Selectbackground;
    [SerializeField]
    RectTransform SelectBackGroundPos;
    [SerializeField]
    RectTransform VideoSavedPOS;
    Vector2 onScreenPosition;
    Vector2 ofScreenPosition;
    [SerializeField]
    [Range(0, 5)]
    float AnimationTime = 1;

    [SerializeField]
    GameObject VideoSavePanel;
    [SerializeField]
    GameObject Loader;
    [SerializeField]
    TMP_Text VideoSavedText;
    void Start()
    {
        ofScreenPosition = new Vector2(0, -Screen.height);
        onScreenPosition = Vector2.zero;
        SelectBackGroundPos = Selectbackground.GetComponent<RectTransform>();
        //Selectbackground.transform.position = ofScreenPosition;
        SelectBackGroundPos.anchoredPosition = new Vector2(0,-Screen.height);
    }

    public void SelectBackground()
    {
        StartCoroutine(TweenUI(SelectBackGroundPos, ofScreenPosition, onScreenPosition, AnimationTime));
    }
    public void closeBackgroudSelection()
    {
        StartCoroutine(TweenUI(SelectBackGroundPos, onScreenPosition, ofScreenPosition, AnimationTime));
    }

    IEnumerator TweenUI(RectTransform Go,Vector2 StartPos,Vector2 FinalPOS,float duration)
    {
        float time = 0;
        while(time< duration)
        {
            Go.anchoredPosition = Vector2.Lerp(StartPos, FinalPOS, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        Go.anchoredPosition = FinalPOS;
    }
    public void ShowSaveVideo()
    {
        StartCoroutine(TweenUI(VideoSavedPOS, new Vector2(0, 150f), Vector2.zero, 0.5f));
        Debug.Log("Video Saved panel");
    }
    public void HideSaveVideo()
    {
        StartCoroutine(TweenUI(VideoSavedPOS,  Vector2.zero, new Vector2(0, 150f), 0.5f));
        Debug.Log("Video Saved panel");
    }
    public void SetVideoSavedText(string Message,bool LoaderStatus=false)
    {
        VideoSavedText.text = Message;
        Loader.SetActive(LoaderStatus);
    }
}
