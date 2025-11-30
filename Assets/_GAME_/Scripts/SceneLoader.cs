using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync("TutorialDung", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("InventoryScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("mod1", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("EnemyScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("TradeScene", LoadSceneMode.Additive);
        
    }

    //to modify later
    public void LoadLevel(string currentLevel, string currentEnemy, string nextLevel, string nextEnemy)
    {
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        StartCoroutine(LoadLevelRoutine(currentLevel, currentEnemy, nextLevel, nextEnemy));
    }

    private IEnumerator LoadLevelRoutine(string currentLevel, string currentEnemy, string nextLevel, string nextEnemy)
    {
        SliderController sliderValue = gameObject.GetComponent<SliderController>();

        SceneManager.UnloadSceneAsync(currentLevel);
        SceneManager.UnloadSceneAsync(currentEnemy);

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);
        AsyncOperation asyncLoadEnemy = SceneManager.LoadSceneAsync(nextEnemy, LoadSceneMode.Additive);

        asyncLoadLevel.allowSceneActivation = false;
        asyncLoadEnemy.allowSceneActivation = false;

        while (asyncLoadLevel.progress < 0.9f && asyncLoadEnemy.progress < 0.9f)
        {
            float progess = (asyncLoadLevel.progress + asyncLoadEnemy.progress) / 2f;
            sliderValue.SetProgress(progess);

            yield return null;
        }

        asyncLoadLevel.allowSceneActivation = true;
        asyncLoadEnemy.allowSceneActivation = true;

        while (!asyncLoadLevel.isDone || !asyncLoadEnemy.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("LoadingScene");
    }
}
