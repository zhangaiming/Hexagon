using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView<T>
{
    T BindingContext { get; set; }
}
