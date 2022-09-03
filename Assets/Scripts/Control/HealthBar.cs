using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarImage;
    void Start()
    {
        HealthBarImage = GetComponent<Image>();
        SetHealthBar(1);
    }
    public void SetHealthBar(float value)
    {
        HealthBarImage.fillAmount = value;
    }
}
