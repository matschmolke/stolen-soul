using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public enum SlotOwner
{
    Player,
    Trader,
    Chest
}

public class TradeSlot : MonoBehaviour,
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
    public GameObject selectedShader;
    public bool thisItemSelected;

    private TradeManager manager;
    private Canvas canvas;
    private Image dragImage;

    private void Awake()
    {
        if (Owner == SlotOwner.Player)
        {
            Inventory = PlayerInventory.Instance;
        }
    }

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
            else
            {
                quantityText.text = "";
                quantityText.enabled = false;
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
        var originTradeSlot = eventData.pointerDrag?.GetComponent<TradeSlot>();
        var originOfferSlot = eventData.pointerDrag?.GetComponent<OfferSlot>();

        // Nie ma źródła
        if (originTradeSlot == null && originOfferSlot == null) return;

        // ===========================================
        //  PRZYPADEK 1 — OfferSlot -> TradeSlot
        //  (zwracamy item do inventory)
        // ===========================================
        if (originOfferSlot != null)
        {
            if (originOfferSlot.Owner != this.Owner) return; // gracz nie może wrzucać do tradera i odwrotnie

            // SLOT PUSTY → po prostu wstawiamy item z oferty do inventory
            if (Item == null)
            {
                Inventory.AddItemAt(slotId, originOfferSlot.Item, originOfferSlot.quantity);
                originOfferSlot.ClearSlot();

                RefreshUI();
                originOfferSlot.RefreshUI();
                return;
            }

            // STACKOWANIE
            if (Item.Id == originOfferSlot.Item.Id)
            {
                int maxStack = Item.maxStackSize;
                int total = quantity + originOfferSlot.quantity;

                if (total <= maxStack)
                {
                    Inventory.ChangeQuantity(slotId, total);
                    originOfferSlot.ClearSlot();
                }
                else
                {
                    Inventory.ChangeQuantity(slotId, maxStack);
                    originOfferSlot.quantity = total - maxStack;
                }

                RefreshUI();
                originOfferSlot.RefreshUI();
                return;
            }

            // RÓŻNY ITEM → swap Offer <-> Inventory
            ItemBase offerItem = originOfferSlot.Item;
            int offerQty = originOfferSlot.quantity;

            // item z TradeSlot → do OfferSlot
            originOfferSlot.Item = this.Item;
            originOfferSlot.quantity = this.quantity;

            // item z Offer → do inventory
            Inventory.AddItemAt(slotId, offerItem, offerQty);

            RefreshUI();
            originOfferSlot.RefreshUI();
            return;
        }

        // ===========================================
        //  PRZYPADEK 2 — TradeSlot -> TradeSlot
        //  (normalne przenoszenie w inventory)
        // ===========================================
        if (originTradeSlot != null)
        {
            if (originTradeSlot == this) return;
            if (originTradeSlot.Owner != this.Owner) return;

            // Przenoszenie przez MoveItem
            Inventory.MoveItem(slotId, originTradeSlot.slotId);

            RefreshUI();
            originTradeSlot.RefreshUI();
        }
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
