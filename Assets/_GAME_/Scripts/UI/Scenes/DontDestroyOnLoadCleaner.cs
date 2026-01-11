using UnityEngine;
using UnityEngine.SceneManagement;

public static class DontDestroyOnLoadCleaner
{
    public static void Clear()
    {
        Scene persistentScene = GetDontDestroyOnLoadScene();
        if (!persistentScene.IsValid()) return;

        GameObject[] roots = persistentScene.GetRootGameObjects();

        foreach (GameObject obj in roots)
        {
            if (obj.GetComponent<SoundManager>() != null)
                continue;

            Object.Destroy(obj);
        }

    }

    private static Scene GetDontDestroyOnLoadScene()
    {
        GameObject temp = new GameObject("Temp");
        Object.DontDestroyOnLoad(temp);

        Scene scene = temp.scene;

        Object.Destroy(temp);
        return scene;
    }
}
