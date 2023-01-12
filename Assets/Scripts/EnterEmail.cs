using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RestSharp;
using UnityAtoms.BaseAtoms;
using System.Text.RegularExpressions;

public class EnterEmail : MonoBehaviour
{
    [SerializeField]
    UIController uiController;
    [SerializeField]
    APICall _apiCall;
    [SerializeField]
    TMP_InputField Email;
    [SerializeField]
    GameObject TickBox;
    [SerializeField]
    GameObject ValidateButton;
    [SerializeField]
    VoidEvent E_AskPassword;
    [SerializeField]
    VoidEvent submitOTP;
    [SerializeField]
    VoidEvent SetPassword;
    [SerializeField]
    StringVariable V_Email;
    [SerializeField]
    StringVariable V_USerDetail;

    [SerializeField]
    RegisterUserResponse.Root user = new();

    public void showTick()
    {
        TickBox.SetActive(ValidateEmail(Email.text));
        ValidateButton.GetComponent<Button>().interactable = ValidateEmail(Email.text);
    }

    public void EmailEntered()
    {
        if(ValidateEmail(Email.text))
        {
            Email.GetComponent<Image>().sprite = null;
            Email.textComponent.color = new Color(0, 0, 0);
        }
    }
    public async void OnEmailVerifyClick()
    {
        if (!ValidateEmail(Email.text))
            return;

        ValidateButton.GetComponent<Button>().interactable = false;
        V_Email.Value = Email.text;
        IRestResponse userStatus = await _apiCall.CheckUser(Email.text);
        V_USerDetail.Value = userStatus.Content;
        Debug.Log(userStatus.Content);
        if(userStatus.IsSuccessful)
        {
            user = JsonUtility.FromJson<RegisterUserResponse.Root>(userStatus.Content);
            if(user.data.isEmailVerify)
            {
                //Ask for OTP
                if(user.data.isPasswordAvailable)
                {
                    E_AskPassword.Raise();
                }
                else
                {
                    SetPassword.Raise();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                var _ = _apiCall.ResendOTP(Email.text);
                submitOTP.Raise();
            }
        }
    }

    public bool ValidateEmail(string email)
    {
        return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    }
}
