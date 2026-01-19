using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    private ItemBase previousItem = null;
    private ItemSlot slot;

    private ChangeArmor armorChanger;
    private PlayerStats stats;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        armorChanger = player.GetComponent<ChangeArmor>();
        stats = PlayerStats.Instance;

        slot = transform.GetComponent<ItemSlot>();

        slot.OnItemChanged += HandleEquip;

        if (GameState.RestoreFromSave)
        {
            switch (slot.accaptableEquipmentTypes)
            {
                case EquipmentType.Weapon:
                    previousItem = GameState.LoadedData.equippedWeapon;
                    break;
                case EquipmentType.Armor:
                    previousItem = GameState.LoadedData.equippedArmor;
                    break;
            }
        }
    }

    private void HandleEquip(ItemBase item)
    {
        if(item != null && item.itemType != ItemType.Equipment) return;

        if(item == null && previousItem != null)
        {
            stats.UnEquip(previousItem);

            if ((slot.accaptableEquipmentTypes & EquipmentType.Armor) != 0)
            {
                SoundManager.PlaySound(SoundType.EQUIP_ARMOR);
                armorChanger.UpdateArmor(item);
            }
            else if ((slot.accaptableEquipmentTypes & EquipmentType.Weapon) != 0)
            {
                SoundManager.PlaySound(SoundType.EQUIP_SWORD);
            }

            previousItem = null;
            return;
        }

        if(item != null)
        {
            if(previousItem != null) stats.UnEquip(previousItem);
            stats.Equip(item);

            if ((slot.accaptableEquipmentTypes & EquipmentType.Armor) != 0)
            {
                SoundManager.PlaySound(SoundType.EQUIP_ARMOR);
                armorChanger.UpdateArmor(item);
            }
            else if ((slot.accaptableEquipmentTypes & EquipmentType.Weapon) != 0)
            {
                SoundManager.PlaySound(SoundType.EQUIP_SWORD);
            }

            previousItem = item;
        }
    }
}
