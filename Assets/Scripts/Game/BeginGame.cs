using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGame : MonoBehaviour
{
    bool Loaded = false;
    public Game game;

    private void Update()
    {
        if (!Loaded)
        {
            Loaded = true;
            List<Team> teams = new List<Team>();
            teams.Add(new Team("team1", Color.Lerp(Color.blue,Color.white,0.3f), TeamType.Player));
            teams.Add(new Team("ai1", Color.red));
            GetComponent<Game>().StartGame(teams, "map::2333::20|15|0::[]6|6|0[]12|7|0::000000000000000000000000000000000000000000000000000000000000000000000000000000001110000000000001110000000000001110000000000001111100000000001111100000000001111110000000001111100000000001111100000000001111000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000::map");
            
        }
    }
}
