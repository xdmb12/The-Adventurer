using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<Player>().coins++;
        }
    }
}
