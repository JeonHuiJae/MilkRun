using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

    public int m_num;
    SpriteRenderer sprite;

	void Start () 
    {
        sprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        switch (m_num) {
            case 0: // normal
                    StartCoroutine("PadeOut");
                    break;
            case 1: // explosion(big, damage)
                    StartCoroutine("Explosion");
                break;
            case 2: // dust
                StartCoroutine("Dust");
                break;
            case 3: // 장난감칼 대각
                StartCoroutine("Sword_Dial");
                break;
            case 4: // 장난감칼 가로
                StartCoroutine("Sword_hori");
                break;
            case 5: // 토마토
                StartCoroutine("Tomato");
                break;
            case 6: // explosion(little)
                StartCoroutine("Explosion");
                break;
            case 7: // 가로 좁은 dust
                StartCoroutine("Dust");
                break;
        }

	}
	
    IEnumerator PadeOut() 
    {
        int time = 0;

        byte alpha = 255;
        while (time < 5)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.02f , transform.localScale.y + 0.02f , transform.localScale.z);
            sprite.color = new Color32(255,255,255,alpha);
            alpha -= 20;
            yield return new WaitForSeconds(0.005f);
            time++;
        }

        Destroy(gameObject);
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.04f);
        for (int i = 0; i < 5; i++) 
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i+1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.04f);
        }
        transform.GetChild(5).gameObject.SetActive(false);
        Destroy(gameObject);
    }

    IEnumerator Dust()
    {
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i + 1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }

    IEnumerator Sword_Dial()
    {
        yield return new WaitForSeconds(0.04f);
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i + 1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.04f);
        }
        Destroy(gameObject);
    }

    IEnumerator Sword_hori()
    {
        yield return new WaitForSeconds(0.04f);
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i + 1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.04f);
        }
        Destroy(gameObject);
    }

    IEnumerator Tomato()
    {
        yield return new WaitForSeconds(0.04f);
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i + 1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.04f);
        }
        Destroy(gameObject);
    }
}
