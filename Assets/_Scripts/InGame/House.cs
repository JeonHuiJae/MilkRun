using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public int m_num;

    int myMilkLevel;
    int myMilkNum;

    AudioSource Puppy1;
    AudioSource CoinGet;
    AudioSource JumpAttack;
    AudioSource HouseDie_s;

    GameObject MilkUI;
    GameManager GM;
    Player player;

    public bool isAlive = true;
    public bool isSell = true;

    void Start () 
    {
        JumpAttack = GameObject.Find("JumpAttack").GetComponent<AudioSource>();
        Puppy1 = GameObject.Find("Puppy1").GetComponent<AudioSource>();
        CoinGet = GameObject.Find("CoinGet").GetComponent<AudioSource>();
        HouseDie_s = GameObject.Find("HouseDie").GetComponent<AudioSource>();

        MilkUI = GameObject.FindGameObjectWithTag("MilkUI");
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        myMilkLevel = PlayerPrefs.GetInt("curMilkLevel");
        myMilkNum = PlayerPrefs.GetInt("curMilkNum");
    }

    void Update () 
    {
        transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed; 

        if (transform.position.x <= -8)
            Destroy(gameObject);
        if (transform.position.y <= -5)
            Destroy(gameObject); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && isSell == true && myMilkNum > 0)   
        {
            myMilkNum = PlayerPrefs.GetInt("curMilkNum");
            if (PlayerPrefs.GetInt("music") == 1)
            {
                Puppy1.Play();
                CoinGet.Play();
            }
            StartCoroutine("Zoom");
            isSell = false;

            GM.StartCoroutine("MilkSellPopUp");
            GM.StartCoroutine("ScoreZoom");
            if (player.isSuper == false)
            {
                GM.GetScore = "+" + GM.Price * myMilkNum;
                GM.Score += myMilkNum * 200;
                GM.Gold += GM.Price * myMilkNum;

                GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
                ShowScore.GetComponent<Chat>().Content = (myMilkNum * 200).ToString();
            }
            else // 수퍼 아이템으로 우유팔면 2배
            {
                GM.GetScore = "+" + GM.Price * myMilkNum * 2;
                GM.Score += myMilkNum * 200 * 2;
                GM.Gold += GM.Price * myMilkNum * 2;

                GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
                ShowScore.GetComponent<Chat>().Content = (myMilkNum * 200 * 2).ToString();
            }

            PlayerPrefs.SetInt("curGold", GM.Gold);

            myMilkNum = 0;
            PlayerPrefs.SetInt("curMilkNum", 0);

            if (PlayerPrefs.GetInt("toutorial") == 1)
            {
                Time.timeScale = 1f;
                player.isStand = true;
                GM.GameSpeed = 0f;
                GM.StartCoroutine("EndToutorial");
            }

            if (player.isSuper == true && isAlive == true)
            {
                Die();
                isAlive = false;
            }
        }
        if (other.gameObject.tag == "Player" && player.isSuper == true && isAlive == true)
        {
            Die();
            isAlive = false;
        }
    }

    IEnumerator Zoom()
    {
        int time = 0;
        while (time < 3)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0);
            MilkUI.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 3)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            MilkUI.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(0.5f);
    }

    void Die()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            JumpAttack.Play();
            HouseDie_s.Play();
        }
        for (int i = 0; i < 5; i++) 
            player.StartCoroutine("Vibe");

        int num = 5;
        if (m_num == 2)
            num = 18;

        transform.GetChild(0).gameObject.SetActive(false);
        Rigidbody2D[] rigid = new Rigidbody2D[num];
        for (int i = 0; i < num; i++)
        {
            transform.GetChild(i+1).gameObject.SetActive(true);
            rigid[i] = transform.GetChild(i + 1).gameObject.GetComponent<Rigidbody2D>();
        }

        for (int i = 0; i < num; i++)
        {
            float randX = Random.Range(-10f, 10f);
            float randY = Random.Range(8f, 12f);

            Vector2 up = new Vector2(randX, randY);
            rigid[i].AddForce(up, ForceMode2D.Impulse);
        }

        Destroy(gameObject, 3f);
    }
        
}







