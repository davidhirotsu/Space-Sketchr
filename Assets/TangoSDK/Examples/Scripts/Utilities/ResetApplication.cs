//-----------------------------------------------------------------------
// <copyright file="ResetApplication.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Reset the application.
/// </summary>
public class ResetApplication : MonoBehaviour 
{
    /// <summary>
    /// Unity GUI updates.
    /// </summary>
    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width/2 - 100, 10, 200, 75), "Reset Application"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
