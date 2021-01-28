using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputEvent
{
    public static Action OnEscDown;
    public static Action<Vector3Int> OnCursorPositionChanged;
    public static Action<bool> OnMouseOverUIStateChanged;
    public static Action<Vector3Int> OnMouseLeftClickOnTilemap;
    public static Action<Vector3Int> OnMouseRightClickOnTilemap;
    public static Action OnMouseLeftClickOnUI;
    public static Action OnMouseRightClickOnUI;
    public static Action OnMouseLeftClick;
    public static Action OnMouseRightClick;
}
