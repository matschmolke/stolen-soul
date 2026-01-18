using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class OfferSlot : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public SlotOwner Owner;

    public ItemBase Item;
    public int quantity;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    public bool thisItemSelected;

    public InventoryBase Inventory;
    private TradeManager manager;
    private Canvas canvas;
    private Image dragImage;

    public event Action OnItemChanged;

    private void Start()
    {
        if (Owner == SlotOwner.Player)
            Inventory = PlayerInventory.Instance;

        manager = TradeManager.Instance;
        canvas = GetComponentInParent<Canvas>();
        RefreshUI();
    }

    public void ClearSlot()
    {
        Item = null;
        quantity = 0;

        itemImage.sprite = null;
        itemImage.enabled = false;

        quantityText.text = "";
        quantityText.enabled = false;
        OnItemChanged?.Invoke();
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
            SoundManager.PlaySound(SoundType.SELECT);
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

    // Obs�uga wyrzucenia na OfferSlot:
    // - je�li origin jest TradeSlot (czyli item pochodzi z inventory), kopiujemy item do OfferSlot
    //   i usuwamy go z inventory (origin.ClearSlot() lub inventory.RemoveItemAt(...))
    // - je�li origin jest OfferSlot (przenoszenie wewn�trz ofert), wykonujemy normalne swap/stack
    public void OnDrop(PointerEventData eventData)
    {
        var originTradeSlot = eventData.pointerDrag?.GetComponent<TradeSlot>();
        var originOfferSlot = eventData.pointerDrag?.GetComponent<OfferSlot>();

        // Nie ma sk�d bra�
        if (originTradeSlot == null && originOfferSlot == null) return;

        // PRZYPADek: TradeSlot -> OfferSlot (przenosimy przedmiot z inventory do oferty)
        if (originTradeSlot != null)
        {
            // wymagamy zgodno�ci w�a�ciciela (np. nie wrzucamy przedmiot�w gracza do oferty tradera)
            if (originTradeSlot.Owner != this.Owner) return;

            // Je�li OfferSlot jest pusty � przenie� ca�y stos (kopiuj do oferty i usu� z inventory)
            if (Item == null)
            {
                Item = originTradeSlot.Item;
                quantity = originTradeSlot.quantity;

                // usu� z inventory �r�d�owego
                originTradeSlot.ClearSlot(); // ClearSlot ju� wywo�uje inventory.RemoveItemAt
            }
            // Ten sam item -> stackuj w OfferSlot (do maxStack)
            else if (Item == originTradeSlot.Item)
            {
                int maxStack = Item.maxStackSize;
                int total = quantity + originTradeSlot.quantity;
                if (total <= maxStack)
                {
                    quantity = total;
                    originTradeSlot.ClearSlot();
                }
                else
                {
                    quantity = maxStack;
                    originTradeSlot.Inventory.ChangeQuantity(originTradeSlot.slotId, total - maxStack);
                    originTradeSlot.RefreshUI();
                }
            }
            // R�ny item -> zamie� miejscami (przedmiot z oferty wraca do inventory w tym samym slotId)
            else
            {
                // przenie� item z inventory (originTradeSlot) do oferty tymczasowo
                ItemBase tradeItem = originTradeSlot.Item;
                int tradeQty = originTradeSlot.quantity;

                // umie�� item oferty z powrotem do inventory (w tym samym slotId)
                originTradeSlot.Inventory.AddItemAt(originTradeSlot.slotId, this.Item, this.quantity);

                // ustaw offer na nowy item
                this.Item = tradeItem;
                this.quantity = tradeQty;

                // je�li inventory.AddItemAt nadpisa�o UI, od�wie� origin i siebie
                originTradeSlot.RefreshUI();
            }

            OnItemChanged?.Invoke();

            RefreshUI();
            originTradeSlot.RefreshUI();
            return;
        }

        // PRZYPADek: OfferSlot -> OfferSlot (zamiana/stack w obr�bie ofert tego samego w�a�ciciela)
        if (originOfferSlot != null)
        {
            if (originOfferSlot.Owner != this.Owner) return;

            if (Item == null)
            {
                Item = originOfferSlot.Item;
                quantity = originOfferSlot.quantity;
                originOfferSlot.ClearSlot();
            }
            else if (Item == originOfferSlot.Item)
            {
                int maxStack = Item.maxStackSize;
                int total = quantity + originOfferSlot.quantity;
                if (total <= maxStack)
                {
                    quantity = total;
                    originOfferSlot.ClearSlot();
                }
                else
                {
                    quantity = maxStack;
                    originOfferSlot.quantity = total - maxStack;
                    originOfferSlot.RefreshUI();
                }
            }
            else
            {
                // swap w ofertach
                ItemBase tmpItem = Item;
                int tmpQty = quantity;
                Item = originOfferSlot.Item;
                quantity = originOfferSlot.quantity;
                originOfferSlot.Item = tmpItem;
                originOfferSlot.quantity = tmpQty;
            }

            OnItemChanged?.Invoke();
            originOfferSlot.OnItemChanged?.Invoke();

            RefreshUI();
            originOfferSlot.RefreshUI();
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
