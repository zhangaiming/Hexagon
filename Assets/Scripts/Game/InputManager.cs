using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager: MonoBehaviour
{
    GameManager GameManager;
    Vector3Int CursorPosition = new Vector3Int(0, 0, 0);
    bool OverUI = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscDown();
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (OverUI)
                InputEvent.OnMouseOverUIStateChanged?.Invoke(false);
            OverUI = false;
            Vector3Int cur = GameManager.StandardTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (cur != CursorPosition)
            {
                CursorPosition = cur;
                OnCursorPositionChanged();
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnMouseLeftClickOnTilemap();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnMouseRightClickOnTilemap();
            }
        }
        else
        {
            if (!OverUI)
                InputEvent.OnMouseOverUIStateChanged?.Invoke(true);
            OverUI = true;
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseLeftClickOnUI();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnMouseRightClickOnUI();
            }
        }
    }

    void OnMouseLeftClickOnTilemap()
    {
        InputEvent.OnMouseLeftClickOnTilemap?.Invoke(CursorPosition);
        InputEvent.OnMouseLeftClick?.Invoke();
    }
    void OnMouseRightClickOnTilemap()
    {
        InputEvent.OnMouseRightClickOnTilemap?.Invoke(CursorPosition);
        InputEvent.OnMouseRightClick?.Invoke();
    }

    void OnMouseLeftClickOnUI()
    {
        InputEvent.OnMouseLeftClickOnUI?.Invoke();
        InputEvent.OnMouseLeftClick?.Invoke();
    }
    void OnMouseRightClickOnUI()
    {
        InputEvent.OnMouseRightClickOnUI?.Invoke();
        InputEvent.OnMouseRightClick?.Invoke();
    }

    void OnEscDown()
    {
        InputEvent.OnEscDown?.Invoke();
    }

    void OnCursorPositionChanged()
    {
        InputEvent.OnCursorPositionChanged?.Invoke(CursorPosition);
    }
}
