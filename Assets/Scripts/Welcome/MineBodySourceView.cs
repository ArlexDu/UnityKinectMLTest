using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Windows.Kinect;

public class MineBodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
	private CameraSpacePoint headposition;
	private CameraSpacePoint neckposition;
    public Vector3 handposition;
	public Transform BodyPosition;
	private CameraSpacePoint[] camerapoints = new CameraSpacePoint[2];
	private ulong nearest_body = 0;
	private ulong last_nearest_body = 0;
	private GestureSourceManager manager;
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
	void Start(){
		CameraSpacePoint head = new CameraSpacePoint();
		head.X = 0;
		head.Y = 0;
		head.Z = 0;
		camerapoints [0] = head;
		CameraSpacePoint neck = new CameraSpacePoint();
		neck.X = 0;
		neck.Y = 0;
		neck.Z = 0;
		camerapoints [1] = neck;
		manager = GestureSourceManager.Instance;
	}

    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }
		//get the nearest body
		float near_Z = 5;
		foreach (var body in data) {
			if (body == null)
			{
				continue;
			}
			if (body.IsTracked) {
				if (body.Joints [Kinect.JointType.SpineMid].Position.Z < near_Z) {
					nearest_body = body.TrackingId;
					near_Z = body.Joints [Kinect.JointType.SpineMid].Position.Z;
				}
			}
		}
		if (nearest_body!=0 && last_nearest_body != nearest_body) {
            Debug.Log("nearest_body is"+nearest_body);
			last_nearest_body = nearest_body;
			manager.UpdatebodyId (nearest_body);
		}

        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
		body.transform.parent = transform;
		body.transform.position = BodyPosition.position;
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			jointObj.layer = LayerMask.NameToLayer ("KinectBody");
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);
            
            jointObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        
        return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            
            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);
			if (body.TrackingId == nearest_body && jt == Kinect.JointType.Head) {
				headposition.X = jointObj.position.x - BodyPosition.position.x;
				headposition.Y = jointObj.position.y - BodyPosition.position.y;
				headposition.Z = jointObj.position.z - BodyPosition.position.z;
				camerapoints [0] = headposition;
			}
			if (body.TrackingId == nearest_body && jt == Kinect.JointType.SpineShoulder) {
				neckposition.X = jointObj.position.x - BodyPosition.position.x;
				neckposition.Y = jointObj.position.y - BodyPosition.position.y;
				neckposition.Z = jointObj.position.z - BodyPosition.position.z;
				camerapoints [1] = neckposition;
			}
            if (body.TrackingId == nearest_body && jt == Kinect.JointType.HandRight)
            {
                handposition = jointObj.localPosition;
            }
            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if(targetJoint.HasValue)
            {
				lr.SetPosition(0, jointObj.localPosition+BodyPosition.position);
				lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value)+BodyPosition.position);
                lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }
        }
    }
    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

	public CameraSpacePoint[] getCameraPoints(){
		return camerapoints;
	}
}
