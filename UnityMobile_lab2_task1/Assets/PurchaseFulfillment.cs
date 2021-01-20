using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseFulfillment : MonoBehaviour
{
    public void grantCredits(int amount)
    {
        Debug.Log("Otrzymałeś "+amount+" monet!");
    }
    public void removeAds()
    {
        Debug.Log("Reklamy zostały usunięte!");
    }
    public void accessPremium()
    {
        Debug.Log("Otrzymałeś wersję premium gry!");
    }
}
