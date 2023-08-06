using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPs : MonoBehaviour, IStoreListener
{
    IStoreController storeController;
    private IStoreController controller;
    private IExtensionProvider extensions;

    [SerializeField] GameObject noAdsButton;
    public NcItem ncItem;

    public static bool ads = true; //This bool will allow or block the ads

    private void Start() {
        setupBilder ();
    }

    void setupBilder () {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(ncItem.id, ProductType.NonConsumable); 
        //new IDs
        /* {
            {"no_ads_google", GooglePlay.Name},
            {"no_ads_mac", MacAppStore.Name}
        }); */

        UnityPurchasing.Initialize (this, builder);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    {
    
        storeController = controller;

        this.controller = controller;
        this.extensions = extensions;

        Debug.Log("IAP initialized");
    }

    //This happens when the user buy the no ads package
    public void RemoveAds()
    {
        Debug.Log("removing ads");
        storeController.InitiatePurchase(ncItem.id);
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
    {
        //Retrieve the purchased product
        var product = e.purchasedProduct;

        //Remove the ads and the ad button
        ads = false;
        noAdsButton.SetActive(false);

        Debug.Log(product.definition.id + " purchased");

        return PurchaseProcessingResult.Complete;
    }

    //////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("initialization failed "+ error);
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("initialization failed "+ error + " " + message);
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("initialization failed "+ product + " " + failureReason);
        throw new System.NotImplementedException();
    }
}

[System.Serializable]

public class NcItem{
    public string name;
    public string id;
    public string description;
    public float price;
}
