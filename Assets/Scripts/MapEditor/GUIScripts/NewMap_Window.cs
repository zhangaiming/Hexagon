using MapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMap_Window : Window
{
    public InputField Name;
    public InputField X;
    public InputField Y;

    public MapEditorMenu_Manager manager;
    public MapEditor.MapManager mapManager;

    public void Confirm()
    {
        int x = int.Parse(X.text), y = int.Parse(Y.text);
        string name = Name.text;
        
        if(x <= 0 || y <= 0)
        {
            manager.ShowMessage("x与y都必须大于0");
            return;
        }

        if(name.Contains("::") || name.Contains("|"))
        {
            manager.ShowMessage("名称中包含非法字符!");
            return;
        }

        mapManager.CreateMap(x, y, name);
        manager.Close();
    }

    public void Cancel()
    {
        manager.LoadWindow("MainEscMenu");
    }

    public override void Close()
    {
        
    }

    public override void Show()
    {
        Name.text = "";
        X.text = "0";
        Y.text = "0";
    }
}
