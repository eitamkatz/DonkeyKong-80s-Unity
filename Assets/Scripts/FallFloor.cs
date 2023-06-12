using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFloor : MonoBehaviour
{
    #region Variables

    private const int BLEARE_MAX = 1;
    private const int MOVE_BREAK = 10;
    private SpriteRenderer _spriteRenderer;
    private GameObject _child;
    private Vector3 _pos;
    private Vector3 _startPos;
    private Color _color;
    private bool _fall;

    #endregion

    private void Start()
    {
        _child = transform.GetChild(0).gameObject;
        _spriteRenderer = _child.GetComponent<SpriteRenderer>();
        _fall = false;
        _color = _spriteRenderer.color;
        _pos = _child.transform.position;
        _startPos = _pos;
    }

    private void Update()
    {
        if (_fall)
        {
            _pos.y = Mathf.MoveTowards(_pos.y, _pos.y - 10, Time.deltaTime);
            _child.transform.position = _pos;
            _color.a =
                Mathf.MoveTowards(_spriteRenderer.color.a, 0, Time.deltaTime);
            _spriteRenderer.color = _color;
            StartCoroutine(BackToPosition());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _fall = true;
        }
    }

    private IEnumerator BackToPosition()
    {
        yield return new WaitForSeconds(1);
        _color.a = BLEARE_MAX;
        _pos.y += MOVE_BREAK;
        _spriteRenderer.color = _color;
        _child.transform.position = _startPos;
        _fall = false;
    }
}