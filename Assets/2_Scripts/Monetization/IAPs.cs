using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPs : MonoBehaviour, IDetailedStoreListener //IStoreListener
{
    public static IStoreController storeController;
    private IStoreController controller;
    private IExtensionProvider extensions;

    [SerializeField] GameObject noAdsButton;
    public NcItem ncItem;

    public static bool ads = false; //This bool will allow or block the ads

    private void Awake()
    {
        setupBilder();

        //Ad a check for the user purchase and modify ads variable
        CheckNonConsumable(ncItem.id);
    }

    void setupBilder () {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(ncItem.id, ProductType.NonConsumable); 
        

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
    public void BuyRemoveAds()
    {
        Debug.Log("Buying no ads package");
        storeController.InitiatePurchase(ncItem.id);
    }

    //Check if the user already consumed an item
    public void CheckNonConsumable (string id) 
    {
        if(storeController != null)
        {
            var product = storeController.products.WithID(id);
            if(product != null)
            {
                if(product.hasReceipt)//purchased
                {
                    Debug.Log("Player has a receip");
                    RemoveAds();
                }
                else
                {
                    Debug.Log("Player has a NO receip");
                    ShowIntAds();
                }
            }
        }
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

        //Hide No ads button
        noAdsButton.SetActive(false);

        Debug.Log(product.definition.id + " purchased");

        //Remove ads
        RemoveAds();

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

    //Remove or show ads
    public void RemoveAds()
    {
        ads = false;
        Debug.Log("Ads removed");
    }
    public void ShowIntAds()
    {
        ads = true;
        Debug.Log("Ads activated");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
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
