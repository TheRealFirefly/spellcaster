using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] float smoothSpeed = 5f;

    float targetPercent = 1f;

    public void SetHP(float currentHP, float maxHP)
    {
        targetPercent = Mathf.Clamp01(currentHP / maxHP);
    }

    private void Update()
    {
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetPercent,
            Time.deltaTime * smoothSpeed
        );

        UpdateColor(fillImage.fillAmount);
    }

    void UpdateColor(float percent)
    {
        if (percent <= 0.1f)
            fillImage.color = Color.red;
        else if (percent <= 0.5f)
            fillImage.color = new Color(1f, 0.65f, 0f);
        else
            fillImage.color = Color.green;
    }
}
