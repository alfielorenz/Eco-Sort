using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class BackButtonUI : MonoBehaviour
{
    [Header("Button Sprites")]
    public Sprite LevelBack;      
    public Sprite LevelBackCLicked;   
    

    private Button button;
    private Image buttonImage;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        
        if (LevelBack != null)
            buttonImage.sprite = LevelBack;

        
        button.onClick.AddListener(OnButtonClick);

        SetupSpriteSwap();
    }

    void OnButtonClick()
    {
        
        SceneManager.LoadScene("MainMenu");
    }

    void SetupSpriteSwap()
    {
        var spriteState = new SpriteState();

        if (LevelBackCLicked != null)
            spriteState.highlightedSprite = LevelBackCLicked;   
            spriteState.pressedSprite = LevelBackCLicked;       

        button.spriteState = spriteState;

        
        button.transition = Selectable.Transition.SpriteSwap;
    }
}