using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider slider;

    private void Start()
    {
        StartCoroutine(UpdateLoadText());
    }

    public void SetProgress(float value)
    {
        slider.value = Mathf.Clamp01(value);
    }

    private IEnumerator UpdateLoadText()
    {
        int dotCount = 1;

        while (true)
        {
            loadingText.text = "Loading" + new string('.', dotCount);
            dotCount = (dotCount % 3) + 1;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
