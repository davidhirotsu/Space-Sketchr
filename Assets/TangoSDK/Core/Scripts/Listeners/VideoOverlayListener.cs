//-----------------------------------------------------------------------
// <copyright file="VideoOverlayListener.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System;

public abstract class VideoOverlayListener : MonoBehaviour 
{
	Tango.VideoOverlayProvider.TangoService_onImageAvailable m_onImageAvailable;

	/// <summary>
	/// Sets the callback for image updates.
	/// </summary>
	/// <param name="cameraId">Camera identifier.</param>
	public virtual void SetCallback(Tango.TangoEnums.TangoCameraId cameraId)
	{
		m_onImageAvailable = new Tango.VideoOverlayProvider.TangoService_onImageAvailable(_OnImageAvailable);
		Tango.VideoOverlayProvider.SetCallback(cameraId, m_onImageAvailable);
	}
	
	/// <summary>
	/// Handle the callback sent by the Tango Service
	/// when a new image is sampled.
	/// </summary>
	/// <param name="cameraId">Camera identifier.</param>
	/// <param name="callbackContext">Callback context.</param>
	/// <param name="imageBuffer">Image buffer.</param>
	protected abstract void _OnImageAvailable(IntPtr callbackContext,
	                                          Tango.TangoEnums.TangoCameraId cameraId, 
	                                          Tango.TangoImageBuffer imageBuffer);
}
