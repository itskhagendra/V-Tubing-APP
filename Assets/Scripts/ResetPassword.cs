using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestSharp;
using TMPro;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class ResetPassword : MonoBehaviour
{
    [SerializeField]
    TMP_InputField OTP;
    [SerializeField]
    TMP_InputField Password;
    [SerializeField]
    TMP_InputField ConfirmPassword;
    [SerializeField]
    Button ResetPasswordBtn;
    [SerializeField]
    APICall _APICall;
    [SerializeField]
    StringVariable V_Email;
    [SerializeField]
    VoidEvent HomeScreen;
    [SerializeField]
    RegisterUserResponse.Root user = new();

    public void CheckPassword()
    {
        Image bgImage = ConfirmPassword.GetComponent<Image>();
        if (Password.text.Length < ConfirmPassword.text.Length)
            return;

        if (Password.text == ConfirmPassword.text)
        {
            bgImage.sprite = null;
            bgImage.color = new Color(0.58f, 1, 0.48f, 0.3f);
            ResetPasswordBtn.interactable = true;
        }
        else
        {
            bgImage.sprite = null;
            bgImage.color = new Color(0.89f, 0.32f, 0.32f, 0.13f);
        }
    }

    public async void OnResetPasswordClick()
    {
        if(!string.IsNullOrEmpty(OTP.text))
        {
            IRestResponse res = await _APICall.RestPassword(V_Email.Value, Password.text, OTP.text);
            if(res.IsSuccessful)
            {
                user = JsonUtility.FromJson<RegisterUserResponse.Root>(res.Content);
                _APICall.PersistLogin(user.data.accessToken);
                _APICall.SaveDetails(res.Content);
                HomeScreen.Raise();
                gameObject.SetActive(false);
            }
                
        }
    }
}
