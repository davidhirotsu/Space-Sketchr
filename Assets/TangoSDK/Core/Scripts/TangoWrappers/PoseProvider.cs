//-----------------------------------------------------------------------
// <copyright file="PoseProvider.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace Tango
{
	/// <summary>
	/// Provide pose related functionality.
	/// </summary>
    public class PoseProvider
    {   
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TangoService_onPoseAvailable(IntPtr callbackContext, [In,Out] TangoPoseData pose);

		private static readonly string CLASS_NAME = "PoseProvider";

		/// <summary>
		/// Sets the callback to be used when a new Pose is
		/// presented by the Tango Service.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public static void SetCallback(TangoCoordinateFramePair[] framePairs, TangoService_onPoseAvailable callback)
        {
            int returnValue = PoseProviderAPI.TangoService_connectOnPoseAvailable(framePairs.Length, framePairs, callback);
            if (returnValue != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR,
                                                   CLASS_NAME + ".SetCallback() Callback was not set!");
            }
            else
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
				                                   CLASS_NAME + ".SetCallback() OnPose callback was set!");
            }
        }

		/// <summary>
		/// Gets the pose at a given time.
		/// </summary>
		/// <param name="poseData">Pose data.</param>
		/// <param name="timeStamp">Time stamp.</param>
        public static void GetPoseAtTime([In,Out] TangoPoseData poseData, 
		                                 double timeStamp, 
		                                 TangoCoordinateFramePair framePair)
        {
            int returnValue = PoseProviderAPI.TangoService_getPoseAtTime(timeStamp, framePair, poseData);
            if (returnValue != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR,
				                                   CLASS_NAME + ".GetPoseAtTime() Could not get pose at time : " + timeStamp);
            }
        }

		public static void SetListenerCoordinateFramePairs(int count,
		                                                   ref TangoCoordinateFramePair frames)
		{
			int returnValue = PoseProviderAPI.TangoService_setPoseListenerFrames (count, ref frames);
			if (returnValue != Common.ErrorType.TANGO_SUCCESS)
			{
				DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR,
				                                   CLASS_NAME + ".SetListenerCoordinateFramePairs() Could not set frame pairs");
			}
		}

		/// <summary>
		/// Resets the motion tracking.
		/// </summary>
        public static void ResetMotionTracking()
        {
            PoseProviderAPI.TangoService_resetMotionTracking();
        }

        #region API_Functions
        private struct PoseProviderAPI
        { 
    #if UNITY_ANDROID
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_connectOnPoseAvailable(int count,
			                                                             TangoCoordinateFramePair[] framePairs,
			                                                             TangoService_onPoseAvailable onPoseAvailable);
            
            
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_getPoseAtTime (double timestamp,
                                                                 TangoCoordinateFramePair framePair,
                                                                 [In, Out] TangoPoseData pose);

			[DllImport(Common.TANGO_UNITY_DLL)]
			public static extern int TangoService_setPoseListenerFrames(int count,
			                                                            ref TangoCoordinateFramePair frames);
            
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern void TangoService_resetMotionTracking();

    #else
            public static int TangoService_connectOnPoseAvailable(TangoService_onPoseAvailable onPoseAvailable)
            {
                return Common.ErrorType.TANGO_SUCCESS;
            }
            public static int TangoService_getPoseAtTime (double timestamp,
                                                          TangoCoordinateFramePair framePair,
                                                          [In, Out] TangoPoseData pose)
            {
                return Common.ErrorType.TANGO_SUCCESS;
			}
			public static int TangoService_setPoseListenerFrames(int count,
			                                                     ref TangoCoordinateFramePair frames)
			{
				return Common.ErrorType.TANGO_SUCCESS;
            }
            public static void TangoService_resetMotionTracking()
            {
            }
    #endif
        }
        #endregion
    }
}








//private static PoseProviderAPI.TangoService_onPoseAvailable m_onPoseAvailable;

