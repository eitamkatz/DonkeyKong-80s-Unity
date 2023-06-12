using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PaulinaAnimation : MonoBehaviour
{
    #region Variables

    [SerializeField] private Sprite heart;
    private Animator _paulinaAnimation;
    private SpriteRenderer _childRenderer;
    private bool _win;

    #endregion

    private void Start()
    {
        _paulinaAnimation = transform.GetComponent<Animator>();
        _paulinaAnimation.SetBool("end", false);
        _childRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!_win)
            _childRenderer.enabled =
                _paulinaAnimation.GetCurrentAnimatorStateInfo(0)
                    .IsName("Help");
        else
        {
            _childRenderer.enabled = true;
            _childRenderer.sprite = heart;
            StartCoroutine(EndScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _win = true;
            _paulinaAnimation.SetBool("end", true);
        }
    }

    private IEnumerator EndScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("End");
    }
}