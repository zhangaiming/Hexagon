using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitSpriteManager
{
    private static Dictionary<string, string> UnitSpriteDictionary = new Dictionary<string, string>();

    static UnitSpriteManager()
    {
        TextAsset obj = Resources.Load<TextAsset>(ResourcesPaths.UnitSpriteIndexPath);
        if (obj == null)
        {
            Debug.LogError("Unable to find the unit index file.");
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
            string directory = ResourcesPaths.UnitSpritePath + temps[1];
            if (Resources.Load<Texture2D>(directory) == null)
            {
                Debug.LogError("An index is unavailable: incorrect directory.\n" + directory);
                continue;
            }
            UnitSpriteDictionary.Add(temps[0], directory);
        }
    }

    public static Sprite GetSprite(string name)
    {
        string dir;

        if (UnitSpriteDictionary.TryGetValue(name, out dir))
        {
            Sprite res = Resources.Load<Sprite>(dir);
            if (res == null)
            {
                Debug.LogWarning("Sprite is null");
            }
            return res;
        }
        else return null;
    }
}
