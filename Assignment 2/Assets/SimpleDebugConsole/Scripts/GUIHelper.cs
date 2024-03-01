using System;
using UnityEngine;

namespace CoolishUI
{
    public static class GUIHelper
    {
        private static GUIStyle logLabel;
        private static GUIStyle btnStyle;
        private static GUIStyle toggleStyle;

        private static float landscapeWidth = 1920.0f;
        private static float landscapeHeight = 1080.0f;
        private static float portraitWidth = 1080.0f;
        private static float portraitHeight = 1920.0f;
        private static float factorX = 1.0f;
        private static float factorY = 1.0f;
        private static float fontSizeFactor = 1.0f;

        public static float rectX = 0;
        public static float rectY = 0;
        public static int logFontSize = 0;

        private static void Setup()
        {
            if (logLabel == null)
            {
                SetScreenSizeFactor();

                logLabel = new GUIStyle(GUI.skin.label);
                logFontSize = Mathf.RoundToInt(fontSizeFactor * 35);
                logLabel.fontSize = logFontSize;                
            }
            if (btnStyle == null)
            {
                btnStyle = new GUIStyle(GUI.skin.button);
                btnStyle.fontSize = Mathf.RoundToInt(fontSizeFactor * 35);
            }
            if (toggleStyle == null)
            {
                toggleStyle = new GUIStyle(GUI.skin.toggle);
                toggleStyle.fontSize = Mathf.RoundToInt(fontSizeFactor * 35);
            }
        }

        public static void DrawArea(Rect area, Action action)
        {
            Setup();

            GUI.Box(area, string.Empty);
            GUILayout.BeginArea(area);

            if (action != null)
                action();
            GUILayout.EndArea();
        }

        public static void DrawLabel(string text, Color color)
        {
            Setup();

            logLabel.normal.textColor = color;            
            GUILayout.Label(text, logLabel);
        }

        public static bool DrawButton(string text, float width, float height)
        {
            Setup();

            return GUILayout.Button(text, btnStyle, GUILayout.Width(width * factorX), GUILayout.Height(height * factorY));
        }

        public static bool DrawToggle(bool value, string text, float width, float height)
        {
            Setup();
            return GUILayout.Toggle(value, text, toggleStyle, GUILayout.Width(width * factorX), GUILayout.Height(height * factorY));
        }

        public static void SetScreenSizeFactor()
        {
            if (Screen.height > Screen.width) //Portrait
            {
                factorX = Screen.width / portraitWidth;
                factorY = Screen.height / portraitHeight;
                fontSizeFactor = factorY;

                float rectHeight = Screen.width / (Screen.height / Screen.height / 16f) / 9f;
                rectY = Screen.height - rectHeight;
            }
            else
            { //Landscape
                factorX = Screen.width / landscapeWidth;
                factorY = Screen.height / landscapeHeight;
                fontSizeFactor = factorX;

                float rectWidth = Screen.height / (Screen.width / Screen.width / 16f) / 9f;
                rectX = Screen.width - rectWidth;
            }
        }
    }
}