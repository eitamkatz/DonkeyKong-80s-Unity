using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterJump : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private AudioClip characterJump;
    [SerializeField] private CharacterGround ground = default;
    [SerializeField] private LayerMask isBarrel;
    [SerializeField] private float jumpSpeed = 3.4f;
    private const float DISTANCE = 0.6f;
    private const int JUMP_SCORE = 100;
    private GameObject _childLabal;
    private Animator _marioAnimation;
    private AudioSource _characterAudioSource;
    private Rigidbody2D _rigidbody;
    private Vector3 _scorePos;
    private float _inputHor;
    private bool _hasInput;
    private bool _firstJump;

    #endregion

    private void Start()
    {
        _marioAnimation = gameObject.GetComponent<Animator>();
        _characterAudioSource = gameObject.GetComponent<AudioSource>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _childLabal = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_marioAnimation.GetBool("die"))
        {
            _characterAudioSource.clip = characterJump;
            _characterAudioSource.Play(0);
            if (ground.IsGrounded)
                Perform();
        }
        if (Time.time > 1)
        {
            _marioAnimation.SetBool("jump", !ground.IsGrounded);
            _rigidbody.gravityScale = 1;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D jumpOverBarrel = Physics2D.Raycast(transform.position,
            Vector2.down, DISTANCE, isBarrel);
        if (jumpOverBarrel.collider != null && !jumpOverBarrel.collider.CompareTag("Player"))
        {
            if (_firstJump)
            {
                _childLabal.SetActive(true);
                GameManager.Shared.AddScore(JUMP_SCORE);
                _firstJump = false;
            }
        }
        else
        {
            
            _childLabal.SetActive(false);
            _firstJump = true;
        }
    }

    private void Perform()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.y = jumpSpeed;
        _inputHor = Input.GetAxisRaw("Horizontal");
        _hasInput = !Mathf.Approximately(_inputHor, 0);
        if (_hasInput)
            velocity.x = _inputHor;
        else
            velocity.x = 0;
        _rigidbody.velocity = velocity;
    }
}