using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    void Start()
    {
        UpdateDisplay();
    }

    void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (CoinManager.Instance != null)
        {
          
            CoinManager.Instance.RegisterAndRefreshUI(GetComponent<TextMeshProUGUI>());
        }
    }
}