using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitNameTranslator
{
    /// <summary>
    /// 获取名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetNameByID(int id)
    {
        switch (id)
        {
            case 1:
                return "莽夫";
            default:
                return null;
        }
    }
    /// <summary>
    /// 获取ID
    /// </summary>
    /// <param name="name"></param>
    /// <returns>返回-1说明名称错误</returns>
    public static int GetIDByName(string name)
    {
        switch(name)
        {
            case "莽夫":
                return 1;
            default:
                return -1;
        }
    }
}
