//-----------------------------------------------------------------------
// <copyright file="TangoEnums.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using UnityEngine;

namespace Tango
{
	public class TangoEnums
	{
		/// <summary>
		/// Possible states for the motion tracking
		/// </summary>
		public enum TangoPoseStatusType
		{
			TANGO_POSE_INITIALIZING = 0,
			TANGO_POSE_VALID,
			TANGO_POSE_INVALID,
			TANGO_POSE_UNKNOWN
		}
		
		/// <summary>
		/// Coordinate frames provided by the Tango Service.
		/// </summary>
		public enum TangoCoordinateFrameType
		{
			/** Coordinate system for the entire Earth.
		   *  See WGS84: http://en.wikipedia.org/wiki/World_Geodetic_System
		   */
			TANGO_COORDINATE_FRAME_GLOBAL = 0,
			/** Origin within a saved area description */
			TANGO_COORDINATE_FRAME_AREA_DESCRIPTION,
			/** Origin when the device started tracking */
			TANGO_COORDINATE_FRAME_START_OF_SERVICE,
			/** Immediately previous device pose */
			TANGO_COORDINATE_FRAME_PREVIOUS_DEVICE_POSE,
			TANGO_COORDINATE_FRAME_DEVICE,             /**< Device coordinate frame */
			TANGO_COORDINATE_FRAME_IMU,                /**< Inertial Measurement Unit */
			TANGO_COORDINATE_FRAME_DISPLAY,            /**< Display */
			TANGO_COORDINATE_FRAME_CAMERA_COLOR,       /**< Color camera */
			TANGO_COORDINATE_FRAME_CAMERA_DEPTH,       /**< Depth camera */
			TANGO_COORDINATE_FRAME_CAMERA_FISHEYE,     /**< Fisheye camera */
			TANGO_COORDINATE_FRAME_INVALID,
			TANGO_MAX_COORDINATE_FRAME_TYPE            /**< Maximum allowed */
		}
		
		/// <summary>
		/// Enumeration containing the ID used for each
		/// Tango camera.
		/// </summary>
		public enum TangoCameraId
		{
			TANGO_CAMERA_COLOR = 0,
			TANGO_CAMERA_RGBIR,
			TANGO_CAMERA_FISHEYE,
			TANGO_CAMERA_DEPTH,
			TANGO_MAX_CAMERA_ID
		}

		/// <summary>
		/// Enumeration containing events provided by the
		/// Tango Service.
		/// </summary>
		public enum TangoEventType
		{
			TANGO_EVENT_UNKNOWN,        /**< TODO */
			TANGO_EVENT_STATUS_UPDATE,  /**< TODO */
			TANGO_EVENT_ADF_UPDATE,
		}
	}
}