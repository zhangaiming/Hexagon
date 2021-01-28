using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapEditor
{
    public class MapEditorPlot : MapEditorItem
    {
        public float FloatSpeed = 6;
        public float FloatHeight = 25;
        
        public PlotType PlotType;
        private bool selected = false;
        public override bool Selected { get => selected;
            set { selected = value; if (!selected) OnPointerExit(); } }

        bool floatingUp = false;
        bool floatingDown = false;
        float curheight = 0;

        bool tonedown = false;

        public override void OnPointerEnter()
        {
            if (Selected || EditorState.EscMenuOpened) return;
            floatingUp = true;
            floatingDown = false;
        }

        public override  void OnPointerExit()
        {
            if (Selected || EditorState.EscMenuOpened) return;
            floatingUp = false;
            floatingDown = true;
        }

        public void OnPointerDown()
        {
            tonedown = true;
            Select();
        }

        public void OnPointerUp()
        {
            tonedown = false;
        }

        private void Start()
        {
            CursorSprite = PlotSpritePathManager.GetSprite(PlotType);
        }

        private void Update()
        {

            if (floatingUp)
            {

                Vector3 v = new Vector3(0, 1) * FloatSpeed * (FloatHeight - curheight) * Time.deltaTime;
                //transform.Translate(v);
                /*Debug.Log(v);
                GetComponent<RectTransform>().anchoredPosition3D += v;*/
                transform.Translate(v);
                curheight += v.magnitude;

                if (curheight >= FloatHeight)
                {
                    floatingUp = false;
                }
            }
            else if (floatingDown)
            {
                Vector3 v = new Vector3(0, -1) * FloatSpeed * curheight * Time.deltaTime;
                transform.Translate(v);
                curheight -= v.magnitude;

                if (curheight <= 0)
                {
                    floatingDown = false;
                }
            }

            if (tonedown)
            {
                GetComponent<Image>().color = new Color(198f / 255f, 195f / 255f, 198f / 255f, 1);
            }
            else
            {
                GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

}
