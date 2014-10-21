//-----------------------------------------------------------------------
// <copyright file="TangoApplication.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;

namespace Tango
{
    /// <summary>
    /// Entry point of Tango applications, maintain the application handler.
    /// </summary>
    public class TangoApplication : MonoBehaviour 
    {
        public bool m_enableMotionTracking = true;
        public bool m_enableDepth = true;
        public bool m_motionTrackingAutoReset = true;

		private const string CLASS_NAME = "TangoApplication";
        private const string ANDROID_PRO_LABEL_TEXT = "<size=30>Tango plugin requires Unity Android Pro!</size>";
        private const float ANDROID_PRO_LABEL_PERCENT_X = 0.5f;
        private const float ANDROID_PRO_LABEL_PERCENT_Y = 0.5f;
        private const float ANDROID_PRO_LABEL_WIDTH = 200.0f;
        private const float ANDROID_PRO_LABEL_HEIGHT = 200.0f;
        private const string DEFAULT_AREA_DESCRIPTION = "/sdcard/defaultArea";
        private const string MOTION_TRACKING_LOG_PREFIX = "Motion tracking mode : ";

        private static TangoApplication m_instance;
        private DepthProvider m_depthProvider;
        private string m_tangoServiceVersion;

        private IntPtr m_tangoConfig;
		private IntPtr m_callbackContext;

        /// <summary>
        /// Singleton instance of TangoApplication.
        /// </summary>
        /// <value> No setter.</value>
        public static TangoApplication Instance
        {
            get
            {
                if (m_instance == null)
                {
                    GameObject owner = new GameObject();
                    m_instance = owner.AddComponent(typeof(TangoApplication)) as TangoApplication;
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Initialize application and providers.
        /// </summary>
        public void InitApplication()
        {
            m_tangoServiceVersion = "Not Initialized";

			// Initialize the Tango Service
            _Initialize();

			// Set up 
            TangoConfig.Allocate();
            TangoConfig.FillOut();

            if (!TangoConfig.SetBool(TangoConfig.Keys.ENABLE_AREA_LEARNING_BOOL, false)) // HACK : this should check enableAreaLearning
            {
                //TODO
            }
            
            if (TangoConfig.SetBool(TangoConfig.Keys.ENABLE_MOTION_TRACKING_BOOL, m_enableMotionTracking) && m_enableMotionTracking)
            {
                TangoCoordinateFramePair[] framePairs = new TangoCoordinateFramePair[1];
                framePairs[0].baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE;
                framePairs[0].targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;

                _SetMotionTrackingCallbacks(framePairs);
            }

            if (TangoConfig.SetBool(TangoConfig.Keys.ENABLE_DEPTH_PERCEPTION_BOOL, m_enableDepth) && m_enableDepth)
            {
                _SetDepthCallbacks();
            }

            if (!TangoConfig.SetBool(TangoConfig.Keys.ENABLE_MOTION_TRACKING_AUTO_RESET_BOOL, m_motionTrackingAutoReset))
            {
                //TODO
            }

            _SetEventCallbacks();

            TangoConfig.GetString(TangoConfig.Keys.GET_TANGO_SERVICE_VERSION_STRING, ref m_tangoServiceVersion);
            TangoConfig.Lock();
            _Connect();
        }

        /// <summary>
        /// Perform constructor logic.
        /// </summary>
        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(this);
                return;
            }
            m_instance = this;
            InitApplication();
        }
        
        /// <summary>
        /// Perform updates for the next frame after the
        /// normal update has run. This loads next frame.
        /// </summary>
        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        /// <summary>
        /// Let the developer know that the scene must
        /// be build with Unity Android Pro, if it wasn't
        /// built this way originally.
        /// </summary>
//        private void OnGUI()
//        {
//            GUI.Label(new Rect(Common.UI_LABEL_START_X, 
//                               Common.UI_LABEL_START_Y, 
//                               Common.UI_LABEL_SIZE_X , 
//                               Common.UI_LABEL_SIZE_Y), "<size=20>Tango Service Version : " + m_tangoServiceVersion + "</size>");
//        }

        /// <summary>
        /// Shutdown this instance.
        /// </summary>
        private void OnApplicationQuit()
        {
            TangoConfig.Unlock();
            TangoConfig.Free();
            _Disconnect();
        }
        
		/// <summary>
		/// Set callbacks on all PoseListener objects.
		/// </summary>
		/// <param name="framePairs">Frame pairs.</param>
        private void _SetMotionTrackingCallbacks(TangoCoordinateFramePair[] framePairs)
        {
            PoseListener[] poseListeners = FindObjectsOfType<PoseListener>();

            foreach (PoseListener poseListener in poseListeners)
            {
                if (poseListener != null)
                {
                    poseListener.AutoReset = m_motionTrackingAutoReset;

                    poseListener.SetCallback(framePairs);
                }
            }
        }

		/// <summary>
		/// Set callbacks for all DepthListener objects.
		/// </summary>
        private void _SetDepthCallbacks()
        {
            DepthListener[] depthListeners = FindObjectsOfType<DepthListener>();

            foreach (DepthListener depthListener in depthListeners)
            {
                if (depthListener != null)
                {
                    depthListener.SetCallback();
                }
            }
        }

        /// <summary>
        /// Set callbacks for all TangoEventListener objects.
        /// </summary>
        private void _SetEventCallbacks()
        {
            TangoEventListener[] eventListeners = FindObjectsOfType<TangoEventListener>();

            foreach (TangoEventListener eventListener in eventListeners)
            {
                if (eventListener != null)
                {
                    eventListener.SetCallback();
                }
            }
        }
        
		/// <summary>
		/// Initialize the Tango Service.
		/// </summary>
        private void _Initialize()
        {
            if (TangoServiceAPI.TangoService_initialize() != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Initialize() The service has not been initialized!");
            }
            else
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
				                                   CLASS_NAME + ".Initialize() Tango was initialized!");
            }
        }
        
		/// <summary>
		/// Connect to the Tango Service.
		/// </summary>
        private void _Connect()
        {
            if (TangoServiceAPI.TangoService_connect(m_callbackContext) != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Connect() Could not connect to the Tango Service!");
            }
            else
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
				                                   CLASS_NAME + ".Connect() Tango client connected to service!");
            }
        }
        
		/// <summary>
		/// Disconnect from the Tango Service.
		/// </summary>
        private void _Disconnect()
        {
            if (TangoServiceAPI.TangoService_disconnect() != Common.ErrorType.TANGO_SUCCESS)
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Disconnect() Could not disconnect from the Tango Service!");
            }
            else
            {
                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
				                                   CLASS_NAME + ".Disconnect() Tango client disconnected from service!");
            }
        }

        #region NATIVE_FUNCTIONS
        /// <summary>
        /// Interface for native function calls to Tango Service.
        /// </summary>
        private struct TangoServiceAPI
        {
            #if UNITY_ANDROID
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_initialize ();
            
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_connect (IntPtr callbackContext);
            
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_disconnect ();
            #else
            public static int TangoService_initialize()
            {
                return Common.ErrorType.TANGO_SUCCESS;
            }
			public static  int TangoService_connect (IntPtr callbackContext)
            {
                return Common.ErrorType.TANGO_SUCCESS;
            }
            public static int TangoService_disconnect ()
            {
                return Common.ErrorType.TANGO_SUCCESS;
            }
            #endif
        }
        #endregion // NATIVE_FUNCTIONS
    }
}
