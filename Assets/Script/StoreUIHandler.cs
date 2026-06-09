using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class StoreUIHandler : MonoBehaviour
{
    public static StoreUIHandler Instance;
    public GameObject skinItemPrefab; 
    public Transform contentPanel;     
    public GameObject notEnoughCoinsPopup;
    private bool popupActive = false;

    private List<SkinItemUI> allUIButtons = new List<SkinItemUI>();

    void Awake() { Instance = this; }

    void Start() { InitializeStore(); }

    void Update()
    {
        // Detect click anywhere if popup is currently visible
        if (popupActive)
        {
            bool interaction = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || 
                               (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

            if (interaction)
            {
                notEnoughCoinsPopup.SetActive(false);
                popupActive = false;
            }
        }
    }

    public void ShowNotEnoughCoinsPopup()
    {
        if (notEnoughCoinsPopup != null) 
        {
            notEnoughCoinsPopup.SetActive(true);
            notEnoughCoinsPopup.transform.SetAsLastSibling(); // Ensure it renders on top
            popupActive = true;
        }
    }


    void InitializeStore()
    {
        // Clear existing list if any
        allUIButtons.Clear();
        foreach (Transform child in contentPanel) Destroy(child.gameObject);

        foreach (var skin in SkinManager.Instance.allSkins)
        {
            GameObject newItem = Instantiate(skinItemPrefab, contentPanel);
            SkinItemUI ui = newItem.GetComponent<SkinItemUI>();
            ui.Setup(skin);
            allUIButtons.Add(ui);
        }
    }

    public void RefreshAllButtons()
    {
        foreach (var item in allUIButtons)
        {
            if (item != null) item.UpdateUI();
        }
    }

}