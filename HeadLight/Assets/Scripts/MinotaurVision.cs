using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurVision : MonoBehaviour
{
    public GameObject enemyParent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyParent.GetComponent<Enemy>().isWaiting = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyParent.GetComponent<Enemy>().isWaiting = false;
        }
    }
}
