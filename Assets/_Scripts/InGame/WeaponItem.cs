using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour {

    int[] WeaponNum = new int[] { -1, 100, 70, -1, 40, 100, 30, 150, 10};//무기

    public int m_value;

    Rigidbody2D rigid;

    GameManager GM;
    Player player;

    AudioSource Get;
    AudioSource Drop;

    bool isBound = false;

    void Start () 
    {
        if (m_value == 8) //레이져
            m_value = 8;
        else if (m_value == -1) // 나무 발판 위에는 좋은 무기들만
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)
                m_value = 4;
            if (rand == 1)
                m_value = 6;
            if (rand == 2)
                m_value = 8;
        }
        else
        {
            m_value = Random.Range(0, 9);
            if (m_value == 8) // 레이져 나오면 50퍼 확률로 획득 가능 
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                    m_value = 8;
                else
                    m_value = Random.Range(0, 9);
            }
        }

        transform.GetChild(m_value + 1).gameObject.SetActive(true);

        if (transform.position.x >= 3f)
            isBound = false;
        else
            isBound = true;

        Get = GameObject.Find("MilkSell").GetComponent<AudioSource>();
        Drop = GameObject.Find("WeaponDrop").GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("music") == 1)
            Drop.Play();

        rigid = gameObject.GetComponent<Rigidbody2D>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (PlayerPrefs.GetInt("WpLevel4") >= 3)
            WeaponNum[4] += 10;
        if (PlayerPrefs.GetInt("WpLevel5") >= 2)
            WeaponNum[5] += 30;
        if (PlayerPrefs.GetInt("WpLevel6") >= 2)
            WeaponNum[6] += 10;
        if (PlayerPrefs.GetInt("WpLevel8") >= 1)
            WeaponNum[8] += 5;

        /*
        float randX = Random.Range(-4f, 10f);
        float randY = Random.Range(5f, 9f);

        Vector2 up = new Vector2(randX, randY);
        rigid.AddForce(up, ForceMode2D.Impulse);
        */
    }

    void Update()
    {
        if(GM.MapNum == 1 && GM.isBossMode == true) // 고라니 숲 보스전
            transform.position += Vector3.left * Time.deltaTime * 2f; 
        else
            transform.position += Vector3.left * Time.deltaTime * GM.GameSpeed;


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
        if (other.gameObject.tag == "Object")
        {
            Objects objs = other.gameObject.GetComponent <Objects>();
            if (objs.m_num == 0 || objs.m_num == 1 || objs.m_num == 2 || objs.m_num == 3)
                transform.position += Vector3.up * 0.5f;
        }
        if(other.gameObject.tag == "Player")       
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Get.Play();
            int pastWp = PlayerPrefs.GetInt("curWp"); // 바뀌기 전 무기
            if (pastWp == m_value) // 똑같은거 먹으면 누적
            {
                if (WeaponNum[m_value] != -1)
                {
                    if (PlayerPrefs.GetInt("curCh") == 2)
                        GM.weaponNum += ( WeaponNum[m_value] ) + ( WeaponNum[m_value] /2 );
                    else
                        GM.weaponNum += WeaponNum[m_value];
                }
            }
            else // 다른거 먹으면 초기화
            {
                if (PlayerPrefs.GetInt("curCh") == 2)    
                    GM.weaponNum = ( WeaponNum[m_value] ) + ( WeaponNum[m_value] / 2 );
                
                else
                    GM.weaponNum = WeaponNum[m_value];
            }
            if (m_value == 3)
            {
                if (PlayerPrefs.GetInt("WpLevel3") >= 1)
                {
                    player.bullet = 12;
                    for (int i = 0; i < 12; i++)
                        player.bulletUI.transform.GetChild(i + 1).gameObject.SetActive(true);
                }
                
                else
                    player.bullet = 8;
            }


            PlayerPrefs.SetInt("curWp", m_value);
            player.curWp = m_value;
            GM.StartCoroutine("WeaponItemPopUp");
            Destroy(gameObject);
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

}
