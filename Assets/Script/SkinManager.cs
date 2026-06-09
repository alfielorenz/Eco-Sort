using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    [System.Serializable]
    public class SkinData
    {
        public string skinID;
        public int price;
        public Sprite displaySprite;
        public Sprite watchingSprite;
        public Sprite happySprite;
        public Sprite angrySprite;
        public Sprite blinkSprite;
    }

    public SkinData[] allSkins;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool BuySkin(SkinData skin)
    {
        if (PlayerPrefs.GetInt(skin.skinID + "_Owned", 0) == 1) return true;

        if (CoinManager.Instance != null && PlayerPrefs.GetInt("TotalCoins", 0) >= skin.price)
        {
            CoinManager.Instance.AddRewards(-skin.price);
            PlayerPrefs.SetInt(skin.skinID + "_Owned", 1);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public void EquipSkin(string skinID)
    {
        PlayerPrefs.SetString("EquippedSkin", skinID);
        PlayerPrefs.Save();
    }
}