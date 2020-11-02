using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    string[] WeaponName = new string[] {"돌멩이","토마토","장난감 칼","글록","미니 미사일", "종이비행기", "상어 미사일", "개사료", "레이져 건"};//무기
    string[] milkNames = new string[] { "","흰 우유", "딸기 우유", "바나나 우유", "커피 우유", "초코 우유", "요구르트", "두유", "고칼슘 우유", "코코에몽", "딸기 요플레" };
    string[] BossNames = new string[] { "","킹 라니","꼬마펭귄",""};

    public int[] wagonSlot = new int[] { 6, 9, 13, 12, 11, 16, 15};
    public int[] sellPrices = new int[] { 0, 15, 30, 48, 65, 90, 115, 140, 180, 235, 280};
    public int[] Increment = new int[] {0, 3, 4, 5, 6, 8, 10, 12, 14, 17, 20};

    public GameObject[] Weapon;
    public GameObject[] Effect;
    public GameObject[] Monster;
    public GameObject[] Object;
    public GameObject[] House;
    public GameObject[] Boss;
    public GameObject[] SuperItem;
    public GameObject[] Coin;
    public GameObject[] Footer;
    public GameObject[] ChangeStage;
    public GameObject[] MobWeapon;
    public Sprite[] Milk;
    public GameObject MilkItem;
    public GameObject WeaponItem;
    public GameObject ShowScore;
    //prefabs

    AudioSource BGM1;
    AudioSource BGM2;
    AudioSource BGM3;
    AudioSource BGM4;
    AudioSource BGM_Boss;
    AudioSource Puppy1;

    AdMobManager AdManager;
    GameObject player;
    Player player_script;
    WagonContentManager MilkManager;

    public GameObject BackGroundObj;
    public GameObject UI;
    public GameObject MapThum;
    public GameObject PauseUI;
    public GameObject PauseBt;
    public GameObject SkipBt;
    public GameObject GameOverUI;
    public GameObject LifeObj;
    public GameObject BossBarUI;
    public GameObject MilkUI;
    public GameObject WeaponItemPopUI;
    public GameObject DropPopUI;

    public GameObject PortraitUI1;
    public GameObject PortraitUI2;
    public GameObject PortraitUI3;
    public GameObject PortraitUI4;
    public GameObject PortraitUI5;
    public GameObject PortraitUI6;
    Vector3[] PortraitPos = new Vector3[10];

    GameObject playerIcon;
    RectTransform RectTransform;
    // UI

    public Text GoldText;
    public Text ScoreText; 
    public Text ScoreText2; // 게임오버 시
    public Text GetScoreText;
    public Text StageText;
    public Text HighScoreText;
    public Text HighScoreText2; // 게임 오버 시
    public Text HighMapLevelText;
    public Text WeaponItemPopText;
    public Text weaponNumText;
    public Text ItemDropNameText;
    public Text GameLevelText;
    // Text

    public int Price; // 현재 맵에서 진짜 우유가격 
    public int MapNum = 0; // 스테이지 
    public float GameSpeed = 3f;
    public float GameSpeedTemp;
    public int Gold = 0; // 현재 총 보유 골드
    public int Life = 3;
    public int Score = 0; 
    public int HighScore = 0; 
    public int HighMapLevel = 0; 
    public string GetScore; // 골드 얻었을때 순간 나오는 변수
    int GetScoreTime = 0;

    public int weaponNum = -1; // 무기 남은개수 초기값 돌멩이 무한 = -1

    public int GameLevel = 0; // 맵 난이도
    public float MapRunTime = 0; // 맵에서 달린 시간 
    public float MapRunTimeSpeed = 5f; // 맵 진행속도

    float CoinRangeTime;
    float CoinPosY = -0.9f;
    // member variable

    public bool GameStart = false;
    public bool GameOver = false;
    public bool isBossMode = false;
    bool isPause = false;

    public bool isIceFooter = false; // 펭귄마을전용 얼음발판
    public bool isBossClear = false;
    bool RevivalCheck = false;
    // boolean

    void Awake()
    {
        if (PlayerPrefs.GetInt("toutorial") == 1)
        {
            BackGroundObj.transform.GetChild(1).gameObject.SetActive(false);
            BackGroundObj.transform.GetChild(0).gameObject.SetActive(true);
            PauseBt.SetActive(false);
            //UI.transform.GetChild(0).gameObject.SetActive(false);
            UI.transform.GetChild(1).gameObject.SetActive(false);
            PortraitUI1.SetActive(false);
            PortraitUI2.SetActive(false);
            SkipBt.SetActive(true);
            StartCoroutine("Toutorial");
            MapNum = -1;
        }
        else
            MapNum = 0;
        
        Score = 0;
        PlayerPrefs.SetInt("curWp", 0);

        AdManager = GameObject.Find("AdMobManager").GetComponent<AdMobManager>();
    }

	void Start () 
    {
        BGM_Boss = GameObject.Find("BGM_Boss").GetComponent<AudioSource>();
        BGM1 = GameObject.Find("BGM1").GetComponent<AudioSource>();
        BGM2 = GameObject.Find("BGM2").GetComponent<AudioSource>();
        BGM3 = GameObject.Find("BGM3").GetComponent<AudioSource>();
        BGM4 = GameObject.Find("BGM4").GetComponent<AudioSource>();
        Puppy1 = GameObject.Find("Puppy1").GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        player_script = player.GetComponent<Player>();
        MilkManager = GameObject.FindGameObjectWithTag("MilkManager").GetComponent<WagonContentManager>();

        playerIcon = BossBarUI.transform.GetChild(3).gameObject;
        RectTransform = playerIcon.GetComponent<RectTransform>();

        PortraitPos[0] = new Vector3(PortraitUI1.transform.position.x, PortraitUI1.transform.position.y + 0.45f, PortraitUI1.transform.position.z);
        PortraitPos[1] = new Vector3(PortraitUI2.transform.position.x, PortraitUI2.transform.position.y + 0.45f, PortraitUI2.transform.position.z);
        PortraitPos[2] = new Vector3(PortraitUI3.transform.position.x, PortraitUI3.transform.position.y + 1.75f, PortraitUI3.transform.position.z);
        PortraitPos[3] = new Vector3(PortraitUI4.transform.position.x, PortraitUI4.transform.position.y - 2f, PortraitUI4.transform.position.z);
        PortraitPos[4] = new Vector3(PortraitUI5.transform.position.x, PortraitUI5.transform.position.y - 2f, PortraitUI5.transform.position.z);
        PortraitPos[5] = new Vector3(PortraitUI5.transform.position.x, PortraitUI5.transform.position.y - 2f, PortraitUI5.transform.position.z);

        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            player_script.Camera.orthographicSize = 5f;
            PortraitUI1.transform.position = PortraitPos[0];
            PortraitUI2.transform.position = PortraitPos[1];
            PortraitUI3.transform.position = PortraitPos[2];

            PortraitUI1.transform.localScale = new Vector3(1f, 1f, 1f);
            PortraitUI2.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            player_script.Camera.orthographicSize = 3f;

            PortraitUI4.transform.position = PortraitPos[3];
            PortraitUI5.transform.position = PortraitPos[4];
            PortraitUI6.transform.position = PortraitPos[5];

            PortraitUI1.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            PortraitUI2.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }

        if (PlayerPrefs.GetInt("toutorial") == 1)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                BGM1.Play();
        }
        else
        {
            if (PlayerPrefs.GetInt("music") == 1)
                BGM2.Play();
        }

        Gold = PlayerPrefs.GetInt("curGold");
        GameLevel = PlayerPrefs.GetInt("GameLevel");
        HighScore = PlayerPrefs.GetInt("p_bestScore"+GameLevel);

        if (GameLevel == 1) // 노말  
        {
            GameSpeed = 3.2f;
            for (int i = 0; i < 10; i++)
                Increment[i] += 2;  
        }
        if (GameLevel == 2) // 하드
        {
            GameSpeed = 3.5f;
            for (int i = 0; i < 10; i++)
                Increment[i] += 5; 
        }
        if (GameLevel == 3) // 익스트림
        {
            GameSpeed = 4.5f;
            for (int i = 0; i < 10; i++)
                Increment[i] += 8; 
        }

        if (PlayerPrefs.GetInt("Revival") == 0)
        {
            //GameOverUI.transform.GetChild(9).gameObject.SetActive(false); // 무한 부활 가능
            MapNum = PlayerPrefs.GetInt("Revival_MapNum");
            Score = PlayerPrefs.GetInt("Revival_Score");
            GameStart = true;
            BGM2.Stop();
            for (int i = 0; i < MapNum; i++)
                MilkManager.IncreasePrice();
            StartCoroutine("GameSetting");
            StartCoroutine("RevivalPopUp");
        }
        else         
            StartCoroutine("PadeOut_ReadyUI");

        LifeDraw();
	}

	void Update () 
    {
        GoldText.text = Gold.ToString();
        ScoreText.text = Score.ToString();
        ScoreText2.text = Score.ToString();
        GetScoreText.text = GetScore.ToString();
        StageText.text = (MapNum + 1).ToString();
        HighScoreText.text = HighScore.ToString();
        HighScoreText2.text = HighScore.ToString();

        if(GameLevel == 0)
            GameLevelText.text = "Easy";
        else if(GameLevel == 1)
            GameLevelText.text = "Normal";
        else if(GameLevel == 2)
            GameLevelText.text = "Hard";
        else if(GameLevel == 3)
            GameLevelText.text = "매우 어려운 Extream";

        if (Life == 0 && GameOver == false) 
        {
            player_script.StartCoroutine("PlayerDie");   
            GameOver = true;
        }

        if(GetScoreTime > 0)
            UI.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        else
            UI.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
 
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerPrefs.GetInt("toutorial") == 0)
        {
            if (GameStart == true && GameOver == false)
                Pause();

            if (GameStart == false || GameOver == true)
                Home();
        }

        if (WeaponItemPopUI.activeSelf == true)
            WeaponItemPopUI.transform.GetChild(0).transform.Rotate(Vector3.back * 200f * Time.deltaTime);
        if (DropPopUI.activeSelf == true)
            DropPopUI.transform.GetChild(0).transform.Rotate(Vector3.back * 200f * Time.deltaTime);

        if(weaponNum == -1)
            weaponNumText.text = "";
        else
            weaponNumText.text = weaponNum+"";

        if (weaponNum == 0) // 다 떨어지면 돌멩이 ㄱ ㄱ
        {
            PlayerPrefs.SetInt("curWp", 0);
            player_script.curWp = 0;
            weaponNum = -1;
        }

        if (RectTransform.localPosition.x < 136f && isBossMode == false)
        {
            if (player_script.isSuper == true)
            {
                MapRunTime += 0.2f;
                RectTransform.localPosition += Vector3.right * Time.deltaTime * MapRunTimeSpeed * 2f;
            }
            else
            {
                MapRunTime += 0.1f;
                RectTransform.localPosition += Vector3.right * Time.deltaTime * MapRunTimeSpeed;
            }
        }
        if (RevivalCheck == true)
        {
            StopCoroutine("StageNamePopUp");
            StartCoroutine("StageNamePopUpStop");
        }
	}

    public void LifeDraw()
    {
        for (int i = 4; i >= 0; i--)
        {
            if(i < Life)
                LifeObj.transform.GetChild( i ).gameObject.SetActive(true);
            else
                LifeObj.transform.GetChild( i ).gameObject.SetActive(false);
        }
    }

    IEnumerator MapRunTimeUp()
    {
        float FinishX = 136f;

        while(RectTransform.localPosition.x < FinishX)
        {
            if (RectTransform.localPosition.x >= (FinishX - 25f) ) // 보스 스폰 전 오프젝트, 몬스터, 집 생성 중단
            {
                StopCoroutine("MonsterSpawn");
                StopCoroutine("ObjectSpawn");
                StopCoroutine("CoinSpawn");
                StopCoroutine("WeaponItemSpawn");
                //StopCoroutine("HouseSpawn");
                if (MapNum == 0 || MapNum == 1)
                    StopCoroutine("WoodFooterSpawn");
                if (MapNum == 2 || MapNum == 3)
                    StartCoroutine("IceFooterReset", 1);
            }
            yield return new WaitForSeconds(0.2f);
        }
        while ( (player_script.isSuper == true && (MapNum == 1 || MapNum == 3) ) || isIceFooter == true) // 슈퍼아이템,발판구간 이라면 풀릴 때 까지 기다린다
        {
            yield return new WaitForSeconds(0.5f);
        } 
        
        isBossMode = true;
        StartCoroutine("BossSpawn");

    }
    IEnumerator BossClear()
    {
        BGM_Boss.Stop();
        
        GameObject playerIcon = BossBarUI.transform.GetChild(3).gameObject;
        RectTransform RectTransform = playerIcon.GetComponent<RectTransform>();

        if(MapNum == 1 || MapNum == 3)
            GameSpeed = GameSpeedTemp;
        
        if (player_script.isStand == true)  
            player_script.isStand = false;
        
        isBossMode = false;
        StartCoroutine("HouseSpawn");

        if (MapNum == 3)// 펭귄 클리어 하면 용궁 바다 빠지기 위해 바닥 열기 
        {
            isIceFooter = true;
            isBossClear = true;
            StopCoroutine("BossClear");
        }
        
        yield return new WaitForSeconds(4f);

        if (MapNum == 1) // 고라니숲에서 펭귄
        {
            Vector3 pos = new Vector3(21f, 0.8f, 0f);
            Instantiate(ChangeStage[0], pos, transform.rotation);
        }
        yield return new WaitForSeconds(2f);

        MapNum += 1;
        if (Life < 3)
            Life = 3;
        
        LifeDraw();

        MapRunTime = 0;
        RectTransform.localPosition = new Vector3(-136f, -15f);

        //MilkManager.ReSetMilk();
        MilkManager.IncreasePrice();
       

        StartCoroutine("GameSetting");
    }
    IEnumerator BossClear_Penguin()
    {
        GameObject playerIcon = BossBarUI.transform.GetChild(3).gameObject;
        RectTransform RectTransform = playerIcon.GetComponent<RectTransform>();

        isIceFooter = false;
        Vector3 pos = new Vector3(-3.5f, -4f, 0f);
        Instantiate(ChangeStage[1], pos, transform.rotation);

        yield return new WaitForSeconds(1f);
        player_script.StartMap3 = false;
        player.transform.position = new Vector3(-1.94f, 3.9f);

        MapNum += 1;
        Life = 3;
        MapRunTime = 0;
        RectTransform.localPosition = new Vector3(-136f, -15f);
        LifeObj.transform.GetChild(0).gameObject.SetActive(true);
        LifeObj.transform.GetChild(1).gameObject.SetActive(true);
        LifeObj.transform.GetChild(2).gameObject.SetActive(true);

        //MilkManager.ReSetMilk();
        MilkManager.IncreasePrice();

        StartCoroutine("GameSetting");
    }

    void OnMouseDown() // Only Pause
    {
        if(GameStart == true && GameOver == false)
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPause = true;
        PauseUI.SetActive(true);
        UI.SetActive(false);
    }
        
    public void Home()
    {
        BGM2.Stop();
        BGM3.Stop();
        BGM4.Stop();
        BGM_Boss.Stop();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void Play()
    {
        Time.timeScale = 1f;
        isPause = true;
        PauseUI.SetActive(false);
        UI.SetActive(true);
    }
    public void ReStart()
    {
        PlayerPrefs.SetInt("curWp", 0);
        PlayerPrefs.SetInt("Revival", 1); // 1:부활 사용 가능 , 0: 부활 썼음
        PlayerPrefs.SetInt("Revival_MapNum", 0); // 부활할 맵 
        PlayerPrefs.SetInt("Revival_Score", 0); // 부활할 스코어 
        Time.timeScale = 1f;

        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    public void Revival()
    {
        AdManager.ShowRewardAd();
    }
    public void Skip()
    {
        BGM1.Stop();
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("toutorial", 2);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void ShowGameOverUI()
    {
        int rand = Random.Range(0, 4);
        if (rand == 0 && PlayerPrefs.GetInt("AdsDelete") != 1)
        {
            AdManager.ShowInterstitialAd();
            AdManager.ShowInterstitialAd();
        }
        
        BGM2.Stop();
        BGM3.Stop();
        BGM4.Stop();
        BGM_Boss.Stop();
        GameOverUI.SetActive(true);
        WeaponItemPopUI.SetActive(false);

        GameOverUI.transform.GetChild( GameLevel + 11 ).gameObject.SetActive(true);

        if (HighScore < Score)
        {
            PlayerPrefs.SetInt("p_bestScore"+GameLevel, Score);
            GameOverUI.transform.GetChild(3).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(4).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(5).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(6).gameObject.SetActive(false);       
        }
        if (MapNum == 6 && Life > 0) // 용궁 보물맵 버전1 클리어 
        {
            GameObject ClearUI = GameOverUI.transform.GetChild(6).gameObject;
            GameOverUI.transform.GetChild(9).gameObject.SetActive(false);
            ClearUI.gameObject.SetActive(true); 
            Text ClearText = GameOverUI.transform.GetChild(6).gameObject.GetComponent<Text>();
            ClearText.text = "Version 1.0 Clear !!";
            if (HighScore < Score)
                ClearUI.transform.position = new Vector3(ClearUI.transform.position.x, ClearUI.transform.position.y + 0.81f, ClearUI.transform.position.z);
        }
            
        Time.timeScale = 0f;
    }

    IEnumerator PadeOut_ReadyUI()
    {
        MapThum.SetActive(true);
        UI.SetActive(false);

        //MapThum.transform.GetChild( MapNum + 2 ).gameObject.SetActive(true);
        //yield return new WaitForSeconds(1f);
        if(PlayerPrefs.GetInt("toutorial") == 0)
            MapThum.transform.GetChild(0).gameObject.SetActive(true);
        else
            MapThum.transform.GetChild(1).gameObject.SetActive(true);
        //yield return new WaitForSeconds(1f);
        //MapThum.transform.GetChild(1).gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        UI.SetActive(true);

        if(PlayerPrefs.GetInt("toutorial") == 0)
            MapThum.transform.GetChild(0).gameObject.SetActive(false);
        else
            MapThum.transform.GetChild(1).gameObject.SetActive(false);
        //MapThum.transform.GetChild(1).gameObject.SetActive(false);
        //MapThum.transform.GetChild( MapNum + 2 ).gameObject.SetActive(false);

        int time = 0;
        Image bg = MapThum.gameObject.GetComponent<Image>();
        float alpha = 1f;
        while (time < 20)
        {
            alpha -= 0.05f;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alpha);
            yield return new WaitForSeconds(0.002f);
            time++;
        }
        MapThum.gameObject.SetActive(false);
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 1);

        time = 0;
        GameStart = true;

        StartCoroutine("GameSetting");
    }
    IEnumerator GameSetting()
    {
        GameObject BossIcon = BossBarUI.transform.GetChild(1).gameObject;
        GameObject HouseIcon = BossBarUI.transform.GetChild(2).gameObject;

        if (PlayerPrefs.GetInt("music") == 1)
        {
            if((MapNum == 0 || MapNum == 1) && BGM2.isPlaying == false)
                BGM2.Play();
            if((MapNum == 2 || MapNum == 3) && BGM3.isPlaying == false)
                BGM3.Play();
            if((MapNum == 4 || MapNum == 5 || MapNum == 6) && BGM4.isPlaying == false)
                BGM4.Play();
        }
        if (MapNum == -1)
        {
            BossIcon.SetActive(false);
            HouseIcon.SetActive(true);
            MapRunTimeSpeed = 14f;
        }
        if (MapNum == 0)
        {
            BossIcon.SetActive(false);
            HouseIcon.SetActive(true);
            MapRunTimeSpeed = 8f;
        }
        if (MapNum == 1)
        {
            BossIcon.SetActive(true);
            HouseIcon.SetActive(false);
            MapRunTimeSpeed = 8f;
        }
        if (MapNum == 2)
        {
            BossIcon.SetActive(false);
            HouseIcon.SetActive(true);
            MapRunTimeSpeed = 7f;
        }
        if (MapNum == 3)
        {
            BossIcon.SetActive(true);
            HouseIcon.SetActive(false);
            MapRunTimeSpeed = 6f;
        }
        if (MapNum == 4)
        {
            BossIcon.SetActive(false);
            HouseIcon.SetActive(true);
            MapRunTimeSpeed = 5f;
        }
        if (MapNum == 5)
        {
            BossIcon.SetActive(true);
            HouseIcon.SetActive(false);
            MapRunTimeSpeed = 5f;
        }
        if (MapNum == 6) // 용궁 보물
        {
            BossIcon.SetActive(false);
            HouseIcon.SetActive(true);
            MapRunTimeSpeed = 11f;
        }

        BackGroundObj.transform.GetChild( 1 ).gameObject.SetActive(false);
        BackGroundObj.transform.GetChild( 2 ).gameObject.SetActive(false);
        if(MapNum == 0 || MapNum == 1)
            BackGroundObj.transform.GetChild( 1 ).gameObject.SetActive(true);
        if(MapNum == 2 || MapNum == 3)
            BackGroundObj.transform.GetChild( 2 ).gameObject.SetActive(true);
        if(MapNum == 4 || MapNum == 5 || MapNum == 6)
            BackGroundObj.transform.GetChild( 3 ).gameObject.SetActive(true);
        if (MapNum == 5 || MapNum == 6)
        {
            for(int i = 0; i<6; i++)
                BackGroundObj.transform.GetChild(3).GetChild( i+19 ).gameObject.SetActive(true);         
        }

        if (MapNum != -1 && MapNum != 6)
        {
            StartCoroutine("StageNamePopUp");

            StartCoroutine("MonsterSpawn");
            StartCoroutine("ObjectSpawn");
            StartCoroutine("CoinSpawn");
            StartCoroutine("WeaponItemSpawn");
        }

        StartCoroutine("MapRunTimeUp");

        if(MapNum == 0 || MapNum == 1)
            StartCoroutine("WoodFooterSpawn");
        if(MapNum == 2 || MapNum == 3)
            StartCoroutine("IceFooterSpawn");
        if (MapNum == 6) // 용궁 보물
        {
            StartCoroutine("StageNamePopUp");
            StartCoroutine("TreasureSpawn");
        }
        

        yield return null;
    }
    IEnumerator ScoreZoom()
    {
        GetScoreTime += 1;
        int time = 0;
        while (time < 3)
        {
            UI.transform.GetChild(0).GetChild(0).transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 3)
        {
            UI.transform.GetChild(0).GetChild(0).transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(0.8f);
        GetScoreTime -= 1;

    }
    IEnumerator MilkUIZoom()
    {
        int time = 0;
        while (time < 3)
        {
            MilkUI.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 3)
        {
            MilkUI.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
    }
    IEnumerator WeaponItemPopUp()
    {
        GameObject Box = WeaponItemPopUI.transform.GetChild(1).gameObject;
        WeaponItemPopText.text = WeaponName[PlayerPrefs.GetInt("curWp")].ToString();
        WeaponItemPopUI.SetActive(true);
        Box.transform.localScale = new Vector3(0f, 0f, 0);
        int time = 0;
        while (time < 6)
        {
            Box.transform.localScale += new Vector3(0.3f, 0.3f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        while (time < 5)
        {
            Box.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(0.5f);
        while (time < 5) // close
        {
            Box.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        WeaponItemPopUI.SetActive(false);
    }
    IEnumerator RevivalPopUp()
    {
        RevivalCheck = true;
        GameObject Box = WeaponItemPopUI.transform.GetChild(1).gameObject;
        WeaponItemPopText.text = "부활!!";
        WeaponItemPopUI.SetActive(true);
        Box.transform.localScale = new Vector3(0f, 0f, 0);
        int time = 0;
        while (time < 6)
        {
            Box.transform.localScale += new Vector3(0.3f, 0.3f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        while (time < 5)
        {
            Box.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(0.5f);
        while (time < 5) // close
        {
            Box.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        RevivalCheck = false;
        WeaponItemPopUI.SetActive(false);
    }
    IEnumerator StageNamePopUp()
    {
        GameObject Box = PortraitUI5.transform.GetChild(0).gameObject;
        PortraitUI5.SetActive(true);
        Box.transform.GetChild( MapNum ).gameObject.SetActive(true);
        Box.transform.localScale = new Vector3(0f, 0f, 0);
        int time = 0;
        while (time < 5)
        {
            Box.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(1.5f);
        while (time < 5) // close
        {
            Box.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;

        for (int i = 0; i < Box.transform.childCount; i++)
            Box.transform.GetChild(i).gameObject.SetActive(false);
        PortraitUI5.SetActive(false);

    }
    IEnumerator StageNamePopUpStop()
    {
        GameObject Box = PortraitUI5.transform.GetChild(0).gameObject;
        for (int i = 0; i < Box.transform.childCount; i++)
            Box.transform.GetChild(i).gameObject.SetActive(false);
        PortraitUI5.SetActive(false);
        yield return null;
    }
    IEnumerator BossNamePopUp()
    {
        Text content;
        content = PortraitUI6.transform.GetChild(1).gameObject.GetComponent<Text>();

        PortraitUI6.SetActive(true);
        PortraitUI6.transform.localScale = new Vector3(0f, 0f, 0);
        if (player_script.isSuper == true)
            PortraitUI6.transform.GetChild(2).gameObject.SetActive(true);

        content.text = BossNames[MapNum];
        int time = 0;
        while (time < 5)
        {
            PortraitUI6.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(1.5f);
        while (time < 5) // close
        {
            PortraitUI6.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        PortraitUI6.transform.GetChild(2).gameObject.SetActive(false);
        PortraitUI6.SetActive(false);

    }
    IEnumerator MilkSellPopUp()
    {
        Text content;
        content = PortraitUI6.transform.GetChild(1).gameObject.GetComponent<Text>();

        PortraitUI6.SetActive(true);
        PortraitUI6.transform.localScale = new Vector3(0f, 0f, 0);
        if (player_script.isSuper == true)
            PortraitUI6.transform.GetChild(2).gameObject.SetActive(true);

        content.text = milkNames[PlayerPrefs.GetInt("curMilkLevel")] + " " + PlayerPrefs.GetInt("curMilkNum") + "개를 팔았습니다.";
        int time = 0;
        while (time < 5)
        {
            PortraitUI6.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(2f);
        while (time < 5) // close
        {
            PortraitUI6.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        PortraitUI6.transform.GetChild(2).gameObject.SetActive(false);
        PortraitUI6.SetActive(false);

    }
    IEnumerator ItemDropPopUp(int num)
    {
        if (PlayerPrefs.GetInt("music") == 1)
            Puppy1.Play();
        
        if (num == 0)
        {
            ItemDropNameText.text = "[대포 수레] 획득";
            PlayerPrefs.SetInt("p_wgLock4", 0);
        }
        if (num == 1)
        {
            ItemDropNameText.text = "[얼음 수레] 획득";
            PlayerPrefs.SetInt("p_wgLock5", 0);
        }
        if (num == 2)
        {
            ItemDropNameText.text = "[로케트] 획득";
            PlayerPrefs.SetInt("p_wgLock6", 0);
        }
        DropPopUI.SetActive(true);
        DropPopUI.transform.GetChild(3).GetChild(num).gameObject.SetActive(true);   
        DropPopUI.transform.localScale = new Vector3(0f, 0f, 0);
        int time = 0;
        while (time < 6)
        {
            DropPopUI.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        while (time < 2)
        {
            DropPopUI.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(1.5f);
        while (time < 5) // close
        {
            DropPopUI.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        DropPopUI.transform.GetChild(3).GetChild(num).gameObject.SetActive(false);
        DropPopUI.SetActive(false);
    }
    public float Get_JumpDamage() // for Jump Attack
    {
        return Weapon[PlayerPrefs.GetInt("curWp")].GetComponent<WeaponAbility>().Get_Damage() * 2; // 점프 공격력은 현재 무기 공격력에 두 배
    }
    IEnumerator MonsterSpawn()
    {
        if (player_script.isSuper == true)
            yield return new WaitForSeconds(Random.Range(0.6f, 0.9f));
        else
        {
            if (MapNum == 4 || MapNum == 5)
                yield return new WaitForSeconds(Random.Range(1f - GameSpeed / 10, 2.3f - GameSpeed / 10));
            else
                yield return new WaitForSeconds(Random.Range(1.2f - GameSpeed / 10, 2.6f - GameSpeed / 10));
        }

        if (MapNum == 0)
        {
            Vector3 vec = new Vector3(9.3f, -1f, 0);
            Instantiate(Monster[0], vec, transform.rotation);
            StartCoroutine("MonsterSpawn");
        }
        if (MapNum == 1)
        {
            int rand = Random.Range(0,4);
            if (rand == 0) 
            {
                Vector3 vec = new Vector3(9.3f, Random.Range(0f, 0.5f), 0);
                Instantiate(Monster[3], vec, transform.rotation);
            }
            else
            {
                Vector3 vec = new Vector3(9.3f, -1f, 0);
                Instantiate(Monster[0], vec, transform.rotation);
            }
            StartCoroutine("MonsterSpawn");
        }
        if (MapNum == 2)
        {
            int rand = Random.Range(0,4);
            if (rand == 0) // 물개
            {
                Vector3 vec = new Vector3(9.3f, -1f, 0);
                Instantiate(Monster[2], vec, transform.rotation);
            }
            else
            {
                Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 0.45f), 0);
                Instantiate(Monster[1], vec, transform.rotation);
            }
            StartCoroutine("MonsterSpawn");
        }
        if (MapNum == 3)
        {
            Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 0.45f), 0);
            Instantiate(Monster[1], vec, transform.rotation);

            StartCoroutine("MonsterSpawn");
        }
        if (MapNum == 4)
        {
            Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 2.8f), 0);
            Instantiate(Monster[4], vec, transform.rotation);

            int rand = Random.Range(0,4);
            if (rand == 0) 
            {
                vec = new Vector3(9.5f, -1.1f, 0);
                Instantiate(Object[6], vec, transform.rotation);
            }

            StartCoroutine("MonsterSpawn");
        }
        if (MapNum == 5)
        {
            int rand = Random.Range(0,2);
            if (rand == 0)
            {
                Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 2.8f), 0);
                Instantiate(Monster[5], vec, transform.rotation);
            }
            else
            {
                Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 2.8f), 0);
                Instantiate(Monster[4], vec, transform.rotation);
            }

            rand = Random.Range(0,3);
            if (rand == 0) 
            {
                Vector3 vec = new Vector3(9.5f, -1.1f, 0);
                Instantiate(Object[6], vec, transform.rotation);
            }
            else
            {
                Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 2.8f), 0);
                Instantiate(Monster[5], vec, transform.rotation);
            }

            StartCoroutine("MonsterSpawn");
        }
    }
    IEnumerator ObjectSpawn()
    {
        if (player_script.isSuper == true)
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        else
        {
            if (MapNum == 4 || MapNum == 5)           
                yield return new WaitForSeconds(Random.Range(3.3f - GameSpeed / 10, 5.3f - GameSpeed / 10));
            else
                yield return new WaitForSeconds(Random.Range(4.3f - GameSpeed / 10, 8.3f - GameSpeed / 10));
        }
        
        if (MapNum == 0 || MapNum == 1)
        {
            Vector3 vec = new Vector3(9.3f, -1f, 0);
            Instantiate(Object[Random.Range(0,2)], vec, transform.rotation);
            StartCoroutine("ObjectSpawn");
        }
        if (MapNum == 2 || MapNum == 3)
        {   
            int rand = Random.Range(0, 4);
            Vector3 vec = new Vector3(9.3f, -1f, 0);
            if(rand == 0)
                Instantiate(Object[3], vec, transform.rotation);
            else
                Instantiate(Object[2], vec, transform.rotation);

            StartCoroutine("ObjectSpawn");
        }
        if (MapNum == 4 || MapNum == 5)
        {   
            int rand = Random.Range(0, 5);
           
            if (rand == 0)
            {
                Vector3 vec = new Vector3(9.3f, 3.4f, 0);
                Instantiate(Object[5], vec, transform.rotation);
            }
            else if (rand == 1 || rand == 2)
            {
                Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 2.8f), 0);
                Instantiate(Object[6], vec, transform.rotation);
            }
            else
            {
                Vector3 vec = new Vector3(9.3f, 0.23f, 0);
                Instantiate(Object[4], vec, transform.rotation);
            }

            rand = Random.Range(0, 5);
            if (rand == 0) // 상어 
            {
                Vector3 vec = new Vector3(15f, Random.Range(-1f, 2.8f), 0);
                Instantiate(Object[7], vec, transform.rotation);
            }

            StartCoroutine("ObjectSpawn");
        }
        if (MapNum == 0 || MapNum == 1 || MapNum == 2 || MapNum == 3)
        {
            CoinPosY = -0.07f;
            yield return new WaitForSeconds(0.4f);

            int rand2 = Random.Range(0, 3);
            if (rand2 == 0 || rand2 == 1)
                CoinPosY = -0.9f;
            if (rand2 == 2)
                CoinPosY = 0.7f;
        }
        else if(MapNum == 4 || MapNum == 5)
        {
            CoinPosY = 2.4f;
            yield return new WaitForSeconds(0.4f);

            int rand2 = Random.Range(0, 4);
            if (rand2 == 0 || rand2 == 1)
                CoinPosY = 0.7f;
            if (rand2 == 2)
                CoinPosY = 2f;
            if (rand2 == 3)
                CoinPosY = -0.9f;
        }
    }
    IEnumerator HouseSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        if (MapNum == 0 || MapNum == 1)
        {
            Vector3 vec = new Vector3(9f, -1.45f, 0);
            Instantiate(House[0], vec, transform.rotation);
            yield return null;
            //StartCoroutine("HouseSpawn");
        }
        if (MapNum == 2 || MapNum == 3)
        {
            Vector3 vec = new Vector3(9f, -1.49f, 0);
            Instantiate(House[1], vec, transform.rotation);
            yield return null;
            //StartCoroutine("HouseSpawn");
        }
        if (MapNum == 4)
        {
            Vector3 vec = new Vector3(9f, -1.45f, 0);
            Instantiate(House[0], vec, transform.rotation);
            yield return null;
            //StartCoroutine("HouseSpawn");
        }
        if (MapNum == 5)
        {
            Vector3 vec = new Vector3(9f, -1.41f, 0);
            Instantiate(House[2], vec, transform.rotation);
            yield return null;
            //StartCoroutine("HouseSpawn");
        }
    }
      
    IEnumerator BossSpawn()
    {
        if (MapNum == 1) // 고라니 숲 : 킹라니 - Player Stand방식
        {   
            //StartCoroutine("BossNamePopUp");
            player_script.isStand = true;
            GameSpeedTemp = GameSpeed;
            GameSpeed = 0;

            Vector3 vec = new Vector3(4f, 1f, 0);
            Instantiate(Boss[0], vec, transform.rotation);

            BGM2.Stop();
            BGM3.Stop();
            BGM4.Stop();
            if (PlayerPrefs.GetInt("music") == 1)
                BGM_Boss.Play();
        }
        if (MapNum == 3) // 펭귄 마을 : 꼬마 펭귄  - RUN
        {   
            //StartCoroutine("BossNamePopUp");
            GameSpeedTemp = GameSpeed;
            GameSpeed += 2;

            Vector3 vec = new Vector3(-13f, -0.83f, 0);
            Instantiate(Boss[1], vec, transform.rotation);

            BGM2.Stop();
            BGM3.Stop();
            BGM4.Stop();
            if (PlayerPrefs.GetInt("music") == 1)
                BGM_Boss.Play();
        }
        if (MapNum == 6) // 용궁 보물 클리어
        {
            ShowGameOverUI();
            GameOver = true;
        }
        if (MapNum == 0 || MapNum == 2 || MapNum == 4 || MapNum == 5)
        {
            StartCoroutine("BossClear");
        }
        yield return null;
    }

    IEnumerator CoinSpawn()
    {
        float Decre1 = 0;
        float Decre2 = 0; 

        if (player_script.isSuper == true)     
            Decre1 = 0.1f;

        for (int i = 0; i < Random.Range(3, 16); i++)
        {
            Vector3 vec = new Vector3(9f, CoinPosY, 0);
            Instantiate(Coin[3], vec, transform.rotation);
            yield return new WaitForSeconds(0.2f - Decre1);
        }
        CoinRangeTime = Random.Range(1.5f,2.5f);

        if (player_script.isSuper == true)     
            Decre2 = 1.5f;
        
        yield return new WaitForSeconds(CoinRangeTime - Decre2);
        StartCoroutine("CoinSpawn");
    }
    IEnumerator WeaponItemSpawn()
    {
        yield return new WaitForSeconds(Random.Range(10f, 15f));
        Vector3 vec = new Vector3(9f, -0.9f, 0);
        Instantiate(WeaponItem, vec, transform.rotation);
        StartCoroutine("WeaponItemSpawn");
    }
    IEnumerator WoodFooterSpawn()
    {
        yield return new WaitForSeconds(Random.Range(8f, 11f));
        Vector3 vec = new Vector3(23f, -0.69f, 0);
        GameObject Wood = Instantiate(Footer[1], vec, transform.rotation);
        Destroy(Wood, 15f);
    }
    IEnumerator IceFooterSpawn()
    {
        if (MapNum == 2 || MapNum == 3)
        {
            yield return new WaitForSeconds(Random.Range(10f , 16f));

            if(MapNum == 2)
                StopCoroutine("MonsterSpawn");
            
            StopCoroutine("ObjectSpawn");
            //StopCoroutine("HouseSpawn");

            isIceFooter = true;

            int time = 2;
            while (time > 0)
            {
                if (player_script.isSuper == true)
                    StartCoroutine("IceFooterReset", 0);

                time--;
                yield return new WaitForSeconds(0.7f); // 1
            }

            StartCoroutine("IceFooterSpawnMini");

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        
            StartCoroutine("IceFooterReset", 0);
        }
    }
    IEnumerator IceFooterReset(int BossArrival)
    {
        BackGroundObj.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        BackGroundObj.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
        BackGroundObj.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
        BackGroundObj.transform.GetChild(2).GetChild(4).gameObject.SetActive(true);
        BackGroundObj.transform.GetChild(2).GetChild(5).gameObject.SetActive(true);
        BackGroundObj.transform.GetChild(2).GetChild(6).gameObject.SetActive(true);
        //yield return new WaitForSeconds(6f);

        StopCoroutine("IceFooterSpawn");
        isIceFooter = false;

        StopCoroutine("IceFooterSpawnMini");

        if (BossArrival == 0)
        {
            StartCoroutine("MonsterSpawn");
            StartCoroutine("ObjectSpawn");
            //StartCoroutine("HouseSpawn");

            StartCoroutine("IceFooterSpawn");
        }
        yield return null;
    }
    IEnumerator IceFooterSpawnMini()
    {
        if (MapNum == 2 || MapNum == 3)
        {
            Vector3 vec = new Vector3(12f, -2.3f, 0);
            Instantiate(Footer[0], vec, transform.rotation);
            yield return new WaitForSeconds(Random.Range(1.3f, 1.6f));
            StartCoroutine("IceFooterSpawnMini");
        }
    }
    IEnumerator TreasureSpawn()
    {
        Vector3 vec = new Vector3(9.49f, 4.33f, 0);
        GameObject treasure = Instantiate(Footer[2], vec, transform.rotation);
        Destroy(treasure, 35f);
        yield return null;
    }
    IEnumerator Toutorial()
    {
        //int time = 0;

        Vector3 vec = new Vector3(9.5f, -1f, 0);
        Instantiate(Object[0], vec, transform.rotation);

        yield return new WaitForSeconds(3.3f);
        PortraitUI1.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSeconds(0.05f);
        PortraitUI1.SetActive(false);

        vec = new Vector3(9.5f, 0.7f, 0);
        Instantiate(Coin[3], vec, transform.rotation);

        yield return new WaitForSeconds(3.1f);
        PortraitUI1.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSeconds(0.1f);
        PortraitUI1.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        PortraitUI1.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSeconds(0.05f);
        PortraitUI1.SetActive(false);

        vec = new Vector3(9.5f, -1f, 0);
        Instantiate(Monster[0], vec, transform.rotation);

        yield return new WaitForSeconds(2f);
        PortraitUI2.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSeconds(0.05f);
        PortraitUI2.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        PortraitUI2.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSeconds(0.05f);
        PortraitUI2.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        PortraitUI2.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSeconds(0.05f);
        PortraitUI2.SetActive(false);

        yield return new WaitForSeconds(2f);
        vec = new Vector3(3f, 0f, 0);
        GameObject Milk = Instantiate(MilkItem, vec, transform.rotation);
        Milk.GetComponent<MilkItem>().type = "Drop";    
        Milk = Instantiate(MilkItem, vec, transform.rotation);
        Milk.GetComponent<MilkItem>().type = "Drop";    
        Milk = Instantiate(MilkItem, vec, transform.rotation);
        Milk.GetComponent<MilkItem>().type = "Drop";
        Milk = Instantiate(MilkItem, vec, transform.rotation);
        Milk.GetComponent<MilkItem>().type = "Drop";
        Milk = Instantiate(MilkItem, vec, transform.rotation);
        Milk.GetComponent<MilkItem>().type = "Drop";


        yield return new WaitForSeconds(1f);
        vec = new Vector3(9.5f, -1f, 0);
        Instantiate(Object[0], vec, transform.rotation);

        yield return new WaitForSeconds(5f);
        vec = new Vector3(9f, -1.45f, 0);
        Instantiate(House[0], vec, transform.rotation);

    }

    IEnumerator EndToutorial()
    {
        yield return new WaitForSeconds(3f);
        PlayerPrefs.SetInt("toutorial", 2);
        BGM1.Stop();

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }



}
