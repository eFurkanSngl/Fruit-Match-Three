using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RestartButton : UIBTN
{
    protected override void OnClick()
    {
        Restart();
    }

    private void Restart()
    {
        GameUIEvents.GameUI?.Invoke();
    }
}