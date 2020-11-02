using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Rigidbody2D rigid;
    BoxCollider2D coll;
    SpriteRenderer sprite;
    public Camera Camera;

    GameManager GM;

    public GameObject DogState;
    GameObject DogHead;

    AudioSource Jump1;
    AudioSource Jump2;
    AudioSource DogHit;
    AudioSource BoosterStart;
    AudioSource BoosterMid;
    AudioSource BoosterEnd;
    AudioSource GoldBoneJump;
    AudioSource GunReroad;

    public int curWp = 0;
    public int curCh = 0;
    public int curWg = 0;

    public float wp_CoolTime = 0;
    public float GhostTime = 0;
    public int MouseOpenTime = 0;
    public int bullet = 8;

    public bool isStand = false;
    bool isDoulbeJump = true;
    bool isTripleJump = true;
    public bool isSuper = false;
    bool Giant = false;
    bool SuperXflag = false; 
    public bool StartMap3 = false;

    public GameObject Dog;
    public GameObject Wagon;

    public GameObject bulletUI;
    private Text bulletNum;
    private Image bulletImg;

    float plusJump = 0f;
    float downJump = 0f;

    void Start () 
    {
        Dog.transform.GetChild(0).gameObject.SetActive(false);
        Wagon.transform.GetChild(0).gameObject.SetActive(false);
        Dog.transform.GetChild(PlayerPrefs.GetInt("curCh")).gameObject.SetActive(true);
        Wagon.transform.GetChild(PlayerPrefs.GetInt("curWg")).gameObject.SetActive(true);

        Jump1 = GameObject.Find("Jump1").GetComponent<AudioSource>();
        Jump2 = GameObject.Find("Jump2").GetComponent<AudioSource>();
        DogHit = GameObject.Find("DogHit").GetComponent<AudioSource>();
        BoosterStart = GameObject.Find("BoosterStart").GetComponent<AudioSource>();
        BoosterMid = GameObject.Find("BoosterMid").GetComponent<AudioSource>();
        BoosterEnd = GameObject.Find("BoosterEnd").GetComponent<AudioSource>();
        GoldBoneJump = GameObject.Find("GoldBoneJump").GetComponent<AudioSource>();
        GunReroad = GameObject.Find("GunReroad").GetComponent<AudioSource>();

        rigid = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<BoxCollider2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
       
        DogState = GameObject.FindGameObjectWithTag("DogState");

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        curWp = PlayerPrefs.GetInt("curWp");
        curCh = PlayerPrefs.GetInt("curCh");
        curWg = PlayerPrefs.GetInt("curWg");
  
        /*
        if (curCh == 3)
            plusJump += 0.5f;
        
        if (curWg == 6)
            plusJump += 0.5f;
        */
        

        StartCoroutine("CoolTimer");
    }
	
	
	void Update ()
    {
        if (GhostTime < 0)
            GhostTime = 0;
        
        if((isSuper == false && SuperXflag == false) || Giant == true)
            transform.position = new Vector3(-1.94f, transform.position.y);
        if(SuperXflag == true)
            transform.position = new Vector3(-0.65f, transform.position.y);

        if (rigid.velocity.y == 0) // JUMP
        {
            if (curWg == 6)
                isTripleJump = true;
            isDoulbeJump = true;
            DogHead = GameObject.FindGameObjectWithTag("DogHead");
            if (isStand == false)
            {
                DogState.transform.GetChild(0).gameObject.SetActive(true);
                DogState.transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                DogState.transform.GetChild(0).gameObject.SetActive(false);
                DogState.transform.GetChild(2).gameObject.SetActive(true);
            }
            DogState.transform.GetChild(1).gameObject.SetActive(false);
        }
        else 
        {
            DogHead = GameObject.FindGameObjectWithTag("DogHead");
            DogState.transform.GetChild(0).gameObject.SetActive(false);
            DogState.transform.GetChild(2).gameObject.SetActive(false);
            DogState.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (MouseOpenTime <= 0) // MOUSE OPEN
        {
            MouseOpenTime = 0;
            DogHead.transform.GetChild(0).gameObject.SetActive(true);
            DogHead.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            DogHead.transform.GetChild(0).gameObject.SetActive(false);
            DogHead.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (transform.position.x <= -2.8f)
            transform.position = new Vector3(-2.8f, transform.position.y);
        if (transform.position.x >= 3f)
            transform.position = new Vector3(3f, transform.position.y);
        if (transform.position.y >= 3f && (GM.MapNum == 4 || GM.MapNum == 5 || GM.MapNum == 6) )
            transform.position = new Vector3(transform.position.x, 3f);

        if (GM.GameOver == false && transform.position.y <= -5)
        {
            if (GM.MapNum == 3 && GM.isBossClear == true)
            {
                GM.StartCoroutine("BossClear_Penguin");
                GM.isBossClear = false;
                StartMap3 = true;
            }
            else
            {
                if (PlayerPrefs.GetInt("music") == 1)
                    DogHit.Play();
                GM.ShowGameOverUI();
                GM.GameOver = true;
            }
        }
        if (GM.GameOver == true)
        {
            transform.Rotate(Vector3.back * 720f * Time.deltaTime);
        }
        if (curWp == 3) // 글록일 때
        {
            bulletUI.SetActive(true);
            bulletImg = bulletUI.transform.GetChild(0).GetComponent<Image>();
        }
        else 
            bulletUI.SetActive(false);

        if (curWp == 0 && curWg == 5)
            curWp = 9;
        
        if (GM.MapNum == 4 || GM.MapNum == 5 || GM.MapNum == 6)// 바다 맵 점프 무한
        {
            downJump = -1.2f;
            isDoulbeJump = true;
            rigid.gravityScale = 0.8f;
        }
        else
        {
            downJump = 0f;
            rigid.gravityScale = 1f;
        }
        
        if(StartMap3 == true) // 플레이어 고정 
            transform.position = new Vector3(-1.94f, 7f);
        
	}

    public void Jump()
    {
        if (PlayerPrefs.GetInt("toutorial") == 1)
            Time.timeScale = 1f;
        if (GM.GameOver == false)
        {
            if (rigid.velocity.y == 0)
            {
                rigid.velocity = new Vector2(0, 8f + plusJump + downJump);
               
                if (PlayerPrefs.GetInt("music") == 1)
                    Jump1.Play();
            }
            else if (isDoulbeJump == true)
            {
                rigid.velocity = new Vector2(0, 8f + plusJump + downJump);
                isDoulbeJump = false;
                if (PlayerPrefs.GetInt("music") == 1)
                    Jump2.Play();
            }
            else if (isDoulbeJump == false && curWg == 6 && isTripleJump == true)
            {
                rigid.velocity = new Vector2(0, 8f + plusJump + downJump);
                isTripleJump = false;
                if (PlayerPrefs.GetInt("music") == 1)
                    Jump2.Play();
            }
        }
    }
    /*
    public void RightJump()
    {
        if (rigid.velocity.y == 0)
        {
            rigid.velocity = new Vector2(0, 8f + plusJump);
            if (PlayerPrefs.GetInt("music") == 1)
                Jump1.Play();
        }
        else if (isDoulbeJump == true)
        { 
            rigid.velocity = new Vector2(0, 8f + plusJump);
            isDoulbeJump = false;
            if (PlayerPrefs.GetInt("music") == 1)
                Jump2.Play();
        }
    }
    */

    public void Attack()
    {
        if (PlayerPrefs.GetInt("toutorial") == 1)
            Time.timeScale = 1f;
        
        float pomeY = 0;
        if (curCh == 3)
        {
            pomeY = 0.1f;
            if (Giant == true)
                pomeY = 0.25f;
        }
        else
            pomeY = 0; // 포메라니안 발사 위치를 위한 코드...
        
        StartCoroutine("MouseOpen");
        Vector3 pos;
        if (Giant ==true)
            pos = new Vector3(transform.position.x + 0.7f, transform.position.y - pomeY, transform.position.z);
        else
            pos = new Vector3(transform.position.x + 0.3f, transform.position.y - pomeY , transform.position.z);

        if (wp_CoolTime == 0 && GM.GameOver == false)
        {
            if (curWp == 3) // 글록일때
            { 
                if (bullet > 0)
                {
                    bullet--;
                    Instantiate(GM.Weapon[curWp], pos, transform.rotation);
                    if (curCh == 4) // 허스키 능력 무기 2번나감
                        StartCoroutine("oneMoreWeapon");

                    bulletUI.transform.GetChild(bullet+1).gameObject.SetActive(false);

                    GameObject DropParts2 = Instantiate(GM.Effect[9], bulletUI.transform.GetChild(bullet+1).position, bulletUI.transform.GetChild(bullet+1).rotation);
                    Rigidbody2D rigid2 = DropParts2.GetComponent<Rigidbody2D>();
                    rigid2.AddForce(new Vector2(Random.Range(-2f,2f), 6f), ForceMode2D.Impulse);
                    Destroy(DropParts2, 0.5f);

                    StartCoroutine("Vibe");

                    if (bullet == 0)
                    {
                        bulletImg.color = new Color(1f, 0f, 0f, 1f);
                        StartCoroutine("bulletLoad"); // 장정 2초 시작

                        GameObject DropParts = Instantiate(GM.Effect[8], transform.position, transform.rotation);
                        Rigidbody2D rigid = DropParts.GetComponent<Rigidbody2D>();
                        rigid.AddForce(new Vector2(-4f, 5f), ForceMode2D.Impulse);
                        Destroy(DropParts, 1.5f);
                    }
                }
            }
            else 
            { // 다른 무기일 때
                Instantiate(GM.Weapon[curWp], pos, transform.rotation);
                if (curCh == 4) // 허스키 능력 무기 2번나감
                    StartCoroutine("oneMoreWeapon");
                if (curWp == 6 && PlayerPrefs.GetInt("WpLevel6") >= 3)
                    StartCoroutine("oneMoreShark");
                if (GM.weaponNum != -1) { // 무한 아닐 때
                    GM.weaponNum--;
                }
            }
        }
    }

    IEnumerator bulletLoad() // 글록 총알 장전 1초
    { 
        if (PlayerPrefs.GetInt("music") == 1)
            GunReroad.Play();
        
        int time = 5;
        if (PlayerPrefs.GetInt("WpLevel3") >= 2)
            time = 2;
            
        for (int i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(0.2f);
        }
       
        if (PlayerPrefs.GetInt("WpLevel3") >= 1)
            bullet = 12;
        else
            bullet = 8;
        bulletImg.color = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < bullet; i++)
            bulletUI.transform.GetChild(i + 1).gameObject.SetActive(true);
    }

    IEnumerator MouseOpen()
    {
        MouseOpenTime += 1;
        yield return new WaitForSeconds(0.15f);
        MouseOpenTime -= 1;
        yield return null;
    }

    IEnumerator oneMoreWeapon()
    {
        yield return null;
        Vector3 pos = new Vector3(transform.position.x + 0.3f, transform.position.y+0.2f, transform.position.z);
        GameObject HalfWeapon = Instantiate(GM.Weapon[curWp], pos, transform.rotation);
        HalfWeapon.GetComponent<WeaponAbility>().Half_Damage();
    }
    IEnumerator oneMoreShark()
    {
        yield return new WaitForSeconds(0.01f);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate(GM.Weapon[curWp], pos, transform.rotation);
    }
    IEnumerator CoolTimer()
    {
        if (wp_CoolTime > 0)
            wp_CoolTime -= 0.01f;
        else
            wp_CoolTime = 0;
        yield return new WaitForSeconds(0.01f);
        StartCoroutine("CoolTimer");
    }
       

    IEnumerator Vibe()
    {
        int time = 0;

        while (time < 2)
        {
            Camera.transform.position += Vector3.up * 0.01f;
            Camera.transform.position += Vector3.right * 0.02f;
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 2)
        {
            Camera.transform.position += Vector3.down * 0.02f;
            Camera.transform.position += Vector3.left * 0.04f;
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 2)
        {
            Camera.transform.position += Vector3.up * 0.01f;
            Camera.transform.position += Vector3.right * 0.02f;
            yield return new WaitForSeconds(0.005f);
            time++;
        }
    }

    IEnumerator PlayerAttack() // 공격 당함 
    {
        if (PlayerPrefs.GetInt("music") == 1)
            DogHit.Play();
        
        int myMilkNum = PlayerPrefs.GetInt("curMilkNum");
        int w = myMilkNum/2;
        for (int i = 0; i < w; i++)
        {
            Instantiate(GM.MilkItem, transform.position, transform.rotation);
            myMilkNum--;
            PlayerPrefs.SetInt("curMilkNum", myMilkNum);
        }

        Instantiate(GM.Effect[0], transform.position, transform.rotation);
        sprite.enabled = true;

        GM.Life -= 1;
        GM.LifeDraw();

        GhostTime = 2;
        rigid.AddForce(new Vector2(-2,2), ForceMode2D.Impulse);
        StartCoroutine("Vibe");
        while (GhostTime > 0)
        {
            GhostTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        GhostTime = 0;
        sprite.enabled = false;
    }
    IEnumerator JumpAttackGhost()
    {
        GhostTime += 0.1f;
        yield return new WaitForSeconds(0.2f);
        GhostTime -= 0.1f;
    }
    IEnumerator PlayerDie()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        //transform.localScale = new Vector3(0.9f, -0.9f, 1f);
        coll.enabled = false;
        rigid.isKinematic = false;
        rigid.velocity = new Vector2(rigid.velocity.x, 7f);
        GM.GameSpeed = 0f;
        yield return new WaitForSeconds(1f);
        GM.ShowGameOverUI();
    }

    IEnumerator BoosterAction()
    {
        GameObject Body = transform.GetChild(1).GetChild(0).gameObject;
        GameObject Fire = transform.GetChild(1).GetChild(1).gameObject;

        isSuper = true;

        transform.GetChild(1).gameObject.SetActive(true);
        int time = 0;
        while (time < 8) // 부스터 기계 오픈
        {
            transform.position += Vector3.right * 0.08f;
            Body.transform.position += Vector3.left * 0.03f;
            yield return new WaitForSeconds(0.02f);
            time++;
        }
        time = 0;
        yield return new WaitForSeconds(0.3f);
        Fire.SetActive(true);
        SuperXflag = true;
        GM.GameSpeed += 8f;
        if (PlayerPrefs.GetInt("music") == 1)
            BoosterStart.Play();
    
        while (time < 5) // 불 점화
        {
            for (int i = 0; i < 15; i++) 
                StartCoroutine("Vibe");
            if (curWg == 6)
            {
                for (int i = 0; i < 15; i++)
                    StartCoroutine("Vibe");
            }
            Fire.transform.localScale = new Vector3(Fire.transform.localScale.x+ 1.2f, Fire.transform.localScale.y + 1.2f);
            yield return new WaitForSeconds(0.02f);
            time++;
        }
        time = 0;
        while (time < 5) // 불 축소
        {
            Fire.transform.localScale = new Vector3(Fire.transform.localScale.x- 1.2f, Fire.transform.localScale.y - 1.2f);
            yield return new WaitForSeconds(0.01f);
            time++;
        }
        time = 0;
        if (PlayerPrefs.GetInt("music") == 1)
            BoosterMid.Play();
        
        int BoosterTime = 6;
        if(curWg == 6)
            BoosterTime = 8;
        
        while (BoosterTime > 0) // 부스터 시간 흐름 
        {
            yield return new WaitForSeconds(1f);
            BoosterTime--;
        }

        BoosterTime = 0;
        GM.GameSpeed -= 8f;
        Fire.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        for (int i = 0; i < 5; i++) 
            StartCoroutine("Vibe");
        if (PlayerPrefs.GetInt("music") == 1)
            BoosterEnd.Play();
        GameObject DropParts = Instantiate(GM.SuperItem[3], new Vector3(transform.position.x-0.8f, transform.position.y), transform.rotation);
        Rigidbody2D rigid = DropParts.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(-4f, 5f), ForceMode2D.Impulse);
        Destroy(DropParts, 1.5f);

        SuperXflag = false;
        while (time < 8) // 부스터 기계 집어 넣기
        {
            transform.position += Vector3.left * 0.15f;
            Body.transform.position += Vector3.right * 0.03f;
            yield return new WaitForSeconds(0.02f);
            time++;
        }

        yield return new WaitForSeconds(2f);
        isSuper = false;
    }

    IEnumerator GoldBoneAction()
    {
        isSuper = true;
        Giant = true;

        int time = 0;
        while (time < 10) // 몸 확대
        {
            for (int i = 0; i < 4; i++) 
                StartCoroutine("Vibe");

            transform.localScale = new Vector3(transform.localScale.x + 0.15f, transform.localScale.y + 0.15f);
            yield return new WaitForSeconds(0.05f);
            time++;
        }
        time = 0;
        GM.GameSpeed += 2f;
        int GoldBoneTime = 12;
        while (GoldBoneTime > 0) // 거인화시간 흐름 
        {
            if (rigid.velocity.y == 0)
            {
                for (int i = 0; i < 4; i++)
                    StartCoroutine("Vibe");
            }
            yield return new WaitForSeconds(0.5f);
            GoldBoneTime--;
        }
        while (time < 10) // 몸 축소
        {
            for (int i = 0; i < 2; i++) 
                StartCoroutine("Vibe");

            transform.localScale = new Vector3(transform.localScale.x - 0.15f, transform.localScale.y - 0.15f);
            yield return new WaitForSeconds(0.05f);
            time++;
        }
        GoldBoneTime = 0;
        Giant = false;
        GM.GameSpeed -= 2f;

        yield return new WaitForSeconds(2f);
        isSuper = false;
    }

    IEnumerator stepFooterIN(GameObject other) {

        for (int i = 0; i <3; i++)
        {
            other.transform.GetChild(0).transform.position += Vector3.down * 0.04f;
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 3; i++)
        {
            other.transform.GetChild(0).transform.position += Vector3.up * 0.03f;
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 2; i++)
        {
            other.transform.GetChild(0).transform.position += Vector3.down * 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator stepFooterOUT(GameObject other)
    {
        for (int i = 0; i < 3; i++)
        {
            other.transform.GetChild(0).transform.position += Vector3.up * 0.03f;
            yield return new WaitForSeconds(0.03f);
        }
        for (int i = 0; i < 5; i++)
        {
            other.transform.GetChild(0).transform.position += Vector3.down * 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 2; i++)
        {
            other.transform.GetChild(0).transform.position += Vector3.up * 0.005f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground" && Giant == true)
        {
            for (int i = 0; i < 5; i++) 
                StartCoroutine("Vibe");
            if (PlayerPrefs.GetInt("music") == 1)
                GoldBoneJump.Play();
            Vector3 vec = new Vector3(transform.position.x - 0.2f, transform.position.y - 1f, 0);
            Instantiate(GM.Effect[2], vec, transform.rotation);
        }
        if (other.gameObject.tag == "Footer") {
            StartCoroutine("stepFooterIN", other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Footer")
        {
            StartCoroutine("stepFooterOUT", other.gameObject);
        }
    }
}

