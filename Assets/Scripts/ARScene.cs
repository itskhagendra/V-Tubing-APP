using ReadyPlayerMe;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;

public class ARScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    StringEvent CustomizedAvatar;
    [SerializeField]
    GameObject GuyAvatar;
    [SerializeField]
    GameObject GirlAvatar;

    private GameObject m_Avatar;

    [SerializeField]
    ARFaceManager arfaceManager;

    [SerializeField]
    VoidEvent CloseWebView;

    [SerializeField]
    List<GameObject> Backgrounds;
    void Start()
    {
        arfaceManager = GetComponent<ARFaceManager>();
        foreach(GameObject x  in Backgrounds)
        {
            x.SetActive(false);
        }
        Backgrounds[0].SetActive(true);
    }
    
    public void SetAvatar(GameObject AvatarURL)
    {
        if(m_Avatar !=null)
        {
            Destroy(m_Avatar);
        }
        m_Avatar = AvatarURL;
        customizeAvatar();
    }

    

    void AvatarLoadingCompleted(object sender,CompletionEventArgs args)
    {
        Debug.Log("Avatar Loaded");
        m_Avatar = args.Avatar;
        Debug.Log(m_Avatar.name);
        customizeAvatar();
        CloseWebView.Raise();
    }
    void AvatarLoadingFailed(object sender, FailureEventArgs args)
    {
        Debug.Log("Avatar Loading Failed");
    }

    void customizeAvatar()
    {
        m_Avatar.transform.position = new Vector3(0, -1f, -4f);
        float l_Scale = 0.8f;
        m_Avatar.transform.localScale = new Vector3(l_Scale, l_Scale, l_Scale);
        //m_Avatar.transform.Rotate(new Vector3(0f, 180, 0));
        if(m_Avatar.GetComponent<ARFace>() == null)
            m_Avatar.AddComponent<ARFace>();

        if(m_Avatar.GetComponent<ARKitBlendShapeVisualizer>() == null)
        {
            var x = m_Avatar.AddComponent<ARKitBlendShapeVisualizer>();
            x.skinnedMeshRenderer = m_Avatar.GetComponentsInChildren<SkinnedMeshRenderer>()[2];
            x.HeadBone = GetHeadTransform(m_Avatar);
        }
        else
        {
            var x = m_Avatar.GetComponent<ARKitBlendShapeVisualizer>();
            x.skinnedMeshRenderer = m_Avatar.GetComponentsInChildren<SkinnedMeshRenderer>()[2];
            x.HeadBone = GetHeadTransform(m_Avatar);
        }
        arfaceManager.facePrefab = m_Avatar;
        FixRotation();
    }
    Transform GetHeadTransform(GameObject go)
    {
        return go.transform.Find("Armature/Hips/Spine/Spine1/Spine2/Neck/Head").transform;
    }

    public void setBackground(int index)
    {
        foreach (GameObject x in Backgrounds)
            x.SetActive(false);
        if(index>0)
            Backgrounds[index].SetActive(true);
    }
    public void FixRotation()
    {
        Camera ARcam = GameObject.FindObjectOfType<Camera>();
        m_Avatar.transform.LookAt(ARcam.transform);
    }
}
