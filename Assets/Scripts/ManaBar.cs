using UnityEngine;
using UnityEngine.UI;

public class ManaBarUI : MonoBehaviour
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

        fillImage.color = Color.cyan;
    }

}
