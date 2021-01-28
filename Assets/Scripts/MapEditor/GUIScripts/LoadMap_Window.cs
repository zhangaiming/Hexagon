using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMap_Window : Window
{
    public InputField inputField;
    public MapEditor.MapManager mapManager;
    public MapEditorMenu_Manager manager;

    public void Confirm()
    {
        Map map = null;
        if(Map.CreateMapByCode(inputField.text, out map))
        {
            mapManager.LoadMap(map);
            manager.Close();
        }
        else
        {
            manager.ShowMessage("地图代码错误.");
        }
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
        inputField.text = "";
    }
}
