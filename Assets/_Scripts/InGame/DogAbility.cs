using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAbility : MonoBehaviour {

    public int curCh;

    void Start () 
    {
        curCh = PlayerPrefs.GetInt("curCh");
    }


    void Update () 
    {

    }
}
