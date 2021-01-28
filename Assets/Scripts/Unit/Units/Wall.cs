using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : UnitBase
{
    GameObject[] Wall_Coms = new GameObject[6];

    public Wall(GameObject gameObject, Vector3Int pos, int teamid) : base(gameObject, "围墙", pos, teamid, 40, 0)
    {
        string dir = ResourcesPaths.UnitPrefabPath;
        GameObject Sli_UpRight = GameObject.Instantiate(Resources.Load(dir + "SliUp_Wall"), gameObject.transform) as GameObject;
        GameObject Sli_UpLeft = GameObject.Instantiate(Resources.Load(dir + "SliUp_Wall"), gameObject.transform) as GameObject;
        Sli_UpLeft.GetComponent<SpriteRenderer>().flipX = true;
        GameObject Sli_DownRight = GameObject.Instantiate(Resources.Load(dir + "SliDown_Wall"), gameObject.transform) as GameObject;
        GameObject Sli_DownLeft = GameObject.Instantiate(Resources.Load(dir + "SliDown_Wall"), gameObject.transform) as GameObject;
        Sli_DownLeft.GetComponent<SpriteRenderer>().flipX = true;
        GameObject Hori_Right = GameObject.Instantiate(Resources.Load(dir + "Hori_Wall_Left"), gameObject.transform) as GameObject;
        GameObject Hori_Left = GameObject.Instantiate(Resources.Load(dir + "Hori_Wall_Right"), gameObject.transform) as GameObject;
        Wall_Coms[0] = Sli_DownRight;
        Wall_Coms[1] = Sli_DownLeft;
        Wall_Coms[2] = Hori_Right;
        Wall_Coms[3] = Hori_Left;
        Wall_Coms[4] = Sli_UpLeft;
        Wall_Coms[5] = Sli_UpRight;
        foreach(GameObject go in Wall_Coms)
        {
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.SetActive(false);
        }

        Connector connector = new Connector(this);
        UnitComponents.Add(connector);
        connector.OnConnected += Connect;
        connector.OnDisconnected += Disconnect;
    }

    void Connect(UnitBase unit, Direction dir)
    {
        Wall_Coms[(int)dir].SetActive(true);
    }

    void Disconnect(UnitBase unit, Direction dir)
    {
        Wall_Coms[(int)dir].SetActive(false);
    }
}
