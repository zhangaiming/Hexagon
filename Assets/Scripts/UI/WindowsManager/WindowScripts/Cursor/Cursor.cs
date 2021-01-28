using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : GUIBase<Cursor_ViewModel>
{
    public Sprite Walkable_Sprite;
    public Sprite Unwalkable_Sprite;
    public SpriteRenderer SpriteRenderer;

    float MoveSpeed = 10f;
    Vector3 TargetPoint;
    bool ShouldMove = false;

    private void Awake()
    {
        BindingContext = new Cursor_ViewModel();
    }

    private void Update()
    {
        MoveToTarget();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Binder.Add<bool>("Hide", OnHidePropertyChanged);
        Binder.Add<Vector3>("Position", OnPositionChanged);
        Binder.Add<bool>("Walkable", OnWalkablePropertyChanged);
    }

    void OnHidePropertyChanged(bool newvalue, bool oldvalue)
    {
        if (newvalue)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    void OnPositionChanged(Vector3 newpos, Vector3 oldpos)
    {
        TargetPoint = newpos;
        ShouldMove = true;
    }

    void OnWalkablePropertyChanged(bool newvalue, bool oldvalue)
    {
        if (newvalue)
        {
            SpriteRenderer.sprite = Walkable_Sprite;
        }
        else
        {
            SpriteRenderer.sprite = Unwalkable_Sprite;
        }
    }

    void MoveToTarget()
    {
        if (ShouldMove && TargetPoint != null && (TargetPoint - gameObject.transform.position).magnitude > 0.01)
        {
            Transform trans = gameObject.transform;
            trans.Translate((TargetPoint - trans.position) * Time.deltaTime * MoveSpeed);
        }
        else
        {
            ShouldMove = false;
        }

    }
}
