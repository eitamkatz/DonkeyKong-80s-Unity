using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    #region Variables

    [SerializeField] private Setup setup;
    private const int MAX_LIVES = 3;
    
    #endregion

    private void CreateScene()
    {
        setup.lives = MAX_LIVES;
        SceneManager.LoadScene("Level_1");
    }
}
