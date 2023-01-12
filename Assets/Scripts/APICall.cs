using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestSharp;
using UnityAtoms.BaseAtoms;
using System;
using System.Threading.Tasks;

public class APICall : MonoBehaviour
{
    public string rootPath = "https://app-dev.getanimo.io/";

    [SerializeField]
    VoidEvent submitOTP;
    [SerializeField]
    BoolEvent LoggedIN;

    [SerializeField]
    StringVariable UserID;
    [SerializeField]
    StringVariable v_UserAccessToken;

    [SerializeField]
    RegisterUserResponse.Root user = new();
    [SerializeField]
    OTPVerifyResponse.Root OTPres = new();

    public string UserEmail;
    [SerializeField]
    VoidEvent E_AskPassword;
    [SerializeField]
    StringVariable UserDetail;


    [SerializeField]
    VoidEvent HomeScreen;

    bool isPasswordForgot;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Debug.Log("Cleared");
            PlayerPrefs.DeleteAll();
        }
    }
    //public async void RegisterUser(string Email)
    //{
    //    UserEmail = Email;
    //    if ((await UserLoginWithOTP(Email, "check-register")))
    //    {
    //        if (user.data.isPasswordAvailable)
    //        {
    //            E_AskPassword.Raise();
    //        }
    //        else
    //        {
    //            await UserLoginWithOTP(Email, "resend-otp");
    //            submitOTP.Raise();
    //        }

    //    }
    //    else
    //    {
    //        submitOTP.Raise();
    //    }

    //}

    //public async void LoginWithPassword(string password)
    //{
    //    if (await UserLoginWithPassword(UserEmail, password, "check-register"))
    //        LoggedIN.Raise(false);

    //}

    //public async void setPassword(string Password)
    //{
    //    if (await SetPasswordAPI(UserEmail, Password, "check-register"))
    //    {

    //    }
    //}

    //public async void ForgotPassword()
    //{
    //    isPasswordForgot = true;
    //    if (await ForgotPasswordAPI(UserEmail, "forgot-password"))
    //    {

    //    }
    //}

    public async Task<bool> ForgotPasswordAPI(string email, string path= "forgot-password")
    {
        RestClient FP = new(rootPath + path);
        var rq = new RestRequest(Method.POST);
        PostRequestBody.RegisterUser ru = new() { email = email };
        var body = JsonUtility.ToJson(ru);
        rq.AddJsonBody(body);
        IRestResponse response = await FP.ExecuteAsync(rq);
        return response.IsSuccessful;
    }

    //public async Task<bool> UserLoginWithOTP(string Email, string Path)
    //{
    //    RestClient Registerclient = new RestClient(rootPath + Path);
    //    var Request = new RestRequest(Method.POST);
    //    PostRequestBody.RegisterUser ru = new() { email = Email };
    //    var body = JsonUtility.ToJson(ru);
    //    Request.AddJsonBody(body);
    //    IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
    //    Debug.Log(restResponse.Content);
    //    try
    //    {
    //        UserDetail.Value = restResponse.Content;
    //        user = JsonUtility.FromJson<RegisterUserResponse.Root>(restResponse.Content);
    //        v_UserAccessToken.Value = user.data.accessToken;

    //    }
    //    catch
    //    {
    //        Debug.Log(restResponse.Content);
    //        Debug.LogError("Bhai Fata");
    //    }
    //    return user.data.isEmailVerify;
    //}

    public async Task<IRestResponse> CheckUser(string Email, string Path = "check-register")
    {
        RestClient Registerclient = new RestClient(rootPath + Path);
        var Request = new RestRequest(Method.POST);
        PostRequestBody.RegisterUser ru = new() { email = Email };
        var body = JsonUtility.ToJson(ru);
        Request.AddJsonBody(body);
        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        return restResponse;
    }

    public async Task<IRestResponse> UserLoginWithPassword(string Email, string password, string Path= "check-register")
    {
        RestClient Registerclient = new RestClient(rootPath + Path);
        var Request = new RestRequest(Method.POST);
        PostRequestBody.LoginUser ru = new() { email = Email, password = password };
        var body = JsonUtility.ToJson(ru);
        Request.AddJsonBody(body);
        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        Debug.Log(restResponse.Content);
        return restResponse;
        //if (restResponse.IsSuccessful)
        //{
        //    try
        //    {
        //        user = JsonUtility.FromJson<RegisterUserResponse.Root>(restResponse.Content);
        //        v_UserAccessToken.Value = user.data.accessToken;
        //        PlayerPrefs.SetString("KEY", user.data.accessToken);
        //        return true;
        //    }
        //    catch
        //    {
        //        Debug.Log(restResponse.Content);
        //        Debug.LogError("Bhai Fata");
        //        return false;
        //    }
        //}
        //else
        //{
        //    Debug.Log("Login Failed");
        //}
        //return false;
    }
    public async Task<IRestResponse> VerifyOTPAPI(string Email, string OTP, string Path= "verify-otp")
    {
        RestClient Registerclient = new RestClient(rootPath + Path);
        var Request = new RestRequest(Method.POST);
        PostRequestBody.VerifyOTP otp = new() { email = Email, otp = OTP };
        var body = JsonUtility.ToJson(otp);
        Request.AddJsonBody(body);
        IRestResponse restResponse1 = await Registerclient.ExecuteAsync(Request);
        return restResponse1;
    }
    public void PersistLogin(string AccessToken)
    {
        v_UserAccessToken.Value = AccessToken;
        PlayerPrefs.SetString("KEY", AccessToken);
    }
    public void SaveDetails(string Data)
    {
        PlayerPrefs.SetString("Data", Data);
    }

    public async Task<IRestResponse> SetPasswordAPI(string Email, string Password, string Path= "check-register")
    {
        RestClient Registerclient = new RestClient(rootPath + Path);
        var Request = new RestRequest(Method.POST);
        PostRequestBody.LoginUser ru = new() { email = Email, password = Password };
        var body = JsonUtility.ToJson(ru);
        Request.AddJsonBody(body);
        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        Debug.Log(restResponse.Content);
        return restResponse;
        
    }

    public async Task<IRestResponse> APIDeleteAccount()
    {
        RestClient Registerclient = new RestClient(rootPath + "delete-profile");
        var Request = new RestRequest(Method.POST);
        Request.AddHeader("Authorization", user.data.accessToken);
        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        return restResponse;
    }

    public async Task<IRestResponse> ResendOTP(string Email, string Path = "resend-otp")
    {
        Debug.Log("Resend OTP");
        RestClient Registerclient = new RestClient(rootPath + Path);
        var Request = new RestRequest(Method.POST);
        PostRequestBody.RegisterUser ru = new() { email = Email };
        var body = JsonUtility.ToJson(ru);
        Request.AddJsonBody(body);
        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        Debug.Log(restResponse.Content);
        return restResponse;

    }

    public async Task<IRestResponse> RestPassword(string Email,string Password, string otp, string path="update-password")
    {
        Debug.Log("Resend OTP");
        RestClient Registerclient = new RestClient(rootPath + path);
        var Request = new RestRequest(Method.POST);
        PostRequestBody.UpdatePassword UP = new() { email = Email,password=Password,otp=otp,type=1 };
        var body = JsonUtility.ToJson(UP);
        Request.AddJsonBody(body);
        IRestResponse restResponse = await Registerclient.ExecuteAsync(Request);
        Debug.Log(restResponse.Content);
        return restResponse;
    }
}