using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MuFSMWindow : EditorWindow
{
    [MenuItem("Window/MuFSM")]
    static void OpenWindow()
    {
        var fsm = GetWindow<MuFSMWindow>();
        fsm.Show();
    }



    private void OnGUI()
    {
        Playing();
    }

  

    List<Type> running = new List<Type>();
    StringBuilder builder= new StringBuilder();
    private void Playing()
    {
        var objs = GameObject.FindObjectsByType<FSMController>(FindObjectsSortMode.None);
        for (int i = 0; i < objs.Length; i++)
        {
            builder.Append($"{objs[i].gameObject.name} : ");
            if (Application.isPlaying)
            {
                objs[i].GetRunningStateTypes(running);
                
                foreach (Type t in running)
                {
                    builder.Append(t.Name);
                    builder.Append(",");
                }
            }
            GUILayout.Label(builder.ToString().TrimEnd(','));
            builder.Clear();
            running.Clear();
        }

    }
}
