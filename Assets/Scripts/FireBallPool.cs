using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireBallPool : MonoBehaviour
{
    #region Variables

    [SerializeField] private FireBallMovement template = default;
    private ObjectPool<FireBallMovement> _pool;
    private Vector3 _startPosition;

    #endregion

    public static FireBallPool Shared { get; private set; }

    private void Awake()
    {
        Shared = this;
        _pool = new ObjectPool<FireBallMovement>(Create, OnActivate,
            OnDeactivate);
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    public FireBallMovement Get()
    {
        return _pool.Get();
    }

    public void Release(FireBallMovement fireBall)
    {
        if (fireBall)
            _pool.Release(fireBall);
    }

    private FireBallMovement Create()
    {
        FireBallMovement fireBall = Instantiate(template);
        fireBall.gameObject.SetActive(false);
        return fireBall;
    }

    private void OnActivate(FireBallMovement fireBall)
    {
        fireBall.gameObject.SetActive(true);
        fireBall.transform.position = _startPosition;
        fireBall.GetComponent<Rigidbody2D>().velocity = Vector2.right;
    }

    private void OnDeactivate(FireBallMovement fireBall)
    {
        fireBall.gameObject.SetActive(false);
    }
}
