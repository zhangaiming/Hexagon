using MapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class MapEditorSpawnPoint : MapEditorItem
    {
        private bool selected = false;
        public override bool Selected { get => selected;
            set { selected = value;if (!selected) OnPointerExit(); } }
        public Sprite Unselected_spr;
        public Sprite Selected_spr;

        Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void PonterDown()
        {
            Select();
        }

        public void PointerUp()
        {
            //Unselect();
        }

        public override void OnPointerEnter()
        {
            image.sprite = Selected_spr;
        }

        public override void OnPointerExit()
        {
            if (!Selected)
            {
                image.sprite = Unselected_spr;
            }
        }
    }
}