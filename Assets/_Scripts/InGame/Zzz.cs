using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zzz : MonoBehaviour {

    SpriteRenderer sprite;


    int rand;
	void Start () 
    {

        rand = Random.Range(0, 3);
        transform.GetChild(rand).gameObject.SetActive(true);
        sprite = transform.GetChild(rand).gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine("PadeOut");
	}
	
    IEnumerator PadeOut() 
    {
        int time = 0;

        byte alpha = 255;
        float size = 0.1f;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        float randX = Random.Range(-0.06f, 0.06f);
        float randY = Random.Range(0.02f, 0.07f);


        while (time < 12)
        {
            transform.GetChild(rand).localScale = new Vector3(transform.localScale.x + size , transform.localScale.y + size , transform.localScale.z);
            transform.position = new Vector3(transform.position.x + randX, transform.position.y + randY);
            sprite.color = new Color32(255,255,255,alpha);
            alpha -= 20;
            size += 0.05f;
            yield return new WaitForSeconds(0.02f);
            time++;
        }
        Destroy(gameObject);
    }
}
