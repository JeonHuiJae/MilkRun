using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonAbility : MonoBehaviour {

    public GameObject canonBall;

    GameManager GM;
    GameObject Wagon;
    Player player;

    public float punchCool = 0;
    float canonCool = 0;

    public int curWg;
    int curCh;

    AudioSource StoneHit;
    AudioSource Jump1;
    AudioSource CannonShot;

    float minusCool = 0;

    void Start () 
    {
        StoneHit = GameObject.Find("StoneHit").GetComponent<AudioSource>();
        Jump1 = GameObject.Find("Jump1").GetComponent<AudioSource>();
        CannonShot = GameObject.Find("CannonShot").GetComponent<AudioSource>();

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        curWg = PlayerPrefs.GetInt("curWg");
        Wagon = transform.GetChild(curWg).gameObject;

        curCh = PlayerPrefs.GetInt("curCh");
        if (curCh == 3)
            minusCool = 0.7f;
    }

    void Update () 
    {
        if (curWg == 6)
        { // 로켓수레 부스터 보이게만 해둠
            if (player.DogState.transform.GetChild(1).gameObject.activeSelf == true) // 뛸떄
                Wagon.transform.GetChild(3).gameObject.SetActive(true);
            else
                Wagon.transform.GetChild(3).gameObject.SetActive(false);
        }
        if (curWg == 3 || curWg == 4 || curWg == 5)
        {
            if (player.isStand == false) // 바퀴는 stand 상태는 안 굴러가게
            {
                Wagon.transform.GetChild(0).transform.Rotate(Vector3.back * 460f * Time.deltaTime);
                Wagon.transform.GetChild(1).transform.Rotate(Vector3.back * 460f * Time.deltaTime);
            }
        }
        else if(player.isStand == false) // 바퀴는 stand 상태는 안 굴러가게
            Wagon.transform.GetChild(0).transform.Rotate(Vector3.back * 360f * Time.deltaTime);
        
        if (curWg == 4 && canonCool == 0 && GM.GameStart == true) // canon일떄 
        {
            Vector3 pos = new Vector3(transform.position.x-0.3f, transform.position.y+0.3f, transform.position.z);
            Instantiate(canonBall, pos, transform.rotation);
            canonCool = 1.4f * (1- minusCool);
            StartCoroutine("canonCoolTimer");
        }
    }

    // punch 

    public void punch() 
    {
        if (punchCool == 0)
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Jump1.Play();
            StartCoroutine("punchHit");
            punchCool = 1f * (1-minusCool);
            StartCoroutine("punchCoolTimer");
        }
    }

    IEnumerator punchCoolTimer()
    {
        while (punchCool > 0) 
        {
            punchCool -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        punchCool = 0;
    }

    IEnumerator punchHit() 
    {
        GameObject punch = Wagon.transform.GetChild(4).gameObject;
        GameObject spring = Wagon.transform.GetChild(5).gameObject;
        GameObject PunchRange = Wagon.transform.GetChild(7).gameObject;
        for (int i = 0; i < 5; i++)
        {
            spring.transform.localScale =
                new Vector3(spring.transform.localScale.x + 0.06f, spring.transform.localScale.y);

            punch.transform.localPosition =
                new Vector3(punch.transform.localPosition.x + 0.34f, punch.transform.localPosition.y - 0.054f);
            yield return new WaitForSeconds(0.002f);
        }
        PunchRange.SetActive(true);

        for (int i = 0; i < 3; i++)
            player.StartCoroutine("Vibe");
        
        yield return new WaitForSeconds(0.02f);
        PunchRange.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            spring.transform.localScale =
                new Vector3(spring.transform.localScale.x - 0.03f, spring.transform.localScale.y );

            punch.transform.localPosition =
                new Vector3(punch.transform.localPosition.x - 0.17f, punch.transform.localPosition.y + 0.027f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    // canon


    IEnumerator canonCoolTimer()
    {
        if (PlayerPrefs.GetInt("music") == 1)
            CannonShot.Play();
        GameObject cannon = Wagon.transform.GetChild(4).gameObject;
        for (int i = 0; i < 3; i++)
        {
            cannon.transform.position += Vector3.left * 0.1f;
            yield return new WaitForSeconds(0.001f);
        }
            
        for (int i = 0; i < 10; i++)
        {
            cannon.transform.position += Vector3.right * 0.03f;
            yield return new WaitForSeconds(0.001f);
        }
        while(canonCool > 0)
        {
            canonCool -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        canonCool = 0;
    }

}
