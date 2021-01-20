using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsScript : MonoBehaviour
{
    string gameId="3981579";
    bool testMode=true;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameId,testMode);
        //StartCoroutine(ShowBannerWhenInitialized());
    }

    // Update is called once per frame
    public void ShowInterstitialAd()
    {
        if(Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later");
        }
    }
    IEnumerator ShowBannerWhenInitialized()
    {
        while(!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show();
    }
}
