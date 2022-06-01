using UnityEngine;
using UnityEngine.Events;
using GoogleMobileAds.Api;

public class AdsAction : MonoBehaviour
{
    [SerializeField] private UnityEvent Event_Resume;

    private RewardedAd rewardedAd;                                            // Test ID:       ca-app-pub-3940256099942544/5224354917
    private const string adUnitId = "ca-app-pub-1620346370654950~3869553477"; // Release ID:    ca-app-pub-1620346370654950~3869553477

    private void OnEnable()
    {
        rewardedAd = new RewardedAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    public void HandleUserEarnedReward(object sender, Reward args) => Event_Resume.Invoke();

    public void Show()
    {
        if (rewardedAd.IsLoaded()) rewardedAd.Show();
    }

    //Обновление рекламы, после каждого показа надо получать новый банер
    public void UpdateAd()
    {
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        rewardedAd = new RewardedAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }
}
