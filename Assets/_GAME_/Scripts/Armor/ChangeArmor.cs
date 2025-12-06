using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ChangeArmor : MonoBehaviour
{
    private SpriteLibrary spriteLibrary;

    [Header("Default sprite armor")]
    [SerializeField] SpriteLibraryAsset defaultArmorLibrary;

    private Image invImage;

    private void Awake()
    {
        spriteLibrary = GetComponent<SpriteLibrary>();

        TryFindInventoryImage();

        spriteLibrary.spriteLibraryAsset = defaultArmorLibrary;
        UpdateInventoryImage(defaultArmorLibrary);
    }

    public void UpdateArmor(ItemBase item)
    {
        SpriteLibraryAsset chosenLibrary;

        if (item is Equipment eq && eq.equipmentType == EquipmentType.Armor)
        {
            chosenLibrary = eq.armorSpriteLibrary;
        }
        else
        {
            chosenLibrary = defaultArmorLibrary;
        }
        
        spriteLibrary.spriteLibraryAsset = chosenLibrary;

        TryFindInventoryImage();

        UpdateInventoryImage(chosenLibrary);
    }

    private void UpdateInventoryImage(SpriteLibraryAsset library)
    {

        if (invImage == null || library == null)
            return;

        Sprite previewSprite = library.GetSprite("IdleDown", "Idle_0");

        if (previewSprite != null)
            invImage.sprite = previewSprite;
    }

    private void TryFindInventoryImage()
    {
        if (invImage != null)
            return;

        GameObject img = GameObject.FindGameObjectWithTag("InvPlayerImg");

        if (img == null)
            return;

        invImage = img.GetComponent<Image>();

        if (invImage == null)
            return;
    }

}
