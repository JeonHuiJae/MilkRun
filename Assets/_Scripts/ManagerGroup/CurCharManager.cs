using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurCharManager : MonoBehaviour {
    private GameObject dog;
    private GameObject wag;

    private GameObject FD;
    private GameObject FW;
    public Animator animator;
    TitleManager TM;

    private AudioSource TitleMusic;

    // Use this for initialization
    void Awake() {

        TM = GameObject.FindGameObjectWithTag("TitleManager").GetComponent<TitleManager>();
        if (!PlayerPrefs.HasKey("curCh")) // 처음 실행시 playerPrefs 초기화
        {
            initPlayerPrefs();
        }
        //initPlayerPrefs();
        //PlayerPrefs.SetInt("curGold", 100000);
        //PlayerPrefs.SetInt("p_wgLock5", 0);
        //PlayerPrefs.SetInt("p_wgLock6", 0);
        //PlayerPrefs.SetInt("p_wpLock5", 0);
        //PlayerPrefs.SetInt("p_wpLock6", 0);

        //PlayerPrefs.SetInt("curMilkNum", 1);
        //PlayerPrefs.SetInt("milkStock",1);
        //PlayerPrefs.Save();

        FD = GameObject.Find("Bdog");
        FW = GameObject.Find("Bwagon");
        changeWag();
        changeChar();

        TitleMusic = GameObject.Find("MilkRun_Title").GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("music")== 1 && PlayerPrefs.GetInt("toutorial") == 0)
            TitleMusic.Play();
    }
    void initPlayerPrefs()
    {
        // 유저 데이터
        PlayerPrefs.SetInt("curCh", 0);
        PlayerPrefs.SetInt("curWg", 0);
        PlayerPrefs.SetInt("curWp", 0);
        PlayerPrefs.SetInt("curGold", 0);

        PlayerPrefs.SetInt("curMilkNum", 10);
        PlayerPrefs.SetInt("curMilkLevel", 1);
        PlayerPrefs.SetInt("milkStockMax", 25);
        PlayerPrefs.SetInt("milkStock", PlayerPrefs.GetInt("milkStockMax"));

        // 설정
        PlayerPrefs.SetInt("toutorial", 1);
        PlayerPrefs.SetInt("music", 1);
        PlayerPrefs.SetInt("language", 1);
        PlayerPrefs.SetInt("ad", 1);

        PlayerPrefs.SetInt("timeFlag", 0);

        // 캐릭터
        PlayerPrefs.SetInt("p_charLock", 0); // 0번만 풀림
        for (int i = 1; i < 5; i++)
        {
            PlayerPrefs.SetInt("p_charLock" + i, 1);
        }
        // 수레
        PlayerPrefs.SetInt("p_wgLock0", 0); // 0번만 풀림
        for (int i = 1; i < 7; i++)
        {
            PlayerPrefs.SetInt("p_wgLock" + i, 1);
        }
        // 무기
        PlayerPrefs.SetInt("p_wpLock0", 0); // 0번만 풀림
        for (int i = 1; i < 7; i++)
        {
            PlayerPrefs.SetInt("p_wpLock" + i, 1);
        }

        // 맵 최고기록
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("p_bestScore" + i, 0);
        }

        PlayerPrefs.Save();
    }

    public void laydown() {
        if (animator.GetBool("lay"))
            animator.SetBool("lay", false);
        else
        {
            animator.SetBool("lay", true);
            StartCoroutine("ZzzSpawn");
        }
    }

    public void changeChar() {
        Destroy(transform.GetChild(0).GetChild(0).gameObject);
        dog = Instantiate( TM.Dog[PlayerPrefs.GetInt("curCh")]);
        dog.transform.SetParent(FD.transform);
        dog.transform.localScale = Vector3.one;
        dog.transform.localPosition = new Vector3(0, 0, 0);
        wag.transform.GetChild(0).rotation = Quaternion.identity;
        animator = dog.GetComponent<Animator>();
    }
    public void changeWag() {
        Destroy(transform.GetChild(1).GetChild(0).gameObject);
        wag = Instantiate(TM.Wagon[PlayerPrefs.GetInt("curWg")]);
        wag.transform.SetParent(FW.transform);
        wag.transform.localScale = Vector3.one;
        wag.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void musicControl(int state)
    { // 음악 on off 
        if (state == 1)
        { // on
            TitleMusic.Play();
        }
        else
        {
            TitleMusic.Stop(); //pause 하면 일시정지 ㅋㄷ
        }
    }
        

    IEnumerator ZzzSpawn()
    {
        GameObject zzz;
        zzz = Instantiate(TM.Zzz);
        zzz.transform.localScale = Vector3.one;
        zzz.transform.localPosition = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));
        if (animator.GetBool("lay"))
            StartCoroutine("ZzzSpawn");
    }
}
