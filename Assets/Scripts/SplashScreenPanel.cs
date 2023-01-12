using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class SplashScreenPanel : MonoBehaviour
{
    // Start is called before the first frame update
    private VideoPlayer SplashVideo;
    [SerializeField] UIController _UIController;
    void Start()
    {
        SplashVideo = GetComponent<VideoPlayer>();
        SplashVideo.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        Debug.Log("VideoPlayer Finished");
        _UIController.StartApplication();
        gameObject.SetActive(false);
    }
}
