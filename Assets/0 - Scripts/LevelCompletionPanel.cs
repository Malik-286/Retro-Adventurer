using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCompletionPanel : MonoBehaviour
{

    [SerializeField] GameObject[] otherPanels;
    [SerializeField] TextMeshProUGUI levelCompletedText;
    [SerializeField] TextMeshProUGUI totalKillsText;
    [SerializeField] TextMeshProUGUI CollectedCoinsText;
  


    KillsCounter killsCounter;
 

    void Start()
    {
        killsCounter = FindObjectOfType<KillsCounter>();

        // PlayerPrefs.SetInt("CollectedCurrency", PlayerPrefs.GetInt("UpdatedCurrency") - PlayerPrefs.GetInt("CurrencyeBeforeLevel"));
        // print("Collected Coins aa:  " + PlayerPrefs.GetInt("CollectedCurrency"));

        if (Adsmanager.Instance != null)
        {
            Adsmanager.Instance.Interstitial.LoadInterstitialAd();
        }
    }
    void Update()
    {
        UpdateTexts();

        foreach (GameObject panels in otherPanels)
        {
            panels.SetActive(false);
        }

    }


    void UpdateTexts()
    {
        levelCompletedText.text = GameManager.GetInstance().GetActiveSceneName() + " Completed !";
        totalKillsText.text = "Total Kills:  " + killsCounter.GetCurrentSceneKills().ToString("0");
        PlayerPrefs.SetInt("CollectedCurrency", PlayerPrefs.GetInt("UpdatedCurrency") - PlayerPrefs.GetInt("CurrencybeforePlay"));
        CollectedCoinsText.text = "Collected Coins:  " + PlayerPrefs.GetInt("CollectedCurrency").ToString("0");
    }

    public void PressHomeButton()
    {
        if(Adsmanager.Instance != null)
        {
            Adsmanager.Instance.Interstitial.ShowInterstitialAd();
        }


        if (AudioManager.GetInstance() != null)
        {
            AudioManager.GetInstance().PlayTouchSoundEffect();
        }
        if (GameManager.GetInstance() != null)
        {
             GameManager.GetInstance().LoadNextScene("Main Menu");
        }
       
    }

    public void PressNextLevelButton()
    {
        if (Adsmanager.Instance != null)
        {
            Adsmanager.Instance.Interstitial.ShowInterstitialAd();
        }

        if (PlayerPrefs.GetInt("SelectedLevel") % 2 == 0)
        {
            if (Adsmanager.Instance)
            {
                Adsmanager.Instance.ShowIntersitial();
            }
        }

        if (AudioManager.GetInstance() != null)
        {
            AudioManager.GetInstance().PlayTouchSoundEffect();
        }
        if (GameManager.GetInstance() != null)
        {
            
            int currentSceneIndex = GameManager.GetInstance().GetCurrentSceneIndex();
            GameManager.GetInstance().LoadScene(currentSceneIndex+1);
        }

        PlayerPrefs.SetInt("SelectedLevel", PlayerPrefs.GetInt("SelectedLevel") + 1);
    }
}
