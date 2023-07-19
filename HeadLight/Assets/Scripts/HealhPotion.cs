using UnityEngine;

public class HealhPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().health += 20;
            Destroy(gameObject);
        }
    }
}
