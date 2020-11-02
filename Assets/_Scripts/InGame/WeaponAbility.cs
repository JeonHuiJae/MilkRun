using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbility : MonoBehaviour {

    public int m_num; // 9: 얼음탄  10:대포알, 11:스플래시 폭발, 12:펀치  13:엑스칼리버
    float[] Speed = new float[] { 10f, 10f, 12f, 14f, 12f, 12f, 12f, 13f, 0f, 12f, 12f, 0f, 0f, 6f};
    float[] CoolTime = new float[] { 0.05f, 0.05f, 0.06f, 0.04f, 0.07f, 0.05f, 0.07f, 0.03f, 0.1f, 0.05f, 0f, 0f, 0f, 0f};
    float[] KnockBack = new float[] { 0.8f, 0.7f, 0.8f, 2f, 1f, 0.8f, 1.5f, 1f, 8f, 1f, 2f, 2f, 10f, 1f};
    float[] Damage = new float[] { 1f, 1f, 1.5f, 2f, 1.5f, 1.5f, 2.5f, 0.8f, 10f, 1.5f, 1.5f, 1f, 2f, 3f};

    GameManager GM;
    Player player;

    AudioSource StoneShot;
    AudioSource StoneHit;
    AudioSource StoneDie;
    AudioSource TomatoHit;
    AudioSource SwordShot;
    AudioSource SwordHit;
    AudioSource GunShot;
    AudioSource GunHit;
    AudioSource Explosion;
    AudioSource JumpAttack;
    AudioSource PunchHit;
    AudioSource Laser;
    AudioSource Spark1;
    AudioSource Spark2;
    AudioSource CannonShot;

    int WpLevel = 0;
	void Start ()
    {
        WpLevel = PlayerPrefs.GetInt("WpLevel" + m_num);

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        StoneShot = GameObject.Find("StoneShot").GetComponent<AudioSource>();
        StoneHit = GameObject.Find("StoneHit").GetComponent<AudioSource>();
        TomatoHit = GameObject.Find("TomatoHit").GetComponent<AudioSource>();
        SwordShot = GameObject.Find("SwordShot").GetComponent<AudioSource>();
        SwordHit = GameObject.Find("SwordHit").GetComponent<AudioSource>();
        GunShot = GameObject.Find("GunShot").GetComponent<AudioSource>();
        GunHit = GameObject.Find("GunHit").GetComponent<AudioSource>();
        Explosion = GameObject.Find("Explosion").GetComponent<AudioSource>();
        JumpAttack = GameObject.Find("JumpAttack").GetComponent<AudioSource>();
        PunchHit = GameObject.Find("PunchHit").GetComponent<AudioSource>();
        Laser = GameObject.Find("Laser").GetComponent<AudioSource>();
        Spark1 = GameObject.Find("Spark").GetComponent<AudioSource>();
        Spark2 = GameObject.Find("Spark2").GetComponent<AudioSource>();
        CannonShot = GameObject.Find("CannonShot").GetComponent<AudioSource>();
        StoneDie = GameObject.Find("RockDie").GetComponent<AudioSource>();


        if (m_num == 0)
        {
            if (WpLevel >= 1)
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                Damage[m_num] += 0.5f;
            }
            if (WpLevel >= 2)
                Speed[m_num] += 3f;
                            
        }
        if (m_num == 1)
        {
            if (WpLevel >= 1)
                Speed[m_num] += 1.5f;
            if (WpLevel >= 2)
                KnockBack[m_num] += 0.7f;
            if (WpLevel >= 3)
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                Damage[m_num] += 0.5f;
            }
        }

        if (m_num == 2)
        {
            if (WpLevel >= 1)
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                KnockBack[m_num] += 0.8f;
            }
            if (WpLevel >= 2)
            {
                Damage[m_num] += 0.5f;
            }
            if (WpLevel >= 3)
            {
                int rand = Random.Range(0, 10);
                if (rand == 0)
                    Instantiate(GM.Weapon[10], transform.position, transform.rotation);
            }
        }
        if (m_num == 4)
        {
            if (WpLevel >= 1)             
                Damage[m_num] += 0.5f;
            
            if (WpLevel >= 2)      
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);          
        }
        if (m_num == 5)
        {
            if (WpLevel >= 1)              
                KnockBack[m_num] += 0.8f; 
        }
        if (m_num == 7)
        {
            if (WpLevel >= 1)             
                Damage[m_num] += 0.5f;
            if (WpLevel >= 2)             
                Damage[m_num] += 0.5f;
            if (WpLevel >= 3)             
                Damage[m_num] += 0.5f;
        }
        if (m_num == 8)
        {
            if (WpLevel >= 1)             
                transform.localScale = new Vector3(1.3f, 1.5f, 1.3f);
        }
        if (PlayerPrefs.GetInt("music") == 1)
        {
            if (m_num == 0)
                StoneShot.Play();
            if (m_num == 1)
                StoneShot.Play();
            if (m_num == 2)
                SwordShot.Play();
            if (m_num == 3)
                GunShot.Play();
            if (m_num == 4)
                StoneShot.Play();
            if (m_num == 5)
                SwordShot.Play();
            if (m_num == 6)
                CannonShot.Play();
            if (m_num == 7)
                StoneShot.Play();
            if (m_num == 8)
                Laser.Play();
            if (m_num == 9)
                StoneShot.Play();
            if (m_num == 13)
                StoneShot.Play();
        }
        
       

        
        if (PlayerPrefs.GetInt("curCh") == 1)
        { // 슈나우져 능력 공격 쿨타임 30% 감소
            player.wp_CoolTime = CoolTime[m_num] / 2;
        }
        else
            player.wp_CoolTime = CoolTime[m_num];

        if (m_num == 2 || m_num == 13)
            transform.Rotate(Vector3.back * 90f);

        if (m_num == 8)
            StartCoroutine("PadeOut");
    }
	
	
	void Update () 
    {
        if (m_num == 13)
        {
            if (transform.position.x >= 6)
                Destroy(gameObject);
        }
        else
        {
            if (transform.position.x >= 4)
            Destroy(gameObject);
        }

        switch (m_num)
        {
            case 0: // 돌멩이
                transform.Rotate(Vector3.back * 720f * Time.deltaTime);
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
            case 1: // 토마토
                transform.Rotate(Vector3.back * 720f * Time.deltaTime);
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                transform.position += Vector3.up * 3.2f * Time.deltaTime;
                if (WpLevel >= 1)                 
                    transform.position += Vector3.up * 0.5f * Time.deltaTime;              
                break;
            case 2: // 장난감칼
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
            case 3: // 글록
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
            case 4: // 미니 미사일
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                transform.Rotate(Vector3.back * 50f * Time.deltaTime);
                transform.position += Vector3.up * 2.7f * Time.deltaTime;
                break;
            case 5: // 종이비행기
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
            case 6: // 상어 미사일
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
            case 7: // 개사료
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
            case 9: // 얼음 탄 
                transform.Rotate(Vector3.back * 720f * Time.deltaTime);
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
            case 10: // 대포 알
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                transform.position += Vector3.up * 2.7f * Time.deltaTime;
                break;
            case 13: // 엑스칼리버
                transform.position += Vector3.right * Time.deltaTime * Speed[m_num];
                break;
        }
		
	}

    void WeaponAction(Collider2D other)
    {
        switch (m_num) 
        {
            case 0: // 돌멩이
                Instantiate(GM.Effect[0], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                    StoneHit.Play();
                if (WpLevel >= 3)
                {
                    Instantiate(GM.Effect[11], other.gameObject.transform.position, other.gameObject.transform.rotation);   
                    if (PlayerPrefs.GetInt("music") == 1)
                        StoneDie.Play();
                }         
                break;
            case 1: // 토마토
                Instantiate(GM.Effect[5], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                    TomatoHit.Play();
                break;
            case 2: // 장난감칼
                if (Random.Range(0, 2) == 0) // 대각선 모양
                    Instantiate(GM.Effect[3], other.gameObject.transform.position, other.gameObject.transform.rotation);
                else  // 가로선 모양
                    Instantiate(GM.Effect[4], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                    SwordHit.Play();
                break;
            case 3: // 글록
                if (PlayerPrefs.GetInt("music") == 1)
                    JumpAttack.Play();
                Instantiate(GM.Effect[0], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (WpLevel >= 3)
                {
                    if (PlayerPrefs.GetInt("music") == 1)
                        Explosion.Play();
                    Instantiate(GM.Effect[1], other.gameObject.transform.position, other.gameObject.transform.rotation);
                }
                break;
            case 4: // 미니 미사일
                if (PlayerPrefs.GetInt("music") == 1)
                    Explosion.Play();
                Instantiate(GM.Effect[1], other.gameObject.transform.position, other.gameObject.transform.rotation);
                break;
            case 5: // 종이비행기
                if (Random.Range(0, 2) == 0) // 대각선 모양
                    Instantiate(GM.Effect[3], other.gameObject.transform.position, other.gameObject.transform.rotation);
                else  // 가로선 모양
                    Instantiate(GM.Effect[4], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                    SwordHit.Play();
                if (WpLevel >= 3)
                {
                    if (PlayerPrefs.GetInt("music") == 1)
                        Explosion.Play();
                    Instantiate(GM.Effect[1], other.gameObject.transform.position, other.gameObject.transform.rotation);
                }
                break;
            case 6: // 상어 미사일
                if (PlayerPrefs.GetInt("music") == 1)
                    Explosion.Play();
                GameObject exp = Instantiate(GM.Effect[1], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (WpLevel >= 1)
                    exp.transform.localScale = new Vector3(1.3f,1.3f,1.3f);
                break;
            case 7: // 개사료
                Instantiate(GM.Effect[0], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                    StoneHit.Play();
                break;
            case 8: // 레이져
                Instantiate(GM.Effect[0], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                        Spark1.Play();
                    else
                        Spark2.Play();
                }
                break;
            case 9: // 얼음탄 
                Instantiate(GM.Effect[10], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                    StoneDie.Play();
                break;
            case 10: // 대포 알
                if (PlayerPrefs.GetInt("music") == 1)
                    Explosion.Play();
                Instantiate(GM.Effect[1], other.gameObject.transform.position, other.gameObject.transform.rotation);
                break;
            case 11: // 스플래시 폭발
                Instantiate(GM.Effect[0], other.gameObject.transform.position, other.gameObject.transform.rotation);
                break;
            case 12: // 펀치
                if (PlayerPrefs.GetInt("music") == 1)
                    PunchHit.Play();
                Instantiate(GM.Effect[0], other.gameObject.transform.position, other.gameObject.transform.rotation);
                break;
            case 13: // 엑스 칼리버
                if (Random.Range(0, 2) == 0) // 대각선 모양
                    Instantiate(GM.Effect[3], other.gameObject.transform.position, other.gameObject.transform.rotation);
                else  // 가로선 모양
                    Instantiate(GM.Effect[4], other.gameObject.transform.position, other.gameObject.transform.rotation);
                if (PlayerPrefs.GetInt("music") == 1)
                    SwordHit.Play();
                break;
        }

        player.StartCoroutine("Vibe");
        if (m_num == 11)
            Destroy(gameObject, 0.5f);
        else if (m_num == 12)
            gameObject.SetActive(false);
        else if (m_num == 5 || m_num == 8 || m_num == 13) 
            Destroy(gameObject, 1f);
        else
            Destroy(gameObject);
         
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            Monster monster = other.gameObject.GetComponent <Monster>();
            if (monster.isAlive == true)
            {
                monster.Get_Damages(Damage[m_num], KnockBack[m_num]);
                monster.StartCoroutine("Hit");
                WeaponAction(other);
            }
        }
        if (other.gameObject.tag == "Object")
        {
            Objects objs = other.gameObject.GetComponent <Objects>();
            if (objs.isAlive == true)
            {
                objs.Get_Damages(Damage[m_num], KnockBack[m_num]);
                objs.StartCoroutine("Hit");
                WeaponAction(other);
            }
        }
        if (other.gameObject.tag == "Boss")
        {
            Boss boss = other.gameObject.GetComponent<Boss>();
            if (boss.isAlive == true)
            {
                boss.Get_Damages(Damage[m_num]);
                boss.StartCoroutine("Hit");
                WeaponAction(other);
            }
        }
        if (other.gameObject.tag == "Boss2")
        {
            Boss2 boss = other.gameObject.GetComponent<Boss2>();
            if (boss.isAlive == true)
            {
                boss.Get_Damages(Damage[m_num]);
                boss.StartCoroutine("Hit");
                WeaponAction(other);
            }
        }
        if (other.gameObject.tag == "Boss2Shark")
        {
            Boss2_weapon wp = other.gameObject.GetComponent<Boss2_weapon>();
            if (wp.isAlive == true)
            {
                wp.Get_Damages(Damage[m_num], KnockBack[m_num]);
                wp.StartCoroutine("Hit");
                WeaponAction(other);
            }
        }
        if ( (other.gameObject.tag == "Ground" || other.gameObject.tag == "Footer") && m_num == 1) // 토마토 바닥 충돌
        {
            Instantiate(GM.Effect[5], transform.position, transform.rotation);
            if (PlayerPrefs.GetInt("music") == 1)
                TomatoHit.Play();
            Destroy(gameObject);
        }
        if ((other.gameObject.tag == "Ground" || other.gameObject.tag == "Footer") && (m_num == 4 || m_num == 10) ) // 미니미사일 , 대포알 바닥 충돌
        {
            Instantiate(GM.Effect[1], transform.position, transform.rotation);
            if (PlayerPrefs.GetInt("music") == 1)
                Explosion.Play();
            Destroy(gameObject);
        }
    }

    public float Get_Damage()
    {
        return Damage[m_num];
    }

    public void Half_Damage()
    {
        Damage[m_num] = Damage[m_num] / 2;
    }

    IEnumerator PadeOut()
    {
        player.StartCoroutine("Vibe");
        player.StartCoroutine("Vibe");
        player.StartCoroutine("Vibe");

        SpriteRenderer sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(0.1f);

        int time = 0;

        byte alpha = 255;
        while (time < 10)
        {
            sprite.color = new Color32(255, 255, 255, alpha);
            alpha -= 25;
            yield return new WaitForSeconds(0.01f);
            time++;
        }

        Destroy(gameObject);
    }


}
