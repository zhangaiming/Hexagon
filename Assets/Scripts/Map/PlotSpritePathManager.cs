using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlotSpritePathManager
{
    private static Dictionary<string, Sprite> PlotSpriteDic = new Dictionary<string, Sprite>();

    static PlotSpritePathManager()
    {
        TextAsset obj /* = AssetDatabase.LoadAssetAtPath<TextAsset>(@"Assets/Database/地块资源索引.txt");*/
            = Resources.Load<TextAsset>(ResourcesPaths.PlotSpriteIndexPath);
        if (obj == null)
        {
            Debug.LogError("Unable to find the plot index file.");
            return;
        }
        string str = obj.text;
        string[] splitedStr = str.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in splitedStr)
        {
            string[] temps = s.Split(new char[] { ',' });
            if (temps.Length != 2)
            {
                Debug.LogError("An index is unavailable: incorrect length-" + temps.Length);
                continue;
            }
            string directory = ResourcesPaths.PlotSpritePath + temps[1];
            Sprite sprite = Resources.Load<Sprite>(directory);
            if (sprite == null)
            {
                Debug.LogError("An index is unavailable: incorrect directory.\n" + directory);
                continue;
            }
            PlotSpriteDic.Add(temps[0], sprite);
        }
    }

    public static Sprite GetSprite(PlotType type)
    {
        if (type == PlotType.None)
            return null;
        if(PlotSpriteDic.ContainsKey(type.ToString()))
            return PlotSpriteDic[type.ToString()];
        else
        {
            Debug.LogError("Unable to find the sprite of " + type.ToString());
            return null;
        }
    }
}
