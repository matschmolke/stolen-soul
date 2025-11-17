using System.Collections;
using UnityEngine;

public class PressSpaceAnim : MonoBehaviour
{
    private Animator SpaceAnim;
    void Start()
    {
        SpaceAnim = GetComponent<Animator>();
        StartCoroutine(PlayAnimLoop());
    }

    IEnumerator PlayAnimLoop()
    {
        while (true) {
            SpaceAnim.SetTrigger("press");
            yield return new WaitForSeconds(2f);
        }
    }
}
