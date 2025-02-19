using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public static class MatchesEvents
{
    public static UnityAction<Tile[,], int, int> FindMatchesEvent;
}
