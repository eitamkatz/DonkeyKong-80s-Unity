using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class BulletsPool : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject character;
    [SerializeField] private BulletsMovement template = default;
    private ObjectPool<BulletsMovement> _pool;

    #endregion

    public static BulletsPool Shared { get; private set; }

    private void Awake()
    {
        Shared = this;
        _pool = new ObjectPool<BulletsMovement>(Create, OnActivate,
            OnDeactivate);
    }

    public BulletsMovement Get()
    {
        return _pool.Get();
    }

    public void Release(BulletsMovement bullet)
    {
        if (bullet)
            _pool.Release(bullet);
    }

    private BulletsMovement Create()
    {
        BulletsMovement bullet = Instantiate(template);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void OnActivate(BulletsMovement bullet)
    {
        bullet.transform.position = character.transform.position;
        bullet.gameObject.SetActive(true);
    }

    private void OnDeactivate(BulletsMovement bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
