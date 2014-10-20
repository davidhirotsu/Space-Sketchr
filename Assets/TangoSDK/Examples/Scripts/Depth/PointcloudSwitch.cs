//-----------------------------------------------------------------------
// <copyright file="YSPointcloudSwitch.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using Tango;

/// <summary>
/// Controls of pointcloud scene of YS.
/// </summary>
public class PointcloudSwitch : MonoBehaviour 
{
    public GUISkin guiSkin;
    public Pointcloud pointcloud;

    /// <summary>
    /// GUI for switch getting data API and status.
    /// </summary>
    private void OnGUI()
    {
        GUI.Label(new Rect(Common.UI_LABEL_START_X, 
                           Common.UI_TANGO_APP_SPECIFIC_START_Y, 
                           Common.UI_LABEL_SIZE_X , 
                           Common.UI_LABEL_SIZE_Y), "<size=20>Point Count: " + pointcloud.m_pointsCount.ToString() + "</size>");

        GUI.Label(new Rect(Common.UI_LABEL_START_X, 
                           Common.UI_TANGO_APP_SPECIFIC_START_Y + Common.UI_LABEL_OFFSET, 
                           Common.UI_LABEL_SIZE_X , 
                           Common.UI_LABEL_SIZE_Y), "<size=20>Average Depth (m): " + pointcloud.m_overallZ.ToString() + "</size>");


        GUI.Label(new Rect(Common.UI_LABEL_START_X, 
                           Common.UI_TANGO_APP_SPECIFIC_START_Y + Common.UI_LABEL_OFFSET * 2.0f, 
                           Common.UI_LABEL_SIZE_X , 
                           Common.UI_LABEL_SIZE_Y), "<size=20>Frame Delta Time (ms): " + pointcloud.GetTimeSinceLastFrame().ToString("0.") + "</size>");
    }
}
