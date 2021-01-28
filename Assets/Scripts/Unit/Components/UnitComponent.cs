using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitComponent
{
    private bool isActive = true;
    public bool IsActive => isActive;

    public UnitBase Owner;
    public MapManager MapManager => Owner.mapManager;
    private Navigate navigate;
    public Navigate Navigate { 
        get
        {
            if(navigate == null)
            {
                navigate = GameObject.Find("Navigator").GetComponent<Navigate>();
            }
            return navigate;
        }
    }

    public UnitComponent(UnitBase owner)
    {
        Owner = owner;
        Owner.ReadyToBeDestroyed += (u) => { SetActive(false); };
        GameEvent.OnTeamBeginRound += (c) =>
        {
            if (c.TeamID == Owner.TeamID)
                BeginRound();
        };
        GameEvent.OnTeamEndRound += (c) =>
        {
            if (c.TeamID == Owner.TeamID)
                EndRound();
        };
        GameObject.Find("GameManager").GetComponent<Updater>().OnUpdate += Update;
    }

    /// <summary>
    /// 当所在单位被构建的时候调用
    /// </summary>
    public virtual void Start() { }
    /// <summary>
    /// 每帧调用一次
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// 所在队伍开始回合时调用
    /// </summary>
    public virtual void BeginRound() { }
    /// <summary>
    /// 所在队伍结束回合时调用
    /// </summary>
    public virtual void EndRound() { }
    /// <summary>
    /// 当组件激活状态发生改变时调用
    /// </summary>
    /// <param name="isactive"></param>
    protected virtual void OnSetActive() { }
    /// <summary>
    /// 设置组件激活状态
    /// </summary>
    /// <param name="isactive"></param>
    public void SetActive(bool isactive)
    {
        isActive = isactive;
        OnSetActive();
    }
}