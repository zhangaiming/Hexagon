using MapEditor;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;

public class MakeCode_Window : Window
{
    public MapEditor.MapManager mapManager;
    public InputField CodeField;
    public MapEditorMenu_Manager manager;

    public void CopyAll()
    {
        //Clipboard.SetDataObject(CodeField.text);
        TextEditor te = new TextEditor();
        te.text = CodeField.text;
        te.SelectAll();
        te.Copy();
    }

    public void CloseWindow()
    {
        manager.LoadWindow("MainEscMenu");
    }

    public override void Close()
    {
        
    }

    public override void Show()
    {
        string code = mapManager.GetMap().GetCode();
        CodeField.text = code;
    }
}
