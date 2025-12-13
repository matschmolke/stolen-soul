using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ItemSlot : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler

{
    public int slotId;

    public ItemBase Item => playerInventory.GetItem(slotId).Item;

    public int quantity => playerInventory.GetItem(slotId).Quantity;

    //public bool IsFull => Item != null && quantity >= Item.maxStackSize;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    public GameObject selectedShader;
    public bool thisItemSelected;
    public ItemType acceptableTypes;
    public EquipmentType accaptableEquipmentTypes = EquipmentType.Weapon | EquipmentType.Armor;

    private PlayerInventory playerInventory;
    private InventoryManager inventoryManager;
    private Canvas canvas;
    private Image dragImage;

    public event Action<ItemBase> OnItemChanged;

    public void Initialize(int id)
    {
        inventoryManager = InventoryManager.Instance;
        playerInventory = PlayerInventory.Instance;
        canvas = GetComponentInParent<Canvas>();
        slotId = id;
    }

    public bool CanAcceptItem(ItemBase item)
    {
        if (item == null) return false;

        if((acceptableTypes & item.itemType) == 0) return false;

        //If Equipment check if given type can be placed here
        if (item.itemType != ItemType.Equipment)
            return true;

        Equipment equip = item as Equipment;
        if (equip == null) return false;

        if ((accaptableEquipmentTypes & equip.equipmentType) == 0)
            return false;

        return true;
    }

    public void ClearSlot()
    {
        playerInventory.RemoveItemAt(slotId);

        itemImage.sprite = null;
        itemImage.enabled = false;

        quantityText.text = "";
        quantityText.enabled = false;
        OnItemChanged?.Invoke(Item);
    }

    public void RefreshUI()
    {
        if (Item != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = Item.itemSprite;


            if (Item.isStackable)
            {
                quantityText.text = quantity.ToString();
                quantityText.enabled = true;
            }
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
            if (thisItemSelected)
            {
                
                if (Item != null && (Item.itemType == ItemType.Consumable || Item.itemType == ItemType.Scroll))
                {
                    inventoryManager.UseItem(Item);
                    playerInventory.ChangeQuantity(slotId, quantity-1);
                    if (this.quantity <= 0)
                    {
                        ClearSlot();
                    }
                }
                RefreshUI();
            }

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
        if (Item == null) return;

        dragImage = new GameObject("DragImage").AddComponent<Image>();
        dragImage.sprite = Item.itemSprite;
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
        if (originSlot == null || originSlot == this) return;

        if (!CanAcceptItem(originSlot.Item))
        {
            Debug.Log("This slot does not accept this type of item: " + originSlot.Item.itemType);
            return;
        }

        if (Item != null && !originSlot.CanAcceptItem(Item))
        {
            Debug.Log("Swap not possible – origin slot does not accept this item.");
            return;
        }

        // Wykorzystanie metody MoveItem
        playerInventory.MoveItem(slotId, originSlot.slotId);

        OnItemChanged?.Invoke(Item);
        originSlot.OnItemChanged?.Invoke(originSlot.Item);

        RefreshUI();
        originSlot.RefreshUI();
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (Item != null)
            ItemTooltip.Instance.ShowTooltip(Item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltip.Instance != null)
            ItemTooltip.Instance.HideTooltip();
    }
}
