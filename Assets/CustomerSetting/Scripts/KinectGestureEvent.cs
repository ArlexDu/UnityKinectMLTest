using System;
using System.Collections;
using UnityEngine;

public class KinectGestureEvent : EventArgs {

	public string name;
	public float confidence; 
	public KinectGestureEvent(string _name, float _confidence) 
	{ 
		name = _name; 
		confidence = _confidence;
	} 
}

public delegate void GestureEvent(object sender,KinectGestureEvent e);
