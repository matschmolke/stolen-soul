using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private float fadeDuration = 2.4f;

    public void FadeToBlack()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;

        yield return new WaitForSeconds(1f);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndScene");
    }
}
