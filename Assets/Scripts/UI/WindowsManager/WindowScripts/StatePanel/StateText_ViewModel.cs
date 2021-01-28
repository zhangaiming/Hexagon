using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateText_ViewModel : ViewModel
{
    public readonly BindableProperty<int> First = new BindableProperty<int>();
    public readonly BindableProperty<int> Second = new BindableProperty<int>();
}
