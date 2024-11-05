using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
 
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] RewardedAdsButton rewardedAdsButton;
    [SerializeField] InterstitialAds interstitialAds;
    [SerializeField] bool _testMode = true;
    private string _gameId;
    public static bool initialized = false;
 
    void Awake()
    {
        #if UNITY_IOS || UNITY_ANDROID
            _testMode = false;
        #endif

        InitializeAds();
    }
 
    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;

        #if UNITY_IOS
                _gameId = _iOSGameId;
        #endif

        #if UNITY_ANDROID
            _gameId = _androidGameId;
        #endif

        Advertisement.Initialize(_gameId, _testMode, this);
        Debug.Log("Initializing ad, and gameId is: "+ _gameId);
    }
 
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        rewardedAdsButton.LoadAd();
        interstitialAds.LoadAd();
        initialized = true;
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        initialized = false;
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
