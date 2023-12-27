using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;
public class InterstitialAds : MonoBehaviour
{
    bool pauseStatus = true;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif
    private InterstitialAd interstitialAd;
    
    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        Debug.Log("Loading the interstitial ad.");
        var adRequest = new AdRequest();
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }
                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                interstitialAd = ad;
            });
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            LoadInterstitialAd();
            if (interstitialAd != null)
            {
                ShowInterstitialAd();
                pauseStatus = false;
            }
            else
                LoadInterstitialAd();
        }
    }
    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
            Debug.LogError("Interstitial ad is not ready yet.");
    }
}