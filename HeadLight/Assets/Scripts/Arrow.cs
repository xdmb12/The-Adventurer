using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    public static int view = 1;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * speed * view, ForceMode2D.Impulse);
        transform.eulerAngles = new Vector3(0, 0, -45);
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !other.isTrigger)
        {
            if (other.TryGetComponent(out FlyingEnemy script))
            {
                script.TakeDamage(damage);
            }
            else
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if(!other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
