using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectExit : UIBTN
{
    protected override void OnClick()
    {
        OnLoadScene();
    }

    private void OnLoadScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
