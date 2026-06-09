using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Levels : MonoBehaviour
{
    [System.Serializable]
    public class LevelData
    {
        public string sceneName;
        public Button button;
        public Sprite unlockedSprite;
        public Sprite lockedSprite;
        [HideInInspector] public bool isUnlocked;
    }

    public LevelData[] levels;
    public LevelSelectionManager selectionManager; 
    public GameObject PopUpMsg; 

    private bool popupActive = false;

    void Start()
    {
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        for (int i = 0; i < levels.Length; i++)
        {
            int levelNumber = i + 1;
            levels[i].isUnlocked = (levelNumber <= reachedLevel);
            SetupButton(levels[i], i);
        }

        if (PopUpMsg != null) PopUpMsg.SetActive(false);
    }

    void SetupButton(LevelData data, int index)
    {
        if (data.button == null) return;
        
        Image img = data.button.GetComponent<Image>();
        if (img != null) img.sprite = data.isUnlocked ? data.unlockedSprite : data.lockedSprite;

        data.button.onClick.RemoveAllListeners();
        data.button.onClick.AddListener(() =>
        {
            
            if (data.isUnlocked)
            {
                if (selectionManager != null) 
                {
                    selectionManager.OpenSpecificPopup(index);
                }
            }
            else
            {
                
                ShowLockedPopup();
            }
        });
    }

    void ShowLockedPopup()
    {
        if (PopUpMsg != null) 
        {
            
            if (selectionManager != null) selectionManager.CloseCurrentPopup();

            PopUpMsg.SetActive(true);
            PopUpMsg.transform.SetAsLastSibling(); 
            popupActive = true;
        }
    }

    void Update()
    {
        if (popupActive)
        {
            bool interaction = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || 
                               (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

            if (interaction)
            {
                PopUpMsg.SetActive(false);
                popupActive = false;
            }
        }
    }
}