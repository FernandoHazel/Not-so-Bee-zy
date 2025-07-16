using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using EventBus;
using DG.Tweening;
 
public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] AudioClip rewardedSound;
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms
    public delegate void ActionReward();
    public static event ActionReward rewarded;
    public static bool adLoaded;
    public static bool playerFell;

    private void Awake() {
        if (AdsInitializer.initialized)
        {
            LoadAd();
        }
    }

    private void OnEnable() {
        CheckEnable();
    }

    private void CheckEnable() {
        //Si el jugador se cayó al precipicio el jugador no verá el botón en la pantalla
        
        if(playerFell)
        {
            _showAdButton.interactable = false;
            Debug.Log("Player fell so there is not rewarded ad");
        }

        //Igualmente si no se cargó ningún anuncio no aparecemos el botón
        if (!adLoaded)
        {
            _showAdButton.interactable = false;
            Debug.Log("Ad is not loaded, trying to load ad again");
        } else
        {
            _showAdButton.interactable = true;
            Debug.Log("Ready to show rewareded ad");
        }
    }

 
    // Load content to the Ad Unit:
    public void LoadAd()
    {
        GetID();
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);

        if(_adUnitId != "")
        {
            Advertisement.Load(_adUnitId, this);
            OnUnityAdsAdLoaded(_adUnitId);
        } else
        {
            Debug.Log("Couldn't get _adUnitId");
        }
        
    }
 
    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        //Debug.Log("Ad Loaded: " + adUnitId);
        //Debug.Log(adUnitId.Equals(_adUnitId));
 
        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }

        adLoaded = true;
    }
 
    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }
 
    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            if(rewarded != null)
            rewarded();

            AudioManager.audioManager.PlaySfxOnce(rewardedSound);

            // Load another ad:
            Advertisement.Load(_adUnitId, this);
        }
    }
 
    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
        adLoaded = false;
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
 
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
 
    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();

        // Limpia los objetos generados por DOTween
        DOTween.Clear(true);
        
    }

    public void GetID()
    {
        // Get the Ad Unit ID for the current platform:
        #if UNITY_IOS
                _adUnitId = _iOSAdUnitId;
        #elif UNITY_ANDROID
                _adUnitId = _androidAdUnitId;
        #endif

        Debug.Log("_adUnitId: " + _adUnitId);

        //Disable the button until the ad is ready to show:
        _showAdButton.interactable = false;
    }
}
