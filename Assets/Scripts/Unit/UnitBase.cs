using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class UnitBase
{
    /// <summary>
    /// 单位位置改变时被调用
    /// |发生改变的单位
    /// |新位置
    /// |旧位置
    /// </summary>
    public event Action<UnitBase, Vector3Int, Vector3Int> OnPositionChanged;
    /// <summary>
    /// 单位被构造时调用
    /// |被构造的单位
    /// </summary>
    public event Action<UnitBase> OnConstructed;
    /// <summary>
    /// 单位被销毁前调用(仍存在于地图当中)
    /// |被销毁的单位
    /// </summary>
    public event Action<UnitBase> ReadyToBeDestroyed;
    /// <summary>
    /// 单位被销毁后调用(不存在于地图中,但仍保留某些信息)
    /// |被销毁的单位
    /// </summary>
    public event Action<UnitBase> OnDestroyed;
    /// <summary>
    /// 单位队伍发生改变时被调用
    /// |发生改变的单位
    /// |改变之前所在队伍ID
    /// </summary>
    public event Action<UnitBase, int> TeamChanged;
    /// <summary>
    /// 单位生命值发生改变时调用
    /// |发生改变的单位
    /// |当前生命值
    /// </summary>
    public event Action<UnitBase, int> OnHealthChanged;
    /// <summary>
    /// 单位行动力发生改变时调用
    /// |发生改变的单位
    /// |当前行动力
    /// </summary>
    public event Action<UnitBase, int> OnActionTimeChanged;

    private Vector3Int _position;
    public Vector3Int Position { 
        get => _position;
        set 
        { 
            Vector3Int temp = _position; 
            _position = value; 
            OnPositionChanged?.Invoke(this, _position, temp); 
            GameEvent.ActiveOnUnitMove(this); 
        } 
    }
    private readonly string name;
    public string Name => name;
    private int teamID;
    public int TeamID { get => teamID;
        protected set { TeamChanged?.Invoke(this, teamID); teamID = value; } }

    private int health;
    public int Health { get => health;
        set { health = Mathf.Min(MaxHealth, value); OnHealthChanged?.Invoke(this, health); if (health <= 0) { Destroy(); } } }
    private readonly int maxHealth;
    public int MaxHealth => maxHealth;

    private int actionTime;
    public int ActionTime { get => actionTime;
        set { actionTime = Mathf.Min(MaxActionTime, value); OnActionTimeChanged?.Invoke(this, actionTime); } }
    private readonly int maxActionTime;
    public int MaxActionTime => maxActionTime;

    public readonly GameObject gameObject;
    public GameObject gameManager;
    public MapManager mapManager;
    public TeamManager teamManager;

    protected List<UnitComponent> UnitComponents = new List<UnitComponent>();

    public UnitBase(GameObject _gameObject, string _name, Vector3Int _position, int _teamid, int _maxHealth, int _maxActionTime)
    {
        gameObject = _gameObject;
        name = _name;
        this._position = _position;
        TeamID = _teamid;
        maxActionTime = actionTime = _maxActionTime;
        maxHealth = health = _maxHealth;
        gameManager = GameObject.Find("GameManager");
        if (gameManager == null)
        {
            Debug.LogError("[[[Can not find GameManager]]]");
        }
        else
        {
            mapManager = gameManager.GetComponent<MapManager>();
            if(mapManager == null)
            {
                Debug.LogError("Can not find MapManager in GameManager");
            }
            gameManager.GetComponent<Updater>().OnUpdate += Update;
        }
        teamManager = gameManager.GetComponent<TeamManager>();

        OnPositionChanged += UpdatePositionInMap;
        OnPositionChanged += UpdateTeamLogo;
        OnConstructed += (u) => { UpdateTeamLogo(u, Position, new Vector3Int(-1, -1, 0)); };

        mapManager.SetUnit(this, this._position);
        gameObject.transform.position = mapManager.MapTilemap.CellToWorld(Position);

        GameEvent.OnTeamBeginRound += (controller) =>
        {
            if (controller.TeamID == teamID)
            {
                ActionTime = MaxActionTime;
            }
        };
    }
    public void Constructed()
    {
        OnConstructed?.Invoke(this);
    }

    /// <summary>
    /// 每帧调用一次
    /// </summary>
    protected virtual void Update() { }
    /// <summary>
    /// 更新单位在MapManager中的相关信息
    /// </summary>
    private void UpdatePositionInMap(UnitBase unit, Vector3Int newpos, Vector3Int oldpos)
    {
        mapManager.SetUnit(null, oldpos);
        mapManager.SetUnit(unit, newpos);
    }
    private void UpdateTeamLogo(UnitBase unit, Vector3Int newpos, Vector3Int oldpos)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = teamManager.TeamLogo_Sprite;
        tile.color = teamManager.TeamColor[TeamID];
        teamManager.TeamTilemap.SetTile(oldpos, null);
        teamManager.TeamTilemap.SetTile(newpos, tile);
    }
    /// <summary>
    /// 销毁单位
    /// </summary>
    public void Destroy()
    {
        OnDestroy();
        ReadyToBeDestroyed?.Invoke(this);
        mapManager.SetUnit(null, Position);
        GameObject.Destroy(gameObject);
        OnDestroyed?.Invoke(this);
    }
    /// <summary>
    /// 当单位即将被销毁前(仍存在于地图中)调用
    /// </summary>
    protected virtual void OnDestroy() { }

    /// <summary>
    /// 根据类型获取单位中的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>若无则为null</returns>
    public T GetComponent<T>() where T : UnitComponent
    {
        foreach(var u in UnitComponents)
        {
            if (u is T)
                return u as T;
        }
        return null;
    }

    public void Select()
    {
        GameState.SelectedUnit = this;
        GameEvent.ActiveOnUnitSelected(this);
        OnSelected();
    }

    public void Unselect()
    {
        GameState.SelectedUnit = null;
        GameEvent.ActiveOnUnitUnselected(this);
        OnUnselected();
    }

    protected virtual void OnSelected()
    {

    }

    protected virtual void OnUnselected()
    {

    }
}
