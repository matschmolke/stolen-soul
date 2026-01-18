using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text valueText;

    private bool isVisable = false;
    private RectTransform rectTransform;

    private void Awake()
    {
        Instance = this;
        tooltipObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!isVisable) return;

        FollowMouse();
    }

    public void ShowTooltip(ItemBase item, Vector3 pos)
    {
        titleText.text = item.itemName;
        descriptionText.text = item.description;
        valueText.text = $"{item.value}";

        tooltipObject.SetActive(true);
        isVisable = true;
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
        isVisable = false;
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 pivot = new Vector2(0f, 1f);

        // Horizontal edge
        if (mousePos.x > Screen.width * 0.5f)
            pivot.x = 1f;

        // Vertical edge
        if (mousePos.y < Screen.height * 0.5f)
            pivot.y = 0f;

        rectTransform.pivot = pivot;

        rectTransform.position = mousePos;
    }
}
