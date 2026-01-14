using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    public GameObject tooltipObject;
    public TMP_Text tooltipText;
    public Vector2 offset = new Vector2(0, -0);

    private RectTransform rectTransform;

    void Awake()
    {
        instance = this;
        rectTransform = tooltipObject.GetComponent<RectTransform>();
        tooltipObject.SetActive(false);
    }

    void Update()
    {
        if (!tooltipObject.activeSelf)
            return;

        RectTransform parentRect = (RectTransform)tooltipObject.transform.parent;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            mousePos,
            null, // Screen Space Overlay
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint + offset;
        }
    }




    public void Show(string text)
    {
        tooltipText.text = text;
        tooltipObject.SetActive(true);
    }

    public void Hide()
    {
        tooltipObject.SetActive(false);
    }
}

