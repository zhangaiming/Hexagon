using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : MonoBehaviour
{
    Game HostingGame = null;
    public bool CreateGame(List<Team> teams, string mapcode)
    {
        if(HostingGame == null)
        {
            HostingGame = new Game(teams, mapcode);
            HostingGame.OnGameEnd += OnHostingGameEnd;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EndGame()
    {
        if(HostingGame != null)
        {
            HostingGame.OnGameEnd -= OnHostingGameEnd;
            HostingGame = null;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    void OnHostingGameEnd(Game game)
    {
        HostingGame = null;
    }
}
