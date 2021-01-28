using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public AIController(int _teamid, string _name, Game _game) : base(_teamid, _name, _game)
    {

    }

    protected override void OnRoundStart()
    {
        EndRound();
    }

    protected override void OnRoundEnd()
    {
        Debug.Log("bgin");
    }
}
