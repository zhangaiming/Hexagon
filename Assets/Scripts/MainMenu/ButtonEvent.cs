using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace MainMenu
{
    public class ButtonEvent : MonoBehaviour
    {
        public Scene MapEditor;
        public Scene Combat;
        public void LoadMapEditor()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadCombatLevel()
        {
            SceneManager.LoadScene(2);
        }
    }
}