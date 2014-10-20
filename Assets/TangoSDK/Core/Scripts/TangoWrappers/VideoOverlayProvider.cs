//-----------------------------------------------------------------------
// <copyright file="VideoOverlayProvider.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Tango;

namespace Tango
{
    /// <summary>
    /// Video Overlay Provider class provide video functions
    /// to get frame textures.
    /// </summary>
    public class VideoOverlayProvider
    {
        public static void ConnectTexture(TangoEnums.TangoCameraId cameraId, int textureId)
        {


            int returnValue = VideoOverlayAPI.TangoService_connectTextureId(cameraId, textureId);

            if (returnValue != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR,
                                                   "VideoOverlayProvider.ConnectTexture() Texture was not connected to camera!");
            }
        }

		public static void RenderLatestFrame(TangoEnums.TangoCameraId cameraId)
        {
            int returnValue = VideoOverlayAPI.TangoService_updateTexture(cameraId);
            
            if (returnValue != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR,
                                                   "VideoOverlayProvider.UpdateTexture() Texture was not updated by camera!");
            }
        }

        #region NATIVE_FUNCTIONS
        /// <summary>
        /// Video overlay native function import.
        /// </summary>
        private struct VideoOverlayAPI
        {
            #if UNITY_ANDROID && !UNITY_EDITOR

            [DllImport(Common.TANGO_UNITY_DLL)]
			public static extern int TangoService_connectTextureId(TangoEnums.TangoCameraId cameraId, int textureHandle);

            [DllImport(Common.TANGO_UNITY_DLL)]
			public static extern int TangoService_updateTexture(TangoEnums.TangoCameraId cameraId);

            #else
            public static int TangoService_connectTextureId(TangoEnums.TangoCameraId cameraId, int textureHandle)
            {
                return Tango.Common.ErrorType.TANGO_SUCCESS;
            }
            
            public static int TangoService_updateTexture(TangoEnums.TangoCameraId cameraId)
            {
                return Tango.Common.ErrorType.TANGO_SUCCESS;
            }
            #endif
            #endregion
        }
    }
}
