using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PropertyBinder<TViewModel> where TViewModel : ViewModel
{
    private delegate void BindHandler(TViewModel viewmodel);
    private delegate void UnBindHandler(TViewModel viewmodel);

    private readonly List<BindHandler> _binders = new List<BindHandler>();
    private readonly List<UnBindHandler> _unbinders = new List<UnBindHandler>();

    public void Bind(TViewModel target)
    {
        if (target == null)
            return;
        if(_binders != null)
        {
            foreach(var binder in _binders)
            {
                binder(target);
            }
        }
    }

    public void Unbind(TViewModel target)
    {
        if (target == null)
            return;
        if (_unbinders != null)
        {
            foreach (var unbinder in _unbinders)
            {
                unbinder(target);
            }
        }
    }

    /// <summary>
    /// 添加一个绑定，当目标属性赋值发生改变时调用eventHandler
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="eventHandler"></param>
    public void Add<TProperty>(string propertyName, BindableProperty<TProperty>.PropertyValueChangedEventHandler eventHandler)
    {
        FieldInfo info = typeof(TViewModel).GetField(propertyName, BindingFlags.Instance | BindingFlags.Public);
        if (info != null)
        {
            _binders.Add(viewModel => { GetProperty<TProperty>(viewModel, info).PropertyChanged += eventHandler; });
            _unbinders.Add(viewModel => { GetProperty<TProperty>(viewModel, info).PropertyChanged -= eventHandler; });
        }
    }

    /// <summary>
    /// 添加一个集合绑定，当集合或其元素发生改变时调用eventHandler
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="collectionName"></param>
    /// <param name="eventHandler"></param>
    public void AddCollection<TElement>(string collectionName, BindableCollection<TElement>.CollectionPropertyChangedEventHandler eventHandler)
    {
        FieldInfo info = typeof(TViewModel).GetField(collectionName, BindingFlags.Instance | BindingFlags.Public);
        if(info != null)
        {
            _binders.Add(viewModel => { GetCollection<TElement>(viewModel, info).CollectionChanged += eventHandler; });
            _unbinders.Add(viewModel => { GetCollection<TElement>(viewModel, info).CollectionChanged -= eventHandler; });
        }
    }

    BindableCollection<TElement> GetCollection<TElement>(TViewModel viewModel, FieldInfo fieldInfo)
    {
        if(viewModel == null)
        {
            Debug.LogWarning("View model is null");
            return null;
        }
        var value = fieldInfo.GetValue(viewModel);
        BindableCollection<TElement> collection = value as BindableCollection<TElement>;
        if(collection == null)
        {
            Debug.LogWarning("Property is null");
        }
        return collection;
    }

    BindableProperty<TProperty> GetProperty<TProperty>(TViewModel viewModel, FieldInfo fieldInfo)
    {
        if (viewModel == null)
        {
            Debug.LogWarning("View model is null");
            return null;
        }
        var value = fieldInfo.GetValue(viewModel);
        BindableProperty<TProperty> bindableProperty = value as BindableProperty<TProperty>;
        if(bindableProperty == null)
        {
            Debug.LogWarning("Property is null");
        }
        return bindableProperty;
    }
}
