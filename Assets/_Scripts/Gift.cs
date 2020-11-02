using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gift : MonoBehaviour {

    public int GiftOpenFlag = 0;
    AudioSource GiftSound;

    public GameObject CurChar;
    public GameObject BG;
    public GameObject Pang;

    public GameObject GoldUI;

    public Text GoldText;
    int Gold;

    MainMenuManager MMG;

	void Start () 
    {
        int rand = Random.Range(0, 15);

        if (rand >= 0 && rand <= 2)
            Gold = 100;
        if (rand >= 3 && rand <= 5)
            Gold = 200;
        if (rand >= 6 && rand <= 8)
            Gold = 300;
        if (rand >= 9 && rand <= 10)
            Gold = 500;
        if (rand >= 11 && rand <= 12)
            Gold = 1000;
        if (rand == 13)
            Gold = 1500;
        if (rand == 14)
            Gold = 2000;

        MMG = BG.GetComponent< MainMenuManager>();
        GiftSound = GameObject.Find("Gift").GetComponent<AudioSource>();
	}
    void Update()
    {
        GoldText.text = Gold.ToString();

        if (GoldUI.activeSelf == true)
            GoldUI.transform.GetChild(0).transform.Rotate(Vector3.back * 120f * Time.deltaTime);
    }
    public void GiftOpenFuc()
    {
        if (GiftOpenFlag == 1)
        {
            StartCoroutine("GiftOpen");
            GiftOpenFlag = 2;
        }
    }
    IEnumerator GiftOpen()
    {
        if (PlayerPrefs.GetInt("music") == 1)
            GiftSound.Play();

        int time = 0;
        while (time < 3)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 3)
        {
            transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
            
        transform.localScale = new Vector3(0, 0, 0);
        time = 0;

        for (int i = 0; i < 25; i++)
        {
            Instantiate(Pang, transform.position, transform.rotation);
            //gold.GetComponent<Pang>().GoldTag = true;
        }
        GoldUI.SetActive(true);
        while (time < 3)
        {
            GoldUI.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 3)
        {
            GoldUI.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }

        GoldUI.transform.localScale = new Vector3(1, 1, 1);
        CurChar.SetActive(true);
        BG.SetActive(true);
        MMG.StartCoroutine("CheakRewardAd");
        yield return null;
    }

    public void GetGoldFuc()
    {
        PlayerPrefs.SetInt("curGold", PlayerPrefs.GetInt("curGold") + Gold);
        GiftOpenFlag = 0;

        transform.localScale = new Vector3(1, 1, 1);
        GoldUI.transform.localScale = new Vector3(1, 1, 1);
        GoldUI.SetActive(false);
        gameObject.SetActive(false);
    }
}
