using UnityEngine;
using System;
using System.Collections;
using Tango;

public class LogTangoEvents : TangoEventListener
{
    private string m_lastTangoEventIssued;

	// Use this for initialization
	void Start ()
	{
        m_lastTangoEventIssued = string.Empty;
	}
	
    protected override void _onEventAvailable(IntPtr callbackContext, TangoEvent tangoEvent)
    {
        Debug.Log("Tango event fired : " + tangoEvent.event_value);
    }
}
