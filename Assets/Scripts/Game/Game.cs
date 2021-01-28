using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    /// <summary>
    /// 当开始一个新的轮回时调用
    /// </summary>
    public event Action NewRound;
    /// <summary>
    /// 当下一个队伍开始回合时调用
    /// |队伍ID
    /// </summary>
    public event Action<int> OnNextTeam;
    /// <summary>
    /// 当一场游戏结束时调用
    /// |结束的游戏
    /// </summary>
    public event Action<Game> OnGameEnd;
    /// <summary>
    /// 当一场游戏被中断时调用
    /// |被中断的游戏
    /// </summary>
    public event Action<Game> OnGameBreakOff;

    private Queue<Controller> controllers = new Queue<Controller>();
    private Queue<Controller> HaveActionControllers = new Queue<Controller>();



    private Controller CurrentController = null;
    public Controller CurController => CurrentController;

    private MapManager MapManager;
    private TeamManager TeamManager;

    public Game(List<Team> teams, string mapcode)
    {
        MapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        TeamManager = GameObject.Find("GameManager").GetComponent<TeamManager>();
        StartGame(teams, mapcode);
    }

    public void StartGame(List<Team> teams, string mapcode)
    {
        if (GameState.IsRunning)
        {
            Debug.LogError("Start game failed. You must end the last game before start a new one.");
            return;
        }
        
        InitGame(teams, mapcode);
        NewRound?.Invoke();
        NextRound();

        GameState.IsRunning = true;
    }
    public void OnControllerJoin(Controller controller)
    {

    }

    void InitGame(List<Team> teams, string mapcode)
    {
        GameState.InitGameState();
        CreateTeams(teams);
        CreateControllersQueue();
        LoadMap(mapcode);
        SetInitialResource();
    }
    void LoadMap(string mapcode)
    {
        MapManager.LoadMap(mapcode);
    }
    void SetInitialResource()
    {
        List<Vector3Int> SpawnPoints = MapManager.GetSpawnPoints();
        int teamNum = TeamManager.CurTeamNum;
        Debug.Log(teamNum);
        for(int i = 0; i < teamNum; i++)
        {
            int index = UnityEngine.Random.Range(0, SpawnPoints.Count);
            Vector3Int pos = SpawnPoints[index];
            MapManager.SetUnit("开拓者", pos, i);
            SpawnPoints.RemoveAt(index);
        }
    }
    void CreateTeams(List<Team> teams)
    {
        TeamManager.ClearTeams();
        foreach(var t in teams)
        {
            if(TeamManager.CreateTeam(t.TeamName, t.TeamType, t.TeamColor) == -1)
            {
                Debug.LogError("Create team failed.");
            }
        }
    }
    void CreateControllersQueue()
    {
        TeamManager tm = GameObject.Find("GameManager").GetComponent<TeamManager>();
        GameState.CurrentTeamNum = tm.CurTeamNum;
        List<Controller> temp = new List<Controller>();
        
        for(int i = 0; i < GameState.CurrentTeamNum; i++)
        {
            if(tm.GetTeamType(i) == TeamType.Computer)
            {
                AIController ai = new AIController(i, tm.GetTeamName(i), this);
                temp.Add(ai);
            }else if(tm.GetTeamType(i) == TeamType.Player)
            {
                PlayerController player = new PlayerController(i, tm.GetTeamName(i), this);
                temp.Add(player);
            }
        }
        while (temp.Count > 0)
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
            int t = UnityEngine.Random.Range(0, temp.Count - 1);
            controllers.Enqueue(temp[t]);
            temp.RemoveAt(t);
        }
    }

    void ATeamLost(int id)
    {
        GameState.CurrentTeamNum -= 1;
        GameState.LostTeam.Add(id);
        if(GameState.CurrentTeamNum <= 1)
        {
            EndGame();
        }
    }

    void NextTeam()
    {
        if(controllers.Count > 0)
        {
            CurController.OnEndRound -= OnControllerEndRound;
            Controller de = controllers.Dequeue();
            HaveActionControllers.Enqueue(de);
            CurrentController = de;

            if (GameState.LostTeam.Contains(de.TeamID))
            {
                NextTeam();
                return;
            }
            GameState.CurrentTeam = de.TeamID;
            CurController.OnEndRound += OnControllerEndRound;
            OnNextTeam?.Invoke(de.TeamID);
        }
        else
        {
            NextRound();
        }
    }

    void OnControllerEndRound(Controller controller)
    {
        if(controller == CurController)
        {
            NextTeam();
        }
    }

    void NextRound()
    {
        while (HaveActionControllers.Count > 0)
        {
            controllers.Enqueue(HaveActionControllers.Dequeue());
        }
        GameState.CurrentRound += 1;
        NewRound?.Invoke();
        NextTeam();
    }

    void EndGame()
    {
        GameState.IsRunning = false;
    }

    public void BreakOffGame()
    {
        if(CurController != null)
        {
            CurController.OnEndRound -= OnControllerEndRound;
        }
    }
}
