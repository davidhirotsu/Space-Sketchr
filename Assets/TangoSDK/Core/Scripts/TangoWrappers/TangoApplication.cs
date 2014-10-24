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
using System.Linq;
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
		public bool m_enableAreaLearning = true;
		public TangoEnums.TangoConfigType m_configType = TangoEnums.TangoConfigType.TANGO_CONFIG_DEFAULT;
		public bool m_testADFExporting = true;
		private bool m_areaLearningSetSucces = false;

		private const string CLASS_NAME = "TangoApplication";
        private const string ANDROID_PRO_LABEL_TEXT = "<size=30>Tango plugin requires Unity Android Pro!</size>";
        private const float ANDROID_PRO_LABEL_PERCENT_X = 0.5f;
        private const float ANDROID_PRO_LABEL_PERCENT_Y = 0.5f;
        private const float ANDROID_PRO_LABEL_WIDTH = 200.0f;
        private const float ANDROID_PRO_LABEL_HEIGHT = 200.0f;
        private const string DEFAULT_AREA_DESCRIPTION = "/sdcard/defaultArea";
        private const string MOTION_TRACKING_LOG_PREFIX = "Motion tracking mode : ";
		private const int MINIMUM_API_VERSION = 1978;

        private DepthProvider m_depthProvider;
        private string m_tangoServiceVersion;

		private IntPtr m_callbackContext = IntPtr.Zero;

        /// <summary>
        /// Initialize application and providers.
        /// </summary>
        public void InitApplication()
        {
            _TangoInitialize();
			TangoConfig.InitConfig(m_configType);
        }

		public void SetFunctionality(string UUID)
		{
			_InitializeMotionTracking(UUID);
			_InitializeDepth();
			_InitializeOverlay();
			_SetEventCallbacks();
		}

		public void ConnectToService()
		{
			m_tangoServiceVersion = "Not Initialized";
			TangoConfig.GetString(TangoConfig.Keys.GET_TANGO_SERVICE_VERSION_STRING, ref m_tangoServiceVersion);
			_TangoConnect();
		}

		/// <summary>
        /// Perform constructor logic.
        /// </summary>
        private void Start()
        {
			// TODO is this the only instance of TangoApplication
			// if not, destroy any other instances
//			if(FindObjectsOfType<TangoApplication>().Contains(this))
//			{
//				GameObject.DestroyImmediate(gameObject);
//			}

        }
        
        /// <summary>
        /// Perform updates for the next frame after the
        /// normal update has run. This loads next frame.
        /// </summary>
        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
			{
				SuspendTangoServices();
				Application.Quit();
			}
        }

		/// <summary>
		/// Helper method that will resume the tango services on App Resume.
		/// Locks the config again and connects the service.
		/// </summary>
		private void ResumeTangoServices()
		{
			_TangoConnect();
		}

		/// <summary>
		/// Helper method that will suspend the tango services on App Suspend.
		/// Unlocks the tango config and disconnects the service.
		/// </summary>
		private void SuspendTangoServices()
		{
			_TangoDisconnect();
		}

		/// <summary>
		/// Callback for when Unity app goes to background/foreground states.
		/// </summary>
		/// <param name="didPause">If <c>true</c> application is in the background.</param>
		private void OnApplicationPause(bool isPaused)
		{
			if(!isPaused)
			{
				ResumeTangoServices();
			}
			else
			{
				SuspendTangoServices();
			}
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

		IEnumerator QuitApplication()
		{
			yield return new WaitForSeconds(3.0f);
			Debug.Log("-------------------------Quitting");
			Application.Quit();
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
		/// Initialize motion tracking.
		/// </summary>
		private void _InitializeMotionTracking(string UUID)
        {
			System.Collections.Generic.List<TangoCoordinateFramePair> framePairs = new System.Collections.Generic.List<TangoCoordinateFramePair>();
			if (TangoConfig.SetBool(TangoConfig.Keys.ENABLE_AREA_LEARNING_BOOL, m_enableAreaLearning) && m_enableAreaLearning)
			{
				m_areaLearningSetSucces = true;
			}
			else
			{
				m_areaLearningSetSucces = false;
			}

            
            if (TangoConfig.SetBool(TangoConfig.Keys.ENABLE_MOTION_TRACKING_BOOL, m_enableMotionTracking) && m_enableMotionTracking)
            {
				TangoCoordinateFramePair motionTracking;
				motionTracking.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE;
				motionTracking.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;
                
				if(!string.IsNullOrEmpty(UUID))
				{
					TangoConfig.SetString(TangoConfig.Keys.LOAD_AREA_DESCRIPTION_UUID_STRING, UUID);
					
					TangoCoordinateFramePair areaDescription;
					areaDescription.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION;
					areaDescription.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;

					TangoCoordinateFramePair startToADF;
					startToADF.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION;
					startToADF.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE;
					
					framePairs.Add(areaDescription);
					framePairs.Add(startToADF);
				}

				framePairs.Add(motionTracking);
            }

			if(framePairs.Count > 0)
			{
				_SetMotionTrackingCallbacks(framePairs.ToArray());
			}
            
			TangoConfig.SetBool(TangoConfig.Keys.ENABLE_MOTION_TRACKING_AUTO_RECOVERY_BOOL, m_motionTrackingAutoReset);
        }

        /// <summary>
        /// Initialize depth perception.
        /// </summary>
        private void _InitializeDepth()
        {
            if (TangoConfig.SetBool(TangoConfig.Keys.ENABLE_DEPTH_PERCEPTION_BOOL, m_enableDepth) && m_enableDepth)
            {
                _SetDepthCallbacks();
            }
			bool depthConfigValue = false;
			TangoConfig.GetBool(TangoConfig.Keys.ENABLE_DEPTH_PERCEPTION_BOOL, ref depthConfigValue);
			DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO, "TangoConfig bool for key: " + TangoConfig.Keys.ENABLE_DEPTH_PERCEPTION_BOOL
			                                   + " has value set of: " + depthConfigValue);
        }

        /// <summary>
        /// Initialize the RGB overlay.
        /// </summary>
        private void _InitializeOverlay()
        {
            //TODO
        }
        
		/// <summary>
		/// Initialize the Tango Service.
		/// </summary>
        private void _TangoInitialize()
        {
			if(_IsValidAPIVersion())
			{
	            if (TangoServiceAPI.TangoService_initialize( IntPtr.Zero, IntPtr.Zero) != Common.ErrorType.TANGO_SUCCESS)
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
			else
			{
				DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Initialize() Invalid API version. please update to " + MINIMUM_API_VERSION + " or higher.");
			}
        }
        
		/// <summary>
		/// Connect to the Tango Service.
		/// </summary>
        private void _TangoConnect()
        {
			if (TangoServiceAPI.TangoService_connect(m_callbackContext, TangoConfig.GetConfig()) != Common.ErrorType.TANGO_SUCCESS)
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
        private void _TangoDisconnect()
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

		/// <summary>
		/// Check the API version to see if it is valid.
		/// </summary>
		/// <returns><c>true</c>, if the API is a valid version, <c>false</c> otherwise.</returns>
		private bool _IsValidAPIVersion()
		{
		#if UNITY_ANDROID && !UNITY_EDITOR
			// Get the activity.
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

			// Get version number from the package manager.
			AndroidJavaObject packageManager = unityActivity.Call<AndroidJavaObject>("getPackageManager");
			AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", "com.projecttango.tango", 0);
			int versionCode = packageInfo.Get<int>("versionCode");
			
			DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
			                                   CLASS_NAME + "._IsValidAPIVersion() Current API Version = " + versionCode);

			return (versionCode >= MINIMUM_API_VERSION);
		#else
			return true;
		#endif
		}

		private void _Quit()
		{
		#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass system = new AndroidJavaClass("java.lang.System");
			system.CallStatic("exit", 0);
		#endif
		}

		/// <summary>
		/// Gets the user-facing android application version.
		/// </summary>
		/// <returns>The android application version.</returns>
		public string GetAndroidApplicationVersion()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

			string packageName = unityActivity.Call<string>("getPackageName");

			// Get version number from the package manager.
			AndroidJavaObject packageManager = unityActivity.Call<AndroidJavaObject>("getPackageManager");
			AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);

			return packageInfo.Get<string>("versionName");
#else
			return "Not Android Build";
#endif
		}

        #region NATIVE_FUNCTIONS
        /// <summary>
        /// Interface for native function calls to Tango Service.
        /// </summary>
        private struct TangoServiceAPI
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            [DllImport(Common.TANGO_UNITY_DLL)]
			public static extern int TangoService_initialize (IntPtr JNIEnv, IntPtr appContext);
            
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_connect (IntPtr callbackContext, IntPtr config);
            
            [DllImport(Common.TANGO_UNITY_DLL)]
            public static extern int TangoService_disconnect ();
            #else
            public static int TangoService_initialize(IntPtr JNIEnv, IntPtr appContext)
            {
                return Common.ErrorType.TANGO_SUCCESS;
            }
			public static  int TangoService_connect (IntPtr callbackContext, IntPtr config)
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
