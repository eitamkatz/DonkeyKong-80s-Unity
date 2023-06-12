using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MagicBarrelsPool : MonoBehaviour
{
    #region Variables

    [SerializeField] private BarrelsMovement template = default;
    private ObjectPool<BarrelsMovement> _pool;
    private const float X_POS = -1.5f;
    private const float Y_POS = 2.0f;
    private Vector3 _startPosition;

    #endregion

    public static MagicBarrelsPool Shared { get; private set; }

    private void Awake()
    {
        Shared = this;
        _pool = new ObjectPool<BarrelsMovement>(Create, OnActivate,
            OnDeactivate);
        _startPosition = new Vector3(X_POS, Y_POS, 0);
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