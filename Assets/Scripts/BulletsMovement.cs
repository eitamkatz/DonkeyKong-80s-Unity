using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsMovement : MonoBehaviour
{
    #region Variables

    private Vector3 _position;
    private float _endPos = 10;

    #endregion

    private void Start()
    {
        _endPos += _position.y;
    }

    private void Update()
    {
        _position.y = Mathf.MoveTowards(_position.y, _endPos, Time.deltaTime);
        transform.position = _position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("barrel"))
        {
            BulletsPool.Shared.Release(this);
        }
        else if (col.transform.CompareTag("firstBarrel"))
        {
            BulletsPool.Shared.Release(this);
        }
        else if (col.transform.CompareTag("magicBarrel"))
        {
            BulletsPool.Shared.Release(this);
        }
        else if (col.transform.CompareTag("HammerDestroy"))
        {
            BulletsPool.Shared.Release(this);
        }
    }

    private void OnEnable()
    {
        _position = transform.position;
    }
}