using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WagonContentManager : MonoBehaviour {

    private Sprite[] milk_img = new Sprite[10];
    int myMilkNum;
    public Text priceText;
    public Text milkNum;
    public Text wagonSlotText;
    GameManager GM;

    void Awake () 
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GM.Price = GM.sellPrices[ PlayerPrefs.GetInt("curMilkLevel") ];

        for (int i = 0; i < 10; i++)
            milk_img[i] = GM.Milk[i];
        
        PlayerPrefs.SetInt("curMilkNum", 0);
        setWagonContent();
    }
	
	
	void Update () 
    {
        priceText.text = GM.Price.ToString();
        milkNum.text = myMilkNum.ToString();
        wagonSlotText.text = "/" + GM.wagonSlot[PlayerPrefs.GetInt("curWg")];

        if (myMilkNum != PlayerPrefs.GetInt("curMilkNum"))
            setWagonContent();
        
	}

    public void setWagonContent()
    {
        myMilkNum = PlayerPrefs.GetInt("curMilkNum");
        if (myMilkNum >= 0)
        {
            // 이미지 설정
            this.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = milk_img[PlayerPrefs.GetInt("curMilkLevel") - 1];
            this.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().SetNativeSize();
            milkNum.text = myMilkNum.ToString();
        }
        else {
            this.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            milkNum.text = myMilkNum.ToString();
        }
    }

    public void ReSetMilk()
    {
        PlayerPrefs.SetInt("curMilkNum", GM.wagonSlot[PlayerPrefs.GetInt("curWg")]);
        setWagonContent();
    }

    public void IncreasePrice()
    {
        GM.Price += GM.Increment[PlayerPrefs.GetInt("curMilkLevel")];
    }
}
