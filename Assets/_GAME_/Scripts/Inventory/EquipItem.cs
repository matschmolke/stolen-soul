using UnityEngine;

public class EquipItem : MonoBehaviour
{
    private PlayerStats1 playerStats;
    private ItemBase previousItem = null;
    private ItemSlot1 slot;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats1>();

        slot = transform.GetComponent<ItemSlot1>();

        slot.OnItemChanged += HandleEquip;
    }

    private void HandleEquip(ItemBase item)
    {
        if(item != null && item.itemType != ItemType.Equipment) return;

        if(item == null && previousItem != null)
        {
            UnEquip(previousItem);
            previousItem = null;
            return;
        }

        if(item != null)
        {
            if(previousItem != null) UnEquip(previousItem);
            Equip(item);
            previousItem = item;
        }
    }

    private void Equip(ItemBase item)
    {
        if(item is Equipment eq)
        {
            playerStats.Attack += eq.attackDmg;
            playerStats.Defence += eq.armorValue;

            if(eq.effects != null)
            {
                foreach(var effect in eq.effects)
                {
                    effect.Apply(playerStats);
                }
            }
        }
    }

    private void UnEquip(ItemBase item)
    {
        if (item is Equipment eq)
        {
            playerStats.Attack -= eq.attackDmg;
            playerStats.Defence -= eq.armorValue;

            if (eq.effects != null)
            {
                foreach (var effect in eq.effects)
                {
                    effect.Remove(playerStats);
                }
            }
        }
    }

}
