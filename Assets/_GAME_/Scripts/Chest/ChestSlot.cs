using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ChestSlot : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler

{
    public SlotOwner Owner;

    public int slotId;

    public ItemBase Item => Inventory.GetItem(slotId).Item;
    public int quantity => Inventory.GetItem(slotId).Quantity;

    public InventoryBase Inventory { get; set; }

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    public bool thisItemSelected;

    private TradeManager manager;
    private Canvas canvas;
    private Image dragImage;

    private void Start()
    {
        manager = TradeManager.Instance;
        canvas = GetComponentInParent<Canvas>();
        RefreshUI();
    }

    public void ClearSlot()
    {
        Inventory.RemoveItemAt(slotId);

        itemImage.sprite = null;
        itemImage.enabled = false;

        quantityText.text = "";
        quantityText.enabled = false;
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
            manager.DeselectAllSlots();
            thisItemSelected = true;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
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
        var originSlot = eventData.pointerDrag?.GetComponent<ChestSlot>();

        if (originSlot == null)
            return;

        if (originSlot.Owner == this.Owner)
        {
            Inventory.MoveItem(slotId, originSlot.slotId);

            RefreshUI();
            originSlot.RefreshUI();
            return;
        }

        // ================================
        //  PRZYPADEK 2 — różne ekwipunki
        //  Player <-> Chest
        // ================================
        InventoryBase originInventory = originSlot.Inventory;
        InventoryBase targetInventory = this.Inventory;

        ItemBase originItem = originSlot.Item;
        int originQty = originSlot.quantity;

        // brak itemu — nic do zrobienia
        if (originItem == null)
            return;

        // 1) JEŚLI slot docelowy pusty → po prostu przenieś item
        if (this.Item == null)
        {
            // wstaw cały item do nowego ekwipunku
            targetInventory.AddItemAt(slotId, originItem, originQty);

            // usuń z poprzedniego ekwipunku
            originSlot.Inventory.RemoveItemAt(originSlot.slotId);

            RefreshUI();
            originSlot.RefreshUI();
            return;
        }

        // 2) JEŚLI item ten sam → stackowanie
        if (this.Item.Id == originItem.Id)
        {
            int maxStack = this.Item.maxStackSize;
            int total = this.quantity + originQty;

            if (total <= maxStack)
            {
                // całość się zmieści
                targetInventory.ChangeQuantity(slotId, total);
                originInventory.RemoveItemAt(originSlot.slotId);
            }
            else
            {
                // tylko część się zmieści
                targetInventory.ChangeQuantity(slotId, maxStack);
                originInventory.ChangeQuantity(originSlot.slotId, total - maxStack);
            }

            RefreshUI();
            originSlot.RefreshUI();
            return;
        }

        // 3) RÓŻNE ITEMY → swap między ekwipunkami
        ItemBase targetItem = this.Item;
        int targetQty = this.quantity;

        // zamiana
        targetInventory.AddItemAt(slotId, originItem, originQty);
        originInventory.AddItemAt(originSlot.slotId, targetItem, targetQty);

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
