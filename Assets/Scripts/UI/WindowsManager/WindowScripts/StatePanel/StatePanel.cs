using Hexagon.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatePanel : WindowBase<StatePanel_ViewModel>
{
    public Image UnitPicture_Image;
    public Text UnitName_Text;
    public Text UnitHealth_Text;
    public Text UnitActionPoint_Text;

    public Button Attack_Button;
    public ToolTipControl AttackButton_ToolTip;
    public Button Move_Button;
    public ToolTipControl MoveButton_ToolTip;

    public ToolTipControl ActionTime_ToolTip;
    public ToolTipControl Health_ToolTip;

    public ToolTipControl NameText_ToolTip;

    int UnitHealth = 0, UnitMaxHealth = 0, UnitActionTime = 0, UnitMaxActionTime = 0;

    private void Awake()
    {
        BindingContext = new StatePanel_ViewModel();
        //gameObject.SetActive(false);
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        //binder.Add<bool>("ShouldShow", OnShouldShowChanged);
        Binder.Add<Sprite>("UnitSprite", OnUnitSpriteChanged);
        Binder.Add<string>("UnitName", OnUnitNameChanged);
        Binder.Add<int>("UnitHealth", OnUnitHealthChanged);
        Binder.Add<int>("UnitMaxHealth", OnUnitMaxHealthChanged);
        Binder.Add<int>("UnitActionTime", OnUnitActionTimeChanged);
        Binder.Add<int>("UnitMaxActionTime", OnUnitMaxActionTimeChanged);
        Binder.Add<bool>("UnitCanMove", OnUnitCanMoveChanged);
        Binder.Add<bool>("UnitCanAttack", OnUnitCanAttackChanged);
    }

    void OnUnitNameChanged(string newName,string oldName)
    {
        UnitName_Text.text = newName;
        NameText_ToolTip.text = newName;
    }

    void OnUnitSpriteChanged(Sprite newSprite, Sprite oldSprite)
    {
        if(newSprite == null)
        {
            UnitPicture_Image.enabled = false;
        }
        else
        {
            UnitPicture_Image.enabled = true;
            UnitPicture_Image.sprite = newSprite;
        }
    }
    #region UpdateStateText
    void OnUnitHealthChanged(int newValue, int oldValue)
    {
        UnitHealth = newValue;
        UpdateHealthState();
    }

    void OnUnitMaxHealthChanged(int newValue, int oldValue)
    {
        UnitMaxHealth = newValue;
        UpdateHealthState();
    }

    void UpdateHealthState()
    {
        string t;
        if (UnitMaxHealth != 0)
        {
            t = "生命值：" + UnitHealth + "/" + UnitMaxHealth;
        }
        else
        {
            t = "生命值：";
        }
        Health_ToolTip.text = t;
        UnitHealth_Text.text = t;
    }

    void OnUnitActionTimeChanged(int newValue, int oldValue)
    {
        UnitActionTime = newValue;
        UpdateActionTimeState();
    }

    void OnUnitMaxActionTimeChanged(int newValue, int oldValue)
    {
        UnitMaxActionTime = newValue;
        UpdateActionTimeState();
    }

    void UpdateActionTimeState()
    {
        string t;
        if (UnitMaxActionTime != 0)
        {
            t = "行动力：" + UnitActionTime + "/" + UnitMaxActionTime;

        }
        else
        {
            t = "行动力：";
        }
        ActionTime_ToolTip.text = t;
        UnitActionPoint_Text.text = t;
    }
    #endregion
    #region Button
        #region UpdateActionButton
    void OnUnitCanMoveChanged(bool newValue, bool oldValue)
    {
        Move_Button.interactable = newValue;
        if (!newValue)
        {
            MoveButton_ToolTip.text = "该单位不可进行移动";
        }
    }

    void OnUnitCanAttackChanged(bool newValue, bool oldValue)
    {
        Attack_Button.interactable = newValue;
        if (!newValue)
        {
            AttackButton_ToolTip.text = "该单位不可进行攻击";
        }
    }
    #endregion
        #region ButtonEvent
    public void AttackButtonDown()
    {
        if(GameState.SelectedUnit != null)
        {
            Attacker attacker = GameState.SelectedUnit.GetComponent<Attacker>();
            if(attacker != null)
            {
                attacker.WaitForAttackTarget();
            }
        }
    }
    public void MoveButtonDown()
    {
        if(GameState.SelectedUnit != null)
        {
            Movement movement = GameState.SelectedUnit.GetComponent<Movement>();
            if(movement != null)
            {
                movement.WaitForMoveTarget();
            }
        }
    }
    #endregion
    #endregion
}
