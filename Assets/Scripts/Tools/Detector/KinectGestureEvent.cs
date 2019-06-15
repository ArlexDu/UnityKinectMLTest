using System;
using System.Collections;
using UnityEngine;

public class KinectGestureEvent : EventArgs {
	//gesture name
	public string name;
	// gestrue detect confident
	public float confidence; 
	//is gesture acting
	public bool doing;
	//options
	private Hashtable options;

	public KinectGestureEvent(string _name, float _confidence) 
	{ 
		name = _name; 
		confidence = _confidence;
		doing = false;
	}

	public void addOpt(string key,string value){
		if (options == null) {
			options = new Hashtable ();
		} 
		options.Add (key,value);
	}

	public string getOpt(string key){
		if (options != null) {
			return (string)options[key];
		}
		return null;
	}
}

public delegate void GestureEvent(object sender,KinectGestureEvent e);
