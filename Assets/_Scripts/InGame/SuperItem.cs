using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperItem : MonoBehaviour {

    public int m_value; // 1 : 부스터, 2: 황금뼈다귀 

    Rigidbody2D rigid;

    Player player;

    GameManager GM;

    AudioSource BoosterDrop;
    AudioSource BoosterReady;
    AudioSource GoldBoneDrop;
    AudioSource GoldBoneGet;
    AudioSource CoinDrop;
    AudioSource Gift;

    bool isBound = false;

    void Start () 
    {
        if (transform.position.x >= 3f)
            isBound = false;
        else
            isBound = true;

        BoosterDrop = GameObject.Find("BoosterDrop").GetComponent<AudioSource>();
        BoosterReady = GameObject.Find("BoosterReady").GetComponent<AudioSource>();
        GoldBoneDrop = GameObject.Find("GoldBoneDrop").GetComponent<AudioSource>();
        GoldBoneGet = GameObject.Find("GoldBoneGet").GetComponent<AudioSource>();
        CoinDrop = GameObject.Find("CoinDrop").GetComponent<AudioSource>();
        Gift = GameObject.Find("Gift").GetComponent<AudioSource>();

        if (m_value == 1)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                BoosterDrop.Play();
        }
        if (m_value == 2)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                GoldBoneDrop.Play();
        }
        if (m_value == 3)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                CoinDrop.Play();
        }
        rigid = gameObject.GetComponent<Rigidbody2D>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        float randX = Random.Range(-2f, 4f);
        float randY = Random.Range(5f, 10f);

        Vector2 up = new Vector2(randX, randY);
        rigid.AddForce(up, ForceMode2D.Impulse);
    }

    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed; 

        if (transform.position.x <= -9)
            Destroy(gameObject);
        //if (transform.position.x >= 3.3f)
        //    Destroy(gameObject);
        if (transform.position.y <= -5)
            Destroy(gameObject);
        //if (transform.position.x >= 3f && isBound == true)
        //    Bound();

        if (m_value != 3 && GM.isBossMode == true)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (m_value == 1 && GM.isBossMode == false)
            {
                if (PlayerPrefs.GetInt("music") == 1)
                    BoosterReady.Play();

                player.StartCoroutine("BoosterAction");

                Destroy(gameObject);
            }
            if (m_value == 2 && GM.isBossMode == false)
            {
                if (PlayerPrefs.GetInt("music") == 1)
                    GoldBoneGet.Play();

                player.StartCoroutine("GoldBoneAction");

                Destroy(gameObject);
            }
            if (m_value == 3)
            {
                if (PlayerPrefs.GetInt("music") == 1)
                    Gift.Play();

                if(GM.Life < 5)
                    GM.Life += 1;
                GM.LifeDraw();

                GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, GM.transform.rotation);
                ShowScore.GetComponent<Chat>().Content = "Life +1";

                Destroy(gameObject);
            }
        }
           
    }

    void Bound()
    {
        isBound = false;
        float randX = Random.Range(-10f, -10f);
        float randY = Random.Range(7f, 10f);

        Vector2 up = new Vector2(randX, randY);
        rigid.AddForce(up, ForceMode2D.Impulse);
    }

}
