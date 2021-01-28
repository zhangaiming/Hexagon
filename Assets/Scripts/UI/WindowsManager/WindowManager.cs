using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexagon.Windows
{
    public static class WindowManager
    {
        static Dictionary<string, IWindow> HidedWnds = new Dictionary<string, IWindow>();
        static Stack<IWindow> WndStack = new Stack<IWindow>();

        public readonly static GameObject ShowCanvas;

        static WindowManager()
        {
            ShowCanvas = GameObject.Instantiate(Resources.Load(ResourcesPaths.WndCanvasPrefab)) as GameObject;
            GameObject.DontDestroyOnLoad(ShowCanvas);
        }

        public static IWindow CreateWindow(string wndName)
        {
            IWindow res;
            GameObject go = GameObject.Instantiate(Resources.Load(ResourcesPaths.WndPanelPrefabPath + wndName), ShowCanvas.transform) as GameObject;
            res = go.GetComponent<IWindow>();
            go.SetActive(false);
            return res;
        }

        public static void PushWnd(IWindow wnd)
        {
            if(WndStack.Count > 0)
                WndStack.Peek().OnPause();
            WndStack.Push(wnd);
            wnd.OnEnter();
        }

        public static void PopWnd(bool destroy)
        {
            if (WndStack.Count > 0)
            {
                IWindow wnd = WndStack.Pop();
                wnd.OnExit();
                if (!destroy)
                {
                    HidedWnds.Add(wnd.GetType().Name, wnd);
                }
                    
                else
                    wnd.Destroy();
                if(WndStack.Count > 0)
                {
                    WndStack.Peek().OnResume();
                }
            }
        }

        public static IWindow GetHidedWnd(string wndName)
        {
            if (HidedWnds.ContainsKey(wndName))
            {
                IWindow res = HidedWnds[wndName];
                HidedWnds.Remove(wndName);
                return res;
            }
            return null;
        }
    }
}