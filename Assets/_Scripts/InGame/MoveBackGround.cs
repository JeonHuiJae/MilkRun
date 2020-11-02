using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour {

    GameManager GM;
    Player player;

    AudioSource JumpAttack;
    AudioSource HouseDie_s;

    GameObject BG1;
    GameObject BG2;
    GameObject BG3;
    GameObject BG4;
    GameObject BG5;
    GameObject BG6;

    public int type; // -2 : change water  -1: change front      0:down       1:up        2:water         3:Ice Footer      4:upup     5:wall
    public int num;

    bool isStep;
    bool isAlive = true; // 나무 발판 터지는 용


	void Start () 
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        JumpAttack = GameObject.Find("JumpAttack").GetComponent<AudioSource>();
        HouseDie_s = GameObject.Find("HouseDie").GetComponent<AudioSource>();

        if (type == -1)
        {
            if (GM.MapNum == 1)
                transform.GetChild(0).gameObject.SetActive(true);
        }

        if (type == 0)
        {
            BG1 = GameObject.Find("BG1");
            BG2 = GameObject.Find("BG2");
            BG3 = GameObject.Find("BG3");
            BG4 = GameObject.Find("BG4");
            BG5 = GameObject.Find("BG5");
            BG6 = GameObject.Find("BG6");
        }
        if (type == 1)
        {
            BG1 = GameObject.Find("UP_BG1");
            BG2 = GameObject.Find("UP_BG2");
            BG3 = GameObject.Find("UP_BG3");
            BG4 = GameObject.Find("UP_BG4");
            BG5 = GameObject.Find("UP_BG5");
            BG6 = GameObject.Find("UP_BG6");

        }
        if (type == 2)
        {
            BG1 = GameObject.Find("Water1");
            BG2 = GameObject.Find("Water2");
            BG3 = GameObject.Find("Water3");
            BG4 = GameObject.Find("Water4");
            BG5 = GameObject.Find("Water5");
            BG6 = GameObject.Find("Water6");

        }
        if (type == 3)
        {
            transform.GetChild(Random.Range(0, 3)).gameObject.SetActive(true);
        }

        if (type == 4)
        {
            BG1 = GameObject.Find("UPUP_BG1");
            BG2 = GameObject.Find("UPUP_BG2");
            BG3 = GameObject.Find("UPUP_BG3");
            BG4 = GameObject.Find("UPUP_BG4");
            BG5 = GameObject.Find("UPUP_BG5");
            BG6 = GameObject.Find("UPUP_BG6");       
        }
        if (type == 5)
        {
            BG1 = GameObject.Find("Wall1");
            BG2 = GameObject.Find("Wall2");
            BG3 = GameObject.Find("Wall3");
            BG4 = GameObject.Find("Wall4");
            BG5 = GameObject.Find("Wall5");
            BG6 = GameObject.Find("Wall6");

        }


	}

	void Update () 
    {
        if (transform.position.x <= -18)
        {
            if (type == 0 && GM.isIceFooter == true)
                gameObject.SetActive(false);

            if(num == 0)
                Destroy(gameObject);
            if(num == 1)
                transform.position = new Vector3(BG6.transform.position.x + 5.6f, transform.position.y);
            if(num == 2)
                transform.position = new Vector3(BG1.transform.position.x + 5.6f, transform.position.y);
            if(num == 3)
                transform.position = new Vector3(BG2.transform.position.x + 5.6f, transform.position.y);
            if(num == 4)
                transform.position = new Vector3(BG3.transform.position.x + 5.6f, transform.position.y);
            if(num == 5)
                transform.position = new Vector3(BG4.transform.position.x + 5.6f, transform.position.y);
            if(num == 6)
                transform.position = new Vector3(BG5.transform.position.x + 5.6f, transform.position.y);
        }

        if (transform.position.x <= -33)
        {
            if(num == -1)
                Destroy(gameObject);
        }
        if (transform.position.x >= 20)
        {
            if(num == -2)
                Destroy(gameObject);
        }

        if (type == -1)
            transform.position += Vector3.left * Time.deltaTime * 15f;
        if (type == -2)
            transform.position += Vector3.up * Time.deltaTime * 16f;


        if(gameObject.layer == 16) // up up back ground
            transform.position += Vector3.left * Time.deltaTime * (GM.GameSpeed / 5);
        if(gameObject.layer == 9) // up back ground
            transform.position += Vector3.left * Time.deltaTime * (GM.GameSpeed / 3);
        if(gameObject.layer == 8) // back ground
            transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed;
        if(gameObject.layer == 4) // water
            transform.position += Vector3.left * Time.deltaTime * (GM.GameSpeed + 2);
        
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && player.isSuper == true && isAlive == true)
        {
            if (type == 0 && num == 0)
            {
                Die();
                isAlive = false;
            }
        }
    }
    void Die() // 나무 발판 터짐
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            JumpAttack.Play();
            HouseDie_s.Play();
        }
        for (int i = 0; i < 5; i++) 
            player.StartCoroutine("Vibe");

        Rigidbody2D rigid;
        rigid = transform.gameObject.AddComponent<Rigidbody2D>();
        BoxCollider2D coll;
        coll = transform.gameObject.GetComponent<BoxCollider2D>();

        coll.enabled = false;
        for (int i = 0; i < 5; i++)
        {
            float randX = Random.Range(-4f, 4f);
            float randY = Random.Range(1f, 3f);

            Vector2 up = new Vector2(randX, randY);
            rigid.AddForce(up, ForceMode2D.Impulse);
        }
        Destroy(gameObject, 2f);
    }
}
