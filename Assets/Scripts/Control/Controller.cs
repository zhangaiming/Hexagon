using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller
{
    /// <summary>
    /// 当控制器结束回合时调用
    /// </summary>
    public event Action<Controller> OnEndRound;
    /// <summary>
    /// 当选择的单位发生改变时调用
    /// </summary>
    public event Action<Controller> OnSelectedUnitChanged;
    /// <summary>
    /// 当控制器在游戏中失败时调用
    /// </summary>
    public event Action<Controller> OnLostGame;

    private readonly int teamid;
    public int TeamID => teamid;

    private readonly string name;
    public string Name => name;

    private bool inRound;
    public bool InRound => inRound;

    private Game game;
    protected Game Game => game;

    private UnitBase selectedUnit = null;
    public UnitBase SelectedUnit
    {
        get => selectedUnit;

        set
        {
            UnitBase old = selectedUnit;
            if(old != null)
            {
                old.Unselect();
            }
            selectedUnit = value;
            if(selectedUnit != null)
            {
                selectedUnit.Select();
            }
            if(selectedUnit != old)
            {
                OnSelectedUnitChanged?.Invoke(this);
            }
        }
    }

    void JoinGame(Game _game)
    {
        if(game == null)
        {
            game = _game;
            game.OnNextTeam += OnGameNextTeam;
            game.OnControllerJoin(this);
        }
    }

    void QuitGame()
    {
        if(game != null)
        {
            game.OnNextTeam -= OnGameNextTeam;
        }
    }

    void OnGameNextTeam(int _teamid)
    {
        if(_teamid == TeamID)
        {
            StartRound();
        }
    }

    public Controller(int _teamid, string _name, Game _game)
    {
        teamid = _teamid;
        name = _name;
        inRound = false;
        JoinGame(_game);
    }

    void StartRound()
    {
        if (!inRound)
        {
            GameEvent.ActiveOnTeamBeginRound(this);
            inRound = true;
            OnRoundStart();
        }
    }

    /// <summary>
    /// 调用此方法以结束当前回合
    /// </summary>
    public void EndRound()
    {
        if (inRound)
        {
            SelectedUnit = null;
            GameEvent.ActiveOnTeamEndRound(this);
            OnRoundEnd();
            inRound = false;
            OnEndRound?.Invoke(this);
        }
    }

    public void LostGame()
    {

    }

    #region Abstract function for child
    /// <summary>
    /// 当控制器开始回合时调用
    /// </summary>
    protected abstract void OnRoundStart();
    /// <summary>
    /// 当控制器结束回合时调用
    /// </summary>
    protected abstract void OnRoundEnd();
    #endregion
}
