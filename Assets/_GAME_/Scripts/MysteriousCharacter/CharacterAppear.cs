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

    private DialogueTrigger dialogue;
    private CameraController cam;

    private Rigidbody2D playerRb;
    private Animator playerAnim;

    public DialogueEndEventChannel dialogueEndEvent;

    void Start()
    {
        appearEffect = GetComponent<Animator>();
        character.enabled = false;

        cam = FindFirstObjectByType<CameraController>();
        cam.CutSceneCamera();

        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        playerAnim = GameObject.FindWithTag("Player").GetComponent<Animator>();

        if (playerRb && playerAnim == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && !hasAppeared)
        {
            playerRb.linearVelocity = Vector2.zero;

            playerAnim.SetFloat("speed", 0f);      
            playerAnim.SetFloat("yVelocity", -1f);

            FreezeGame.LockForDialogue();
            //FreezeScene();

            hasAppeared = true;
            SoundManager.PlaySound(SoundType.WHOOSH);
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
        SoundManager.PlaySound(SoundType.WHOOSH2);
        appearEffect.SetTrigger("appear");

        yield return new WaitForSeconds(0.3f);
        character.enabled = false;

        yield return new WaitForSeconds(2f);
        cam.Initialize();

        FreezeGame.UnlockAfterDialogue();
        //UnfreezeScene();
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

        dialogue = GetComponent<DialogueTrigger>();

        yield return new WaitForSeconds(3f);
        dialogue.StartDialogue();
    }

    public static void FreezeScene()
    {
        foreach (MonoBehaviour mb in FindObjectsByType<Movements>(FindObjectsSortMode.None))
        {
            if (mb is CharacterAppear) continue;
            mb.enabled = false;
        }
    }

    public static void UnfreezeScene()
    {
         foreach (MonoBehaviour mb in FindObjectsByType<Movements>(FindObjectsSortMode.None))
         {
             mb.enabled = true;
         }
    }

    private void HandleDialogueEnded()
    {
        StartCoroutine(Disappear());
    }


    private void OnEnable()
    {
        if (dialogueEndEvent != null)
            dialogueEndEvent.OnEventRaised += HandleDialogueEnded;
    }

    private void OnDisable()
    {
        if (dialogueEndEvent != null)
            dialogueEndEvent.OnEventRaised -= HandleDialogueEnded;
    }


}
