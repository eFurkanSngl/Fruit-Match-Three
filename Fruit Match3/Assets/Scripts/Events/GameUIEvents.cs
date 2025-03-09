using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public static class GameUIEvents
{
    public static UnityAction GameUI;
    public static UnityAction OnPause;
    public static UnityAction OnResume;
    public static UnityAction<float> TimerUI;

}