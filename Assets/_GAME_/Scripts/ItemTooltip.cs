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

    private void Awake()
    {
        Instance = this;
        tooltipObject.SetActive(false);
    }

    public void ShowTooltip(ItemBase item, Vector3 pos)
    {
        titleText.text = item.itemName;
        descriptionText.text = item.description;
        valueText.text = $"{item.value}";

        tooltipObject.transform.position = pos + new Vector3(200, -50, 0); // offset from slot
        tooltipObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
