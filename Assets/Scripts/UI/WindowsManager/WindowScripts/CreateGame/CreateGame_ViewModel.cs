using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateGame_ViewModel : ViewModel
{
    public readonly BindableProperty<bool> ShouldShow = new BindableProperty<bool>();
    public readonly BindableProperty<bool> ShouldShowColorPan = new BindableProperty<bool>();

    public readonly BindableCollection<bool> IsColorUsableCollection = new BindableCollection<bool>();

    public CreateGame_ViewModel()
    {
        
    }
}
