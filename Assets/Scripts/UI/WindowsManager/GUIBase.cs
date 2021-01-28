using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GUIBase<T> : MonoBehaviour, IView<T> where T : ViewModel
{
    protected readonly BindableProperty<T> ViewModelProperty = new BindableProperty<T>();
    public T BindingContext
    {
        get => ViewModelProperty.Value;
        set
        {
            if (value == null) return;
            if (!_initialized)
            {
                OnInitialize();
                _initialized = true;
            }
        }
    }
    
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly PropertyBinder<T> Binder = new PropertyBinder<T>();

    private bool _initialized;

    protected virtual void OnInitialize()
    {
        ViewModelProperty.PropertyChanged += OnBindingContextChanged;
    }

    protected virtual void OnBindingContextChanged(T newValue, T oldValue)
    {
        Binder.Unbind(oldValue);
        Binder.Bind(newValue);
    }
}
