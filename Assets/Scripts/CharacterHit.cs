using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHit : MonoBehaviour
{
    #region Variables

    private Animator _characterAnimator;
    private bool _die;
    private Vector3 _pos;

    #endregion

    private void Start()
    {
        _characterAnimator = GetComponent<Animator>();
        _characterAnimator.SetBool("die", false);
        _die = false;
    }

    private void Update()
    {
        if (_die && GameManager.Shared.inGame)
            transform.position = _pos;
        else if (!GameManager.Shared.inGame)
        {
            _die = false;
            _characterAnimator.SetBool("die", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("barrel") ||
            col.gameObject.CompareTag("magicBarrel") ||
            col.gameObject.CompareTag("fireBall") ||
            col.gameObject.CompareTag("firstBarrel"))
        {
            _pos = transform.position;
            _characterAnimator.SetBool("die", true);
            GameManager.Shared.DecreaseLive();
            _die = true;
        }
    }
}