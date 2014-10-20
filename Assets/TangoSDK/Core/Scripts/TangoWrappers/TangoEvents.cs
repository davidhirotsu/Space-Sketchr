//-----------------------------------------------------------------------
// <copyright file="TangoEvent.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tango
{
    /// <summary>
    /// Wraps the interface from Tango Service to register
    /// for callbacks that are fired on new events.
    /// </summary>
    public class TangoEvents
    {
        // Signature used by the onTangoEvent callback.
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TangoService_onEventAvailable(IntPtr callbackContext, [In,Out] TangoEvent tangoEvent);
            
        /// <summary>
        /// Sets the callback that is called when a new tango
        /// event has been issued by the Tango Service.
        /// </summary>
        /// <param name="callback">Callback.</param>
        public static void SetCallback(TangoService_onEventAvailable callback)
        {
            int returnValue = EventsAPI.TangoService_connectOnTangoEvent(callback);
            if (returnValue != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR,
                                                   "TangoEvents.SetCallback() Callback was not set!");
            }
            else
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
                                                   "TangoEvents.SetCallback() Callback was set!");
            }
        }

        private struct EventsAPI
        {
            #if UNITY_ANDROID
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_connectOnTangoEvent(TangoService_onEventAvailable onEventAvaialable);
            #else
            public static int TangoService_connectOnTangoEvent(TangoService_onEventAvailable onEventAvaialable)
            {
                return Common.ErrorType.TANGO_SUCCESS;
            }
            #endif
        }
    }
}