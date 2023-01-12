using RestSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Load2DAvatar : MonoBehaviour
{
    string baseUrl = "https://render.readyplayer.me/render";
    [SerializeField]
    string AvatarURL;
    [SerializeField]
    SceneID SceneType = new();
    [SerializeField]
    [Range(0.1f, 1)]
    float smile = 0.2f;
    [SerializeField]
    Image Boyimage;
    [SerializeField]
    Image Girlimage;

    [SerializeField]
    Avatars2D.Root avatars2D = new();

    string scene_Type;
    public enum SceneID
    {
        fullbody_portrait_v1 = 1,
        fullbody_portrait_v1_transparent = 2,
        halfbody_portrait_v1 = 3,
        halfbody_portrait_v1_transparent = 4,
        fullbody_posture_v1_transparent=5
    }


    
    // Start is called before the first frame update
    void Start()
    {
        switch(SceneType)
        {
            case SceneID.fullbody_portrait_v1:
                scene_Type = "fullbody - portrait - v1";
                break;
            case SceneID.fullbody_portrait_v1_transparent:
                scene_Type = "fullbody-portrait-v1-transparent";
                break;
            case SceneID.fullbody_posture_v1_transparent:
                scene_Type = "halfbody-portrait-v1";
                break;
            case SceneID.halfbody_portrait_v1:
                scene_Type = "halfbody-portrait-v1-transparent";
                break;
            case SceneID.halfbody_portrait_v1_transparent:
                scene_Type = "fullbody-posture-v1-transparent";
                break;
        }
        GetAvatarImage();
    }
    public void GetAvatarImage()
    {
        GetBoyAvatar();
        GetGirlAvatar();
    }

    public async void GetBoyAvatar()
    {
        RestClient Registerclient = new RestClient(baseUrl);
        var Request = new RestRequest(Method.POST);

        var body1 = @"{
                    " + "\n" +
                    @"          ""model"": ""https://api.readyplayer.me/v1/avatars/638b3afc59d13f203a33fba4.glb"",
                    " + "\n" +
                    @"          ""scene"": ""fullbody-portrait-v1"",
                    " + "\n" +
                    @"          ""armature"": ""ArmatureTargetMale"",
                    " + "\n" +
                    @"          ""blendShapes"": {""Wolf3D_Avatar"": {""mouthSmile"": 0.2 }}     
                    " + "\n" +
                    @"}";
        Request.AddParameter("text/plain", body1, ParameterType.RequestBody);

        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        Debug.Log(Request.Body.Value);
        avatars2D = JsonUtility.FromJson<Avatars2D.Root>(restResponse.Content);
        StartCoroutine(SetImage(avatars2D.renders[0], Boyimage));
        
        Debug.Log(restResponse.Content);
    }
    public async void GetGirlAvatar()
    {
        RestClient Registerclient = new RestClient(baseUrl);
        var Request = new RestRequest(Method.POST);

        var body1 = @"{
                    " + "\n" +
                    @"          ""model"": ""https://api.readyplayer.me/v1/avatars/638b366a59d13f203a33f892.glb"",
                    " + "\n" +
                    @"          ""scene"": ""fullbody-portrait-v1"",
                    " + "\n" +
                    @"          ""armature"": ""ArmatureTargetMale"",
                    " + "\n" +
                    @"          ""blendShapes"": {""Wolf3D_Avatar"": {""mouthSmile"": 0.2 }}     
                    " + "\n" +
                    @"}";
        Request.AddParameter("text/plain", body1, ParameterType.RequestBody);

        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        Debug.Log(Request.Body.Value);
        avatars2D = JsonUtility.FromJson<Avatars2D.Root>(restResponse.Content);
        StartCoroutine(SetImage(avatars2D.renders[0],Girlimage));

        Debug.Log(restResponse.Content);
    }
    
    IEnumerator SetImage(string url,Image image)
    {
        
        yield return new WaitForEndOfFrame();
        Davinci.get()
            .load(url)
            .setFadeTime(2) // 0 for disable fading
            .into(image)
            .start();
    }
}
