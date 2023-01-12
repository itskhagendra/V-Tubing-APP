using ReadyPlayerMe;
using RestSharp;
using Siccity.GLTFUtility;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AvatarConfigurator : MonoBehaviour
{
    [SerializeField]
    GameObjectEvent CustomAvatarURL;

    [SerializeField]
    GameObject WebViewObject;
    [SerializeField]
    WebView webView;

    [SerializeField]
    GameObject GuyAvatar;
    [SerializeField]
    GameObject GirlAvatar;
    [SerializeField]
    GameObject ARScreenUI;
    [SerializeField]
    TMP_Text Loading;
    
    [SerializeField]
    private GameObject m_Avatar;
    private float LastValue;
    private bool isLoading;

    List<string> AUrl = new();
    AvatarLoader avatar;
    int Count = 0;

    void OnEnable()
    {
        AUrl.Add("https://api.readyplayer.me/v1/avatars/63b6ed9d7bd52a558f8771d7.glb");
        AUrl.Add("https://models.readyplayer.me/v1/avatars/639609f33747d77532699e96.glb");
        AUrl.Add("https://models.readyplayer.me/v1/avatars/63b70a4e7bd52a558f87a0d9.glb");
        AUrl.Add("https://api.readyplayer.me/v1/avatars/63b709637bd52a558f879f61.glb");
        if (webView == null)
        {
            webView = FindObjectOfType<WebView>();
        }
        if(!WebViewObject.activeInHierarchy)
        {
            WebViewObject.SetActive(true);
        }
        webView.KeepSessionAlive = true;
        displayWebviw();
        #if UNITY_EDITOR || UNITY_ANDROID
        onAvatarCreated(AUrl[Count]);
        #endif
    }

    public void onAvatarCreated(string url)
    {
        Debug.Log("Avatar URL "+ url);
        string URLAddon = "?meshLod=0&textureAtlas=none";
        url = url + URLAddon;
        url.Replace("https://models.readyplayer.me/", "https://api.readyplayer.me/");
        if (m_Avatar != null)
        {
            AvatarCache.Clear();
            Destroy(m_Avatar);
        }

        avatar = new AvatarLoader();
        avatar.LoadAvatar(url);
        avatar.OnCompleted += AvatarLoadingCompleted;
        avatar.OnFailed += AvatarLoadingFailed;
        avatar.OnProgressChanged += AvatarLoadingProgress;
        
    }
    private void AvatarLoadingProgress(object sender, ProgressChangeEventArgs e)
    {

        if (LastValue != e.Progress)
        {
            Debug.Log("Avatar Loading Progress " + e.Progress);
            Loading.text = string.Format("Loading {0}%", Mathf.RoundToInt(e.Progress * 100));
            LastValue = e.Progress;
        }

    }

    void AvatarLoadingCompleted(object sender, CompletionEventArgs args)
    {
        Debug.Log("Avatar Loaded");
        isLoading = false;
        Count += 1;
        GameObject avatar = args.Avatar;
        //EditorUtilities.CreatePrefab(avatar, $"{DirectoryUtility.GetRelativeProjectPath(avatar.name)}/{avatar.name}.prefab");
        m_Avatar = avatar;
        //args.Avatar.gameObject.SetActive(false);
        CustomAvatarURL.Raise(avatar);
        //Destroy(args.Avatar);
        Debug.Log(m_Avatar.name);
        HideWebview();

    }
    void AvatarLoadingCompleted(GameObject Avatar)
    {
        Debug.Log("Avatar Loaded");
        GameObject avatar = Avatar;
        //EditorUtilities.CreatePrefab(avatar, $"{DirectoryUtility.GetRelativeProjectPath(avatar.name)}/{avatar.name}.prefab");
        m_Avatar = avatar;
        //args.Avatar.gameObject.SetActive(false);
        CustomAvatarURL.Raise(avatar);
        Debug.Log(m_Avatar.name);
        HideWebview();
    }
    void AvatarLoadingFailed(object sender, FailureEventArgs args)
    {
        Debug.Log("Avatar Loading Failed");
        Loading.text = string.Format("Avatar Loading Failed\n Try Again.");
        isLoading = false;
    }

    public void HideWebview()
    {
        Debug.Log("Closing WebView");
        if (isLoading)
            avatar.Cancel();
        webView.SetVisible(false);
        WebViewObject.SetActive(false);
        ARScreenUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void displayWebviw()
    {
        if(webView.Loaded)
        {
            Debug.Log("Showing WebView");
            webView.SetVisible(true);
        }
        else
        {
            Debug.Log("Creating Web view");
            webView.CreateWebView();
            webView.OnAvatarCreated = onAvatarCreated;
        }


    }
}
