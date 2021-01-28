using Hexagon.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndRound : GUIBase<EndRound_ViewModel>
{
    public Sprite Unreach_Sprite;
    public Sprite Reach_Sprite;
    public Image image;
    public Game game;

    bool Pop = false;
    float TargetLength = -80f;
    float CurrentLength = 0f;

    float PopSpeed = 5f;

    bool Draging = false;
    float Strength = 0.5f;
    float MaxLength = -100f;
    float MinLength = 20f;

    Vector3 MousePos;

    Vector3 OriPos;

    bool Reached = false;

    float GoBack_Time = 1f;
    float GoBack_Timer = 0f;
    bool Toggled = false;

    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        OriPos = transform.position;
        BindingContext = new EndRound_ViewModel();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Binder.Add<bool>("ShouldShow", OnShouldShowChanged);
    }

    private void Update()
    {
        if (Draging)
        {
            float delta = (Input.mousePosition - MousePos).x;
            
            if (CurrentLength > 0)
            {
                delta *= Strength * ((MinLength - CurrentLength) / MinLength);
                
            }
            else
            {
                delta *= Strength * ((MaxLength - CurrentLength) / MaxLength);
            }
            float cur = CurrentLength + delta;
            cur = Mathf.Clamp(cur, MaxLength, MinLength);
            float x = Mathf.Clamp(cur + OriPos.x, MaxLength + OriPos.x, MinLength + OriPos.x);
            CurrentLength = x - OriPos.x;
            gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
            MousePos = Input.mousePosition;

            if(CurrentLength <= TargetLength)
            {
                image.sprite = Reach_Sprite;
                GoBack_Timer = 0f;
                Reached = true;
            }
            else
            {
                image.sprite = Unreach_Sprite;
                Reached = false;
            }
        }
        else if (Reached)
        {
            if(!Toggled)
            {
                CurrentLength = 0;
                NextRound();
                Toggled = true;
            }
            GoBack_Timer += Time.deltaTime;
            gameObject.transform.Translate(new Vector3(30, 0, 0));
            if(GoBack_Timer >= GoBack_Time)
            {
                Reached = false;
                gameObject.transform.position = OriPos;
                BindingContext.ShouldShow.Value = false;
            }
        }
        else
        {
            if (CurrentLength <= 0)
            {
                float dis = PopSpeed * Time.deltaTime * Math.Abs(CurrentLength);
                gameObject.transform.position += new Vector3(dis, 0, 0);
                CurrentLength += dis;
            }
            else if (CurrentLength > 0)
            {
                float dis = PopSpeed * Time.deltaTime * CurrentLength;
                gameObject.transform.position -= new Vector3(dis, 0, 0);
                CurrentLength -= dis;
            }
        }
    }

    public void PointerEnter()
    {
        Pop = true;
    }

    public void PointerExit()
    {
        Pop = false;
    }

    public void OnBeginDrag()
    {
        Draging = true;
        MousePos = Input.mousePosition;
    }

    public void OnEndDrag()
    {
        Draging = false;
    }

    void NextRound()
    {
        game.CurController.EndRound();
    }

    void OnShouldShowChanged(bool newvalue, bool oldvalue)
    {
        //Debug.Log(newvalue);
        gameObject.SetActive(newvalue);
        if (newvalue)
        {
            CurrentLength = 0;
            GoBack_Timer = 0;
            Reached = false;
            Toggled = false;
            image.sprite = Unreach_Sprite;
        }
    }
}
