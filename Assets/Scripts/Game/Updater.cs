using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
    public event Action OnUpdate;

    // Update is called once per frame
    void Update()
    {
        OnUpdate?.Invoke();
    }
}
