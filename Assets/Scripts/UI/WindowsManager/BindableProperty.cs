using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BindableProperty<T>
{
    private T value;
    public T Value
    {
        get => value;
        set
        {
            if (Equals(this.value, value)) return;
            var temp = Value;
            this.value = value;
            OnValueChanged(this.value, temp);
        } 
    }
    public delegate void PropertyValueChangedEventHandler(T newValue, T oldValue);
    /// <summary>
    /// 属性发生改变时调用
    /// |新值
    /// |旧值
    /// </summary>
    public event PropertyValueChangedEventHandler PropertyChanged;

    private void OnValueChanged(T newValue, T oldValue)
    {
        PropertyChanged?.Invoke(newValue, oldValue);
    }
}
