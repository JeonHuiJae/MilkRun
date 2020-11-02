using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    static SoundManager instance = null;

    void Awake ()
    {
        if (instance != null) {
            Destroy (gameObject);
        } else {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);    
        }
    }
}
