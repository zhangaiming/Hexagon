using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCursor : GUIBase<SelectCursor_ViewModel>
{
    //public Sprite Show_Sprite;
    public SpriteRenderer sprite_renderer;
    public Animation select_animation;

    private void Awake()
    {
        BindingContext = new SelectCursor_ViewModel();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Binder.Add<bool>("ShouldShow", OnShouldShowChanged);
        Binder.Add<Vector3>("Position", OnPositionChanged);
    }

    void OnPositionChanged(Vector3 newvalue, Vector3 oldvalue)
    {
        transform.position = newvalue;
    }

    void OnShouldShowChanged(bool newvalue, bool oldvalue)
    {
        //Debug.Log("hello");
        if (newvalue)
        {
            if(sprite_renderer != null)
            {
                sprite_renderer.enabled = true;
                select_animation.Stop();
                select_animation.Play();
            }
        }
        else
        {
            if (sprite_renderer != null)
            {
                sprite_renderer.enabled = false;
            }
        }
    }
}
