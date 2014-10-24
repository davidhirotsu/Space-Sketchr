using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {
	
	public float m_updateFrequency = 1.0f;

    public string m_FPSText;
	private int m_currentFPS;
	private int m_framesSinceUpdate;
	private float m_accumulation;
	private float m_currentTime;
    private string m_currentLibrary = string.Empty;

    private Rect m_button;
    private Rect m_label;
	
	// Use this for initialization
	void Start () 
	{
		m_currentFPS = 0;
		m_framesSinceUpdate = 0;
		m_currentTime = 0.0f;
		m_FPSText = "Current FPS = Calculating";
		Application.targetFrameRate = 30;
        m_button = new Rect(Screen.width * 0.15f - 50, Screen.height * 0.45f - 25, 150.0f, 50.0f);
        m_label = new Rect(Screen.width * 0.025f - 50, Screen.height * 0.96f - 25, 600.0f, 50.0f);
        //m_currentLibrary = Tango.Utilities.GetVersionString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_currentTime += Time.deltaTime;
		++m_framesSinceUpdate;
		m_accumulation += Time.timeScale / Time.deltaTime;
		if(m_currentTime >= m_updateFrequency)
		{
			m_currentFPS = (int)(m_accumulation/m_framesSinceUpdate);
			m_currentTime = 0.0f;
			m_framesSinceUpdate = 0;
			m_accumulation = 0.0f;
			m_FPSText = "Current FPS = " + m_currentFPS;
		}
	}
	
	void OnGUI()
	{
        //if (GUI.Button(m_button, 
        //               (Application.targetFrameRate == 60) ? "Toggle to 30 fps" : "Toggle to 60 fps"))
        //{
        //    if(Application.targetFrameRate == 60)
        //    {
        //        Application.targetFrameRate = 30;
        //    }
        //    else
        //    {
        //        Application.targetFrameRate = 60;
        //    }
        //}

        GUI.Label(m_label,
                  "<size=20>" + m_FPSText + "</size>");
	}
}
