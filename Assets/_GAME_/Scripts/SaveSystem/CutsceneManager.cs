using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public GameObject obj;

    private void Start()
    {
        ShowCutsceneObject(obj);
    }
    public static void ShowCutsceneObject(GameObject obj)
    {
        if (SaveLoad.restoreInventory == true)
        {
            obj.SetActive(false);

            var cam = FindFirstObjectByType<CameraController>();
            cam.Initialize();
        }
        else
        {
            obj.SetActive(true);
        }
    }
}
