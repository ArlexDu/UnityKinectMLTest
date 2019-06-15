using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
using Windows.Kinect; 
using Microsoft.Kinect.VisualGestureBuilder; 
using System.Runtime.InteropServices;
using System;
// Adapted from DiscreteGestureBasics-WPF by Momo the Monster 2014-11-25 
// For Helios Interactive - http://heliosinteractive.com 
public class GestureSourceManager : MonoBehaviour { 
	public BodySourceManager _BodySource;

    private string databasePath_Lessons1;
    private string databasePath_Lessons2;
    private string databasePath_Lessons3;
    public string DatabasePath
    {
        get { return databasePath; }
        set
        {
            databasePath = value;
        }
    }
    [SerializeField]
    private string databasePath = "";

	private KinectSensor _Sensor; 
	private VisualGestureBuilderFrameSource _Source; 
	private VisualGestureBuilderFrameReader _Reader; 
	private static VisualGestureBuilderDatabase _Database;
	private static IList<Gesture> gesturesList;
	// Gesture Detection Events 
	public event GestureEvent OnGesture;

	private static GestureSourceManager instance = null;
	private bool confirm = false;

		public static GestureSourceManager Instance{
			get{
				return instance;
			}
		}
    void Awake()
    {
        databasePath_Lessons1 = @"gesture_detector_nature.gbd";
        databasePath_Lessons2 = @"gesture_detector_lesson2.gbd";
        databasePath_Lessons3 = @"gesture_detector_lesson2.gbd";
        DatabasePath = @"gesture_detector_nature_all.gbd";
        //DatabasePath = @"gesture_detector_nature.gbd";
        //DatabasePath = databasePath_Lessons1;
        //SetDatabasePath(1);
    }
    void Start()
    {
        Init();
    }
    
	// Use this for initialization 
	void Init() { 
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
			gesturesList = _Database.AvailableGestures; 
			instance = this;
	//	gameObject.GetComponent<GestureController> ().InitialGesture();
			} 
		for (int g = 0; g < gesturesList.Count; g++) { 
			Gesture gesture = gesturesList [g];
			Debug.Log ("gname is "+ gesture.Name);
		}
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
	} 

	public void confirmBody(ulong bodyid){
		confirm = true;
		SetBody (bodyid);
	}
	//get the nearest body id as detect body
	public void UpdatebodyId(ulong bodyid){
		if (!confirm) {
			SetBody (bodyid);
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
						//	if (gesture.Name == "recognize") {
						//			Debug.Log ("Detected Gesture " + gesture.Name + " with Confidence " + result.Confidence);
						//		}
								// Fire Event 
 						//Debug.Log ("Detected Gesture " + gesture.Name + " with Confidence " + result.Confidence);
							if (OnGesture != null) {
								KinectGestureEvent eventData = new KinectGestureEvent (gesture.Name, result.Confidence);
								if (gesture.Name.Equals ("recognize")) {
									eventData.addOpt ("BodyId", _Source.TrackingId.ToString ());
								}	
								OnGesture (this, eventData);	
							} else {
								Debug.LogError ("On Gesture is null");
							} 
						} 
					} 
				} 
			} 
		}
	} 
	// detector one gesture one time
	public void AddDetector(string name){ 
	//	Debug.Log ("gesturelist is "+gesturesList[0].Name);
		for (int g = 0; g < gesturesList.Count; g++) { 
			Gesture gesture = gesturesList [g];
			string gname = gesture.Name;
	//		Debug.Log ("gname is "+ gname);
			if (gesture.Name == name) {
				Debug.Log ("Add gesture is " + gesture.Name);
				_Source.AddGesture (gesture); 	
			}
		}
	}

	public void RemoveDetectors(){
		IList<Gesture> addedList = _Source.Gestures;  
		for (int g = 0; g < addedList.Count; g++) { 
			Gesture gesture = addedList [g];
            Debug.Log("Remove gesture:" + gesture.Name);
            _Source.RemoveGesture (gesture);
		}
		if (OnGesture != null) {
			Delegate[] events =  OnGesture.GetInvocationList ();
			foreach(Delegate e in events){
				Debug.Log ("Remove delegate:"+e.Method.Name);
				OnGesture -= (e as GestureEvent);
			}
		}
	}
}