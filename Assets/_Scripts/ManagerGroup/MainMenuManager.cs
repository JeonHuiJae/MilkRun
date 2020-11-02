using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour {
    
    public GameObject PortraitUI;
    public GameObject CutScene;
    AdMobManager AdManager;

    AudioSource TitleMusic;
    AudioSource Button;

    Vector3[] PortraitPos = new Vector3[10];
    GameObject canvas;
    CanvasScaler can;

    public int CutTime = 0;
    bool ClickFlag = false;
   
    public GameObject FreeButton;
    public GameObject GiftButton;
    public GameObject CurChar;

    public GameObject Gift;
    Gift gift_s;


    Vector3 tempVec;
   

    private void Awake()
    {
        AdManager = GameObject.Find("AdMobManager").GetComponent<AdMobManager>();
        gift_s = Gift.GetComponent<Gift>();

        TitleMusic = GameObject.Find("MilkRun_Title").GetComponent<AudioSource>();
        Button = GameObject.Find("Button").GetComponent<AudioSource>();

        canvas = GameObject.Find("Canvas");
        can = canvas.GetComponent<CanvasScaler>();

        PlayerPrefs.SetInt("curWp", 0);
        PlayerPrefs.SetInt("GameLevel", 0); // 난이도
        PlayerPrefs.SetInt("Revival", 1); // 1:부활 사용 가능 , 0: 부활 썼음
        PlayerPrefs.SetInt("Revival_MapNum", 0); // 부활할 맵 
        PlayerPrefs.SetInt("Revival_Score", 0); // 부활할 스코어 

        //PlayerPrefs.SetInt("p_MapNum", 0);
        //PlayerPrefs.SetInt("p_Score", 0);
        PlayerPrefs.SetInt("curMilkNum", 0);

        if (PlayerPrefs.GetInt("toutorial") == 1)
        {
            CutScene.SetActive(true);
            TitleMusic.Stop();
            StartCoroutine("ToutoAnim");
        }
        if (PlayerPrefs.GetInt("toutorial") == 2) // 튜토리얼 마치고 디바이스 방향 최초 설정
        {
            CutScene.SetActive(true);
            CutScene.transform.GetChild(7).gameObject.SetActive(true);
            CutScene.transform.GetChild(8).gameObject.SetActive(false);
            TitleMusic.Stop();
        }

        GameObject PortraitUI2 = canvas.transform.GetChild(2).GetChild(1).gameObject;
        GameObject PortraitUI3 = canvas.transform.GetChild(2).GetChild(0).gameObject;
        GameObject PortraitUI4 = canvas.transform.GetChild(4).GetChild(1).gameObject;
        GameObject PortraitUI5 = canvas.transform.GetChild(5).GetChild(1).gameObject;

        PortraitPos[0] = new Vector3(PortraitUI.transform.position.x, PortraitUI.transform.position.y, PortraitUI.transform.position.z);
        PortraitPos[1] = new Vector3(PortraitUI2.transform.position.x, PortraitUI2.transform.position.y, PortraitUI2.transform.position.z);
        PortraitPos[2] = new Vector3(PortraitUI3.transform.position.x, PortraitUI3.transform.position.y, PortraitUI3.transform.position.z);
        PortraitPos[3] = new Vector3(PortraitUI4.transform.position.x, PortraitUI4.transform.position.y, PortraitUI4.transform.position.z);
        PortraitPos[4] = new Vector3(PortraitUI5.transform.position.x, PortraitUI5.transform.position.y, PortraitUI5.transform.position.z);

        PortraitPos[5] = new Vector3(PortraitUI.transform.position.x, PortraitUI.transform.position.y+ 0.2f, PortraitUI.transform.position.z);
        PortraitPos[6] = new Vector3(PortraitUI2.transform.position.x, PortraitUI2.transform.position.y - 0.5f, PortraitUI2.transform.position.z);
        PortraitPos[7] = new Vector3(PortraitUI3.transform.position.x, PortraitUI3.transform.position.y - 0.2f, PortraitUI3.transform.position.z);
        PortraitPos[8] = new Vector3(PortraitUI4.transform.position.x, PortraitUI4.transform.position.y - 0.5f, PortraitUI4.transform.position.z);
        PortraitPos[9] = new Vector3(PortraitUI5.transform.position.x, PortraitUI5.transform.position.y - 0.8f, PortraitUI5.transform.position.z);

    }

    void Start()
    {
        StartCoroutine("CheakRewardAd");
    }


    void Update ()
    {
        GameObject PortraitUI2 = canvas.transform.GetChild(2).GetChild(1).gameObject;
        GameObject PortraitUI3 = canvas.transform.GetChild(2).GetChild(0).gameObject;
        GameObject PortraitUI4 = canvas.transform.GetChild(4).GetChild(1).gameObject;
        GameObject PortraitUI5 = canvas.transform.GetChild(5).GetChild(1).gameObject;

        if (PlayerPrefs.GetInt("ad") == 0)
        {
            Screen.orientation = ScreenOrientation.Landscape;
            PortraitUI.transform.position = PortraitPos[5];
            PortraitUI2.transform.position = PortraitPos[6];
            PortraitUI3.transform.position = PortraitPos[7];
            PortraitUI4.transform.position = PortraitPos[8];
            PortraitUI5.transform.position = PortraitPos[9];     
            PortraitUI.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            can.matchWidthOrHeight = 0.75f;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
            PortraitUI.transform.position = PortraitPos[0];
            PortraitUI2.transform.position = PortraitPos[1];
            PortraitUI3.transform.position = PortraitPos[2];
            PortraitUI4.transform.position = PortraitPos[3];
            PortraitUI5.transform.position = PortraitPos[4];   
            PortraitUI.transform.localScale = new Vector3(1f, 1f, 1f);
            can.matchWidthOrHeight = 1f;
        }
 
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerPrefs.GetInt("toutorial") == 0)
        {
            PlayerPrefs.Save();
            Application.Quit();
        }
	}



    public void GoldUp()
    {
        int gold;
        gold = PlayerPrefs.GetInt("curGold");
        gold += 50000;
        PlayerPrefs.SetInt("curGold", gold);
    }

    public void ToutorialShow()
    {
        PlayerPrefs.SetInt("ad", 1);
        PlayerPrefs.SetInt("toutorial", 1);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ReSet()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void NextCut()
    {
        if (ClickFlag == true)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Button.Play();
            CutTime += 1; 
            StartCoroutine("ToutoAnim");
        }
    }

    IEnumerator ToutoAnim()
    {
        ClickFlag = false;
        if (CutTime == 0)
        {
            Image sprite = CutScene.transform.GetChild(1).GetComponent<Image>();
            CutScene.transform.GetChild(1).gameObject.SetActive(true);
            int time = 0;
            byte alpha = 0;
            while (time < 20)
            {
                sprite.color = new Color32(255, 255, 255, alpha);
                alpha += 13;
                yield return new WaitForSeconds(0.02f);
                time++;
            }
            sprite.color = new Color32(255, 255, 255, 255);
        }
        if (CutTime == 1)
        {
            CutScene.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (CutTime == 2)
        {
            Image sprite = CutScene.transform.GetChild(3).GetComponent<Image>();
            CutScene.transform.GetChild(3).gameObject.SetActive(true);
            int time = 0;
            byte alpha = 0;
            while (time < 20)
            {
                sprite.color = new Color32(255, 255, 255, alpha);
                alpha += 13;
                yield return new WaitForSeconds(0.02f);
                time++;
            }
            sprite.color = new Color32(255, 255, 255, 255);
        }
        if (CutTime == 3)
        {
            CutScene.transform.GetChild(4).gameObject.SetActive(true);
        }
        if (CutTime == 4)
        {
            Image sprite = CutScene.transform.GetChild(5).GetComponent<Image>();
            CutScene.transform.GetChild(5).gameObject.SetActive(true);
            int time = 0;
            byte alpha = 0;
            while (time < 20)
            {
                sprite.color = new Color32(255, 255, 255, alpha);
                alpha += 13;
                yield return new WaitForSeconds(0.02f);
                time++;
            }
            sprite.color = new Color32(255, 255, 255, 255);
        }
        if (CutTime == 5)
        {
            CutScene.transform.GetChild(1).gameObject.SetActive(false);
            CutScene.transform.GetChild(2).gameObject.SetActive(false);
            CutScene.transform.GetChild(3).gameObject.SetActive(false);
            CutScene.transform.GetChild(4).gameObject.SetActive(false);
            CutScene.transform.GetChild(5).gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            CutScene.transform.GetChild(6).gameObject.SetActive(true);
            int time = 0;
            while (time < 2)
            {
                CutScene.transform.GetChild(6).transform.position += Vector3.up * 0.1f;
                CutScene.transform.GetChild(6).transform.position += Vector3.right * 0.2f;
                yield return new WaitForSeconds(0.005f);
                time++;
            }
            time = 0;
            while (time < 2)
            {
                CutScene.transform.GetChild(6).transform.position += Vector3.down * 0.2f;
                CutScene.transform.GetChild(6).transform.position += Vector3.left * 0.4f;
                yield return new WaitForSeconds(0.005f);
                time++;
            }
            time = 0;
            while (time < 2)
            {
                CutScene.transform.GetChild(6).transform.position += Vector3.up * 0.1f;
                CutScene.transform.GetChild(6).transform.position += Vector3.right * 0.2f;
                yield return new WaitForSeconds(0.005f);
                time++;
            }
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        if (CutTime == 6)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        ClickFlag = true;
    }


    public void Portrait()
    {
        PlayerPrefs.SetInt("toutorial", 0);
        PlayerPrefs.SetInt("ad", 1);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void Landscape()
    {
        PlayerPrefs.SetInt("toutorial", 0);
        PlayerPrefs.SetInt("ad", 0);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    IEnumerator CheakRewardAd()
    {
        yield return new WaitForSeconds(1f);
        if (AdManager.isRewardAdLoad() && gift_s.GiftOpenFlag == 0)
            FreeButton.SetActive(true);  
        else
            FreeButton.SetActive(false);

        if (!AdManager.isRewardAdLoad())
            AdManager.RequestRewardAd();

        StartCoroutine("CheakRewardAd");
    }

    public void ShowRewardAdFuc()
    {
        if (AdManager.isRewardAdLoad())
            AdManager.ShowRewardAd();
    }

    public void ShowGiftButton()
    {
        gift_s.GiftOpenFlag = 1;
        GiftButton.SetActive(true);
        CurChar.SetActive(false);
        FreeButton.SetActive(false);
        gameObject.SetActive(false);
    }


}
