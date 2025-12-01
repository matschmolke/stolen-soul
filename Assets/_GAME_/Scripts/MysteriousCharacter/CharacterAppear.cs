using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterAppear : MonoBehaviour
{
    private Animator appearEffect;
    [SerializeField] private Animator characterAnim;
    [SerializeField] private SpriteRenderer character;

    private float walkSpeed = 2.5f;
    private bool isWalking = false;
    private bool hasAppeared = false;
    private bool dialogueStarted = false;

    [SerializeField] private Transform destinationPoint;
    [SerializeField] private LeverMechanic lever;

    private DialogueTransition firstDialogue;
    private CameraController cam;

    void Start()
    {
        appearEffect = GetComponent<Animator>();
        character.enabled = false;

        cam = FindFirstObjectByType<CameraController>();
    }

    void Update()
    {
        if (Input.anyKeyDown && !hasAppeared)
        {
            FreezeScene();

            hasAppeared = true;
            appearEffect.SetTrigger("appear");
        }

        if(isWalking)
        {
            CharacterWalking();
        }
    }

    public void CharacterWalking()
    {
        characterAnim.SetBool("walk", true);
        character.transform.position = Vector3.MoveTowards(
            character.transform.position,
            destinationPoint.position,
            walkSpeed * Time.deltaTime
        );

        if (Mathf.Abs(character.transform.position.x - destinationPoint.position.x) < 0.05f)
        {
            StopWalking();
        }
    }

    public void StartWalking()
    {
        if (dialogueStarted) return;
        character.enabled = true;

        StartCoroutine(CharacterWait(1f));
    }

    private void StopWalking()
    {
        isWalking = false;
        characterAnim.SetBool("walk", false);

        lever.TurnOnLever();

        StartCoroutine(StartDialogue());
    }


    private IEnumerator CharacterWait(float wait)
    {
        yield return new WaitForSeconds(wait);

        characterAnim.SetTrigger("look");

        yield return new WaitForSeconds(3f);

        isWalking = true;
    }
        
    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2f);
        appearEffect.SetTrigger("appear");

        yield return new WaitForSeconds(0.3f);
        character.enabled = false;

        yield return new WaitForSeconds(2f);
        cam.Initialize();

        UnfreezeScene();
    }

    private IEnumerator StartDialogue()
    {
        if (dialogueStarted) yield break;

        dialogueStarted = true;

        AsyncOperation loading = SceneManager.LoadSceneAsync("GuideScene", LoadSceneMode.Additive);

        while (!loading.isDone)
        {
            yield return null;
        }

        firstDialogue = FindFirstObjectByType<DialogueTransition>();

        yield return new WaitForSeconds(3f);
        firstDialogue.firstDialogue = true;
    }

    public static void FreezeScene()
    {
        foreach (MonoBehaviour mb in FindObjectsOfType<Movements>())
        {
            if (mb is CharacterAppear) continue;
            mb.enabled = false;
        }
    }

    public static void UnfreezeScene()
    {
         foreach (MonoBehaviour mb in FindObjectsOfType<Movements>())
         {
             mb.enabled = true;
         }
    }



}
