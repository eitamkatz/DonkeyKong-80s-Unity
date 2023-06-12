using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnrealLadder : MonoBehaviour
{
    #region Variables

    private SpriteRenderer _spriteRenderer;

    #endregion

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _spriteRenderer.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _spriteRenderer.enabled = true;
    }
}