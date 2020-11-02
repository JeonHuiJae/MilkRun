using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pang : MonoBehaviour {

    Rigidbody2D rigid;
    public bool GoldTag = false;

	void Start () 
    {
        rigid = GetComponent<Rigidbody2D>();
        if (GoldTag == false)
        {
            int rand = Random.Range(0, 4);
            transform.GetChild(rand).gameObject.SetActive(true);
        }
        else
            transform.GetChild(4).gameObject.SetActive(true);

        float randX = Random.Range(-8f, 8f);
        float randY = Random.Range(6f, 18f);
        Vector2 up = new Vector2(randX, randY);
        rigid.AddForce(up, ForceMode2D.Impulse);

        Destroy(gameObject, 2f);
		
	}
}
