using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMove : MonoBehaviour
{
    #region Variables

    [SerializeField] private CharacterGround ground = default;
    [SerializeField] private float hammerTime = 10f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private LayerMask isLadder;
    [SerializeField] private float climbingSpeed = 1f;
    [SerializeField] private AudioClip characterWalk;
    private const float DISTANCE = 0.05f;
    private float _targetSpeed;
    private float _upDownSpeed;
    private float _inputHor;
    private float _inputVer;
    private float _gravityPlayer;
    private bool _climbing;
    private bool _wantToClimbUp;
    private bool _hasInput;
    private bool _wantToClimbDown;
    private bool _moveHammer;
    private bool _fall;
    private Vector3 _scale;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Collider2D _ground;
    private Animator _marioAnimation;
    private AudioSource _characterAudioSource;

    #endregion

    private void Start()
    {
        _marioAnimation = gameObject.GetComponent<Animator>();
        _characterAudioSource = gameObject.GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _gravityPlayer = _rigidbody.gravityScale;
    }

    private void Update()
    {
        if (GameManager.Shared.inGame && ground.IsGrounded &&
            !_marioAnimation.GetBool("die"))
        {
            _inputHor = Input.GetAxisRaw("Horizontal");
            _hasInput = !Mathf.Approximately(_inputHor, 0);
        }

        _scale = transform.localScale;
        if (_inputHor == 0)
        {
            _marioAnimation.SetBool("walk", false);
        }
        else if (Mathf.Sign(_inputHor) > 0)
        {
            _scale.x = -Mathf.Abs(_scale.x);
            transform.localScale = _scale;
            if (!_marioAnimation.GetBool("jump"))
                _marioAnimation.SetBool("walk", true);
            else
                _marioAnimation.SetBool("walk", false);
        }
        else
        {
            _scale.x = Mathf.Abs(_scale.x);
            transform.localScale = _scale;

            if (!_marioAnimation.GetBool("jump"))
                _marioAnimation.SetBool("walk", true);
            else
                _marioAnimation.SetBool("walk", false);
        }

        _targetSpeed = maxSpeed * _inputHor;
        if (_climbing)
        {
            _inputVer = Input.GetAxisRaw("Vertical");
        }
        else
        {
            _inputVer = _rigidbody.velocity.y;
        }

        _wantToClimbUp = Input.GetKey(KeyCode.UpArrow);
        _wantToClimbDown = Input.GetKey(KeyCode.DownArrow);

        if ((_hasInput || _wantToClimbUp || _wantToClimbDown) &&
            !_characterAudioSource.isPlaying)
        {
            _characterAudioSource.clip = characterWalk;
            _characterAudioSource.Play(0);
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = _targetSpeed;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,
            Vector2.up, DISTANCE, isLadder);
        RaycastHit2D hitInfoD = Physics2D.Raycast(transform.position,
            Vector2.down, DISTANCE, isLadder);

        if (_climbing &&
            (hitInfoD.collider != null || hitInfo.collider != null))
        {
            if (_ground)
            {
                Physics2D.IgnoreCollision(_ground,
                    _collider, true);
            }

            _marioAnimation.SetBool("climbing", true);
            velocity.y = _inputVer * climbingSpeed;
            _rigidbody.gravityScale = 0;
        }
        else if (_climbing && !_wantToClimbUp)
        {
            _rigidbody.gravityScale = _gravityPlayer;
        }
        else
        {
            _marioAnimation.SetBool("climbing", false);
            _rigidbody.gravityScale = _gravityPlayer;
        }

        if (_fall)
        {
            if (_ground)
            {
                Physics2D.IgnoreCollision(_collider, _ground);
            }
        }

        if (!GameManager.Shared.inGame)
        {
            velocity = Vector2.zero;
        }

        _rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Gate"))
        {
            _ground = col.collider;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Hammer"))
        {
            StartCoroutine(HammerMode());
        }

        if (col.CompareTag("ladder"))
        {
            if (_wantToClimbUp || _wantToClimbDown)
                _climbing = true;
        }

        if (col.CompareTag("Fall"))
        {
            _fall = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ladder"))
        {
            if (_wantToClimbUp || _wantToClimbDown)
                _climbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("ladder"))
        {
            _marioAnimation.SetBool("climbing", false);
            _marioAnimation.SetBool("walk", true);
            if (_ground)
                Physics2D.IgnoreCollision(_ground, _collider, false);
            _climbing = false;
        }

        if (other.CompareTag("Fall"))
        {
            _fall = false;
        }
    }

    private IEnumerator HammerMode()
    {
        GameManager.Shared.hammerMode = true;
        yield return new WaitForSeconds(hammerTime);
        GameManager.Shared.hammerMode = false;
    }
}