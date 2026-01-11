using UnityEngine;
using UnityEngine.UI;

public class EntranceMechanic : MonoBehaviour
{
    private Animator entranceAnim;
    [SerializeField]
    private Collider2D entranceColl;

    private void Awake()
    {
        entranceAnim = GetComponent<Animator>();
        
    }
    
    public void EntranceOpen()
    {
        SoundManager.PlaySound(SoundType.GATE_OPEN);
        entranceAnim.SetBool("open",true);
    }

    public void EntranceClose()
    {
        SoundManager.PlaySound(SoundType.GATE_CLOSE);
        entranceAnim.SetBool("open", false);
    }

    public void EnableEntrance()
    {
        if (entranceColl.isActiveAndEnabled)
        {
            entranceColl.enabled = false;
        }
        else
        {
            entranceColl.enabled = true;
        }
    }
}