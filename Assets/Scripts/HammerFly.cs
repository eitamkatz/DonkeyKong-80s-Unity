using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerFly : MonoBehaviour
{
    #region Variables
    
    private const int SWING_HAMMER = 500;
    private const float X_POS = -2.4f;
    private const float Y_POS = 2.6f;
    private bool _startHammer;
    private Vector3 _donkeyHead = new Vector3(X_POS, Y_POS, 0);
    private Rigidbody2D _rigidbody;

    #endregion

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_startHammer)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                _donkeyHead, Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _startHammer = true;
            _rigidbody.angularVelocity = SWING_HAMMER;
        }
    }
}