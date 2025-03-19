using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : UIBTN
{
    protected override void OnClick()
    {
        GoToLevelSelect();
    }
    private void GoToLevelSelect()
    {
        SceneManager.LoadScene("Level-Select");
    }
}
