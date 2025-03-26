using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine.Events;
public static class GameEvent
{
    public static UnityAction<Tile> OnClickEvents;
    public static UnityAction<Tile> UnSelectsTile;
    public static UnityAction<Tile> SelectsTile;
    public static UnityAction ShuffleEvents;
}
