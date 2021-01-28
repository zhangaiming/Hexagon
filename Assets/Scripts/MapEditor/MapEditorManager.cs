using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("here");
        EditorState.InitEditor();
    }

    private void Update()
    {
        
    }
}
