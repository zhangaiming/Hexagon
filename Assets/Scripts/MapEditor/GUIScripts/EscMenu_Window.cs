using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu_Window : Window
{
    [Tooltip("界面管理者")]
    public MapEditorMenu_Manager manager;

    public void NewMap()
    {
        manager.LoadWindow("NewMap");
    }

    public void LoadMap()
    {
        manager.LoadWindow("LoadMap");
    }

    public void MakeCode()
    {
        manager.LoadWindow("MakeCode");
    }

    public void Continue()
    {
        manager.Close();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public override void Show()
    {
        
    }

    public override void Close()
    {
        
    }
}
