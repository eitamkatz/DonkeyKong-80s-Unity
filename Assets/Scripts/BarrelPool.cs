using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public sealed class BarrelPool : MonoBehaviour
{
    #region Variables

    [SerializeField] private BarrelsMovement template = default;
    private ObjectPool<BarrelsMovement> _pool;
    private const float X_POS = -1.5f;
    private const float Y_POS = 2.0f;
    private const float Z_POS = 0f;
    private Vector3 _startPosition;

    #endregion

    public static BarrelPool Shared { get; private set; }

    private void Awake()
    {
        Shared = this;
        _pool = new ObjectPool<BarrelsMovement>(Create, OnActivate,
            OnDeactivate);
        _startPosition = new Vector3(X_POS, Y_POS, Z_POS);
    }

    public BarrelsMovement Get()
    {
        return _pool.Get();
    }

    public void Release(BarrelsMovement barrel)
    {
        if (barrel)
            _pool.Release(barrel);
    }

    public void ReleaseAll()
    {
        _pool.Dispose();
    }

    private BarrelsMovement Create()
    {
        BarrelsMovement barrel = Instantiate(template);
        barrel.gameObject.SetActive(false);
        return barrel;
    }

    private void OnActivate(BarrelsMovement barrel)
    {
        barrel.gameObject.SetActive(true);
        barrel.transform.position = _startPosition;
        barrel.GetComponent<Rigidbody2D>().velocity = Vector2.right;
    }

    private void OnDeactivate(BarrelsMovement barrel)
    {
        barrel.gameObject.SetActive(false);
    }
}