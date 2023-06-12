using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBarrel : MonoBehaviour
{
    #region Variables

    private const float GRAVITY_DOWN_LADDER = 0.5f;
    private const float INCREASE_SPEED = 1.5f;
    private const float DIE_CORUTINE = 2f;
    private Animator _barrelAnimator;
    private Collider2D _currentFloor;
    private Rigidbody2D _rigidbody;
    private bool _touchOnGround;
    private bool _die;

    #endregion

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _barrelAnimator = GetComponent<Animator>();
        _touchOnGround = false;
    }

    private void Update()
    {
        if (GameManager.Shared.destroyBarrels)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (_die)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else if (!_touchOnGround && transform.CompareTag("firstBarrel"))
        {
            _rigidbody.velocity = Vector2.down * INCREASE_SPEED;
            _barrelAnimator.SetBool("down", true);
            if (_currentFloor)
                Physics2D.IgnoreCollision(_currentFloor,
                    gameObject.GetComponent<Collider2D>());
            _rigidbody.gravityScale = GRAVITY_DOWN_LADDER;
        }
        else if (_touchOnGround)
        {
            _rigidbody.velocity = Vector2.left;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("firstground"))
        {
            _touchOnGround = true;
            _barrelAnimator.SetBool("down", false);
        }
        else
        {
            _currentFloor = col.collider;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet"))
        {
            _die = true;
            StartCoroutine(DieAnimationBarrel());
        }
    }

    private IEnumerator DieAnimationBarrel()
    {
        _barrelAnimator.SetTrigger("die");
        yield return new WaitForSeconds(DIE_CORUTINE);
        Destroy(gameObject);
    }
}