using UnityEngine;
using UnityEngine.Advertisements;
 
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] RewardedAdsButton rewardedAdsButton;
    [SerializeField] bool _testMode = true;
    private string _gameId;
    public static bool AdReady;
 
    void Awake()
    {
        #if UNITY_IOS
            _testMode = false;
        #endif

        #if UNITY_ANDROID
            _testMode = false;
        #endif

        AdReady = false;
        InitializeAds();
    }
 
    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }
 
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        AdReady = true;
        rewardedAdsButton.LoadAd();
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
