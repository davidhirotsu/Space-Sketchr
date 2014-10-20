//-----------------------------------------------------------------------
// <copyright file="PoseListener.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using UnityEngine;
using Tango;

/// <summary>
/// Abstract base class that can be used to
/// automatically register for onPoseAvailable
/// callbacks from the Tango Service.
/// </summary>
public abstract class PoseListener : MonoBehaviour
{
    public Tango.PoseProvider.TangoService_onPoseAvailable m_poseAvailableCallback;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PoseListener"/>
    /// is using auto reset.
    /// </summary>
    /// <value><c>true</c> if auto reset; otherwise, <c>false</c>.</value>
    public bool AutoReset
    {
        get;
        set;
    }

    /// <summary>
    /// Registers the callback.
    /// </summary>
    /// <param name="framePairs">Frame pairs.</param>
    public virtual void SetCallback(TangoCoordinateFramePair[] framePairs)
    {
        m_poseAvailableCallback = new Tango.PoseProvider.TangoService_onPoseAvailable(_OnPoseAvailable);
        Tango.PoseProvider.SetCallback(framePairs, m_poseAvailableCallback);
    }

    /// <summary>
    /// Handle the callback sent by the Tango Service
    /// when a new pose is sampled.
    /// </summary>
    /// <param name="callbackContext">Callback context.</param>
    /// <param name="pose">Pose.</param>
    protected abstract void _OnPoseAvailable(IntPtr callbackContext, TangoPoseData pose);
}