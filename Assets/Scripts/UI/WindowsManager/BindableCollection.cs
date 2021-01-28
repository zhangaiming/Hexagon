using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个可以绑定添加元素/删除元素/元素值发生改变等事件的集合
/// </summary>
/// <typeparam name="T"></typeparam>
public class BindableCollection<T> : ICollection<T>
{
    private readonly List<BindableProperty<T>> list;
    public delegate void CollectionPropertyChangedEventHandler(BindableCollection<T> collection);
    public event CollectionPropertyChangedEventHandler CollectionChanged;

    public BindableCollection()
    {
        list = new List<BindableProperty<T>>();
    }

    public int Count => list.Count;

    public bool IsReadOnly => false;

    public BindableProperty<T> this[int index] => list[index];

    public void Add(T item)
    {
        BindableProperty<T> element = new BindableProperty<T>();
        element.Value = item;
        element.PropertyChanged += OnElementChanged;
        list.Add(element);
    }

    public void Clear()
    {
        foreach(var e in list)
        {
            e.PropertyChanged -= OnElementChanged;
        }
        list.Clear();
    }

    public bool Contains(T item)
    {
        foreach(var e in list)
        {
            if (e.Value.Equals(item))
                return true;
        }
        return false;
    }

    public bool Remove(T item)
    {
        int index = -1;
        bool found = false;
        foreach(var e in list)
        {
            index++;
            if (e.Value.Equals(item))
            {
                e.PropertyChanged -= OnElementChanged;
                found = true;
            }
        }
        if (found)
        {
            list.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var e in list) yield return e.Value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var e in list) yield return e;
    }

    void OnElementChanged(T newValue, T oldValue)
    {
        OnCollectionChanged();
    }

    void OnCollectionChanged()
    {
        CollectionChanged?.Invoke(this);
    }

    public int IndexOf(T item)
    {
        int res = -1;
        foreach(var e in list)
        {
            res++;
            if (e.Value.Equals(item))
            {
                return res;
            }
        }
        return -1;
    }

    public void Insert(int index, T item)
    {
        BindableProperty<T> element = new BindableProperty<T>();
        element.Value = item;
        element.PropertyChanged += OnElementChanged;
        list.Insert(index, element);
    }

    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        var e = list.GetEnumerator();
        int temp = arrayIndex;
        while (temp-- != 0) e.MoveNext();
        do
        {
            array[arrayIndex++] = e.Current.Value;
        } while (e.MoveNext());
    }
}
