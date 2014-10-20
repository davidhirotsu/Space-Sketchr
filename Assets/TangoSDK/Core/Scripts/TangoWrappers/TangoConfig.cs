//-----------------------------------------------------------------------
// <copyright file="TangoConfig.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using Tango;
using UnityEngine;

namespace Tango
{
	/// <summary>
	/// Functionality for interacting with the Tango Service
	/// configuration.
	/// </summary>
	public class TangoConfig
	{
		/// <summary>
		/// Key/Value pairs supported by the Tango Service.
		/// </summary>	
	    public struct Keys
	    {
	        // Motion Tracking
	        public static readonly string ENABLE_MOTION_TRACKING_BOOL = "config_enable_motion_tracking";
	        public static readonly string ENABLE_MOTION_TRACKING_AUTO_RESET_BOOL = "config_enable_auto_reset";

	        // Area Learning
	        public static readonly string ENABLE_AREA_LEARNING_BOOL = "config_enable_learning_mode";
	        public static readonly string LOAD_AREA_DESCRIPTION_UUID_STRING = "config_load_area_description_uuid";

	        // Depth Perception
	        public static readonly string ENABLE_DEPTH_PERCEPTION_BOOL = "config_enable_depth";

	        // Utilities
	        public static readonly string ENABLE_DATASET_RECORDING = "config_enable_dataset_recording";
	        public static readonly string GET_TANGO_SERVICE_VERSION_STRING = "tango_service_library_version";
	    }

		private static readonly string CLASS_NAME = "TangoConfig";
	    private static readonly string NO_CONFIG_FOUND = "No config file found.";

	    private static IntPtr m_tangoConfig;

		/// <summary>
		/// Gets the handle to the current Tango configuration.
		/// </summary>
		/// <returns> Handle to the Tango configuration.</returns>
	    public static IntPtr GetConfig()
	    {
	        return m_tangoConfig;
	    }
	     
		/// <summary>
		/// Allocate a Tango configuration object.
		/// </summary>
	    public static void Allocate()
	    {
	        m_tangoConfig = TangoConfigAPI.TangoConfig_alloc();
	        Debug.Log("Allocating Tango Service config! --- " + m_tangoConfig.ToString());

	        if (m_tangoConfig == IntPtr.Zero)
	        {
	            DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Allocate() The Tango Config was not allocated!");
	        } 
	        else
	        {
	            DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
				                                   CLASS_NAME + ".Allocate() The Tango Config was allocated!");
	        }
	    }

		/// <summary>
		/// Lock the current Tango configuration.
		/// </summary>
	    public static void Lock()
	    {
	        if (m_tangoConfig != IntPtr.Zero)
	        {
	            if (TangoConfigAPI.TangoService_lockConfig(m_tangoConfig) != Common.ErrorType.TANGO_SUCCESS)
	            {
	                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
					                                   CLASS_NAME + ".Lock() Unable to lock the Tango Config!");
	            }
	            else
	            {
	                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
					                                   CLASS_NAME + ".Lock() Congif was locked!");
	            }
	        } 
	        else
	        {
	            DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Lock() No allocated Tango Config found!");
	        }
	    }

		/// <summary>
		/// Unlock the current Tango configuration.
		/// </summary>
	    public static void Unlock()
	    {
	        if (m_tangoConfig != IntPtr.Zero)
	        {
	            if (TangoConfigAPI.TangoService_unlockConfig() != Common.ErrorType.TANGO_SUCCESS)
	            {
	                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
					                                   CLASS_NAME + ".Unlock() Unable to unlock the Tango Config!");
	            }
	            else
	            {
	                DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO,
					                                   CLASS_NAME + ".Unlock() Congif was unlocked!");
	            }
	        } 
	        else
	        {
	            DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Unlock() No allocated Tango Config found!");
	        }
	    }

		/// <summary>
		/// Fills out a given Tango configuration with the currently set configuration settings.
		/// </summary>
	    public static void FillOut()
	    {
	        TangoConfigAPI.TangoService_getConfig(0, m_tangoConfig);
	    }

		/// <summary>
		/// Deallocate a Tango configuration object.
		/// </summary>
	    public static void Free()
	    {
	        if (m_tangoConfig != IntPtr.Zero)
	        {
	            TangoConfigAPI.TangoConfig_free(m_tangoConfig);
	        } 
	        else
	        {
	            DebugLogger.GetInstance.WriteToLog(DebugLogger.EDebugLevel.DEBUG_CRITICAL,
				                                   CLASS_NAME + ".Free() No allocated Tango Config found!");
	        }
	    }

		/// <summary>
		/// Gets a string representing the current settings
		/// of the Tango configuration.
		/// </summary>
		/// <returns> String representing the current settings.
		/// Null if no configuration is found.</returns>
	    public static string GetSettings()
	    {
	        if (m_tangoConfig != IntPtr.Zero)
	        {
	            return TangoConfigAPI.TangoConfig_toString(m_tangoConfig);
	        } 
	        else
	        {
	            return NO_CONFIG_FOUND;
	        }
	    }

		/// <summary>
		/// Sets the value of a boolean key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if bool was set, <c>false</c> otherwise.</returns>
		/// <param name="key"> Key.</param>
		/// <param name="value"> If set to <c>true</c> value.</param>
	    public static bool SetBool(string key, bool value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
	            return (TangoConfigAPI.TangoConfig_setBool(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }

	        return false;
	    }

		/// <summary>
		/// Sets the value of an int32 key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if int32 was set, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool SetInt32(string key, Int32 value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
	            return (TangoConfigAPI.TangoConfig_setInt32(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }
	        
	        return false;
	    }

		/// <summary>
		/// Sets the value of an int64 key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if int64 was set, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool SetInt64(string key, Int64 value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
	            return (TangoConfigAPI.TangoConfig_setInt64(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }
	        
	        return false;
	    }
	    
		/// <summary>
		/// Sets the value of a double key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if double was set, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool SetDouble(string key, double value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
				return (TangoConfigAPI.TangoConfig_setDouble(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }
	        
	        return false;
	    }
	    
		/// <summary>
		/// Sets the value of a string key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if string was set, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool SetString(string key, string value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
				return (TangoConfigAPI.TangoConfig_setString(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }
	        
	        return false;
	    }
	    
		/// <summary>
		/// Gets the value of a bool key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool GetBool(string key,ref bool value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
				return (TangoConfigAPI.TangoConfig_getBool(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }

	        return false;
	    }
	    
		/// <summary>
		/// Gets the value of an int32 kay/value pair.
		/// </summary>
		/// <returns><c>true</c>, if int32 was gotten, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool GetInt32(string key, ref Int32 value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
				return (TangoConfigAPI.TangoConfig_getInt32(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }
	        
	        return false;
	    }
	    
		/// <summary>
		/// Gets the value of an int64 key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if int64 was gotten, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool GetInt64(string key, ref Int64 value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
				return (TangoConfigAPI.TangoConfig_getInt64(m_tangoConfig, key, value) == Common.ErrorType.TANGO_SUCCESS);
	        }
	        
	        return false;
	    }
	    
		/// <summary>
		/// Gets the value of a double key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if double was gotten, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool GetDouble(string key, ref double value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
	            return (TangoConfigAPI.TangoConfig_getDouble(m_tangoConfig, key, value) == 0);
	        }
	        
	        return false;
	    }
	    
		/// <summary>
		/// Gets the value of a string key/value pair.
		/// </summary>
		/// <returns><c>true</c>, if string was gotten, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
	    public static bool GetString(string key, ref string value)
	    {
			if (m_tangoConfig != IntPtr.Zero)
	        {
	            UInt32 stringLength = 512;
	            //char[] tempString = new char[512];
	            StringBuilder tempString = new StringBuilder(512); 

				bool returnValue = (TangoConfigAPI.TangoConfig_getString(m_tangoConfig, key, tempString, stringLength) == Common.ErrorType.TANGO_SUCCESS);
	        
	            if(returnValue)
	            {
	                value = tempString.ToString();
	                return true;
	            }
	        }
	        
	        return false;
	    }


	    /// <summary>
		/// Interface for the Tango Service API.
		/// </summary>
	    private struct TangoConfigAPI
	    {
	#if UNITY_ANDROID
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoService_lockConfig(IntPtr tangoConfig);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoService_unlockConfig ();
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern IntPtr TangoConfig_alloc();
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern void TangoConfig_free(IntPtr tangoConfig);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern string TangoConfig_toString(IntPtr TangoConfig);

	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_setBool(IntPtr tangoConfig,
	                                                     [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                     bool value);
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoService_getConfig(int config_type, IntPtr config);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_setInt32(IntPtr tangoConfig,
	                                                     [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                     Int32 value);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_setInt64(IntPtr tangoConfig,
	                                                      [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                      Int64 value);

	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_setDouble(IntPtr tangoConfig,
	                                                      [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                      double value);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_setString(IntPtr tangoConfig,
	                                                       [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                       [MarshalAs(UnmanagedType.LPStr)] string value);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_getBool(IntPtr tangoConfig,
	                                                     [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                     [In, Out] bool value);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_getInt32(IntPtr tangoConfig,
	                                                     [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                     [In, Out] Int32 value);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_getInt64(IntPtr tangoConfig,
	                                                      [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                      [In, Out] Int64 value);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_getDouble(IntPtr tangoConfig,
	                                                      [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                      [In, Out] double value);
	        
	        [DllImport(Common.TANGO_UNITY_DLL)]
	        public static extern int TangoConfig_getString(IntPtr tangoConfig,
	                                                       [MarshalAs(UnmanagedType.LPStr)] string key,
	                                                       [In, Out] StringBuilder value,
	                                                       UInt32 size);
	#else
	        public static int TangoService_lockConfig(IntPtr tangoConfig)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoService_unlockConfig ()
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static IntPtr TangoConfig_alloc()
	        {
	            return IntPtr.Zero;
	        }
	        public static void TangoConfig_free(IntPtr tangoConfig)
	        {

			}
			public static int TangoService_getConfig(int config_type, IntPtr config)
			{
				return Common.ErrorType.TANGO_SUCCESS;
			}
	        public static string TangoConfig_toString(IntPtr TangoConfig)
	        {
	            return "Editor Mode";
	        }
	        public static int TangoConfig_setBool(IntPtr tangoConfig,
	                                              string key,
	                                              bool value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_setInt32(IntPtr tangoConfig,
	                                               [MarshalAs(UnmanagedType.LPStr)] string key,
	                                               Int32 value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_setInt64(IntPtr tangoConfig,
	                                               string key,
	                                               Int64 value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_setDouble(IntPtr tangoConfig,
	                                                string key,
	                                                double value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_setString(IntPtr tangoConfig,
	                                                string key,
	                                                string value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_getBool(IntPtr tangoConfig,
	                                              string key,
	                                              bool value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_getInt32(IntPtr tangoConfig,
	                                               string key,
	                                               Int32 value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_getInt64(IntPtr tangoConfig,
	                                               string key,
	                                               Int64 value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_getDouble(IntPtr tangoConfig,
	                                                string key,
	                                                double value)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	        public static int TangoConfig_getString(IntPtr tangoConfig,
	                                                string key,
	                                                string value,
	                                                UInt32 size)
	        {
	            return Common.ErrorType.TANGO_SUCCESS;
	        }
	#endif
	    }
	}
}