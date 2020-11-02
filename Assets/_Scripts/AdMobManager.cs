using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdMobManager : MonoBehaviour {

        
    string android_interstitial_id = "ca-app-pub-9138423677262630/8061243769";
    //public string ios_interstitial_id;
    string android_reward_id = "ca-app-pub-3940256099942544/5224354917";
    // google admob

    /*
    string android_game_id = "2647303";
    string ios_game_id = "2647302";
    string rewarded_video_id = "rewardedVideo";
    */
    // unity ads
    MainMenuManager MM;
    GameManager GM;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardAd;
    public int a;
    static AdMobManager instance = null;

    void Awake ()
    {
        if (instance != null) {
            Destroy (gameObject);
        } else {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);    
        }
    }

    public void Start()
    {
        //MobileAds.Initialize("ca-app-pub-9138423677262630~2090228520");

        //InitializeUnityAd();
        //RequestBannerAd();
        //ShowBannerAd();

        rewardAd = RewardBasedVideoAd.Instance;
        rewardAd.OnAdLoaded += OnAdLoaded;
        rewardAd.OnAdFailedToLoad += OnAdFailedToLoad;
        rewardAd.OnAdOpening += OnAdOpening;
        rewardAd.OnAdStarted += OnAdStarted;
        rewardAd.OnAdRewarded += OnAdRewarded;
        rewardAd.OnAdClosed += OnAdClosed;
        rewardAd.OnAdLeavingApplication += OnAdLeavingApplication;

        RequestInterstitialAd();
        RequestRewardAd();

    }

    public void RequestInterstitialAd()
    {
        string adUnitId = string.Empty;

        #if UNITY_ANDROID
        adUnitId = android_interstitial_id;
        #elif UNITY_IOS
        adUnitId = ios_interstitialAdUnitId;
        #endif

        interstitialAd = new InterstitialAd(adUnitId);
        //AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("4FB88470E3887CDA").Build();
        AdRequest request = new AdRequest.Builder().Build();
         
        interstitialAd.LoadAd(request);

        interstitialAd.OnAdClosed += HandleOnInterstitialAdClosed;
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        print("HandleOnInterstitialAdClosed event received.");

        interstitialAd.Destroy();

        RequestInterstitialAd();
    }
        
    public void ShowInterstitialAd()
    {
        if (!interstitialAd.IsLoaded())
        {
            RequestInterstitialAd();
            return;
        }

        interstitialAd.Show();
    }


    public void RequestRewardAd()
    {
        string AdUnitId = android_reward_id; 

        //AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("4FB88470E3887CDA").Build();
        AdRequest request = new AdRequest.Builder().Build();

        rewardAd.LoadAd(request, AdUnitId);

        rewardAd.OnAdClosed += HandleOnRewardAdClosed;
    }
    public void HandleOnRewardAdClosed(object sender, EventArgs args)
    {
        print("HandleOnRewardAdClosed event received.");

        RequestRewardAd();
    }

    public void ShowRewardAd()
    {
        if (!rewardAd.IsLoaded())
        {
            RequestRewardAd(); 
            return;
        }

        rewardAd.Show();
    }

    void OnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("OnAdLoaded");
    }
    void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("OnAdFailedToLoad");
        Debug.Log(e.Message);
    }
    void OnAdOpening(object sender, EventArgs e)
    {
        Debug.Log("OnAdOpening");
    }
    void OnAdStarted(object sender, EventArgs e)
    {
        Debug.Log("OnAdStarted");
    }
    void OnAdRewarded(object sender, Reward e)
    {
        Debug.Log("OnAdRewarded");
        Reward();
    }
    void OnAdClosed(object sender, EventArgs e)
    {
        Debug.Log("OnAdClosed");
        RequestRewardAd();
    }
    void OnAdLeavingApplication(object sender, EventArgs e)
    {
        Debug.Log("OnAdLeavingApplication");
    }


    public bool isRewardAdLoad()
    {
        return rewardAd.IsLoaded();
    }
        



    void Reward()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            MM = GameObject.FindGameObjectWithTag("MainMenuManager").GetComponent<MainMenuManager>();
            MM.ShowGiftButton();
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            PlayerPrefs.SetInt("Revival", 0);
            PlayerPrefs.SetInt("Revival_MapNum", GM.MapNum);
            PlayerPrefs.SetInt("Revival_Score", GM.Score);
            Time.timeScale = 1f;
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

    }
        
}
