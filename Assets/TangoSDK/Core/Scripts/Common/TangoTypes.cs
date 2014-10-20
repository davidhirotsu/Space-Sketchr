//-----------------------------------------------------------------------
// <copyright file="TangoTypes.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tango
{
    /// <summary>
    /// Represents the ordered point cloud data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TangoXYZij
    {
        [MarshalAs(UnmanagedType.I4)]
        public int version;
        
        [MarshalAs(UnmanagedType.R8)]
        public double timestamp;
        
        [MarshalAs(UnmanagedType.I4)]
        public int xyz_count;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.SysUInt)]
        public IntPtr[] xyz;
        
        [MarshalAs(UnmanagedType.I4)]
        public int ij_rows;
        
        [MarshalAs(UnmanagedType.I4)]
        public IntPtr ij_cols;
        
        public IntPtr ij;
        
        public override string ToString()
        {
            return ("timestamp : " + timestamp + "\n" +
                    "xyz_count : " + xyz_count + "\n" + 
                    "ij_rows : " + ij_rows + "\n" + 
                    "ij_cols : " + ij_cols);
        }
    }

    /// <summary>
    /// Tango event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TangoEvent
    {
        [MarshalAs(UnmanagedType.R8)]
        public double timestamp;
        
        [MarshalAs(UnmanagedType.I4)]
        public TangoEnums.TangoEventType type;
        
        [MarshalAs(UnmanagedType.LPStr)]
        public string description;
    }

    /// <summary>
    /// Tango coordinate frame pair.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TangoCoordinateFramePair
    {
        [MarshalAs(UnmanagedType.I4)]
        public TangoEnums.TangoCoordinateFrameType baseFrame;
        
        [MarshalAs(UnmanagedType.I4)]
        public TangoEnums.TangoCoordinateFrameType targetFrame;
    }
    
    /// <summary>
    /// Data representing a pose from the Tango Service.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TangoPoseData
    {
        [MarshalAs(UnmanagedType.I4)]
        public int version;
        
        [MarshalAs(UnmanagedType.R8)]
        public double timestamp;
        
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.R8)]
        public double[] orientation;
        
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R8)]
        public double[] translation;
        
        [MarshalAs(UnmanagedType.I4)]
        public TangoEnums.TangoPoseStatusType status_code;
        
        [MarshalAs(UnmanagedType.Struct)]
        public TangoCoordinateFramePair framePair;
        
        [MarshalAs(UnmanagedType.I4)]
        public int confidence;
        
        public TangoPoseData()
        {
            version = 0;
            timestamp = 0.0;
            orientation = new double[4];
            translation = new double[3];
            status_code = TangoEnums.TangoPoseStatusType.TANGO_POSE_UNKNOWN;
            framePair.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE;
            framePair.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;
            confidence = 0;
        }
    }
}