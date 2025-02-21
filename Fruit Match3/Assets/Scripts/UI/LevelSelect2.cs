using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect2 : UIBTN
{
    protected override void OnClick()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Level-2");
    }
}
