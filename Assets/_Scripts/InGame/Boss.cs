using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Boss : MonoBehaviour {

    float MaxHP = 180f;
    float HP = 180f;
   
    GameManager GM;
    Player player;
    WagonAbility wagon;
   
    GameObject Chat;
    GameObject HPbar;
    float barSacle;

    AudioSource Explosion;

    Rigidbody2D rigid;
    //BoxCollider2D coll;

    public bool Jumping = false;

    int curWg;

    float Damage;
    //player에게 받는 피해

    public bool isAlive = true;
    bool apperFlag = false;
    float jumpScale;

    public bool isReady = false;
    float Decre1 = 0f; // 체력 낮을 수록 패턴 딜레이 감 소
    float Decre2 = 0f; // 체력 낮을 수록 패턴 딜레이 감 소

    Vector3[] dustPos =  new Vector3[] { Vector3.down * 0.45f + Vector3.left * 0.28f, Vector3.down * 0.45f + Vector3.right * 0.85f }; //왼발 오른발

    bool isSattle = false;

    void Start()
    {
        Explosion = GameObject.Find("Explosion").GetComponent<AudioSource>();

        rigid = gameObject.GetComponent<Rigidbody2D>();
 
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        HPbar = GameObject.FindGameObjectWithTag("BossHP");
        wagon = GameObject.FindGameObjectWithTag("Wagon").GetComponent<WagonAbility>();

        Chat = transform.GetChild(4).gameObject;

        curWg = PlayerPrefs.GetInt("curWg");

        if (GM.GameLevel == 1)
        {
            HP += 40;
            MaxHP += 40;
        }
        if (GM.GameLevel == 2)
        {
            HP += 80;
            MaxHP += 80;
        }
        if (GM.GameLevel == 3)
        {
            HP += 140;
            MaxHP += 140;
        }

        StartCoroutine("Appear"); // 등.장
    }


    void Update()
    {
        if(apperFlag == true)
            transform.position = new Vector3(1.9f, transform.position.y);

        if (isAlive == true && rigid.velocity.y == 0)
        {
            Jumping = false;
            OnOff(0, 1);
        }

        if (apperFlag == true && isReady == true && Jumping == false && isAlive == true)
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)
                StartCoroutine("KingRaniJump");
            else
                StartCoroutine("RobotGorani");

            StartCoroutine("CoolTime");
        }
        if (transform.position.y > 0 && transform.GetChild(0).gameObject.activeSelf == true && apperFlag == true)
            StartCoroutine("KingRaniJump");
        
            
        if (isAlive == true && HP <= 0)
            StartCoroutine("Die");
        
        if (apperFlag == false && rigid.velocity.y == 0) // Appear 착지
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Explosion.Play();
            
            OnOff(0, 1); // stand
            for (int i = 0; i < 3; i++)
                player.StartCoroutine("Vibe");
            Instantiate(GM.Effect[7], transform.position + dustPos[0], transform.rotation);
            Instantiate(GM.Effect[7], transform.position + dustPos[1], transform.rotation);

            HPbar.transform.GetChild(0).gameObject.SetActive(true);
            HPbar.transform.GetChild(1).gameObject.SetActive(true);
            HPbar.transform.GetChild(2).gameObject.SetActive(true);

            HPbar.transform.GetChild(2).GetComponent<Text>().text = "킹라니";
            barSacle = HPbar.transform.GetChild(1).localScale.x;
            StartCoroutine("CoolTime");
            apperFlag = true;
        }

        if (HP <= MaxHP / 2)
        {
            Decre1 = 0.3f; 
            Decre2 = 1f;          
        }
        if (HP <= MaxHP / 4)
        {
            Decre1 = 0.5f;
            Decre2 = 1.5f;          
        }

        if (isAlive == true && player.MouseOpenTime > 0 && isSattle == false && player.curWp == 8 && PlayerPrefs.GetInt("WpLevel8") >= 3)
        {
            GameObject laser = Instantiate(GM.Weapon[8], transform.position, transform.rotation);
            laser.transform.Rotate(Vector3.back * 90f);
            laser.transform.position = new Vector3(laser.transform.position.x, laser.transform.position.y + 8.8f, laser.transform.position.z);
            StartCoroutine("SattleCoolTime");
        }
        
    }
    IEnumerator CoolTime()
    {
        isReady = false;
        yield return new WaitForSeconds(Random.Range(1f- Decre1, 2.5f - Decre2));
        isReady = true;
    }
    void OnOff(int on, int off) // 보이고, 안보이고
    {
        transform.GetChild(on).gameObject.SetActive(true);
        transform.GetChild(off).gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && player.GhostTime == 0 && isAlive == true)       // Player Attack
        {
            player.StartCoroutine("PlayerAttack");
            player.rigid.AddForce(new Vector2(-6,2), ForceMode2D.Impulse);
        }
        
        if (curWg == 3 && other.gameObject.tag == "punchWagonColl") // punch수레 범위안에
            wagon.punch();

        if (isAlive && Jumping && other.gameObject.tag == "Ground")
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Explosion.Play();

            Instantiate(GM.Effect[7], transform.position + dustPos[0], transform.rotation);
            Instantiate(GM.Effect[7], transform.position + dustPos[1], transform.rotation);
            for (int i = 0; i < 3; i++)
                player.StartCoroutine("Vibe");
            if (player.DogState.transform.GetChild(2).gameObject.activeSelf == true && player.GhostTime == 0 && isAlive == true) // player가 뛰지 않았을 때
                player.StartCoroutine("PlayerAttack");
            Jumping = false;
        }
    }
    public void Get_Damages(float damage)
    {
        if(apperFlag)
            Damage = damage;
    }

    IEnumerator Hit() // 피격당함 
    {
        if (apperFlag)
        {
            HP -= Damage;
            if (HP < 0)
                HP = 0;
            HPbar.transform.GetChild(1).localScale = new Vector3((HP / MaxHP) * barSacle, HPbar.transform.GetChild(1).localScale.y);
            yield return null;
        }
    }

    IEnumerator Die()
    {
        ChatPopUp("으앆!!!!!!!!", 4.5f, 1);

        isAlive = false;
        StopCoroutine("Hit");
        StopCoroutine("KingRaniJump");
        StopCoroutine("RobotGorani");
        OnOff(2, 1);
        OnOff(2, 0); // die로 전환
        SpriteRenderer c = transform.GetChild(2).GetComponent<SpriteRenderer>();
        Instantiate(GM.Effect[7], transform.position + dustPos[0], transform.rotation);
        Instantiate(GM.Effect[7], transform.position + dustPos[1], transform.rotation);
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < 8; i++)
        {
            c.color = c.color + new Color(0, 0, 0, -0.5f);
            yield return new WaitForSeconds(0.05f);
            c.color = c.color + new Color(0, 0, 0, +0.5f);
            Vector3 pos = transform.position + Vector3.right * (Random.Range(-0.52f, 0.77f)) + Vector3.up * (Random.Range(-0.46f, 1.00f));
            Instantiate(GM.Effect[6], pos, transform.rotation);
            if (PlayerPrefs.GetInt("music") == 1)
                Explosion.Play();
            player.StartCoroutine("Vibe");
            yield return new WaitForSeconds(0.12f);
        }
        yield return new WaitForSeconds(0.5f);
        if (PlayerPrefs.GetInt("music") == 1)
            Explosion.Play();
        Instantiate(GM.Effect[1], transform.position, transform.rotation); // 폭.발.
        Instantiate(GM.Coin[2], transform.position, transform.rotation);

        HPbar.transform.GetChild(1).localScale = new Vector3(barSacle, HPbar.transform.GetChild(1).localScale.y);
        StartCoroutine("PadeOut");
        HPbar.transform.GetChild(0).gameObject.SetActive(false);
        HPbar.transform.GetChild(1).gameObject.SetActive(false);
        HPbar.transform.GetChild(2).gameObject.SetActive(false);

        GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
        ShowScore.GetComponent<Chat>().Content = "5000";
        GM.Score += 5000;
        StartCoroutine("RewardDrop");

        int decrease = 0;
        if (GM.GameLevel == 1)
            decrease = 1;
        if (GM.GameLevel == 2)
            decrease = 2;
        if (GM.GameLevel == 3)
            decrease = 3;
        
        int rand = Random.Range(0, 5 - decrease); // 50
        if(rand == 0 && PlayerPrefs.GetInt("p_wgLock4") == 1) 
            GM.StartCoroutine("ItemDropPopUp", 0);
    }

    IEnumerator RewardDrop()
    {
        int increase = 0;
        if (GM.GameLevel == 1)
            increase = 5;
        if (GM.GameLevel == 2)
            increase = 10;
        if (GM.GameLevel == 3)
            increase = 15;
        
        for (int i = 0; i < 20 + increase ; i++)
        {
            GameObject Item;

            int rand = Random.Range(0, 300);
            if(rand == 0)
                Item = Instantiate(GM.Coin[2], transform.position, transform.rotation); // crystal
            else if(rand == 1 || rand == 2)
                Item = Instantiate(GM.Coin[1], transform.position, transform.rotation); // coin poket
            else               
                Item = Instantiate(GM.Coin[0], transform.position, transform.rotation); // coin

            Rigidbody2D rigid = Item.GetComponent<Rigidbody2D>();
            rigid.AddForce(new Vector2(0f, Random.Range(3f, 7f)), ForceMode2D.Impulse);
        }
        for (int i = 0; i < 10; i++)
        {
            int rand2 = Random.Range(0, 3);
            if (rand2 == 0 && (PlayerPrefs.GetInt("curMilkNum") != GM.wagonSlot[PlayerPrefs.GetInt("curWg")])) //우유 드랍
            {
                GameObject Milk = Instantiate(GM.MilkItem, transform.position, transform.rotation);
                Milk.GetComponent<MilkItem>().type = "Drop";
            }
        }
        yield return null;
        GM.StartCoroutine("BossClear"); // 코인을 다 떨구고 나서 보스클리어 진행 
    }

    IEnumerator PadeOut()
    {
        SpriteRenderer sprite = transform.GetChild(2).GetComponent<SpriteRenderer>();

        int time = 0;

        byte alpha = 255;
        while (time < 20)
        {
           //transform.localScale = new Vector3(transform.localScale.x + 0.02f, transform.localScale.y + 0.02f, transform.localScale.z);
            sprite.color = new Color32(255, 255, 255, alpha);
            alpha -= 12;
            yield return new WaitForSeconds(0.01f);
            time++;
        }

        Destroy(gameObject);
    }

    IEnumerator Appear()
    {
        for (int i = 0; i < 15; i++)
        {
            transform.position += Vector3.left * Time.deltaTime * 6f;
            yield return new WaitForSeconds(0.02f);
        }
        transform.position = new Vector3(1.9f, transform.position.y); // 위치 고정
        ChatPopUp("숲의 침입자여..!!", 3.5f, 1);
    }

    IEnumerator KingRaniJump() // 뜀
    {
        jumpScale = Random.Range(5f, 10f);
        rigid.velocity = new Vector2(0, jumpScale);
        Jumping = true;
        OnOff(1, 0); // jump

        ChatPopUp("으랏차차", 2f, 2);
        yield return null;
    }

    IEnumerator RobotGorani() // 로봇 소환
    {
        if (!Jumping)
        {
            Instantiate(GM.Effect[7], transform.position + dustPos[0], transform.rotation);
            int rand = Random.Range(0, 2);
            if(rand == 0)
                Instantiate(GM.Monster[0], transform.position, transform.rotation);
            else
                Instantiate(GM.Monster[3], transform.position, transform.rotation);

        }
        yield return null;
    }

    void ChatPopUp(string content, float X, int randRange)
    {
        int rand = Random.Range(0,randRange);
        if (rand == 0)
        {
            Chat.SetActive(true);
            Chat.GetComponent<Chat>().Content = content;
            Chat.transform.GetChild(0).localScale = new Vector3(X, 1f, 1f);
        }
    }

    IEnumerator SattleCoolTime()
    {
        isSattle = true;
        yield return new WaitForSeconds(0.3f);
        isSattle = false;
    }

}
