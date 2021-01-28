using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePanel_ViewModel : ViewModel
{
    MapManager MapManager;
    bool Selected = false;
    UnitBase ShowingUnit = null;
    UnitBase SelectedUnit = null;
    bool MouseOverUI = false;

    public readonly BindableProperty<bool> ShouldShow = new BindableProperty<bool>();

    public readonly BindableProperty<Sprite> UnitSprite = new BindableProperty<Sprite>();

    public readonly BindableProperty<string> UnitName = new BindableProperty<string>();

    public readonly BindableProperty<int> UnitHealth = new BindableProperty<int>();
    public readonly BindableProperty<int> UnitMaxHealth = new BindableProperty<int>();

    public readonly BindableProperty<int> UnitActionTime = new BindableProperty<int>();
    public readonly BindableProperty<int> UnitMaxActionTime = new BindableProperty<int>();

    public readonly BindableProperty<bool> UnitCanAttack = new BindableProperty<bool>();
    public readonly BindableProperty<bool> UnitCanMove = new BindableProperty<bool>();

    public StatePanel_ViewModel()
    {
        GameEvent.OnUnitSelected += Select;
        GameEvent.OnUnitUnselected += Unselect;
        InputEvent.OnCursorPositionChanged += CursorPositionChanged;
        InputEvent.OnMouseOverUIStateChanged += MouseOverUIStateChanged;
        MapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        ShouldShow.Value = true;
    }

    void MouseOverUIStateChanged(bool state)
    {
        MouseOverUI = state;
    }

    void CursorPositionChanged(Vector3Int pos)
    {
        if (MouseOverUI)
        {
            if (Selected)
                Show(SelectedUnit);
            else
                Hide();
            return;
        }
        if (MapManager.IsOccupied(pos))
        {
            UnitBase unit = MapManager.GetUnitAt(pos.x, pos.y);
            //ShowingUnit = unit;
            Show(unit);
        }
        else
        {
            if (Selected)
                Show(SelectedUnit);
            else
                Hide();
        }
    }

    void Select(UnitBase unit)
    {
        Selected = true;
        SelectedUnit = unit;
        Show(unit);
    }

    void Unselect(UnitBase unit)
    {
        Selected = false;
        SelectedUnit = null;
        Hide();
    }

    void Show(UnitBase unit)
    {
        if (unit != null)
        {
            if(ShowingUnit != null)
            {
                EndShow(ShowingUnit);
            }

            ShowingUnit = unit;
            //ShouldShow.Value = true;
            UnitSprite.Value = UnitSpriteManager.GetSprite(unit.Name);
            UnitName.Value = unit.Name;
            UnitHealth.Value = unit.Health;
            unit.OnHealthChanged += ShowingUnitHealthChanged;
            UnitMaxHealth.Value = unit.MaxHealth;
            UnitActionTime.Value = unit.ActionTime;
            unit.OnActionTimeChanged += ShowingUnitActionTimeChanged;
            UnitMaxActionTime.Value = unit.MaxActionTime;

            unit.ReadyToBeDestroyed += ShowingUnitDead;

            if(unit.TeamID != GameState.CurrentTeam)
            {
                UnitCanMove.Value = false;
                UnitCanAttack.Value = false;
            }
            else
            {
                Movement movement = unit.GetComponent<Movement>();
                if (movement != null)
                {
                    movement.OnMoveablityChanged += ShowingUnitMoveabilityChanged;
                    UnitCanMove.Value = movement.Moveable;
                }
                else
                    UnitCanMove.Value = false;

                Attacker attacker = unit.GetComponent<Attacker>();
                if (attacker != null)
                {
                    attacker.OnAttackabilityChanged += ShowingUnitAttackabilityChanged;
                    UnitCanAttack.Value = attacker.CanAttack;
                }
                else
                    UnitCanAttack.Value = false;
            }
        }
        else
        {
            Hide();
        }
    }

    void EndShow(UnitBase unit)
    {
        if (unit == null)
            return;


        Movement movement = unit.GetComponent<Movement>();
        if (movement != null)
            movement.OnMoveablityChanged -= ShowingUnitMoveabilityChanged;

        Attacker attacker = unit.GetComponent<Attacker>();
        if (attacker != null)
            attacker.OnAttackabilityChanged -= ShowingUnitAttackabilityChanged;

        unit.ReadyToBeDestroyed -= ShowingUnitDead;
    }

    void ShowingUnitMoveabilityChanged(Movement movement)
    {
        UnitCanMove.Value = movement.Moveable;
    }
    void ShowingUnitAttackabilityChanged(Attacker attacker)
    {
        UnitCanAttack.Value = attacker.CanAttack;
    }
    void ShowingUnitHealthChanged(UnitBase unit, int value)
    {
        UnitHealth.Value = value;
    }
    void ShowingUnitActionTimeChanged(UnitBase unit, int value)
    {
        UnitActionTime.Value = value;
    }
    void ShowingUnitDead(UnitBase unit)
    {
        if (Selected)
            Show(SelectedUnit);
        else
            Hide();
    }

    void Hide()
    {
        EndShow(ShowingUnit);

        ShowingUnit = null;
        //ShouldShow.Value = false;
        UnitSprite.Value = null;
        UnitName.Value = "";
        UnitHealth.Value = 0;
        UnitMaxHealth.Value = 0;
        UnitActionTime.Value = 0;
        UnitMaxActionTime.Value = 0;
        UnitCanMove.Value = false;
        UnitCanAttack.Value = false;
    }
}
