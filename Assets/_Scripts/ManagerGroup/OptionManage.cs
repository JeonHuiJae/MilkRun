using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManage : MonoBehaviour {

    private bool music;
    private bool language;
    public GameObject musicObject;
    public GameObject languageObject;
    public GameObject adObject;

    void Awake()
    {
        setMusic();
        setLanguage();
        setAd();
    }
    public void musicBtn() { // 음악버튼 눌렀을 때

        if (PlayerPrefs.GetInt("music") == 0)
            PlayerPrefs.SetInt("music", 1);
        else
            PlayerPrefs.SetInt("music", 0);

        PlayerPrefs.Save();
        GameObject.Find("curChar").SendMessage("musicControl", PlayerPrefs.GetInt("music"));
        setMusic();
    }

    public void languageBtn() { // 언어 버튼 눌렀을 떄 (true: 한국어 false: 영어)

        if (PlayerPrefs.GetInt("language") == 0)
            PlayerPrefs.SetInt("language", 1);
        else
            PlayerPrefs.SetInt("language", 0);
        PlayerPrefs.Save();
        setLanguage();
    }

    public void adBtn() { // 광고 버튼 눌렀을 떄

        if (PlayerPrefs.GetInt("ad") == 0)
        {
            PlayerPrefs.SetInt("ad", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ad", 0);
        }
        PlayerPrefs.Save();
        setAd();
    }


    // 버튼 설정

    public void setMusic() {  // 배경음
        if (PlayerPrefs.GetInt("music")==1) // on
        {
            musicObject.transform.GetChild(0).gameObject.SetActive(true);
            musicObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        { // off
            musicObject.transform.GetChild(0).gameObject.SetActive(false);
            musicObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void setLanguage() { // 언어
        if (PlayerPrefs.GetInt("language") == 1) // kor
        {
            languageObject.transform.GetChild(0).gameObject.SetActive(true);
            languageObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        { // eng
            languageObject.transform.GetChild(0).gameObject.SetActive(false);
            languageObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void setAd() // 광고
    {
        if (PlayerPrefs.GetInt("ad") == 1) // on
        {
            adObject.transform.GetChild(0).gameObject.SetActive(true);
            adObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        { // off
            adObject.transform.GetChild(0).gameObject.SetActive(false);
            adObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
