//-----------------------------------------------------------------------
// <copyright file="DepthProvider.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tango
{
    /// <summary>
    /// Wraps depth related Tango Service functionality.
    /// </summary>
    public class DepthProvider
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TangoService_onDepthAvailable(IntPtr callbackContext, [In,Out] TangoXYZij xyzij);

        /// <summary>
        /// Sets the callback that is called when new depth
        /// points have been sampled by the Tango Service.
        /// </summary>
        /// <param name="callback">Callback.</param>
        public static void SetCallback(TangoService_onDepthAvailable callback)
        {
            int returnValue = DepthAPI.TangoService_connectOnXYZijAvailable(callback);
            if (returnValue != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR,
                                                   "DepthProvider.SetCallback() Callback was not set!");
            }
            else
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
                                                   "DepthProvider.SetCallback() OnDepth callback was set!");
            }
        }

        /// <summary>
        /// Wraps depth functionality from Tango Service.
        /// </summary>
        private struct DepthAPI
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_connectOnXYZijAvailable(TangoService_onDepthAvailable onDepthAvailalble);

 #else
            public static int TangoService_connectOnXYZijAvailable(TangoService_onDepthAvailable onDepthAvailalble)
            {
                return Common.ErrorType.TANGO_SUCCESS;
            }
#endif
        }
    }
}
