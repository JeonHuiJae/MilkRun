using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPop : MonoBehaviour {


    public GameObject EquipMenu; 
    public GameObject StoreMenu; 
    public GameObject Level; 

    EquipManager EM;

	void Start () {

        EM = EquipMenu.GetComponent<EquipManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (PlayerPrefs.GetInt("FirstPop0") == 0 && EquipMenu.activeSelf == true && EM.cur_button == 0)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);
        
        if (PlayerPrefs.GetInt("FirstPop1") == 0 && EquipMenu.activeSelf == true && EM.cur_button == 1)
            transform.GetChild(1).gameObject.SetActive(true);
        else
            transform.GetChild(1).gameObject.SetActive(false); 
       
        if (PlayerPrefs.GetInt("FirstPop2") == 0 && EquipMenu.activeSelf == true && EM.cur_button == 2)
            transform.GetChild(2).gameObject.SetActive(true);
        else
            transform.GetChild(2).gameObject.SetActive(false); 
        
        if (PlayerPrefs.GetInt("FirstPop3") == 0 && StoreMenu.activeSelf == true)
            transform.GetChild(3).gameObject.SetActive(true);
        else
            transform.GetChild(3).gameObject.SetActive(false); 
        
        if (PlayerPrefs.GetInt("FirstPop4") == 0 && Level.activeSelf == true)
            transform.GetChild(4).gameObject.SetActive(true);
        else
            transform.GetChild(4).gameObject.SetActive(false); 
        
    }

    public void Exit(int type)
    {
        if (PlayerPrefs.GetInt("FirstPop" + type) != 1)
            PlayerPrefs.SetInt("FirstPop" + type, 1);
    }
}
