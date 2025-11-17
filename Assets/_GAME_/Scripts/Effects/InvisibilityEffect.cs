using UnityEngine;
using System.Collections;

public static class InvisibilityEffect
{
    public static void Apply(GameObject target, float duration)
    {
        Debug.Log("InvisibilityEffect: Apply");
        target.GetComponent<MonoBehaviour>().StartCoroutine(DoInvisibility(target, duration));
    }

    private static IEnumerator DoInvisibility(GameObject target, float duration)
    {
        Debug.Log("InvisibilityEffect: DoInvisibility");
        SpriteRenderer[] renderers = target.GetComponentsInChildren<SpriteRenderer>();
        Color[] originalColors = new Color[renderers.Length];

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<BreadcrumbTrail>().dropCrumbTrail = false;
        player.transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
            Color c = renderers[i].color;
            c.a = 0.5f;
            renderers[i].color = c;
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].color = originalColors[i];

        player.GetComponent<BreadcrumbTrail>().dropCrumbTrail = true;
        player.transform.GetChild(0).gameObject.SetActive(true);

        Debug.Log("Invisibility ended!");
    }
}