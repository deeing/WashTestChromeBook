using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Symbol", menuName = "Wash/CaligraphySymbol", order = 1)]
public class CaligraphySymbol : ScriptableObject
{
    public List<CaligraphyConnection> symbolConnections;
}
