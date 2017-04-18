using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
using Windows.Kinect; 
using Microsoft.Kinect.VisualGestureBuilder; 
// Adapted from DiscreteGestureBasics-WPF by Momo the Monster 2014-11-25 
// For Helios Interactive - http://heliosinteractive.com 
public class GestureSourceManager : MonoBehaviour { 
public BodySourceManager _BodySource;
public string databasePath; 
private KinectSensor _Sensor; 
private VisualGestureBuilderFrameSource _Source; 
private VisualGestureBuilderFrameReader _Reader; 
private VisualGestureBuilderDatabase _Database;
// Gesture Detection Events 
public event GestureEvent OnGesture;
private static GestureSourceManager instance = null;


	public static GestureSourceManager Instance{
		get{
			return instance;
		}
	}
// Use this for initialization 
void Start() { 
	_Sensor = KinectSensor.GetDefault(); 
	if (_Sensor != null) { 
		if (!_Sensor.IsOpen) {
			_Sensor.Open(); 
		}
// Set up Gesture Source 
		_Source = VisualGestureBuilderFrameSource.Create(_Sensor, 0); 
// open the reader for the vgb frames 
		_Reader = _Source.OpenReader(); 
		if (_Reader != null) { 
			_Reader.IsPaused = true;
			_Reader.FrameArrived += GestureFrameArrived; 
		} 
// load the 'Seated' gesture from the gesture database 
		string path = System.IO.Path.Combine(Application.streamingAssetsPath, databasePath);
			Debug.Log ("database path is "+path);
		_Database = VisualGestureBuilderDatabase.Create(path); 
// Load all gestures 
		IList<Gesture> gesturesList = _Database.AvailableGestures; 
		for (int g = 0; g < gesturesList.Count; g++) { 
			Gesture gesture = gesturesList[g];
				Debug.Log ("database name is "+ gesture.Name);
			_Source.AddGesture(gesture); 
		} 
	}
	instance = this;
} 
// Public setter for Body ID to track 
public void SetBody(ulong id) { 
	if (id > 0) { 
	_Source.TrackingId = id; 
	_Reader.IsPaused = false; 
			Debug.Log ("id is "+id);
	} else { 
		_Source.TrackingId = 0;
		_Reader.IsPaused = true; 
	}
	gameObject.GetComponent<GestureController> ().AddGesture ();
} 
// Update Loop, set body if we need one 
void Update() { 
	if (!_Source.IsTrackingIdValid) {
		FindValidBody();
	}
} 
// Check Body Manager, grab first valid body 
void FindValidBody() { 
	if (_BodySource != null) { 
		Body[] bodies = _BodySource.GetData();
		if (bodies != null) { 
			foreach (Body body in bodies) {
				if (body.IsTracked) { 
					SetBody(body.TrackingId); 
					break; 
				} 
			}
		} 
	}
} 
/// Handles gesture detection results arriving from the sensor for the associated body tracking Id 
private void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e) { 
	VisualGestureBuilderFrameReference frameReference = e.FrameReference; 
	using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame()) { 
		if (frame != null) { 
			// get the discrete gesture results which arrived with the latest frame 
			IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults; 
			if (discreteResults != null) { 
				foreach (Gesture gesture in _Source.Gestures) {
					if (gesture.GestureType == GestureType.Discrete) { 
						DiscreteGestureResult result = null; 
						discreteResults.TryGetValue(gesture, out result); 
							if (gesture.Name == "point_right") {
								GameObject.Find ("Border").GetComponent<HistogramTexture> ().setHeight (result.Confidence);
							}
							/*if (result.Confidence > 0.05) { 
							// Fire Event 
								if(OnGesture!=null){
								//	Debug.Log("Detected Gesture " + gesture.Name + " with Confidence " + result.Confidence);
								//	OnGesture(this,new KinectGestureEvent(gesture.Name, result.Confidence));	
								} 
						}*/
					} 
				} 
			} 
		} 
	}
} 

	//private void Setheight(value){
	//	GameObject.Find ("Border").GetComponent<HistogramTexture> ().setHeight ();
	//}
}