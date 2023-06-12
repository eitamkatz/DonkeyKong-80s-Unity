using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireBallMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] private Vector2 speed = Vector2.right;
    private const float GRAVITY_HIGH = 2f;
    private const int NEG_DIRECTION = -1;
    private const int MIN_RANDOM = 1;
    private const int MAX_RANDOM = 3;
    private const int CONDITION_RANDOM = 2;
    private Rigidbody2D _rigidbody;
    private Collider2D _ignoreGame;
    private bool _hitWall;
    private bool _whileClimbing;
    
    #endregion
    
    private void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody.velocity = speed;
        _hitWall = false;
    }

    private void Update()
    {
        if (_whileClimbing)
        {
            _rigidbody.velocity = Vector2.up;
        }
        else
        {
            _rigidbody.velocity = speed;
        }

        if (GameManager.Shared.destroyBarrels)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            FireBallPool.Shared.Release(this);
        }
        else
        {
            _ignoreGame = col.gameObject.GetComponent<Collider2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("donkeyKong"))
        {
            Destroy(gameObject);
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            speed *= -1;
            _hitWall = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ladder"))
        {
            if (Climb())
            {
                if (_ignoreGame)
                    Physics2D.IgnoreCollision(_ignoreGame,
                        gameObject.GetComponent<Collider2D>());
                _whileClimbing = true;
                _rigidbody.velocity = Vector2.up;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ladder"))
        {
            if (_whileClimbing)
            {
                if (!_hitWall)
                {
                    speed *= NEG_DIRECTION;
                }
                else
                {
                    _hitWall = false;
                }

                _whileClimbing = false;
            }

            _rigidbody.gravityScale = GRAVITY_HIGH;
        }
    }

    private bool Climb()
    {
        if (Random.Range(MIN_RANDOM, MAX_RANDOM) > CONDITION_RANDOM)
        {
            return true;
        }

        if (_hitWall)
        {
            return true;
        }

        return false;
    }
}