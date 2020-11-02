using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Boss2 : MonoBehaviour {

    float MaxHP = 230f;
    float HP = 230f;

    public GameObject[] Weapon;

    GameManager GM;
    Player player;
    WagonAbility wagon;

    GameObject Chat;
    GameObject HPbar;
    float barSacle;

    AudioSource Explosion;
    AudioSource Penguin1;

    Rigidbody2D rigid;
    //BoxCollider2D coll;

    int curWg;

    float Damage;
    //player에게 받는 피해

    public bool isAlive = true;
    bool apperFlag = false;
    bool isReady = false;
    bool isStand = true;
    float Decre1 = 0f; // 체력 낮을 수록 패턴 딜레이 감소
    float Decre2 = 0f; // 체력 낮을 수록 패턴 딜레이 감소

    bool isSattle = false;

    void Start()
    {
        Explosion = GameObject.Find("Explosion").GetComponent<AudioSource>();
        Penguin1 = GameObject.Find("Penguin1").GetComponent<AudioSource>();

        rigid = gameObject.GetComponent<Rigidbody2D>();
        //coll = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        HPbar = GameObject.FindGameObjectWithTag("BossHP");
        wagon = GameObject.FindGameObjectWithTag("Wagon").GetComponent<WagonAbility>();

        Chat = transform.GetChild(2).gameObject;

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
        if (GM.GameLevel == 2)
        {
            HP += 140;
            MaxHP += 140;
        }

        StartCoroutine("Appear"); // 등.장
    }


    void Update()
    {
        if (isReady == true && isStand == true && isAlive == true)
        {
            int rand = Random.Range(0, 5);
            if (rand == 0)
                StartCoroutine("SharkShot");
            if (rand == 1)
                StartCoroutine("Shot");
            if (rand == 2)
                StartCoroutine("Throw");
            if (rand == 3)
                StartCoroutine("Dash");
            if (rand == 4)
                StartCoroutine("Shot");
       
            StartCoroutine("CoolTime");
            isStand = false;
        }

        if (isAlive == true && HP <= 0)
            StartCoroutine("Die");
        
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
        yield return new WaitForSeconds(Random.Range(1.5f- Decre1, 3.8f - Decre2));
        isReady = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (curWg == 3 && other.gameObject.tag == "punchWagonColl") // punch수레 범위안에
            wagon.punch();
    }
    public void Get_Damages(float damage)
    {
        if(isAlive == true && apperFlag == true)
            Damage = damage;
    }

    IEnumerator Hit() // 피격당함 
    {
        if (isAlive == true && apperFlag == true)
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
        if (PlayerPrefs.GetInt("music") == 1)
            Penguin1.Play();
        
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);

        ChatPopUp("엄마!!!", 2f, 1);

        StopCoroutine("SharkShot");
        StopCoroutine("Shot");
        StopCoroutine("Dash");
        StopCoroutine("Throw");

        StopCoroutine("ObjSpawn");
        StopCoroutine("MobSpawn");
        StopCoroutine("CoolTime");
        isReady = false;
        isAlive = false;
        if (PlayerPrefs.GetInt("music") == 1)
            Explosion.Play();
        player.StartCoroutine("Vibe");
        Instantiate(GM.Effect[2], transform.position, transform.rotation);

        HPbar.transform.GetChild(1).localScale = new Vector3(barSacle, HPbar.transform.GetChild(1).localScale.y);
        HPbar.transform.GetChild(0).gameObject.SetActive(false);
        HPbar.transform.GetChild(1).gameObject.SetActive(false);
        HPbar.transform.GetChild(2).gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        rigid.isKinematic = false;
        rigid.velocity = new Vector2(rigid.velocity.x, 7f);

        if (PlayerPrefs.GetInt("music") == 1)
            Explosion.Play();
        player.StartCoroutine("Vibe");
        Instantiate(GM.Effect[1], transform.position, transform.rotation); // 폭.발.
        Instantiate(GM.Coin[2], transform.position, transform.rotation);

        GM.StartCoroutine("BossClear"); 

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
        
        int rand = Random.Range(0, 5 - decrease); // 60
        if(rand == 0 && PlayerPrefs.GetInt("p_wgLock5") == 1)
            GM.StartCoroutine("ItemDropPopUp", 1);
        Destroy(gameObject, 1.5f);

        yield return null;
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
    }
       

    IEnumerator Appear()
    {
        while (transform.position.x <= 2.17f)
        {
            transform.position += Vector3.right * Time.deltaTime * 5f;
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = new Vector3(2.17f, transform.position.y); // 위치 고정
        ChatPopUp("이 자식 나랑\n붙어볼래?", 3f, 1);

        HPbar.transform.GetChild(0).gameObject.SetActive(true);
        HPbar.transform.GetChild(1).gameObject.SetActive(true);
        HPbar.transform.GetChild(2).gameObject.SetActive(true);

        HPbar.transform.GetChild(2).GetComponent<Text>().text = "꼬마 펭귄";
        barSacle = HPbar.transform.GetChild(1).localScale.x;

        StartCoroutine("ObjSpawn");
        StartCoroutine("MobSpawn");
        apperFlag = true;
        isReady = true;

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
        
    IEnumerator ObjSpawn()
    {
        yield return new WaitForSeconds(Random.Range(4f, 9f));

        int rand = Random.Range(0, 3);
        Vector3 vec = new Vector3(9.5f, -1f, 0);
        if (rand == 0)
            Instantiate(GM.Object[2], vec, transform.rotation);
        else
            Instantiate(GM.Object[3], vec, transform.rotation);

        StartCoroutine("ObjSpawn");

    }

    IEnumerator MobSpawn()
    {
        yield return new WaitForSeconds(Random.Range(4f, 9f));
        int rand = Random.Range(0,4);
        if (rand == 0) // 물개
        {
            Vector3 vec = new Vector3(9.3f, -1f, 0);
            Instantiate(GM.Monster[2], vec, transform.rotation);
        }
        else
        {
            Vector3 vec = new Vector3(9.3f, Random.Range(-1f, 0.45f), 0);
            Instantiate(GM.Monster[1], vec, transform.rotation);
        }
        StartCoroutine("MobSpawn");

    }

    IEnumerator SharkShot()
    {
        if (PlayerPrefs.GetInt("music") == 1)
            Explosion.Play();
        player.StartCoroutine("Vibe");
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
        Instantiate(Weapon[0], vec, transform.rotation);

        for(int i = 0; i<3; i++)
        {
            transform.GetChild(0).gameObject.transform.position += Vector3.right * Time.deltaTime * 4f;
            yield return new WaitForSeconds(0.01f);
        }
        for(int i = 0; i<3; i++)
        {
            transform.GetChild(0).gameObject.transform.position += Vector3.left * Time.deltaTime * 4f;
            yield return new WaitForSeconds(0.01f);
        }
        isStand = true;
    }
    IEnumerator Shot()
    {   
        transform.GetChild(3).gameObject.SetActive(true);
        SpriteRenderer sprite = transform.GetChild(3).GetComponent<SpriteRenderer>();
        byte alpha = 50;
        for (int i = 0; i < 25; i++)
        {
            sprite.color = new Color32(255, 0, 0, alpha);
            alpha -= 2;
            yield return new WaitForSeconds(0.04f);
        }
        transform.GetChild(3).gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("music") == 1)
            Explosion.Play();
        player.StartCoroutine("Vibe");
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
        Instantiate(Weapon[1], vec, transform.rotation);

        for(int i = 0; i<3; i++)
        {
            transform.GetChild(0).gameObject.transform.position += Vector3.right * Time.deltaTime * 4f;
            yield return new WaitForSeconds(0.01f);
        }
        for(int i = 0; i<3; i++)
        {
            transform.GetChild(0).gameObject.transform.position += Vector3.left * Time.deltaTime * 4f;
            yield return new WaitForSeconds(0.01f);
        }
        isStand = true;
    }

    IEnumerator Throw()
    {
        ChatPopUp("에잇", 2f, 2);

        transform.GetChild(3).gameObject.SetActive(true);
        SpriteRenderer sprite = transform.GetChild(3).GetComponent<SpriteRenderer>();
        byte alpha = 50;
        for (int i = 0; i < 25; i++)
        {
            sprite.color = new Color32(255, 0, 0, alpha);
            alpha -= 2;
            yield return new WaitForSeconds(0.04f);
        }
        transform.GetChild(3).gameObject.SetActive(false);


        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);

        Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
        Instantiate(Weapon[2], vec, transform.rotation);

        yield return new WaitForSeconds(0.5f);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);

        isStand = true;
    }
    IEnumerator Dash()
    {
        ChatPopUp("돌진!!", 2f, 1);

        if (PlayerPrefs.GetInt("music") == 1)
            Penguin1.Play();
        
        transform.GetChild(3).gameObject.SetActive(true);
        SpriteRenderer sprite = transform.GetChild(3).GetComponent<SpriteRenderer>();
        byte alpha = 50;
        for (int i = 0; i < 25; i++)
        {
            sprite.color = new Color32(255, 0, 0, alpha);
            alpha -= 2;
            yield return new WaitForSeconds(0.04f);
        }
        transform.GetChild(3).gameObject.SetActive(false);

        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);

        Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
        Instantiate(Weapon[3], vec, transform.rotation);

        yield return new WaitForSeconds(1f);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);

        isStand = true;
    }

    IEnumerator SattleCoolTime()
    {
        isSattle = true;
        yield return new WaitForSeconds(0.3f);
        isSattle = false;
    }
}
