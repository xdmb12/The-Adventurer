using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextSign : MonoBehaviour
{
    [SerializeField] private GameObject text;

    private void Start()
    {
        text.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StartCoroutine(HideTheText());
    }

    IEnumerator HideTheText()
    {
        yield return new WaitForSeconds(3);
        text.SetActive(false);
    }
}
