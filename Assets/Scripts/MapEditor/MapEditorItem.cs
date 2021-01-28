using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    public abstract class MapEditorItem : MonoBehaviour
    {
        public MapEditorPlotManager manager;
        public Sprite CursorSprite;
        public abstract bool Selected { get; set; }
        public abstract void OnPointerEnter();
        public abstract void OnPointerExit();

        protected void Select()
        {
            manager.SelectedItem = this;
        }

        protected void Unselect()
        {
            manager.SelectedItem = null;
        }
    }
}