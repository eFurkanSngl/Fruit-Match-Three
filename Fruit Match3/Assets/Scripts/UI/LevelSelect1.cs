using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LevelSelect1 : UIBTN
{
    protected override void OnClick()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Level-1");
    }
}
