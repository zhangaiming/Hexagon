using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public GameObject Grid;
    public Tilemap StandardTilemap => Grid.transform.GetChild(0).GetComponent<Tilemap>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        GameObject.DontDestroyOnLoad(Grid);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
