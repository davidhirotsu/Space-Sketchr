//-----------------------------------------------------------------------
// <copyright file="TangoEventListener.cs" company="Google">
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
/// automatically register for onEventAvailable
/// callbacks from the Tango Service.
/// </summary>
public abstract class TangoEventListener : MonoBehaviour
{
    public TangoEvents.TangoService_onEventAvailable m_onEventAvaialableCallback;

    /// <summary>
    /// Sets the callback.
    /// </summary>
    public virtual void SetCallback()
    {
        m_onEventAvaialableCallback = new TangoEvents.TangoService_onEventAvailable(_onEventAvailable);
        TangoEvents.SetCallback(m_onEventAvaialableCallback);
    }

    /// <summary>
    /// Handle the callback sent by the Tango Service
    /// when a new event is issued.
    /// </summary>
    /// <param name="callbackContext">Callback context.</param>
    /// <param name="tangoEvent">Tango event.</param>
    protected abstract void _onEventAvailable(IntPtr callbackContext, TangoEvent tangoEvent);
}
