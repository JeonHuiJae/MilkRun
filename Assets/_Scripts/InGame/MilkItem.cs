using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkItem : MonoBehaviour {

    public string type = "";

    Rigidbody2D rigid;
    SpriteRenderer image;
    CapsuleCollider2D coll;
    Sprite[] milk_img = new Sprite[10];

    GameManager GM;
    GameObject MilkUI;

    AudioSource Get;
    AudioSource DropPla;
    AudioSource DropGla;

    bool isBound = false;
    bool GetFlag = false;

    bool isSound = false;

    void Start () 
    {
        if (transform.position.x >= 3f)
            isBound = false;
        else
            isBound = true;

        Get = GameObject.Find("MilkGet").GetComponent<AudioSource>();
        DropGla = GameObject.Find("MilkDropGlass").GetComponent<AudioSource>();
        DropPla = GameObject.Find("MilkDropPla").GetComponent<AudioSource>();

        MilkUI = GameObject.FindGameObjectWithTag("MilkUI");
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rigid = gameObject.GetComponent<Rigidbody2D>();
        coll = transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>();
        image = GetComponent<SpriteRenderer>();
        image.sprite = GM.Milk[PlayerPrefs.GetInt("curMilkLevel") - 1];

        float randX;
        float randY;
        if (type == "Drop") // 몬스터에게 드랍되는거
        {
            if (PlayerPrefs.GetInt("music") == 1)
            {
                if (PlayerPrefs.GetInt("curMilkLevel") == 1 || PlayerPrefs.GetInt("curMilkLevel") == 2)
                    DropGla.Play();
                else
                    DropPla.Play();
            }

            randX = Random.Range(-1f, 6f);
            randY = Random.Range(5f, 9f);
            Vector2 up = new Vector2(randX, randY);
            rigid.AddForce(up, ForceMode2D.Impulse);
            GetFlag = true;
        }
        else
        {
            randX = Random.Range(-1f, 6f);
            randY = Random.Range(7f, 13f);
            Vector2 up = new Vector2(randX, randY);
            rigid.AddForce(up, ForceMode2D.Impulse);
            coll.enabled = false;
            Invoke("GetFunc", 0.2f);
        }
    }

    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed; 

        if (transform.position.x <= -9)
            Destroy(gameObject);
       // if (transform.position.x >= 3.3f)
       //     Destroy(gameObject);
        if (transform.position.y <= -5)
            Destroy(gameObject);
        //if (transform.position.x >= 3f && isBound == true)
        //    Bound();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && GetFlag == true && PlayerPrefs.GetInt("curMilkNum") != GM.wagonSlot[PlayerPrefs.GetInt("curWg")])       
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Get.Play();

            int myMilkNum;
            GM.StartCoroutine("MilkUIZoom");
            myMilkNum = PlayerPrefs.GetInt("curMilkNum");
            PlayerPrefs.SetInt("curMilkNum", myMilkNum + 1);
           
            Destroy(gameObject);
        }
        
        if (other.gameObject.tag == "Ground" && isSound == false)
        {
            if (PlayerPrefs.GetInt("music") == 1)
            {
                if (PlayerPrefs.GetInt("curMilkLevel") == 1 || PlayerPrefs.GetInt("curMilkLevel") == 2)
                    DropGla.Play();
                else
                    DropPla.Play();
            }
                
            isSound = true;
        }
    }

    void Bound()
    {
        isBound = false;
        float randX = Random.Range(-5f, -8f);
        float randY = Random.Range(5f, 8f);

        Vector2 up = new Vector2(randX, randY);
        rigid.AddForce(up, ForceMode2D.Impulse);
    }


    void GetFunc()
    {
        coll.enabled = true;
        GetFlag = true;
    }

}
