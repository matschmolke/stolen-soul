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
        entranceAnim.SetBool("open",true);
        entranceColl.enabled = false;
    }

    public void EntranceClose()
    {
        entranceAnim.SetBool("open", false);
        entranceColl.enabled = true;
    }

}