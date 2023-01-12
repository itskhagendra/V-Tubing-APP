using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine.UI;

public class SetPassword : MonoBehaviour
{
    [SerializeField]
    UIController UiController;
    [SerializeField]
    TMP_InputField Password;
    [SerializeField]
    TMP_InputField ConfirmPassword;
    [SerializeField]
    StringEvent E_SetPassword;
    [SerializeField]
    APICall _ApiCall;
    [SerializeField]
    StringVariable V_Email;
    [SerializeField]
    RegisterUserResponse.Root user = new();

    public async void OnSetPasswordClick()
    {
        if(Password.text == ConfirmPassword.text)
        {
            var res = await _ApiCall.SetPasswordAPI(V_Email.Value, Password.text);
            if (res.IsSuccessful)
            {
                user = JsonUtility.FromJson<RegisterUserResponse.Root>(res.Content);
                _ApiCall.PersistLogin(user.data.accessToken);
                _ApiCall.SaveDetails(res.Content);

            }
            E_SetPassword.Raise(Password.text);
        }
        
    }
    public void isPasswordSame()
    {
        Image bgImage = ConfirmPassword.GetComponent<Image>();
        if (Password.text.Length < ConfirmPassword.text.Length)
            return;

        if(Password.text == ConfirmPassword.text)
        {
            bgImage.sprite = null;
            bgImage.color = new Color(0.58f, 1, 0.48f,0.3f);
        }
        else
        {
            bgImage.sprite = null;
            bgImage.color = new Color(0.89f, 0.32f, 0.32f,0.13f);
        }
    }
}
