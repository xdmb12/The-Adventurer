using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("AttackThePlayer");
        }
    }
}
