using System.Collections.Generic;
using UnityEngine;

namespace CoolishUI
{
    public class SimpleDebugConsole : MonoBehaviour
    {
        [SerializeField, Tooltip("Hotkey to show and hide th console.")]
        KeyCode toggleKey = KeyCode.Escape;

        private List<Log> logs = new List<Log>();
        private Vector2 scrollViewVector = Vector2.zero;

        private float oldDrag;
        private Rect logsRect = Rect.zero;

        private Vector2 downPos;
        private Vector2 mousePosition;

        int startIndex;
        bool newLogAdded = false;

        private bool displayError = true;
        private bool displayWarning = true;
        private bool displayNormal = true;

        private bool isVisible;
        private float blockToggleConsoleTime = 0f;

        private class Log
        {
            public string log;
            public LogType logType;

            public Log(string log, LogType logType)
            {
                this.log = log;
                this.logType = logType;
            }
        }

        void OnEnable()
        {
            Application.logMessageReceivedThreaded += LogThreadedHandler;
        }

        void OnDisable()
        {
            Application.logMessageReceivedThreaded -= LogThreadedHandler;
        }

        void Update()
        {
            if (Application.platform == RuntimePlatform.Android ||
               Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (blockToggleConsoleTime > 0f)
                {
                    blockToggleConsoleTime -= Time.deltaTime;
                }
                if (blockToggleConsoleTime <= 0f)
                {
                    if (Input.touchCount == 3
                        && Input.GetTouch(0).phase == TouchPhase.Moved
                        && Input.GetTouch(1).phase == TouchPhase.Moved
                        && Input.GetTouch(2).phase == TouchPhase.Moved)
                    {
                        isVisible = true;
                        blockToggleConsoleTime = 0.5f;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(toggleKey))
                {
                    isVisible = !isVisible;
                }
            }
            
            if(newLogAdded && GUIHelper.logFontSize != 0)
            {
                calculateStartIndex();
                int totalCount = logs.Count;
                int totalVisibleCount = (int)(Screen.height * 0.80f / GUIHelper.logFontSize);
                if (startIndex >= (totalCount - totalVisibleCount))
                    scrollViewVector.y += GUIHelper.logFontSize * 2;
                newLogAdded = false;
            }            
        }

        void OnGUI()
        {
            if (!isVisible) return;

            getDownPos();
            logsRect.x = GUIHelper.rectX / 2;
            logsRect.y = GUIHelper.rectY / 2;
            logsRect.width = Screen.width - GUIHelper.rectX;
            logsRect.height = Screen.height - GUIHelper.rectY;

            Vector2 drag = getDrag();
            if (drag.y != 0 && logsRect.Contains(new Vector2(downPos.x, Screen.height - downPos.y)))
            {
                scrollViewVector.y += (drag.y - oldDrag);
            }

            GUIHelper.DrawArea(new Rect(GUIHelper.rectX / 2,
                                        GUIHelper.rectY / 2,
                                        Screen.width - GUIHelper.rectX,
                                        Screen.height - GUIHelper.rectY),
                              () => {
                                  scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);                                  
                                  oldDrag = drag.y;
                                  drawLogs();
                                  GUILayout.EndScrollView();

                                  GUILayout.BeginHorizontal();
                                  if (GUIHelper.DrawButton("Clear", 120, 50))
                                  {
                                      this.logs.Clear();
                                  }

                                  displayNormal = GUIHelper.DrawToggle(displayNormal, "Normal", 130, 50);
                                  displayWarning = GUIHelper.DrawToggle(displayWarning, "Warning", 130, 50);
                                  displayError = GUIHelper.DrawToggle(displayError, "Error", 130, 50);

                                  GUILayout.FlexibleSpace();
                                  if (GUIHelper.DrawButton("Close", 120, 50))
                                  {
                                      isVisible = false;
                                  }
                                  GUILayout.EndHorizontal();
                              });
        }

        void LogThreadedHandler(string message, string stackTrace, LogType type)
        {            
            //logs.Insert(0, new Log(message + stackTrace, type));
            logs.Add(new Log(message, type));
            newLogAdded = true;            
        }

        void drawLogs()
        {
            foreach (Log log in logs)
            {
                Color color;
                switch (log.logType)
                {
                    case LogType.Exception:
                    case LogType.Error:
                        if (!displayError) continue;
                        color = Color.red;
                        break;
                    case LogType.Warning:
                        if (!displayWarning) continue;
                        color = Color.yellow;
                        break;
                    default:
                        if (!displayNormal) continue;
                        color = Color.white;
                        break;
                }                
                GUIHelper.DrawLabel(log.log, color);                
            }
        }

        void calculateStartIndex()
        {
            startIndex = (int)(scrollViewVector.y / GUIHelper.logFontSize);
            startIndex = Mathf.Clamp(startIndex, 0, logs.Count);
        }

        Vector2 getDownPos()
        {
            if (Application.platform == RuntimePlatform.Android ||
               Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began)
                {
                    downPos = Input.touches[0].position;
                    return downPos;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    downPos.x = Input.mousePosition.x;
                    downPos.y = Input.mousePosition.y;
                    return downPos;
                }
            }

            return Vector2.zero;
        }

        Vector2 getDrag()
        {

            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touches.Length != 1)
                {
                    return Vector2.zero;
                }
                return Input.touches[0].position - downPos;
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    mousePosition = Input.mousePosition;
                    return mousePosition - downPos;
                }
                else
                {
                    return Vector2.zero;
                }
            }
        }
    }
}