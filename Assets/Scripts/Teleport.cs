using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    #region Variables

    private const float DELAY_PORT = 0.3f;
    private const float X_POS = 10f;
    private const float Y_POS = -2.5f;
    private Vector3 _midPosition = new Vector3(X_POS, Y_POS, 0);
    private Animator _portalAnimator;
    private GameObject _child;
    private Animator _childAnimator;

    #endregion

    private void Start()
    {
        _portalAnimator = GetComponent<Animator>();
        _child = transform.GetChild(0).gameObject;
        _childAnimator = _child.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(DelayPort(col));
        }
    }

    private IEnumerator DelayPort(Collider2D col)
    {
        _childAnimator.SetTrigger("teleport");
        _portalAnimator.SetTrigger("teleport");
        col.transform.position = _midPosition;
        yield return new WaitForSeconds(DELAY_PORT);
        col.transform.position = _child.transform.position;
    }
}