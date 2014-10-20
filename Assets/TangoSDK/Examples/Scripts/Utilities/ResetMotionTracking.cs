using UnityEngine;
using System.Collections;

public class ResetMotionTracking : MonoBehaviour
{
    private const int RESET_BUTTON_WIDTH = 200;
    private const int RESET_BUTTON_HEIGHT = 75;

    private bool m_shouldReset;
    private Rect m_resetButtonRect;

    public void ShowResetButton()
    {
        m_shouldReset = true;
    }

	void Start ()
    {
        m_shouldReset = false;
        m_resetButtonRect = new Rect(Screen.width * 0.5f - RESET_BUTTON_WIDTH,
                                     Screen.height * 0.5f - RESET_BUTTON_HEIGHT,
                                     RESET_BUTTON_WIDTH,
                                     RESET_BUTTON_HEIGHT);

	}
	
    private void OnGUI()
    {
        if (m_shouldReset)
        {
            if(GUI.Button(m_resetButtonRect, "<size=30>RESET</size>"))
            {
                Tango.PoseProvider.ResetMotionTracking();
                m_shouldReset = false;
            }
        }
    }
}
