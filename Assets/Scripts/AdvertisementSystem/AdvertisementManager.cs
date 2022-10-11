using UnityEngine;
using UnityEngine.Advertisements;

namespace Lesson_7_5.AdvertisementSystem
{
    public enum AdvertisementTypes : byte
    {
        Interstitial = 0,
        Rewarded = 1,
        Banner = 2,
    }

    public class AdvertisementManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
    {
        [SerializeField]
        private string _androidGameId;
        [SerializeField]
        private string _iosGameId;

        [SerializeField]
        private bool _testMode = true;

        [SerializeField]
        private string _interstitialAndroid;
        [SerializeField]
        private string _interstitialIos;
        [SerializeField]
        private string _rewardedAndroid;
        [SerializeField]
        private string _rewardedIos;
        [SerializeField]
        private string _bannerAndroid;
        [SerializeField]
        private string _bannerIos;

        private bool Initialized => Advertisement.isInitialized;
        private string GameID => Application.platform == RuntimePlatform.IPhonePlayer ? _iosGameId : _androidGameId;
        private string Interstitial => Application.platform == RuntimePlatform.IPhonePlayer ? _interstitialIos : _interstitialAndroid;
        private string Rewarded => Application.platform == RuntimePlatform.IPhonePlayer ? _rewardedIos : _rewardedAndroid;
        private string Banner => Application.platform == RuntimePlatform.IPhonePlayer ? _bannerIos : _bannerAndroid;

        private void Awake()
        {
            Advertisement.Initialize(GameID, _testMode, this);
        }

        public void OnInitializationComplete()
        {
#if UNITY_EDITOR
            Debug.Log("Advertisement initialization success.");
#endif
            ShowAd(AdvertisementTypes.Banner);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            // TODO
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            // TODO
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            // TODO
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId != Banner)
            {
                Advertisement.Show(placementId);
            }
            else
            {   
                Advertisement.Banner.Show(placementId);
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }

        private void OnBannerLoaded()
        {
            Advertisement.Banner.Show(Banner);
        }

        private void OnBannerLoadError(string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }

        public void ShowAd(AdvertisementTypes advertisementType)
        {
            if (!Initialized)
            {
                return;
            }

            switch (advertisementType)
            {
                case AdvertisementTypes.Interstitial:
                    Advertisement.Load(Interstitial, this);
                    break;
                case AdvertisementTypes.Rewarded:
                    Advertisement.Load(Rewarded, this);
                    break;
                case AdvertisementTypes.Banner:
                    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                    Advertisement.Banner.Load(Banner, new BannerLoadOptions()
                    {
                        loadCallback = OnBannerLoaded,
                        errorCallback = OnBannerLoadError
                    });
                    break;
            }
        }
    }
}