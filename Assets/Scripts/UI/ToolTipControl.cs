using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipControl : MonoBehaviour
{
    public string text = "";
    public float DelayTime = 0.1f;
    RectTransform rect;
    GameObject GameObject;
    VerticalLayoutGroup VerticalLayoutGroup;
    Text Text;

    float CurrentTime = 0f;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        GameObject go = GameObject.FindWithTag("ToolTipCanvas");
        Transform canvas;
        if (go == null)
            canvas = (GameObject.Instantiate(Resources.Load(ResourcesPaths.ToolTipCanvasPath)) as GameObject).transform;
        else
            canvas = go.transform;
        GameObject = GameObject.Instantiate(Resources.Load(ResourcesPaths.ToolTipGameObjectPath), canvas) as GameObject;
        VerticalLayoutGroup = GameObject.GetComponent<VerticalLayoutGroup>();
        Text = GameObject.GetComponentInChildren<Text>();
        GameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (rect != null)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
            {
                if(CurrentTime >= DelayTime)
                {
                    if (GameObject == null)
                        GameObject = GameObject.Instantiate(Resources.Load(ResourcesPaths.ToolTipGameObjectPath)) as GameObject;
                    GameObject.SetActive(true);
                    Text.text = text;
                    Vector2 mousePos = Input.mousePosition;
                    GameObject.transform.position = new Vector3(mousePos.x, mousePos.y, -10);
                    GameObject.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
                }
                else
                {
                    CurrentTime += Time.deltaTime;
                }
            }
            else
            {
                CurrentTime = 0f;
                if (GameObject == null)
                    GameObject = GameObject.Instantiate(Resources.Load(ResourcesPaths.ToolTipGameObjectPath)) as GameObject;
                GameObject.SetActive(false);
            }
        }
    }
}
