using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static string nextScene;


    [SerializeField]
    Image progressBar;
    public GameObject UI;
    public GameObject Dog;

    private void Start()
    {
        StartCoroutine(LoadScene());
        Dog.transform.GetChild( Random.Range(0,5) ).gameObject.SetActive(true);
    }
    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            UI.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            UI.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        }
    }

    string nextSceneName;
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync("TitleScene");
        
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                    op.allowSceneActivation = true;
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
   