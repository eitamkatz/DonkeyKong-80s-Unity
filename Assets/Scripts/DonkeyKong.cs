using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DonkeyKong : MonoBehaviour
{
    #region Variables

    [SerializeField] private int minGap = 3;
    [SerializeField] private int maxGap = 7;
    [SerializeField] private int magicBarrelsRat;
    [SerializeField] private GameObject firstBarrel;
    private const float THROW_ANIMATION_TIME = 1;
    private const float FIRST_BARREL_TIMER = 1f;
    private const float X_VEC = -2.45f;
    private const float Y_VEC = 2f;
    private const int SHOCK_TIMER = 7;
    private Animator _donkeyAnimator;
    private bool _flag = true;
    private int _count = 0;
    private Vector3 _startFirstPosition = new Vector3(X_VEC, Y_VEC, 0);

    #endregion

    private void Start()
    {
        _donkeyAnimator = transform.GetComponent<Animator>();
        StartCoroutine(FirstCreator());
    }

    private void Update()
    {
        _donkeyAnimator.SetBool("throw", false);
        _donkeyAnimator.SetBool("throwMagic", false);
        if (!GameManager.Shared.hammerMode && GameManager.Shared.inGame)
        {
            StartCoroutine(Creator());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Hammer"))
        {
            _donkeyAnimator.SetBool("shock", true);
            Destroy(col.gameObject);
            StartCoroutine(Shock());
        }
    }

    private IEnumerator Shock()
    {
        yield return new WaitForSeconds(SHOCK_TIMER);
        _donkeyAnimator.SetBool("shock", false);
    }

    private IEnumerator ThrowAnimation()
    {
        _count++;
        if (_count == magicBarrelsRat)
            _donkeyAnimator.SetBool("throwMagic", true);
        else
            _donkeyAnimator.SetBool("throw", true);
        yield return new WaitForSeconds(THROW_ANIMATION_TIME);
        if (_count == magicBarrelsRat)
        {
            MagicBarrelsPool.Shared.Get();
            _count = 0;
        }
        else
        {
            BarrelPool.Shared.Get();
        }
    }

    private IEnumerator Creator()
    {
        if (_flag)
        {
            _flag = false;
            yield return new WaitForSeconds(Random.Range(minGap, maxGap));
            StartCoroutine(ThrowAnimation());
            _flag = true;
        }
    }

    private IEnumerator FirstCreator()
    {
        _donkeyAnimator.SetBool("firstMagic", true);
        yield return new WaitForSeconds(FIRST_BARREL_TIMER);
        firstBarrel = Instantiate(firstBarrel, _startFirstPosition,
            Quaternion.identity);
        _donkeyAnimator.SetBool("firstMagic", false);
    }
}