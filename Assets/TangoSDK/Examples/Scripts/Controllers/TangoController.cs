//-----------------------------------------------------------------------
// <copyright file="TangoController.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using Tango;
using System;

/// <summary>
/// This is a basic movement controller based on
/// pose estimation returned from the Tango Service.
/// </summary>
public class TangoController : PoseListener
{
    public float m_movementScale = 1.0f;

    public readonly Quaternion COORDINATE_FRAME_FIX = new Quaternion(Mathf.Sqrt(2) / 2.0f, 0.0f, 0.0f, Mathf.Sqrt(2) / 2.0f);

    private Vector3 m_startingOffset;
	private TangoPoseData m_tangoPoseData;
	private Quaternion m_tangoRotation;
	private Vector3 m_tangoPosition;
	private bool m_isDirty;

    /// <summary>
    /// Initialize the controller.
    /// </summary>
    private void Awake()
    {
		m_isDirty = false;
        m_startingOffset = transform.position;
        m_tangoPoseData = new TangoPoseData();
    }

    /// <summary>
    /// Apply any needed changes to the pose.
    /// </summary>
	private void Update()
	{
		if (m_isDirty)
		{
			// This rotation needs to be put into Unity coordinate space.
			Quaternion rotationFix = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            Quaternion axisFix = Quaternion.Euler(-m_tangoRotation.eulerAngles.x,
                                                  -m_tangoRotation.eulerAngles.z,
                                                  m_tangoRotation.eulerAngles.y);

            transform.rotation = rotationFix * axisFix;
            transform.position = m_tangoPosition + m_startingOffset;
			m_isDirty = false;
		}
	}

    /// <summary>
    /// Handle the callback sent by the Tango Service
    /// when a new pose is sampled.
	/// DO NOT USE THE UNITY API FROM INSIDE THIS FUNCTION!
    /// </summary>
    /// <param name="callbackContext">Callback context.</param>
    /// <param name="pose">Pose.</param>
    protected override void _OnPoseAvailable(IntPtr callbackContext, TangoPoseData pose)
    {
		if (pose != null && pose.status_code == TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID)
        {
            m_tangoPoseData.timestamp = pose.timestamp;
            m_tangoPoseData.version = pose.version;
            
			// Cache the position and rotation to be set in the update function.
			// This needs to be done because this callback does not
			// happen in the main game thread.
			m_tangoPosition = new Vector3((float)pose.translation [0],
			                              (float)pose.translation [2],
			                              (float)pose.translation [1]);

			m_tangoRotation = new Quaternion((float)pose.orientation [0],
			                                 (float)pose.orientation [2], // these rotation values are swapped on purpose
			                                 (float)pose.orientation [1],
			                                 (float)pose.orientation [3]);

            m_isDirty = true;
        } 
		else if (pose.status_code == TangoEnums.TangoPoseStatusType.TANGO_POSE_INVALID && !AutoReset)
        {
            // Try to reset
//            ResetMotionTracking resetter = GetComponent<ResetMotionTracking>();
//
//            if(resetter != null)
//            {
//                resetter.ShowResetButton();
//            }
        }
    }
}