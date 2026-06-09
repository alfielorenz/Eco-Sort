using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    private const string TOTAL_COINS_KEY = "TotalCoins";
    private int cachedCoins; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            cachedCoins = PlayerPrefs.GetInt(TOTAL_COINS_KEY, 0);
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
    }

    public void RegisterAndRefreshUI(TextMeshProUGUI textComp)
    {
        if (textComp != null)
        {
            textComp.text = cachedCoins.ToString();
        }
    }

    public void AddRewards(int coins)
    {
        cachedCoins += coins;
        PlayerPrefs.SetInt(TOTAL_COINS_KEY, cachedCoins);
        PlayerPrefs.Save(); 
        
        UpdateAllCountersInScene();
    }

    public void UpdateAllCountersInScene()
    {
    
        GameObject[] counters = GameObject.FindGameObjectsWithTag("CoinCounter");
        foreach (GameObject obj in counters)
        {
            var t = obj.GetComponent<TextMeshProUGUI>();
            if (t != null) t.text = cachedCoins.ToString();
        }
    }
}