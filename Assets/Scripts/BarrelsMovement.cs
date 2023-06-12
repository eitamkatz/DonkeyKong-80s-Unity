using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BarrelsMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] private float barrelIncreaser = 2.7f;
    [SerializeField] private int levelNumber = 1;
    private const float GAP_TIME = 33f;
    private const float GRAVITY_DOWN_LADDER = 0.5f;
    private const float DECREASE_RADIUS = 0.1f;
    private const float INCREASE_RADIUS = 10f;
    private const float RESCALE_GRAVITY = 15f;
    private const float UP_GAP_HEIGTH = 0.1f;
    private const float DOWN_GAP_HEIGTH = -0.1f;
    private const float DIE_CORUTINE = 2f;
    private const float MAX_RANDOM = 2f;
    private const int DESTROY_BARREL_SCORE = 500;
    private const int RANDOM_MODULE = 4;
    private const int RANDOM_LIMIT = 3;
    private const int DIVIDE_BY = 2;
    private const int MAX_LEVEL = 5;
    private float _timer = 0f;
    private bool _touchTopLadder;
    private bool _touchOnGround;
    private bool _die;
    private GameObject _character;
    private Coroutine _downLadder;
    private Rigidbody2D _rigidbody;
    private Collider2D _currentFloor;
    private Animator _barrelAnimator;
    private Vector2 _barrelSpeed = Vector2.right;

    #endregion

    private void Start()
    {
        _barrelSpeed *= barrelIncreaser;
        _timer = Time.time;
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody.velocity = _barrelSpeed;
        _barrelAnimator = transform.GetComponent<Animator>();
        _character = GameManager.Shared.character;
    }

    private void Update()
    {
        if (levelNumber < MAX_LEVEL)
        {
            if (Time.time - _timer >= GAP_TIME)
            {
                levelNumber++;
                _timer = Time.time;
            }
        }

        if (_downLadder != null)
        {
            StopCoroutine(_downLadder);
            end_cor();
        }

        if (GameManager.Shared.destroyBarrels)
        {
            if (CompareTag("barrel"))
                BarrelPool.Shared.Release(this);
            else
                MagicBarrelsPool.Shared.Release(this);
        }
    }

    private void FixedUpdate()
    {
        if (((_touchOnGround && _downLadder == null) ||
             _rigidbody.velocity.magnitude == 0) && !_die)
            _rigidbody.velocity = _barrelSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("LadderTrigger"))
        {
            if (DownTheLadderAI())
            {
                _touchOnGround = false;
                StartCoroutine(BarrelGoDownTheLadder());
            }
        }
        else if (col.gameObject.CompareTag("Wall"))
        {
            _barrelSpeed = -_barrelSpeed;
        }

        if (col.CompareTag("bullet"))
        {
            if (!_die)
                GameManager.Shared.AddScore(DESTROY_BARREL_SCORE);
            if (_rigidbody)
                _rigidbody.velocity = Vector2.zero;
            _die = true;
            StartCoroutine(DieAnimationBarrel());
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground") ||
            col.gameObject.CompareTag("firstground"))
        {
            _touchOnGround = true;
        }
        else if (col.gameObject.CompareTag("Gate"))
        {
            _currentFloor = col.collider;
        }
    }

    private bool touchOnGround()
    {
        return _touchOnGround;
    }

    private IEnumerator BarrelGoDownTheLadder()
    {
        Physics2D.IgnoreCollision(_currentFloor,
            gameObject.GetComponent<Collider2D>());
        _rigidbody.velocity = Vector2.down;
        _rigidbody.gravityScale = GRAVITY_DOWN_LADDER;
        transform.GetComponent<CircleCollider2D>().radius *= DECREASE_RADIUS;

        _barrelSpeed = -_barrelSpeed;
        yield return new WaitUntil(touchOnGround);

        end_cor();
    }

    private IEnumerator DieAnimationBarrel()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        _barrelAnimator.SetTrigger("die");
        yield return new WaitForSeconds(DIE_CORUTINE);
        transform.GetChild(0).gameObject.SetActive(false);
        if (CompareTag("magicBarrel"))
            MagicBarrelsPool.Shared.Release(this);
        else
            BarrelPool.Shared.Release(this);
        _die = false;
    }

    private void end_cor()
    {
        _barrelAnimator.SetBool("down", false);
        transform.GetComponent<CircleCollider2D>().radius *= INCREASE_RADIUS;
        _rigidbody.gravityScale = RESCALE_GRAVITY;
    }

    private bool DownTheLadderAI()
    {
        float randomNum = 0;
        if (!GameManager.Shared.oilFire)
        {
            _barrelAnimator.SetBool("down", true);
            return true;
        }

        if (_character.transform.position.y >= transform.position.y)
        {
            return false;
        }

        randomNum = Random.Range(0, MAX_RANDOM);
        if ((randomNum % RANDOM_MODULE) >= ((levelNumber / DIVIDE_BY) + 1))
            return false;
        var gap = transform.position.x - _character.transform.position.x;
        if (DOWN_GAP_HEIGTH < gap || gap < UP_GAP_HEIGTH)
        {
            _barrelAnimator.SetBool("down", true);
            return true;
        }

        if (gap < 0 && Input.GetKey(KeyCode.LeftArrow))
        {
            _barrelAnimator.SetBool("down", true);
            return true;
        }

        if (gap > 0 && Input.GetKey(KeyCode.RightArrow))
        {
            _barrelAnimator.SetBool("down", true);
            return true;
        }

        return !(randomNum % RANDOM_MODULE <=
                 RANDOM_LIMIT); // approximately of 75%
    }
}