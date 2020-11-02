using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour {

    public int m_num;
    float[] Speed = new float[] { 0 , 0 , 0 , 0 , 0 , 1.5f};
    float[] HP = new float[] {20f, 20f, 1000f, 20f, 30f, 100f};

    GameManager GM;
    Player player;
    WagonAbility wagon;

    AudioSource JumpAttack;
    AudioSource PunchHit;
    AudioSource StoneDie;

    SpriteRenderer sprite;
    Rigidbody2D rigid;
    BoxCollider2D coll;

    float KnockBack;
    float Damage;
    //player에게 받는 피해

    int curCh;
    int curWg;
    int curWp;

    public bool isAlive = true;
    private bool isFire = false;

    void Start () 
    {
        wagon = GameObject.FindGameObjectWithTag("Wagon").GetComponent<WagonAbility>();
        JumpAttack = GameObject.Find("JumpAttack").GetComponent<AudioSource>();
        PunchHit = GameObject.Find("PunchHit").GetComponent<AudioSource>();
        StoneDie = GameObject.Find("RockDie").GetComponent<AudioSource>();

        sprite = GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        coll = transform.gameObject.GetComponent<BoxCollider2D>();

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        curCh = PlayerPrefs.GetInt("curCh");
        curWg = PlayerPrefs.GetInt("curWg");
        curWp = PlayerPrefs.GetInt("curWp");

        if(GM.GameLevel == 1)
            HP[m_num] += 5;
        if(GM.GameLevel == 2)
            HP[m_num] += 10;
        if(GM.GameLevel == 3)
            HP[m_num] += 15;
    }

    void Update () 
    {
        if (isAlive == true)
        {
            //if (m_num == 0 || m_num == 1 || m_num == 2)
                //transform.position = new Vector3(transform.position.x, -1f, transform.position.z);

            if (m_num == 0)
            {
                if (GM.isBossMode == true)
                    Destroy(gameObject);
            }
            if(m_num == 5) // 상어 
                transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed * Speed[m_num]; 
            else
                transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed; 
        }
            
        if (isAlive == true && transform.position.x <= -9)
            Destroy(gameObject);
        if (isAlive == true && transform.position.y <= -5)
            Destroy(gameObject);
        
        if (isAlive == true && HP[m_num] <= 0 && (m_num == 0 || m_num == 1 || m_num == 3))
        {
            StartCoroutine("RockDie");
            GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
            ShowScore.GetComponent<Chat>().Content = "150";
            GM.Score += 150;
        }
        if (isAlive == true && HP[m_num] <= 0 && (m_num == 4 || m_num == 5))
        {
            StartCoroutine("SpikeDie");
            GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
            ShowScore.GetComponent<Chat>().Content = "150";
            GM.Score += 150;
        }

        if ( (m_num == 0 || m_num == 1) && player.isSuper == true) // 슈퍼아이템 밟히기 위해 콜라이더 크기 확대
            coll.size = new Vector2(coll.size.x, 1.6f);
        if ( (m_num == 0 || m_num == 1) && player.isSuper == false)
            coll.size = new Vector2(coll.size.x, 0.6f);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if( (m_num == 0 || m_num == 1 || m_num == 3) && other.gameObject.tag == "Player" && player.GhostTime == 0 && player.isSuper == false) // Player Attack
        {
            player.StartCoroutine("PlayerAttack");
            StartCoroutine("RockDie");
        }
        if( (m_num == 4 || m_num == 5) && other.gameObject.tag == "Player" && player.GhostTime == 0 && player.isSuper == false) // Player Attack
            player.StartCoroutine("PlayerAttack");
        
        if (curWg == 3 && other.gameObject.tag == "punchWagonColl") // punch수레 범위안에
            wagon.punch();

        if (other.gameObject.tag == "Player" && player.isSuper == true && isAlive == true)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                JumpAttack.Play();
            Instantiate(GM.Effect[0], transform.position, transform.rotation);
            Damage = 9999f;
            StartCoroutine("Hit");
            player.StartCoroutine("Vibe");
        }

        if (other.gameObject.tag == "Player" && isAlive == true && m_num == 2) // 얼음 벽
        {
            StartCoroutine("IceWallDie");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shark" && isAlive == true && m_num != 5)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                JumpAttack.Play();
            Instantiate(GM.Effect[0], transform.position, transform.rotation);
            Damage = 9999f;
            StartCoroutine("Hit");
            player.StartCoroutine("Vibe");
        }
    }

    public void Get_Damages(float damage, float knock)
    {
        Damage = damage;
        KnockBack = knock;
    }
    IEnumerator Hit() // 피격당함 
    {
        HP[m_num] -= Damage;
        yield return null;
    }

    IEnumerator RockDie()
    {
        HP[m_num] = 0;
        int num = 3;
        if (m_num == 3)
            num = 9;
        
        if (PlayerPrefs.GetInt("music") == 1)
            StoneDie.Play();
        
        isAlive = false;
        coll.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        Rigidbody2D[] rigid = new Rigidbody2D[num];
        for (int i = 0; i < num; i++)
        {
            transform.GetChild(i+1).gameObject.SetActive(true);
            rigid[i] = transform.GetChild(i + 1).gameObject.GetComponent<Rigidbody2D>();
        }

        for (int i = 0; i < num; i++)
        {
            float randX = Random.Range(-5f, 5f);
            float randY = Random.Range(5f, 8f);

            Vector2 up = new Vector2(randX, randY);
            rigid[i].AddForce(up, ForceMode2D.Impulse);
        }

        Destroy(gameObject, 1f);
        yield return null;
    }

    IEnumerator IceWallDie()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            JumpAttack.Play();
            StoneDie.Play();
        }

        isAlive = false;
        coll.enabled = false;

        for (int i = 0; i < 5; i++) 
            player.StartCoroutine("Vibe");

        Rigidbody2D[] rigid = new Rigidbody2D[6];
        rigid[0] = transform.GetChild(0).gameObject.AddComponent<Rigidbody2D>();
        rigid[1] = transform.GetChild(1).gameObject.AddComponent<Rigidbody2D>();
        rigid[2] = transform.GetChild(2).gameObject.AddComponent<Rigidbody2D>();
        rigid[3] = transform.GetChild(3).gameObject.AddComponent<Rigidbody2D>();
        rigid[4] = transform.GetChild(4).gameObject.AddComponent<Rigidbody2D>();
        rigid[5] = transform.GetChild(5).gameObject.AddComponent<Rigidbody2D>();

        for (int i = 0; i < 6; i++)
        {
            float randX = Random.Range(-10f, 10f);
            float randY = Random.Range(8f, 12f);

            Vector2 up = new Vector2(randX, randY);
            rigid[i].AddForce(up, ForceMode2D.Impulse);
        }

        GameObject ShowScore = Instantiate(GM.ShowScore, transform.position, transform.rotation);
        ShowScore.GetComponent<Chat>().Content = "150";
        GM.Score += 150;

        Destroy(gameObject, 1f);
        yield return null;
    }
    IEnumerator SpikeDie()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            JumpAttack.Play();
            StoneDie.Play();
        }

        BoxCollider2D coll2 = transform.GetChild(0).GetComponent<BoxCollider2D>();  
        rigid.isKinematic = false;
        isAlive = false;
        coll.enabled = false;
        coll2.enabled = false;

        for (int i = 0; i < 5; i++)
            player.StartCoroutine("Vibe");

        float randX = Random.Range(-10f, 10f);
        float randY = Random.Range(8f, 12f);

        Vector2 up = new Vector2(randX, randY);
        rigid.AddForce(up, ForceMode2D.Impulse);

        Destroy(gameObject, 1f);
        yield return null;
    }


}







