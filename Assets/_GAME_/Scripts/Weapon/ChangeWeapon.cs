using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ChangeWeapon : MonoBehaviour
{
    private SpriteLibrary spriteLibrary;

    [Header("Default sprite weapon")]
    [SerializeField] SpriteLibraryAsset defaultWeaponLibrary;

    private Image invImage;

    private void Awake()
    {
        spriteLibrary = GetComponent<SpriteLibrary>();

        TryFindInventoryImage();

        spriteLibrary.spriteLibraryAsset = defaultWeaponLibrary;
        UpdateInventoryImage(defaultWeaponLibrary);
    }

    public void UpdateWeapon(ItemBase item)
    {
        SpriteLibraryAsset chosenLibrary;

        if (item is Equipment eq && eq.equipmentType == EquipmentType.Weapon)
        {
            chosenLibrary = eq.weaponSpriteLibrary;
        }
        else
        {
            chosenLibrary = defaultWeaponLibrary;
        }

        spriteLibrary.spriteLibraryAsset = chosenLibrary;

        TryFindInventoryImage();

        UpdateInventoryImage(chosenLibrary);
    }

    private void UpdateInventoryImage(SpriteLibraryAsset library)
    {

        if (invImage == null || library == null)
            return;

        Sprite previewSprite = library.GetSprite("IdleDown", "Idle_Sword_0");

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
