using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindow
{
    void OnEnter();
    void OnExit();
    void OnPause();
    void OnResume();

    void Destroy();
}
