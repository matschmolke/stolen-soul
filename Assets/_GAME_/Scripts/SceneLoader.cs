using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        //loads all scenes together
        SceneManager.LoadSceneAsync("TutorialDung", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("InventoryScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);
<<<<<<< Updated upstream
=======
        SceneManager.LoadSceneAsync("mod1", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("GuideScene", LoadSceneMode.Additive); //comment this
>>>>>>> Stashed changes
    }
}
