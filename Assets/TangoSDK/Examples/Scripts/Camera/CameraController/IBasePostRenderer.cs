//-----------------------------------------------------------------------
// <copyright file="IBasePostRenderer.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract base class for anything that should
/// automagically draw objects using OnPostRender.
/// </summary>
public abstract class IBasePostRenderer : MonoBehaviour
{
    /// <summary>
    /// Must be implemented in any derived class.
    /// This function is automatically called by the
    /// camera, as long as it has a CameraRenderer attached.
    /// </summary>
    public abstract void OnPostRender();
}
