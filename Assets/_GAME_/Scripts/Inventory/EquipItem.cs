using UnityEngine;

public class EquipItem : MonoBehaviour
{
    private PlayerStats playerStats;
    private ItemBase previousItem = null;
    private ItemSlot slot;

    private ChangeArmor armorChanger;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        armorChanger = player.GetComponent<ChangeArmor>();

        slot = transform.GetComponent<ItemSlot>();

        slot.OnItemChanged += HandleEquip;
    }

    private void HandleEquip(ItemBase item)
    {
        if(item != null && item.itemType != ItemType.Equipment) return;

        if(item == null && previousItem != null)
        {
            UnEquip(previousItem);
            armorChanger.UpdateArmor(item);
            previousItem = null;
            return;
        }

        if(item != null)
        {
            if(previousItem != null) UnEquip(previousItem);
            Equip(item);
            armorChanger.UpdateArmor(item);
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
