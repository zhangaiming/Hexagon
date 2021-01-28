using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexagon.Windows{
    public abstract class WindowBase<T> : GUIBase<T>, IWindow where T : ViewModel
    {
        

        /*public static WindowBase<T> CreateWindow(string wndName)
        {
            IWindow iwnd = WindowManager.GetHidedWnd(wndName);
            if(iwnd is WindowBase<T>)
            {
                WindowBase<T> wnd = iwnd as WindowBase<T>;
                if (wnd != null)
                {
                    return wnd;
                }
                GameObject go = GameObject.Instantiate(Resources.Load(ResourcesPaths.WndPanelPrefabPath + "wndName"), WindowManager.ShowCanvas.transform) as GameObject;
                iwnd = go.GetComponent<IWindow>();
                go.SetActive(false);
                return wnd;
            }
            return null;
        }*/

        public virtual void OnEnter()
        {
            gameObject.SetActive(true);
            gameObject.transform.SetAsLastSibling();
            ViewModelProperty.Value.OnEnter();
        }
        public virtual void OnPause()
        {
            ViewModelProperty.Value.OnPause();
        }
        public virtual void OnResume()
        {
            ViewModelProperty.Value.OnResume();
        }
        public virtual void OnExit()
        {
            gameObject.SetActive(false);
            ViewModelProperty.Value.OnExit();
        }
        public virtual void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}