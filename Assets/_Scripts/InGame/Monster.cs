using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public int m_num;
    float[] Speed = new float[] { 1f, 1f, 1f, 1f, 1f, 1f};
    float[] HP = new float[] {3f, 3f, 10f, 3f, 4f, 5f};
    float[] HPMax = new float[] {3f, 3f, 10f, 3f, 4f, 5f};


    GameManager GM;
    Player player;
    WagonAbility wagon;

    GameObject Chat;

    AudioSource JumpAttack;
    AudioSource PunchHit;
    AudioSource Penguin1;
    AudioSource Penguin2;
    AudioSource CannonShot;

    SpriteRenderer sprite;
    Rigidbody2D rigid;
    BoxCollider2D coll;

    float KnockBack;
    float Damage;
    //player에게 받는 피해

    int curCh;
    int curWg;
    int curWp;

    private bool isFire = false;
    public bool isAlive = true;

    GameObject HPbar;

    bool isAttakReady = false;
    bool isSattle = false;

	void Start () 
    {
        JumpAttack = GameObject.Find("JumpAttack").GetComponent<AudioSource>();
        PunchHit = GameObject.Find("PunchHit").GetComponent<AudioSource>();
        Penguin1 = GameObject.Find("Penguin1").GetComponent<AudioSource>();
        Penguin2 = GameObject.Find("Penguin2").GetComponent<AudioSource>();
        CannonShot = GameObject.Find("CannonShot").GetComponent<AudioSource>();

        sprite = GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        coll = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        wagon = GameObject.FindGameObjectWithTag("Wagon").GetComponent<WagonAbility>();

        HPbar = transform.GetChild(2).gameObject;
        Chat = transform.GetChild(1).gameObject;

        curCh = PlayerPrefs.GetInt("curCh");
        curWg = PlayerPrefs.GetInt("curWg");
        curWp = PlayerPrefs.GetInt("curWp");

        if (GM.GameLevel == 1)
        {
            HP[m_num] += 2;
            HPMax[m_num] += 2;
        }
        if (GM.GameLevel == 2)
        {
            HP[m_num] += 4;
            HPMax[m_num] += 4;
        }
        if (GM.GameLevel == 3)
        {
            HP[m_num] += 8;
            HPMax[m_num] += 8;
        }

        if(m_num != 2)
            Speed[m_num] = Random.Range(1f, 1.4f);
        
        if (PlayerPrefs.GetInt("toutorial") == 1)
            Speed[m_num] = 1.4f;

        if( (m_num == 0 || m_num == 2 ) && PlayerPrefs.GetInt("toutorial") == 0)
            StartCoroutine("Jump");
     
        if(m_num == 1)
            ChatPopUp("안녕", 1.3f, 3);
        if(m_num == 2)
            ChatPopUp("ㅡㅅㅡ", 1.5f, 2);
        if(m_num == 5)
            ChatPopUp("꼬북꼬북", 1.5f, 2);
    }
	
	
	void Update () 
    {
        if (isAlive == true && KnockBack == 0)
        {
            if (GM.isBossMode && (m_num == 0 || m_num == 3))
                transform.position += Vector3.left * Time.deltaTime * Random.Range(1f, 5f);
            else
                transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed * Speed[m_num];
        }

        if (isAlive == true && transform.position.x <= -9)
            Destroy(gameObject);
        if (isAlive == true && transform.position.y <= -5)
            StartCoroutine("Die");
        if (isAlive == true && HP[m_num] <= 0)
            StartCoroutine("Die");
        if (HP[m_num] <= 0)
            HPbar.SetActive(false);

        if (m_num == 5 && isAlive == true && isAttakReady == false && transform.position.x <= 3.5f)
        {
            StartCoroutine("TurtleShot");
            isAttakReady = true;
        }

        if (m_num == 2 && isAlive == true)
        {
            if (rigid.velocity.y == 0)
            {
                sprite.enabled = true;
                transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                Chat.SetActive(false);
                sprite.enabled = false;
                transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        if (isAlive == true && player.MouseOpenTime > 0 && isSattle == false && player.curWp == 8 && PlayerPrefs.GetInt("WpLevel8") >= 3)
        {
            GameObject laser = Instantiate(GM.Weapon[8], transform.position, transform.rotation);
            laser.transform.Rotate(Vector3.back * 90f);
            laser.transform.position = new Vector3(laser.transform.position.x, laser.transform.position.y + 8.8f, laser.transform.position.z);
            StartCoroutine("SattleCoolTime");
        }

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "JumpAttackColl" && player.rigid.velocity.y < 0)      // Jump Attack 당함 
        {
            if (PlayerPrefs.GetInt("music") == 1)
                JumpAttack.Play();
            Instantiate(GM.Effect[0], transform.position, transform.rotation);
            Damage = GM.Get_JumpDamage();
            KnockBack = 0;

            if (curCh == 3) // 포메 밟기 데미지 +2
                Damage += 2;

            StartCoroutine("Hit");
            player.StartCoroutine("Vibe");
            if (player.isSuper == false)
            {
                player.rigid.velocity = new Vector2(rigid.velocity.x, 8f);
                player.StartCoroutine("JumpAttackGhost");
            }
        }

        if (curWg == 3 && other.gameObject.tag == "punchWagonColl") // punch수레 범위안에
            wagon.punch();

        if (other.gameObject.tag == "Player" && player.GhostTime == 0 && player.isSuper == false && isAlive == true)       // Player Attack
        {
            player.StartCoroutine("PlayerAttack");
        }

        if(other.gameObject.tag == "Player" && player.isSuper == true && isAlive == true) // super attack
        {
            if (PlayerPrefs.GetInt("music") == 1)
                JumpAttack.Play();
            Instantiate(GM.Effect[0], transform.position, transform.rotation);
            Damage = 9999f;
            StartCoroutine("Hit");
            player.StartCoroutine("Vibe");
            HPbar.SetActive(false);
        }
    }
    public void Get_Damages(float damage, float knock)
    {
        Damage = damage;
        KnockBack = knock;
    }
    IEnumerator Hit() // 피격당함 
    {
        HPbar.SetActive(true);
        HP[m_num] -= Damage;
        HPbar.transform.GetChild(1).localScale = new Vector3((HP[m_num] / HPMax[m_num]) * 0.15f, HPbar.transform.GetChild(1).localScale.y);

        for (int i = 0; i < 8; i++)
        {
            transform.position += Vector3.right * Time.deltaTime * KnockBack;
            yield return new WaitForSeconds(0.01f);
        }
        KnockBack = 0;
    }

    IEnumerator Die()
    {
        HPbar.SetActive(false);
        if(GM.isBossMode == false && GM.MapNum >= 0)  
        {
            int decrease = 0;
            if (GM.GameLevel == 1)
                decrease = 50;
            if (GM.GameLevel == 2)
                decrease = 100;
            if (GM.GameLevel == 3)
                decrease = 150;

            if(Random.Range(0,9) == 0 && player.isSuper == false && GM.isIceFooter == false) // super item drop
                Instantiate(GM.SuperItem[Random.Range(0, 3)], transform.position, transform.rotation);

            int rand = Random.Range(0, 300 - decrease);
            if(rand == 0)
                Instantiate(GM.Coin[2], transform.position, transform.rotation); // crystal
            else if(rand == 1 || rand == 2)
                Instantiate(GM.Coin[1], transform.position, transform.rotation); // coin poket

            int range = 0;
            if (GM.MapNum == 0 || GM.MapNum == 1)
                range = 3;
            if (GM.MapNum == 2 || GM.MapNum == 3)
                range = 2;
            if (GM.MapNum == 4 || GM.MapNum == 5)
                range = 2;
            rand = Random.Range(0, range);
            if (rand == 0 && (PlayerPrefs.GetInt("curMilkNum") != GM.wagonSlot[PlayerPrefs.GetInt("curWg")]) ) //우유 드랍
            {
                GameObject Milk = Instantiate(GM.MilkItem, transform.position, transform.rotation);
                Milk.GetComponent<MilkItem>().type = "Drop";
            }
        }
        if (GM.isBossMode == true)
        {
            int rand = Random.Range(0, 5);
            if(rand == 0)
                Instantiate(GM.WeaponItem, transform.position, transform.rotation);
        }
        if (PlayerPrefs.GetInt("toutorial") == 1)
        {
            GameObject Milk = Instantiate(GM.MilkItem, transform.position, transform.rotation);
            Milk.GetComponent<MilkItem>().type = "Drop";
        }
        if (m_num == 0)
            ChatPopUp("으앙ㅠㅠ", 1.8f, 3);

        if (m_num == 1 && PlayerPrefs.GetInt("music") == 1)
        {
            int rand = Random.Range(0, 2);
            if(rand == 0)
                Penguin1.Play();
            else
                Penguin2.Play();
        }
        if (m_num == 2)
        {
            sprite.enabled = true;
            transform.GetChild(3).gameObject.SetActive(false);
        }
        if (m_num == 5)
        {
            int decrease = 0;
            if (GM.GameLevel == 1)
                decrease = 3;
            if (GM.GameLevel == 2)
                decrease = 5;
            if (GM.GameLevel == 3)
                decrease = 7;
            
            int rand2 = Random.Range(0,15 - decrease); // 120
            if(rand2 == 0 && PlayerPrefs.GetInt("p_wgLock6") == 1)
                GM.StartCoroutine("ItemDropPopUp", 2);
        }

        GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
        ShowScore.GetComponent<Chat>().Content = "100";
        GM.Score += 100;

        isAlive = false;
        sprite.flipY = true;
        coll.enabled = false;
        rigid.isKinematic = false;
        rigid.velocity = new Vector2(rigid.velocity.x, 7f);
        StopCoroutine("Hit");
        Destroy(gameObject, 1f);

        yield return null;
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(Random.Range(1f, 6f));
        if (isAlive == true)
        {
            rigid.velocity = new Vector2(0, Random.Range(5f, 10f));
            StartCoroutine("Jump");
            if(m_num == 0)
                ChatPopUp("점프^^", 1.2f, 2);
        }
    }
    IEnumerator TurtleShot()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        sprite.enabled = false;
        transform.GetChild(3).gameObject.SetActive(true);
        for(int i = 0; i<3; i++)
        {
            transform.position += Vector3.up * Time.deltaTime * 1f;
            yield return new WaitForSeconds(0.01f);
        }
        for(int i = 0; i<3; i++)
        {
            transform.position += Vector3.down * Time.deltaTime * 1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.3f);

        int rand = Random.Range(1, 4);

        for(int w = 0; w<rand; w++)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                CannonShot.Play();
            player.StartCoroutine("Vibe");
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.3f, 0);
            Instantiate(GM.MobWeapon[0], vec, transform.rotation);

            for(int i = 0; i<3; i++)
            {
                transform.position += Vector3.right * Time.deltaTime * 4f;
                yield return new WaitForSeconds(0.01f);
            }
            for(int i = 0; i<3; i++)
            {
                transform.position += Vector3.left * Time.deltaTime * 4f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        sprite.enabled = true;
        transform.GetChild(3).gameObject.SetActive(false);
        for(int i = 0; i<3; i++)
        {
            transform.position += Vector3.up * Time.deltaTime * 1f;
            yield return new WaitForSeconds(0.01f);
        }
        for(int i = 0; i<3; i++)
        {
            transform.position += Vector3.down * Time.deltaTime * 1f;
            yield return new WaitForSeconds(0.01f);
        }
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
