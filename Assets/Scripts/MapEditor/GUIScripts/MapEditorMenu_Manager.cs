using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorMenu_Manager : MonoBehaviour
{
    public GameObject MenuCanvas;
    public Window MainEscMenu;
    public Window NewMap;
    public Window LoadMap;
    public Window MakeCode;
    public Window CurrentWindow;

    bool Showed = false;
    // Start is called before the first frame update
    void Start()
    {
        MainEscMenu.Show();
        NewMap.Show();
        LoadMap.Show();
        MakeCode.Show();
        CurrentWindow = null;
        MenuCanvas.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Showed)
            {
                Close();
            }
            else
            {
                MenuCanvas.SetActive(true);
                LoadWindow("MainEscMenu");
                Showed = true;
                EditorState.EscMenuOpened = true;

            }
        }
    }

    public void Close()
    {
        if (CurrentWindow != null)
            CurrentWindow.Close();
        MenuCanvas.SetActive(false);
        Showed = false;
        EditorState.EscMenuOpened = false;
    }

    public void LoadWindow(string windowName)
    {
        if(CurrentWindow != null)
        {
            CurrentWindow.Close();
            CurrentWindow.gameObject.SetActive(false);
        }
        Window target = null;
        switch (windowName)
        {
            case "MainEscMenu":
                target = MainEscMenu;
                break;
            case "NewMap":
                target = NewMap;
                break;
            case "LoadMap":
                target = LoadMap;
                break;
            case "MakeCode":
                target = MakeCode;
                break;
        }
        if(target != null)
        {
            CurrentWindow = target;
            target.gameObject.SetActive(true);
            target.Show();
        }
        else
        {
            Debug.LogError("Can not find target window.");
        }
    }

    public MessageBox_Window MessageBox;

    public void ShowMessage(string content)
    {
        MessageBox.gameObject.SetActive(true);
        MessageBox.SetContent(content);
        MessageBox.Show();
    }
}
