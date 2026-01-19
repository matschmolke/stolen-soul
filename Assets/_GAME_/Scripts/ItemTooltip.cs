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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        rectTransform = GetComponent<RectTransform>();

        // Check if tooltip is not null
        if (tooltipObject != null)
            tooltipObject.SetActive(false);
    }

    private void Update()
    {
        if (this == null || !isVisable || tooltipObject == null) return;

        FollowMouse();
    }

    public void ShowTooltip(ItemBase item, Vector3 pos)
    {
        // Check references
        if (tooltipObject == null || titleText == null) return;

        titleText.text = item.itemName;
        descriptionText.text = item.description;
        valueText.text = $"{item.value}";

        tooltipObject.SetActive(true);
        isVisable = true;
    }

    public void HideTooltip()
    {
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
        }
        isVisable = false;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void FollowMouse()
    {
        if (rectTransform == null) return;

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
