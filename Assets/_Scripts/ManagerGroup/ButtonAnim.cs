using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    bool check;
    public string type = "";

    Vector3 StartScale;
    Vector3 DownScale;

    AudioSource Cute2;
    AudioSource StoneHit;
    AudioSource Close;
    AudioSource Button;


    void Start()
    {
        Cute2 = GameObject.Find("Cute2").GetComponent<AudioSource>();
        StoneHit = GameObject.Find("StoneHit").GetComponent<AudioSource>();
        Close = GameObject.Find("Close").GetComponent<AudioSource>();
        Button = GameObject.Find("Button").GetComponent<AudioSource>();


        StartScale = transform.localScale;
        DownScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        check = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        check = false;
        if (type == "Close")
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Cute2.Play();
        }
        else if (type == "PutMilk")
        {
            if (PlayerPrefs.GetInt("music") == 1)
                StoneHit.Play();
        }
        else
        {
            if (PlayerPrefs.GetInt("music") == 1)
                Button.Play();
        }
    }

    void Update() 
    {
        if (check)
            transform.localScale = DownScale;
        else
            transform.localScale = StartScale;

    }

}