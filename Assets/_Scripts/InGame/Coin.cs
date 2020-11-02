using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int m_value;
    public string type;

    Rigidbody2D rigid;
    CircleCollider2D coll;

    GameManager GM;

    AudioSource CoinGet;
    AudioSource CoinDrop;

    bool isBound = false;
    int score;

	void Start () 
    {
        if (transform.position.x >= 3f)
            isBound = false;
        else
            isBound = true;
        
        CoinGet = GameObject.Find("CoinGet").GetComponent<AudioSource>();
        CoinDrop = GameObject.Find("CoinDrop").GetComponent<AudioSource>();

        rigid = gameObject.GetComponent<Rigidbody2D>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (m_value == 1) // 동전들 
            coll = gameObject.transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>();
        
        if (type == "") // air 동전이 아니면 등장시 바운스 
        {
            if (PlayerPrefs.GetInt("music") == 1)
                CoinDrop.Play();

            float randX = Random.Range(-1f, 4f);
            float randY = Random.Range(6f, 10f);

            Vector2 up = new Vector2(randX, randY);
            rigid.AddForce(up, ForceMode2D.Impulse);

        }
        else if (type == "treasure")
        {
        }
        else
            coll.enabled = false;

        if (m_value == 1)
        {
            if (GM.GameLevel == 1)
                m_value = 2;
            if (GM.GameLevel == 2)
                m_value = 3;
            if (GM.GameLevel == 3)
                m_value = 4;
        }
        if (m_value == 50)
        {
            if (GM.GameLevel == 1)
                m_value = 65;
            if (GM.GameLevel == 2)
                m_value = 80;
            if (GM.GameLevel == 3)
                m_value = 100;
        }
        if (m_value == 100)
        {
            if (GM.GameLevel == 1)
                m_value = 125;
            if (GM.GameLevel == 2)
                m_value = 150;
            if (GM.GameLevel == 3)
                m_value = 180;
        }
    
    }
	
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed; 
        if (type == "air")
        {
            if (GM.isBossMode == true)
                Destroy(gameObject);
        }
        if (transform.position.x <= -9)
            Destroy(gameObject);
        //if (transform.position.x >= 3.3f)
        //    Destroy(gameObject);
        if (transform.position.y <= -5)
            Destroy(gameObject);
        //if (transform.position.x >= 3f && isBound == true)
        //    Bound();
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")       
        {
            if (m_value < 5)
                score = 1;
            if (m_value >= 50 && m_value < 100)
                score = 50;
            if (m_value >= 100)
                score = 100;
            
            if (PlayerPrefs.GetInt("music") == 1)
                CoinGet.Play();
            GM.StartCoroutine("ScoreZoom");
            GM.GetScore = "+" + m_value;
            GM.Gold += m_value;
            GM.Score += score*10;
            PlayerPrefs.SetInt("curGold", GM.Gold);

            GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
            ShowScore.GetComponent<Chat>().Content = (score*10).ToString();

            if (m_value < 5) // 동전들 
            {
                rigid.isKinematic = true;
                rigid.velocity = new Vector2(rigid.velocity.x, 7f);
                coll.enabled = false;
                Destroy(gameObject, 0.15f);
            }
            else
                Destroy(gameObject);
            
        }
        if(other.gameObject.tag == "House")       
            Bound();
        
    }

    void Bound()
    {
        isBound = false;
        float randX = Random.Range(-5f, -8f);
        float randY = Random.Range(5f, 8f);

        Vector2 up = new Vector2(randX, randY);
        rigid.AddForce(up, ForceMode2D.Impulse);
    }
	
}
