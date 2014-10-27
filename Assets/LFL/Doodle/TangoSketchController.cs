using System.Collections;
using UnityEngine;
using Tango;
using System;

/// <summary>
/// This is a basic movement controller based on
/// pose estimation returned from the Tango Service.
/// </summary>
public class TangoSketchController : PoseListener
{
	public enum TrackingTypes
	{
		NONE,
		MOTION,
		ADF,
		RELOCALIZED
	}
	
	public float m_movementScale = 1.0f;
	public bool m_useADF = false;
	
	public readonly Quaternion COORDINATE_FRAME_FIX = new Quaternion(Mathf.Sqrt(2) / 2.0f, 0.0f, 0.0f, Mathf.Sqrt(2) / 2.0f);
	
	private TangoApplication m_tangoApplication;
	private Vector3 m_startingOffset;
	
	// Tango pose data for debug logging and transform update.
	// Index 0: device with respect to start frame.
	// Index 1: device with respect to adf frame.
	// Index 2: start with respect to adf frame.
	private float[] m_frameDeltaTime;
	private float[] m_prevFrameTimestamp;
	private int[] m_frameCount;
	private TangoEnums.TangoPoseStatusType[] m_status;
	private Quaternion[] m_tangoRotation;
	private Vector3[] m_tangoPosition;
	
	private bool m_isRelocalized = false;
	private bool m_isDirty = false;
	
	private string m_applicationVersion;
	
	/// <summary>
	/// Initialize the controller.
	/// </summary>
	private void Awake()
	{
		m_isDirty = false;
		m_startingOffset = transform.position;
		m_frameDeltaTime = new float[3];
		m_prevFrameTimestamp = new float[3];
		m_frameCount = new int[3];
		m_status = new TangoEnums.TangoPoseStatusType[3];
		m_tangoRotation = new Quaternion[3];
		m_tangoPosition = new Vector3[3];
	}
	
	private void Start()
	{
		m_tangoApplication = FindObjectOfType<TangoApplication>();
		
		if(m_tangoApplication != null)
		{
			m_tangoApplication.InitApplication();
			
			if(m_useADF)
			{
				// Query the full adf list.
				PoseProvider.RefreshADFList();
				// loading last recorded ADF
				string uuid = PoseProvider.GetLatestADFUUID().GetStringDataUUID();
				m_tangoApplication.SetFunctionality(uuid);
			}
			else
			{
				m_tangoApplication.SetFunctionality(string.Empty);
			}
			
			m_tangoApplication.ConnectToService();
			
			m_applicationVersion = m_tangoApplication.GetAndroidApplicationVersion();
		}
		else
		{
			Debug.Log("No Tango Manager found in scene.");
		}
		
	}
	
	/// <summary>
	/// Apply any needed changes to the pose.
	/// </summary>
	private void Update()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (m_isDirty)
		{
			if (!m_isRelocalized) 
			{
				// This rotation needs to be put into Unity coordinate space.
				Quaternion rotationFix = Quaternion.Euler(90.0f, 0.0f, 0.0f);
				Quaternion axisFix = Quaternion.Euler(-m_tangoRotation[0].eulerAngles.x,
				                                      -m_tangoRotation[0].eulerAngles.z,
				                                      m_tangoRotation[0].eulerAngles.y);
				
				transform.rotation = rotationFix * axisFix;
				transform.position = (m_tangoPosition[0] * m_movementScale) + m_startingOffset;
			}
			else 
			{
				// This rotation needs to be put into Unity coordinate space.
				Quaternion rotationFix = Quaternion.Euler(90.0f, 0.0f, 0.0f);
				Quaternion axisFix = Quaternion.Euler(-m_tangoRotation[1].eulerAngles.x,
				                                      -m_tangoRotation[1].eulerAngles.z,
				                                      m_tangoRotation[1].eulerAngles.y);
				
				transform.rotation = rotationFix * axisFix;
				transform.position = (m_tangoPosition[1] * m_movementScale) + m_startingOffset;
			}
			m_isDirty = false;
		}
		#else
		Vector3 tempPosition = transform.position;
		Quaternion tempRotation = transform.rotation;
		PoseProvider.GetMouseEmulation(ref tempPosition, ref tempRotation);
		
		transform.rotation = tempRotation;
		transform.position = tempPosition;
		#endif
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
		int currentIndex = 0;
		if (pose == null)
		{
			Debug.Log("TangoPoseDate is null.");
			return;
		}
		// The callback pose is for device with respect to start of service pose.
		if (pose.framePair.baseFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE &&
		    pose.framePair.targetFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE)
		{
			currentIndex = 0;
		}
		// The callback pose is for device with respect to area description file pose.
		else if (pose.framePair.baseFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION &&
		         pose.framePair.targetFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE)
		{
			currentIndex = 1;
		} 
		// The callback pose is for start of service with respect to area description file pose.
		else if (pose.framePair.baseFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION &&
		         pose.framePair.targetFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE)
		{
			currentIndex = 2;
		}
		
		// Cache the position and rotation to be set in the update function.
		// This needs to be done because this callback does not
		// happen in the main game thread.
		m_tangoPosition[currentIndex] = new Vector3((float)pose.translation [0],
		                                            (float)pose.translation [2],
		                                            (float)pose.translation [1]);
		
		m_tangoRotation[currentIndex] = new Quaternion((float)pose.orientation [0],
		                                               (float)pose.orientation [2], // these rotation values are swapped on purpose
		                                               (float)pose.orientation [1],
		                                               (float)pose.orientation [3]);
		
		// Reset the current status frame count if the status code changed.
		if (pose.status_code != m_status[currentIndex]) {
			m_frameCount[currentIndex] = 0;
		}
		m_status [currentIndex] = pose.status_code;
		m_frameCount[currentIndex]++;
		
		// Compute delta frame timestamp.
		m_frameDeltaTime[currentIndex] = (float)pose.timestamp - m_prevFrameTimestamp[currentIndex];
		m_prevFrameTimestamp [currentIndex] = (float)pose.timestamp;
		
		// Switch m_isDirty to true, so that the new pose get rendered in update.
		m_isDirty = true;
	}
	
//	void OnGUI()
//	{
//		Color oldColor = GUI.color;
//		GUI.color = Color.black;
//		
//		GUI.Label(new Rect(Common.UI_LABEL_START_X, 
//		                   Common.UI_LABEL_START_Y, 
//		                   Common.UI_LABEL_SIZE_X , 
//		                   Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_TANGO_SERVICE_VERSION, m_applicationVersion) + "</size>");
//		
//		// MOTION TRACKING
//		GUI.Label( new Rect(Common.UI_LABEL_START_X, 
//		                    Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET, 
//		                    Common.UI_LABEL_SIZE_X , 
//		                    Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_TARGET_TO_BASE_FRAME,
//		                                                     TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE,
//		                                                     TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE) + "</size>");
//		
//		GUI.Label( new Rect(Common.UI_LABEL_START_X, 
//		                    Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET * 2, 
//		                    Common.UI_LABEL_SIZE_X , 
//		                    Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_STATUS,
//		                                                     m_status[0],
//		                                                     m_frameCount[0],
//		                                                     m_frameDeltaTime[0],
//		                                                     m_tangoPosition[0],
//		                                                     m_tangoRotation[0]) + "</size>");
//		
//		// ADF
//		GUI.Label( new Rect(Common.UI_LABEL_START_X, 
//		                    Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET * 3, 
//		                    Common.UI_LABEL_SIZE_X , 
//		                    Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_TARGET_TO_BASE_FRAME,
//		                                                     TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE,
//		                                                     TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION) + "</size>");
//		
//		GUI.Label( new Rect(Common.UI_LABEL_START_X, 
//		                    Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET * 4, 
//		                    Common.UI_LABEL_SIZE_X , 
//		                    Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_STATUS,
//		                                                     m_status[1],
//		                                                     m_frameCount[1],
//		                                                     m_frameDeltaTime[1],
//		                                                     m_tangoPosition[1],
//		                                                     m_tangoRotation[1]) + "</size>");
//		
//		// RELOCALIZATION
//		GUI.Label( new Rect(Common.UI_LABEL_START_X, 
//		                    Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET * 5, 
//		                    Common.UI_LABEL_SIZE_X , 
//		                    Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_TARGET_TO_BASE_FRAME,
//		                                                     TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION,
//		                                                     TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE) + "</size>");
//		
//		GUI.Label( new Rect(Common.UI_LABEL_START_X, 
//		                    Common.UI_LABEL_START_Y + Common.UI_LABEL_OFFSET * 6, 
//		                    Common.UI_LABEL_SIZE_X , 
//		                    Common.UI_LABEL_SIZE_Y), "<size=15>" + String.Format(Common.UX_STATUS,
//		                                                     m_status[2],
//		                                                     m_frameCount[2],
//		                                                     m_frameDeltaTime[2],
//		                                                     m_tangoPosition[2],
//		                                                     m_tangoRotation[2]) + "</size>");
//		
//		GUI.color = oldColor;
//	}
}