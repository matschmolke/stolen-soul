using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance;

    public List<TradeSlot> playerInventorySlots;
    public List<TradeSlot> traderInventorySlots;

    public List<OfferSlot> playerOfferSlots;
    public List<OfferSlot> traderOfferSlots;

    public TMP_Text playerOfferValueDisplay;
    public TMP_Text traderOfferValueDisplay;

    public TMP_Text playerCurrencyAmount;
    public TMP_Text traderCurrencyAmount;

    public GameObject TradeWindow;

    public ItemBase CurrencyItem;

    private bool windowActivated;
    private List<TradeSlot> allTradeSlots = new List<TradeSlot>();
    private List<OfferSlot> allOfferSlots = new List<OfferSlot>();

    private int playerOfferValue;
    private int traderOfferValue;

    private PlayerInventory playerInventory;
    private TraderInventory traderInventory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < playerInventorySlots.Count; i++) 
        {
            playerInventorySlots[i].slotId = i;
        }

        for (int i = 0; i < traderInventorySlots.Count; i++)
        {
            traderInventorySlots[i].slotId = i;
        }

        playerInventory = PlayerInventory.Instance;
        traderInventory = TraderInventory.Instance;

        allTradeSlots.AddRange(playerInventorySlots);
        allTradeSlots.AddRange(traderInventorySlots);
        allOfferSlots.AddRange(playerOfferSlots);
        allOfferSlots.AddRange(traderOfferSlots);

        foreach (OfferSlot slot in playerOfferSlots)
        {
            slot.OnItemChanged += CalculatePlayerOfferValue;
        }

        foreach (OfferSlot slot in traderOfferSlots)
        {
            slot.OnItemChanged += CalculateTraderOfferValue;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && windowActivated)
        {
            ReturnAllItemsFromOffers();
            Time.timeScale = 1;
            TradeWindow.SetActive(false);
            windowActivated = false;
            if (ItemTooltip.Instance != null)
                ItemTooltip.Instance.HideTooltip();
        }
        else if (Input.GetKeyDown(KeyCode.B) && !windowActivated)
        {
            Time.timeScale = 0;
            TradeWindow.SetActive(true);
            windowActivated = true;
            RefreshUI();
        }
    }

    public void Trade()
    {
        if(playerOfferValue == traderOfferValue)
        {
            foreach (OfferSlot slot in playerOfferSlots) 
            {
                if(slot.Item != null)
                {
                    traderInventory.AddItem(slot.Item, slot.quantity);
                    slot.ClearSlot();
                }
            }

            foreach (OfferSlot slot in traderOfferSlots)
            {
                if (slot.Item != null)
                {
                    playerInventory.AddItem(slot.Item, slot.quantity);
                    slot.ClearSlot();
                }
            }
            RefreshUI();
        }
    }

    public void BalanceOffer()
    {
        bool traderPays = playerOfferValue > traderOfferValue;

        InventoryBase payerInventory = traderPays ? traderInventory : playerInventory;

        List<OfferSlot> payerOfferSlots = traderPays ? traderOfferSlots : playerOfferSlots;

        int offerDiff= traderPays ? 
            playerOfferValue - traderOfferValue : 
            traderOfferValue - playerOfferValue;

        int currencyTaken = payerInventory.RemoveItem(CurrencyItem, offerDiff);

        int amountRemaining = currencyTaken;

        foreach (var slot in payerOfferSlots)
        {
            if (amountRemaining <= 0) break;

            if (slot.Item == null)
            {
                int put = Mathf.Min(amountRemaining, CurrencyItem.maxStackSize);
                slot.Item = CurrencyItem;
                slot.quantity = put;

                amountRemaining -= put;
                continue;
            }

            if (slot.Item == CurrencyItem)
            {
                int space = CurrencyItem.maxStackSize - slot.quantity;
                if (space > 0)
                {
                    int put = Mathf.Min(space, amountRemaining);
                    slot.quantity += put;

                    amountRemaining -= put;
                }
            }
        }

        RefreshUI();
        CalculatePlayerOfferValue();
        CalculateTraderOfferValue();
    }

    private void CalculatePlayerOfferValue()
    {
        playerOfferValueDisplay.text = "0";
        playerOfferValue = 0;

        int value = 0;
        foreach(OfferSlot slot in playerOfferSlots)
        {
            if (slot.Item == null) continue;
            value += slot.Item.value * slot.quantity;
        }

        playerOfferValueDisplay.text = $"{value}";
        playerOfferValue = value;
    }

    private void CalculateTraderOfferValue()
    {
        traderOfferValueDisplay.text = "0";
        traderOfferValue = 0;

        int value = 0;
        foreach (OfferSlot slot in traderOfferSlots)
        {
            if (slot.Item == null) continue;
            value += slot.Item.value * slot.quantity;
        }

        traderOfferValueDisplay.text = $"{value}";
        traderOfferValue = value;
    }

    private void ReturnAllItemsFromOffers()
    {
        foreach (var slot in playerOfferSlots)
        {
            if (slot.Item == null) continue;

            playerInventory.AddItem(slot.Item, slot.quantity);
            slot.ClearSlot();
        }

        foreach (var slot in traderOfferSlots)
        {
            if (slot.Item == null) continue;

            traderInventory.AddItem(slot.Item, slot.quantity);
            slot.ClearSlot();
        }
    }

    private void DisplayCurrencyAmounts() 
    {
        int traderCurrency = traderInventory.GetQuantityOf(CurrencyItem);

        traderCurrencyAmount.text = $"{traderCurrency}";

        int playerCurrency = playerInventory.GetQuantityOf(CurrencyItem);

        playerCurrencyAmount.text = $"{playerCurrency}";
    }

    public void DeselectAllSlots()
    {
        foreach (TradeSlot slot in allTradeSlots)
        {
            slot.selectedShader.SetActive(false);
            slot.thisItemSelected = false;
        }

        foreach (OfferSlot slot in allOfferSlots)
        {
            slot.selectedShader.SetActive(false);
            slot.thisItemSelected = false;
        }
    }

    public void RefreshUI()
    {
        DisplayCurrencyAmounts();

        foreach (TradeSlot slot in allTradeSlots)
        {
            slot.RefreshUI();
        }

        foreach (OfferSlot slot in allOfferSlots)
        {
            slot.RefreshUI();
        }
    }
}
