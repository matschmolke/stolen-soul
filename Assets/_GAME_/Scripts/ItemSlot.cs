using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;
    private Canvas canvas;
    private Transform originalParent;
    private Image dragImage;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        canvas = GetComponentInParent<Canvas>();
    }

    public void AddItem(string name, int qty, Sprite sprite)
    {
        itemName = name;
        quantity = qty;
        itemSprite = sprite;
        isFull = true;

        itemImage.enabled = true;
        itemImage.sprite = sprite;

        quantityText.text = qty.ToString();
        quantityText.enabled = true;
    }

    public void ClearSlot()
    {
        itemName = "";
        quantity = 0;
        itemSprite = null;
        isFull = false;

        itemImage.sprite = null;
        itemImage.enabled = false;

        quantityText.text = "";
        quantityText.enabled = false;
    }

    public void RefreshUI()
    {
        if (isFull && itemSprite != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = itemSprite;

            quantityText.enabled = true;
            quantityText.text = quantity.ToString();
        }
        else
        {
            ClearSlot();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            selectedShader.SetActive(false);
            thisItemSelected = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isFull) return;

        dragImage = new GameObject("DragImage").AddComponent<Image>();
        dragImage.sprite = itemSprite;
        dragImage.raycastTarget = false;
        dragImage.transform.SetParent(canvas.transform, false);
        dragImage.rectTransform.sizeDelta = new Vector2(64, 64);
        dragImage.color = new Color(1, 1, 1, 0.8f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragImage != null)
            dragImage.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage != null)
            Destroy(dragImage.gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var originSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (originSlot == null || originSlot == this)
        {
            RefreshUI();
            return;
        }

        string tempName = itemName;
        int tempQty = quantity;
        Sprite tempSprite = itemSprite;
        bool tempFull = isFull;

        // Swap if slot is not empty
        if (isFull)
        {
            AddItem(originSlot.itemName, originSlot.quantity, originSlot.itemSprite);
            originSlot.AddItem(tempName, tempQty, tempSprite);
        }
        else // If slot is empty - move
        {
            AddItem(originSlot.itemName, originSlot.quantity, originSlot.itemSprite);
            originSlot.ClearSlot();
        }
        
        RefreshUI();
        originSlot.RefreshUI();
    }
}
