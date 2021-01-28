using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public PlayerController(int _teamid, string _name, Game _game):base(_teamid, _name, _game)
    {

    }

    protected override void OnRoundEnd()
    {
        Camera.main.GetComponent<CursorControl>().CanSelect = false;
    }

    protected override void OnRoundStart()
    {
        Camera.main.GetComponent<CursorControl>().CanSelect = true;
        Camera.main.GetComponent<CursorControl>().CurrentController = this;
    }
}
