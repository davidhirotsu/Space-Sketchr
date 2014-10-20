//-----------------------------------------------------------------------
// <copyright file="VideoOverlayPlaneRender.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using Tango;

/// <summary>
/// Responsible for drawing the AR screen video overlay.
/// </summary>
public class VideoOverlayPlaneRender : IBasePreRenderer
{
    private int TEX_WIDTH = Screen.width;
    private int TEX_HEIGHT = Screen.height;
    private const TextureFormat TEX_FORMAT = TextureFormat.RGB565;

    private Texture2D m_texture;

    /// <summary>
    /// Perform any Camera.OnPreRender() logic
    /// here.
    /// </summary>
    public sealed override void OnPreRender()
    {
		VideoOverlayProvider.RenderLatestFrame(TangoEnums.TangoCameraId.TANGO_CAMERA_COLOR);
        GL.InvalidateState();
    }

	public void SetTargetCameraTexture(TangoEnums.TangoCameraId cameraId)
    {
        VideoOverlayProvider.ConnectTexture(cameraId,
                                            m_texture.GetNativeTextureID());
    }

    /// <summary>
    /// Initialize this instance.
    /// </summary>
    private void Awake()
    {
        m_texture = new Texture2D(TEX_WIDTH, TEX_HEIGHT, TEX_FORMAT, false);
        renderer.material.mainTexture = m_texture;
    }
}