using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinItemUI : MonoBehaviour
{
    public SkinManager.SkinData skinData;
    public Image displayImage;
    public Button actionButton;
    public Image buttonImage;
    public TMP_Text statusLabel;

    [Header("Button Sprites")]
    public Sprite buySprite;
    public Sprite useSprite;
    public Sprite equippedSprite;

    public void Setup(SkinManager.SkinData data)
    {
        skinData = data;
        if (displayImage != null) displayImage.sprite = data.displaySprite;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (skinData == null || actionButton == null) return;

        string equippedID = PlayerPrefs.GetString("EquippedSkin", "Default");
        bool isOwned = (skinData.skinID == "Default") || (PlayerPrefs.GetInt(skinData.skinID + "_Owned", 0) == 1);
        bool isEquipped = (equippedID == skinData.skinID);

        actionButton.onClick.RemoveAllListeners();

        if (isEquipped)
        {
            if(buttonImage != null) buttonImage.sprite = equippedSprite;
            actionButton.interactable = false;
            if(statusLabel != null) statusLabel.text = "";
        }
        else if (isOwned)
        {
            if(buttonImage != null) buttonImage.sprite = useSprite;
            actionButton.interactable = true;
            if(statusLabel != null) statusLabel.text = "OWNED";
            actionButton.onClick.AddListener(UseSkin);
        }
        else
        {
            if(buttonImage != null) buttonImage.sprite = buySprite;
            actionButton.interactable = true;
            if(statusLabel != null) statusLabel.text = "$" + skinData.price.ToString();
            actionButton.onClick.AddListener(BuySkin);
        }
    }

    void BuySkin()
    {
        if (SkinManager.Instance.BuySkin(skinData))
        {
            StoreUIHandler.Instance.RefreshAllButtons();
        }
        else
        {
            StoreUIHandler.Instance.ShowNotEnoughCoinsPopup();
        }
    }

    void UseSkin()
    {
        SkinManager.Instance.EquipSkin(skinData.skinID);
        StoreUIHandler.Instance.RefreshAllButtons();
    }
}