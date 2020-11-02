using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelMenu : MonoBehaviour {
    public GameObject MainUI;

    AudioSource Button;

    private void Awake()
    {
        Button = GameObject.Find("Button").GetComponent<AudioSource>();

    }


    void Update ()
    {   
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }


    public void Easy()
    {
        AudioSource MilkRun_Title;
        AudioSource Puppy1;

        MilkRun_Title = GameObject.Find("MilkRun_Title").GetComponent<AudioSource>();
        Puppy1 = GameObject.Find("Puppy1").GetComponent<AudioSource>();

        MilkRun_Title.Stop();

        if (PlayerPrefs.GetInt("music") == 1)
            Puppy1.Play();

        PlayerPrefs.SetInt("GameLevel", 0); // 난이도
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    public void Normal()
    {
        AudioSource MilkRun_Title;
        AudioSource Puppy1;

        MilkRun_Title = GameObject.Find("MilkRun_Title").GetComponent<AudioSource>();
        Puppy1 = GameObject.Find("Puppy1").GetComponent<AudioSource>();

        MilkRun_Title.Stop();

        if (PlayerPrefs.GetInt("music") == 1)
            Puppy1.Play();

        PlayerPrefs.SetInt("GameLevel", 1); // 난이도
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    public void Hard()
    {
        AudioSource MilkRun_Title;
        AudioSource Puppy1;

        MilkRun_Title = GameObject.Find("MilkRun_Title").GetComponent<AudioSource>();
        Puppy1 = GameObject.Find("Puppy1").GetComponent<AudioSource>();

        MilkRun_Title.Stop();

        if (PlayerPrefs.GetInt("music") == 1)
            Puppy1.Play();

        PlayerPrefs.SetInt("GameLevel", 2); // 난이도
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    public void Extream()
    {
        AudioSource MilkRun_Title;
        AudioSource Puppy1;

        MilkRun_Title = GameObject.Find("MilkRun_Title").GetComponent<AudioSource>();
        Puppy1 = GameObject.Find("Puppy1").GetComponent<AudioSource>();

        MilkRun_Title.Stop();

        if (PlayerPrefs.GetInt("music") == 1)
            Puppy1.Play();

        PlayerPrefs.SetInt("GameLevel", 3); // 난이도
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

}
