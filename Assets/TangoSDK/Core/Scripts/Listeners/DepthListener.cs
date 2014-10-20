//-----------------------------------------------------------------------
// <copyright file="DepthListener.cs" company="Google">
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
/// automatically register for onDepthAvailable
/// callbacks from the Tango Service.
/// </summary>
public abstract class DepthListener : MonoBehaviour
{
    private Tango.DepthProvider.TangoService_onDepthAvailable m_onDepthAvailableCallback;
    
    /// <summary>
    /// Register this class to receive the OnDepthAvailable callback.
    /// </summary>
    public virtual void SetCallback()
    {
        m_onDepthAvailableCallback = new Tango.DepthProvider.TangoService_onDepthAvailable(_OnDepthAvailable);
        Tango.DepthProvider.SetCallback(m_onDepthAvailableCallback);
    }

    /// <summary>
    /// Callback that gets called when depth is available
    /// from the Tango Service.
    /// </summary>
    /// <param name="callbackContext">Callback context.</param>
    /// <param name="xyzij">Xyzij.</param>
    protected abstract void _OnDepthAvailable(IntPtr callbackContext, TangoXYZij xyzij);
}