using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    APICall APICall;
    [SerializeField]
    string PrivacyPolicyURL;
    [SerializeField]
    string MeetingModeURL;
    [SerializeField]
    string TermsAndCon;
    [SerializeField]
    string DiscordURL;
    [SerializeField]
    StringVariable UserDetails;
    [SerializeField]
    TMP_Text Email;
    [SerializeField]
    TMP_Text UserName;
    [SerializeField]
    GameObject FirstPage;
    [SerializeField]
    GameObject ARScenePanel;

    [SerializeField]
    RegisterUserResponse.Root user = new();

    private void OnEnable()
    {
        try
        {
            user = JsonUtility.FromJson<RegisterUserResponse.Root>(PlayerPrefs.GetString("Data"));
            //user = JsonUtility.FromJson<RegisterUserResponse.Root>(UserDetails.Value);
            Email.text = user.data.email;
            UserName.text = user.data.username;
        }
        catch
        {
            Debug.Log("Data Not Available");
        }
    }

    public void ReadPrivacyPolicy()
    {
        Application.OpenURL(PrivacyPolicyURL);
        
    }
    public void MeetingMode()
    {
        Application.OpenURL(MeetingModeURL);
    }
    public void Terms()
    {
        Application.OpenURL(TermsAndCon);
    }
    
    public void ConnectToDiscord()
    {
        Application.OpenURL(DiscordURL);
    }

    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        FirstPage.SetActive(true);
        ARScenePanel.SetActive(false);
        gameObject.SetActive(false);

    }
    public void DeleteAccount()
    {
        _ = APICall.APIDeleteAccount();
        FirstPage.SetActive(true);
        ARScenePanel.SetActive(false);
        gameObject.SetActive(false);
    }


}
