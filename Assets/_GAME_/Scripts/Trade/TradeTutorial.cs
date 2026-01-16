using UnityEngine;
using UnityEngine.UI;

public class TradeTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject TutorialWindow;

    private bool isOpen = false;
    private Image tutorialImage;

    private void Awake()
    {
        tutorialImage = GetComponent<Image>();
    }

    public void Open()
    {
        isOpen = !isOpen;

        tutorialImage.raycastTarget = isOpen;

        TutorialWindow.SetActive(isOpen);
    }
}
