using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReGame : MonoBehaviour
{
    #region Variables

    private const int SENCE_SMOVE = 3;
    
    #endregion

    private void Start()
    {
        StartCoroutine(MoveSences());
    }
    
    private IEnumerator MoveSences()
    {
        yield return new WaitForSeconds(SENCE_SMOVE);
        SceneManager.LoadScene("Level_1");
    }
}
