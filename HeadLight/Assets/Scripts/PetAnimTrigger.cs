using UnityEngine;

public class PetAnimTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isNearPlayer", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isNearPlayer", false);
        }
    }
}
