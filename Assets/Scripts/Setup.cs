using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SetupScript", menuName = "ScriptableObject/Setup")]
public class Setup : ScriptableObject
{
    #region Variables

    [Range(0,3)] public int lives;
    public int highScore;
    
    #endregion
}
