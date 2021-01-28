using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TeamType
{
    Player,
    Computer
}

public class TeamManager : MonoBehaviour
{
    public Sprite TeamLogo_Sprite;
    public Tilemap TeamTilemap;

    private string[] TeamName = new string[MaxTeamNum];
    private TeamType[] TeamType = new TeamType[MaxTeamNum];
    public Color[] TeamColor = new Color[MaxTeamNum];
    const int MaxTeamNum = 8;
    private int curTeamNum = 0;
    public int CurTeamNum => curTeamNum;

    /// <summary>
    /// 建立一个新的队伍,并返回队伍编号
    /// </summary>
    /// <param name="name">队伍名称</param>
    /// <returns>返回的队伍编号,为-1时即建立失败</returns>
    public int CreateTeam(string name, TeamType teamType, Color color)
    {
        if(MaxTeamNum > curTeamNum)
        {
            TeamName[curTeamNum] = name;
            TeamType[CurTeamNum] = teamType;
            TeamColor[CurTeamNum] = color;
            curTeamNum += 1;
            return curTeamNum - 1;
        }
        return -1;
    }
    /// <summary>
    /// 获取对应的队伍编号
    /// </summary>
    /// <param name="name"></param>
    /// <returns>获取到的队伍编号,返回-1即队伍名不存在</returns>
    public int GetTeamID(string name)
    {
        for(int i = 0; i < MaxTeamNum; i++)
        {
            if(TeamName[i] == name)
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// 获取队伍名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns>获取到的队伍名称,队伍不存在时返回null</returns>
    public string GetTeamName(int id)
    {
        if (id < 0 || id >= CurTeamNum) return null;
        return TeamName[id];
    }
    public TeamType GetTeamType(int id)
    {
        if (id < 0 || id >= CurTeamNum) return global::TeamType.Computer;
        return TeamType[id];
    }
    /// <summary>
    /// 清除所有队伍
    /// </summary>
    public void ClearTeams()
    {
        curTeamNum = 0;
        TeamName = new string[MaxTeamNum];
        TeamColor = new Color[MaxTeamNum];
        TeamType = new TeamType[MaxTeamNum];
    }
}
