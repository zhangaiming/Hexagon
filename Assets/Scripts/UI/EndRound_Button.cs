using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRound_Button : MonoBehaviour
{
    public Game game;
    public void EndRound()
    {
        game.CurController.EndRound();
    }
}
