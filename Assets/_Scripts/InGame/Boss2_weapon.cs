using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_weapon : MonoBehaviour {

    GameManager GM;
    Player player;

    float HP = 10f;
    public int m_num;
    public bool isAlive = true;

    float KnockBack;
    float Damage;

    AudioSource Explosion;
    //AudioSource SwordShot;
    AudioSource GoldBoneDrop;

	void Start () 
    {
        //SwordShot = GameObject.Find("SwordShot").GetComponent<AudioSource>();   
        Explosion = GameObject.Find("Explosion").GetComponent<AudioSource>();
        GoldBoneDrop = GameObject.Find("GoldBoneDrop").GetComponent<AudioSource>();

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	void Update () 
    {
        if (transform.position.x <= -12)
            Destroy(gameObject);

        switch (m_num)
        {
            case 0: // 상어 바주카
                if(isAlive == true)
                    transform.position += Vector3.left * Time.deltaTime * 2f;
                else
                    transform.Rotate(Vector3.back * 100f * Time.deltaTime);
                break;
            case 1: // 미니 바주카
                transform.position += Vector3.left * Time.deltaTime * 12f;
                break;
            case 2: // 바주카 본체
                transform.Rotate(Vector3.back * 720f * Time.deltaTime);
                transform.position += Vector3.left * Time.deltaTime * 10f;
                break;
            case 3: // 직접 날아감
                transform.position += Vector3.left * Time.deltaTime * 15f;
                break;
         
        }
        if (isAlive == true && HP <= 0)
            StartCoroutine("Die");
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && player.GhostTime == 0 && player.isSuper == false && (m_num == 0 || m_num == 1))       // Player Attack
        {
            player.StartCoroutine("PlayerAttack");
            if (PlayerPrefs.GetInt("music") == 1)
                Explosion.Play();
            player.StartCoroutine("Vibe");
            Instantiate(GM.Effect[1], transform.position, transform.rotation); // 폭.발. 
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player" && player.GhostTime == 0 && player.isSuper == false && (m_num == 2))       // Player Attack
        {
            player.StartCoroutine("PlayerAttack");
            if (PlayerPrefs.GetInt("music") == 1)
                GoldBoneDrop.Play();
            player.StartCoroutine("Vibe");
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player" && player.GhostTime == 0 && player.isSuper == false && (m_num == 3))       // Player Attack
        {
            player.StartCoroutine("PlayerAttack");
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
        if (m_num == 0)
        {
            HP -= Damage;

            for (int i = 0; i < 8; i++)
            {
                transform.position += Vector3.right * Time.deltaTime * KnockBack*2f;
                yield return new WaitForSeconds(0.01f);
            }
            KnockBack = 0;
        }
    }

    IEnumerator Die()
    {
        isAlive = false;
        if (PlayerPrefs.GetInt("music") == 1)
            Explosion.Play();
        player.StartCoroutine("Vibe");
        Instantiate(GM.Effect[1], transform.position, transform.rotation); // 폭.발.

        StopCoroutine("Hit");
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.isKinematic = false;
        Vector2 up = new Vector2(8f, 4f);
        rigid.AddForce(up, ForceMode2D.Impulse);
        Destroy(gameObject,0.5f);

        yield return null;
    }

}
