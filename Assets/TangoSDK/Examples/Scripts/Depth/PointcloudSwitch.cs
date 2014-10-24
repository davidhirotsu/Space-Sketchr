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
using System;


public class PointcloudSwitch : MonoBehaviour 
{
    public GUISkin guiSkin;
    public Pointcloud pointcloud;



    /// <summary>
    /// GUI for switch getting data API and status.
    /// </summary>
    private void OnGUI()
    {
		
		Color oldColor = GUI.color;
		GUI.color = Color.gray;
		
		GUI.Label(new Rect(Common.UI_LABEL_START_X, 
		                   Common.UI_LABEL_START_Y, 
		                   Common.UI_LABEL_SIZE_X , 
		                   Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_TANGO_SERVICE_VERSION, pointcloud.GetTangoServiceVersion()) + "</size>");


        GUI.Label(new Rect(Common.UI_LABEL_START_X, 
		                   Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET, 
                           Common.UI_LABEL_SIZE_X , 
                           Common.UI_LABEL_SIZE_Y), "<size=15>Point Count: " + pointcloud.m_pointsCount.ToString() + "</size>");

        GUI.Label(new Rect(Common.UI_LABEL_START_X, 
		                   Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET * 2.0f, 
                           Common.UI_LABEL_SIZE_X , 
                           Common.UI_LABEL_SIZE_Y), "<size=15>Average Depth (m): " + pointcloud.m_overallZ.ToString() + "</size>");


        GUI.Label(new Rect(Common.UI_LABEL_START_X, 
		                   Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET * 3.0f, 
                           Common.UI_LABEL_SIZE_X , 
                           Common.UI_LABEL_SIZE_Y), "<size=15>Frame Delta Time (ms): " + pointcloud.GetTimeSinceLastFrame().ToString("0.") + "</size>");
    
		GUI.color = oldColor;
	}
}
