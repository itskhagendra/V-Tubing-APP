using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using RestSharp;

public class Password : MonoBehaviour
{
    [SerializeField]
    Button SubmitPassword;
    [SerializeField]
    TMP_InputField PasswordInput;
    [SerializeField]
    VoidEvent E_ForgotPassword;
    [SerializeField]
    VoidEvent HomeScreen;
    [SerializeField]
    APICall _ApiCall;
    [SerializeField]
    StringVariable v_Email;
    [SerializeField]
    GameObject PasswordError;
    [SerializeField]
    RegisterUserResponse.Root user = new();

    public async void onEnterComplete()
    {
        if(PasswordInput.text.Length>1)
        {
            SubmitPassword.interactable = true;
            IRestResponse res = await _ApiCall.UserLoginWithPassword(v_Email.Value, PasswordInput.text);
            if(res.IsSuccessful)
            {
                user = JsonUtility.FromJson<RegisterUserResponse.Root>(res.Content);
                _ApiCall.PersistLogin(user.data.accessToken);
                _ApiCall.SaveDetails(res.Content);
                HomeScreen.Raise();
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine("ShowError");
            }
        }
    }
    public async void OnForgotPasswordClick()
    {
        if (await _ApiCall.ForgotPasswordAPI(v_Email.Value))
        {
            E_ForgotPassword.Raise();
            gameObject.SetActive(false);
        }
        
    }

    IEnumerator ShowError()
    {
        PasswordError.SetActive(true);
        yield return new WaitForSeconds(2f);
        PasswordError.SetActive(false);
    }
}
