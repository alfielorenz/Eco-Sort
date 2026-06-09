using UnityEngine;
using UnityEngine.UI;

public class MusicVol : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image volumeImage;
    [SerializeField] private Button muteButton, halfButton, fullButton;

    [Header("Sprites")]
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite halfVolumeSprite;
    [SerializeField] private Sprite fullVolumeSprite;

    private AudioSource bgMusicSource;
    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    private void Start()
{
   
    GameObject musicObj = GameObject.Find("AudioManager"); 
    if (musicObj != null) bgMusicSource = musicObj.GetComponent<AudioSource>();

    
    float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    
    
    UpdateVolumeSprite(savedVolume);

    
    muteButton.onClick.AddListener(() => SetVolume(0f));
    halfButton.onClick.AddListener(() => SetVolume(0.5f));
    fullButton.onClick.AddListener(() => SetVolume(1f));
}

    private void LinkAndApply()
    {
        
        GameObject musicObj = GameObject.Find("AudioManager"); 
        if (musicObj != null)
        {
            bgMusicSource = musicObj.GetComponent<AudioSource>();
        }

        
        float savedVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        if (bgMusicSource == null)
        {
            
            GameObject musicObj = GameObject.Find("AudioManager");
            if (musicObj != null) bgMusicSource = musicObj.GetComponent<AudioSource>();
        }

        if (bgMusicSource != null)
        {
            bgMusicSource.volume = volume;
        }

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();
        UpdateVolumeSprite(volume);
    }

    private void UpdateVolumeSprite(float volume)
    {
        if (volumeImage == null) return;
        if (volume <= 0f) volumeImage.sprite = muteSprite;
        else if (volume <= 0.5f) volumeImage.sprite = halfVolumeSprite;
        else volumeImage.sprite = fullVolumeSprite;
    }
}