using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearTime : MonoBehaviour {
	public Wk6Manager wk6Manager;

	void Update()
	{
		GetComponent<Text> ().text = printText();
	}

	string printText()
	{
		string text;
		text = "Time:"+ wk6Manager.DataTime + "\n" + "Weather:" + wk6Manager.weather;

		return text;
	}
}
