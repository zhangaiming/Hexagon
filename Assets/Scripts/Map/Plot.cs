using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlotType
{
    None = 0,
    Flat = 1,
    Void = 2
}

public class Plot
{
    public int x, y;
    public PlotType type;
    public Plot(int _x, int _y, PlotType _type)
    {
        x = _x;
        y = _y;
        type = _type;
    }
}