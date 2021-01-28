using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public KeyCode[] DirCtrlKey = new KeyCode[4] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
    public float MoveSpeed;
    public float ZoomSpeed;
    public float MaxSize = 8;
    public float MinSize = 2;

    float TargetSize = 5;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(DirCtrlKey[0]))
        {
            transform.Translate(new Vector3(0, MoveSpeed * Time.deltaTime));
        }
        else if (Input.GetKey(DirCtrlKey[1]))
        {
            transform.Translate(new Vector3(0, -MoveSpeed * Time.deltaTime));
        }

        if (Input.GetKey(DirCtrlKey[2]))
        {
            transform.Translate(new Vector3(-MoveSpeed * Time.deltaTime, 0));
        }else if (Input.GetKey(DirCtrlKey[3]))
        {
            transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0));
        }
        SetTargetSize(TargetSize - Input.mouseScrollDelta.y);

        if (Math.Abs(GetComponent<Camera>().orthographicSize - TargetSize) > 0.02)
            GetComponent<Camera>().orthographicSize += (TargetSize - GetComponent<Camera>().orthographicSize) * Time.deltaTime * ZoomSpeed;
    }

    void SetTargetSize(float size)
    {
        TargetSize = Math.Max(Math.Min(size, MaxSize), MinSize);
    }
}
