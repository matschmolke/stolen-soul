using UnityEngine;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public GameObject InventoryMenu;
    public GameObject spellsHUD;
    public ItemSlot[] itemSlot;
    
    private bool menuActivated;

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

    void Start()
    {
        if (spellsHUD == null)
        {
            spellsHUD = GameObject.Find("SpellsHUD");
        }

        if (spellsHUD != null)
        {
            spellsHUD.SetActive(!InventoryMenu.activeSelf);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (menuActivated)
            {
                Time.timeScale = 1;
                InventoryMenu.SetActive(false);

                if (spellsHUD == null)
                    spellsHUD = GameObject.Find("SpellsHUD");

                if (spellsHUD != null)
                    spellsHUD.SetActive(true);

                menuActivated = false;
            }
            else
            {
                Time.timeScale = 0;
                InventoryMenu.SetActive(true);

                if (spellsHUD == null)
                    spellsHUD = GameObject.Find("SpellsHUD");

                if (spellsHUD != null)
                    spellsHUD.SetActive(false);

                menuActivated = true;
            }
        }
    }


    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        Debug.Log("itemName = " + itemName +  ", quantity = " + quantity);

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite);
                return;
            }
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
