using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RestSharp;
using UnityAtoms.BaseAtoms;

public class OTPverify : MonoBehaviour
{
    [SerializeField]
    TMP_InputField OtpInput;
    [SerializeField]
    Button VerifyButton;
    [SerializeField]
    APICall _APICall;
    [SerializeField]
    VoidEvent SetPassword;
    [SerializeField]
    StringVariable V_Email;
    [SerializeField]
    GameObject OTPErrorMessage;
    [SerializeField]
    OTPVerifyResponse.Root OTPres = new();
    public void EmailEntered()
    {
        if (OtpInput.text.Length>=4)
        {
            OtpInput.GetComponent<Image>().sprite = null;
            OtpInput.textComponent.color = new Color(0, 0, 0);
            VerifyButton.interactable = true;
        }
    }

    public async void OnVerifyOTPClick()
    {
        IRestResponse VerifyOTP = await _APICall.VerifyOTPAPI(V_Email.Value, OtpInput.text);
        Debug.Log(VerifyOTP.Content);
        OtpInput.GetComponent<Image>().color = Color.white;
        if (VerifyOTP.IsSuccessful)
        {
            OTPres = JsonUtility.FromJson<OTPVerifyResponse.Root>(VerifyOTP.Content);
            _APICall.PersistLogin(OTPres.data.accessToken);
            _APICall.SaveDetails(VerifyOTP.Content);
            SetPassword.Raise();
            gameObject.SetActive(false);
        }
        else if(VerifyOTP.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            SetPassword.Raise();
            gameObject.SetActive(false);
            StartCoroutine("ShowError");
        }
        else
        {
            StartCoroutine("ShowError");
        }
    }

    IEnumerator ShowError()
    {
        OTPErrorMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        OTPErrorMessage.SetActive(false);
    }
}
