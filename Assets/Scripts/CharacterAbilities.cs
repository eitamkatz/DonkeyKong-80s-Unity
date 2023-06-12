using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterAbilities : MonoBehaviour
{
    #region Variables

    [SerializeField] private float gap = 0;
    [SerializeField] private AudioClip characterShoot;
    [SerializeField] private CharacterGround ground = default;
    private const float SHIELD_LOOP = 5f;
    private const float SHIELD_FINAL = 0.3f;
    private const int SHILD_MOVE = 7;
    private const int SHILD_RET = 8;
    private GameObject _child;
    private Animator _animator;
    private float _timer;
    private AudioSource _characterAudioSource;

    #endregion

    private void Start()
    {
        _timer = Time.time;
        _characterAudioSource = gameObject.GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - _timer > gap &&
            ground.IsGrounded)
        {
            _animator.SetTrigger("shoot");
            _characterAudioSource.clip = characterShoot;
            _characterAudioSource.Play(0);
            BulletsPool.Shared.Get();
            _timer = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("shield"))
        {
            StartCoroutine(Shield());
        }
    }

    private IEnumerator Shield()
    {
        _child = transform.GetChild(0).gameObject;
        _child.SetActive(true);
        gameObject.layer = SHILD_MOVE;
        yield return new WaitForSeconds(SHIELD_LOOP);
        _child.SetActive(false);
        StartCoroutine(TrueShield());
    }

    private IEnumerator FalseShield()
    {
        yield return new WaitForSeconds(SHIELD_FINAL);
        _child.SetActive(false);
        StartCoroutine(FinalFalseShield());
    }
    
    private IEnumerator TrueShield()
    {
        yield return new WaitForSeconds(SHIELD_FINAL);
        _child.SetActive(true);
        StartCoroutine(FalseShield());
    }
    
    private IEnumerator FinalFalseShield()
    {
        _child.SetActive(true);
        yield return new WaitForSeconds(SHIELD_FINAL);
        _child.SetActive(false);
        gameObject.layer = SHILD_RET;
    }
}