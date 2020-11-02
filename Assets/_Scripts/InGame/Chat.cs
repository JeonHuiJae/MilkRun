using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour {

    public bool isScore;
    public string Content;
    public Text ContentText;
    bool flag = true;



	void Start () 
    {
        
		
	}
	
	
	void Update () 
    {
        ContentText.text = Content;
        if (gameObject.activeSelf == true && flag == true)
        {
            flag = false;
            StartCoroutine("PopUp");
        }
        if (gameObject.activeSelf == true && isScore == true)
        {
            isScore = false;
            StartCoroutine("PopUp2");
        }
	}

    IEnumerator PopUp()
    {
        yield return new WaitForSeconds(3f);
        flag = true;
        gameObject.SetActive(false);
    }
    IEnumerator PopUp2()
    {
        for (int i = 0; i < 15; i++)
        {
            transform.position += Vector3.up * Time.deltaTime * 1.5f;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }

}
