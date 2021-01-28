using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox_Window : Window
{
    public Text text;
    public void Confirm()
    {
        gameObject.SetActive(false);
    }

    public void SetContent(string content)
    {
        text.text = content;
    }

    public override void Close()
    {
        
    }

    public override void Show()
    {
        
    }
}
