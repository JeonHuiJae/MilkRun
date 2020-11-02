using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreManager : MonoBehaviour
{
    AudioSource Gift;

    public GameObject MainUI;

    public Text gold;//현재 골드
                      //private Text wagon;//현재 수레
                      //private Text max;//용량
    public Text popUp_text;
    public GameObject popUp;
    public Text content;
    public Text upgradePrice;
    public Text milkName;
    public Text sellPrice;
    public Text stock;
    public Image goldImg;
    public GameObject milkImg;
    public GameObject Pang;
    public GameObject timeCount;
    TitleManager TM;

    private Text count;
    private int stockMax;
    private int preLevel = 1;
    private int milkNum = 0;
    private int level = 0;
    private string wagStr;
    private Sprite[] milk_img = new Sprite[10];
    string[] milkNames = new string[] { "","흰 우유", "딸기 우유", "바나나 우유", "커피 우유", "초코 우유", "요구르트", "두유", "고칼슘 우유", "코코에몽", "딸기 요플레" };
    int[] upgradePrices = new int[] { 0, 800, 2000, 3500, 7500, 11000, 19000, 30000, 45000, 68000, 0 };
    int[] sellPrices = new int[] { 0, 15, 30, 48, 65, 90, 115, 140, 180, 235, 280};
    int[] wagonSlot = new int[] { 6, 9, 13, 12, 11, 16, 15};

    private int[] wagonContent = new int[12];

    // Use this for initialization
    void Awake()
    {
        if (PlayerPrefs.GetInt("timeFlag") == 1)
            timeCount.SetActive(true);
        else
            timeCount.SetActive(false);

        Gift = GameObject.Find("Gift").GetComponent<AudioSource>();
        count = timeCount.transform.GetChild(0).GetComponent<Text>();
        TM = GameObject.FindGameObjectWithTag("TitleManager").GetComponent<TitleManager>();
        for (int i = 0; i < 10; i++) {
            milk_img[i] = TM.Milk[i];
        }
        gold.text = PlayerPrefs.GetInt("curGold")+"";
        level = PlayerPrefs.GetInt("curMilkLevel");
        stockMax = PlayerPrefs.GetInt("milkStockMax");
        setLevel();
        wagonCon();
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("timeFlag") == 1)
        {
            System.DateTime now = System.DateTime.Now.ToLocalTime();
            System.TimeSpan span = (now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            int nowTime = (int)span.TotalSeconds;
            int endTime = PlayerPrefs.GetInt("endTime");

            if (endTime <= nowTime) { // 다됨
                PlayerPrefs.SetInt("timeFlag", 0);
                PlayerPrefs.SetInt("milkStock", PlayerPrefs.GetInt("milkStockMax")); // 리필
                PlayerPrefs.Save();
                timeCount.SetActive(false);
                wagonCon();
            }
            else { // 아직
                int diff = endTime - nowTime;
                count.text = diff / 60 + ":" + diff % 60;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (PlayerPrefs.GetInt("timeFlag")==1)
            timeCount.SetActive(true);
        else
            timeCount.SetActive(false);
        gold.text = PlayerPrefs.GetInt("curGold") + "";
        wagonCon();
        popUp.SetActive(false);
    }


    //==== function ====//

    public void ad() { // 바로 리필 광고
        PlayerPrefs.SetInt("timeFlag", 0);
        PlayerPrefs.SetInt("milkStock", PlayerPrefs.GetInt("milkStockMax")); // 리필
        PlayerPrefs.Save();
        timeCount.SetActive(false);
        wagonCon();
    }

    public void upgrade() { // 업그레이드 하기
        int curGold = PlayerPrefs.GetInt("curGold");
        if (curGold >= upgradePrices[level])
        {
            if (level != 10)
            {
                StartCoroutine("UpGradeAnim");// 팡 애니메이션 추가
                PlayerPrefs.SetInt("curGold", curGold - upgradePrices[level]);
                preLevel = level;
                level++;
                PlayerPrefs.SetInt("curMilkLevel", level);
                setLevel();
                gold.text = PlayerPrefs.GetInt("curGold") + "";

                PlayerPrefs.SetInt("curMilkNum", wagonSlot[PlayerPrefs.GetInt("curWg")]); // 꽉채워주기
                wagonCon();
                PlayerPrefs.Save();
                if (level == 10)
                {
                    upgradePrice.gameObject.SetActive(false);
                    goldImg.gameObject.SetActive(false);
                }
                PlayerPrefs.SetInt("milkStock", PlayerPrefs.GetInt("milkStockMax")); // 리필
                wagonCon();
            }
            else
            {
                popUp_text.text = "최고 레벨입니다";
                StartCoroutine("PopUpOpenAnim");
                popUp.SetActive(true);
            }
        }
        else {
            popUp_text.text = "골드가 부족합니다.";
            StartCoroutine("PopUpOpenAnim");
            popUp.SetActive(true);
        }
    }

    public void milkToWagon() // 수레에 담기
    {
        if (PlayerPrefs.GetInt("timeFlag")==0)
        {
            if (PlayerPrefs.GetInt("curMilkNum") == wagonSlot[PlayerPrefs.GetInt("curWg")])
            {
                popUp_text.text = "이미 수레가 가득찼습니다.";
                StartCoroutine("PopUpOpenAnim");
                popUp.SetActive(true);
            }
            else
            {
                int nowStock = PlayerPrefs.GetInt("milkStock"); // 재고
                int nowSlot = wagonSlot[PlayerPrefs.GetInt("curWg")]; // 내 수레 크기
                int myMilkNum = PlayerPrefs.GetInt("curMilkNum"); // 가지고있는 우유 수

                if (nowStock > nowSlot - myMilkNum)
                {
                    PlayerPrefs.SetInt("curMilkNum", nowSlot); // 꽉 채움
                    PlayerPrefs.SetInt("milkStock", nowStock - (nowSlot - myMilkNum));
                }
                else //재고로 꽉 채우지 못함
                {
                    PlayerPrefs.SetInt("curMilkNum", myMilkNum + nowStock); //재고가득 털어 채움
                    PlayerPrefs.SetInt("milkStock", 0);
                    // 현재시간
                    System.DateTime now = System.DateTime.Now.ToLocalTime();
                    System.TimeSpan span = (now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
                    int nowTime = (int)span.TotalSeconds;

                    PlayerPrefs.SetInt("endTime", nowTime + 180); // 3분뒤
                    PlayerPrefs.SetInt("timeFlag", 1);
                    timeCount.SetActive(true);
                }
                wagonCon();
            }
            PlayerPrefs.Save();
        }
    }

    private void setLevel() {
        if (level == 10)
        {
            upgradePrice.gameObject.SetActive(false);
            goldImg.gameObject.SetActive(false);
        }
        milkImg.transform.GetChild(preLevel-1).gameObject.SetActive(false);
        milkImg.transform.GetChild(level - 1).gameObject.SetActive(true);
        upgradePrice.text = upgradePrices[level] + "";
        sellPrice.text = "원가 : " + sellPrices[level];
        milkName.text = milkNames[level] + " (lv. " + level + ")";
    }

    private void wagonCon() // 내용물 개수 
    {
        stock.text = "재고" + '\n' + PlayerPrefs.GetInt("milkStock") + "/" +stockMax;
        content.text = PlayerPrefs.GetInt("curMilkNum") + " / " + wagonSlot[PlayerPrefs.GetInt("curWg")];
    }

    public void PopUpClose()
    {
        StartCoroutine("PopUpCloseAnim");
    }
    IEnumerator PopUpOpenAnim()
    {
        popUp.transform.localScale = new Vector3(0f, 0f, 0);
        int time = 0;
        while (time < 6)
        {
            popUp.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        while (time < 4)
        {
            popUp.transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
    }
    IEnumerator PopUpCloseAnim()
    {
        int time = 0;
        while (time < 5)
        {
            popUp.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
    }
    IEnumerator UpGradeAnim()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            Gift.Play();
        }
        for (int i = 0; i < 25; i++)
            Instantiate(Pang, milkImg.transform.position, milkImg.transform.rotation);
        
        int time = 0;
        while (time < 3)
        {
            milkImg.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 3)
        {
            milkImg.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        milkImg.transform.localScale = new Vector3(1, 1, 1);

        yield return null;
    }
        

}
