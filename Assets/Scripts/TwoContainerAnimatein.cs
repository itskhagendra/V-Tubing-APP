using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using TMPro;

public class TwoContainerAnimatein : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject MainContent;
    [SerializeField]
    GameObject AnimatedContent;
    [SerializeField]
    TMP_InputField UserEmail;
    [SerializeField]
    StringVariable Email;
    
    [SerializeField]
    [Range(3, 10)]
    int delay = 3;

    
    [SerializeField]
    VoidEvent HomeScreen;

    public void OnEnable()
    {
        UserEmail.text = Email.Value;
    }
    public void ConfigureScreen(bool state)
    {
        if (state)
        {
            UserEmail.text = Email.Value;
            MainContent.SetActive(true);
            AnimatedContent.SetActive(false);
        }
        else
        {
            MainContent.SetActive(false);
            AnimatedContent.SetActive(true);
            StartCoroutine(ShowMainContent(delay));
        }
    }

    public void TriggerNext()
    {
        StartCoroutine(ShowMainContent(delay));
    }

    IEnumerator ShowMainContent(int delay)
    {
        AnimatedContent.SetActive(true);
        MainContent.SetActive(false);
        yield return new WaitForSeconds(delay);
        HomeScreen.Raise();
    }
}
