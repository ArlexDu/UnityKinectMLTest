using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using System.IO;
using Microsoft.Kinect.VisualGestureBuilder;

public class GestureDetectors : MonoBehaviour {
	VisualGestureBuilderDatabase _gestureDatabase;
	VisualGestureBuilderFrameSource _gestureFrameSource;
	VisualGestureBuilderFrameReader _gestureFrameReader;
	KinectSensor _kinect;
	Gesture wavehand_right;
	DiscreteGestureResult result;
	// Use this for initialization
	void Start () {
		_kinect = KinectSensor.GetDefault ();
		_gestureDatabase = VisualGestureBuilderDatabase.Create (Application.streamingAssetsPath+"/wavehand_Right.gba");
		_gestureFrameSource = VisualGestureBuilderFrameSource.Create (_kinect,0);
		foreach (var gesture in _gestureDatabase.AvailableGestures) {
			_gestureFrameSource.AddGesture (gesture);
			print("gesture available name : "+gesture.Name);
			if (gesture.Name == "wavehand_right") {
				wavehand_right = gesture;
			}
		}
		_gestureFrameReader = _gestureFrameSource.OpenReader ();
		_gestureFrameReader.IsPaused = true;
	}
		
	
	// Update is called once per frame
	void Update () {
		
	}


	void _gestureFrameReader_FrameArrived(object sender , VisualGestureBuilderFrameArrivedEventArgs e){
		VisualGestureBuilderFrameReference frameReference = e.FrameReference;
		using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame ()) {
			if (frame != null && frame.DiscreteGestureResults != null) {
				result = frame.DiscreteGestureResults [wavehand_right];
			}
			if (result == null) {
				return;
			}
			if (result.Detected == true && result.Confidence > 0.95) {
			}
		}
	}
}
