using UnityEngine;
using System.Collections;
using System;

public class ConvertHelper
{
	public static bool GetIntFromString(string text, out int value)
	{
		bool wasSuccessful = false;
		int intValue = 0;

		try
		{
			intValue = Convert.ToInt32(text);
		}
		catch (FormatException e)
		{
			// TODO : log this
		}
		catch (OverflowException e)
		{
			// TODO : log this
		}
		finally
		{
			wasSuccessful = true;
			value = intValue;
		}

		return wasSuccessful;
	}
}
