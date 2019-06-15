using UnityEngine;
using System.Collections;
using Windows.Kinect;
using UnityEngine.UI;
public class HeadShowManager : MonoBehaviour 
{
    public int ColorWidth { get; private set; }
    public int ColorHeight { get; private set; }

    private KinectSensor _Sensor;
    private ColorFrameReader _Reader;
    private Texture2D _Texture;
    private byte[] _Data;
	private int[] _Key = {0,0,0,0};
	private ColorSpacePoint head;
	private ColorSpacePoint neck;
	private CoordinateMapper m_pCoordinateMapper;

	const int        cColorWidth  = 1920;
	const int        cColorHeight = 1080;

    public Texture2D GetColorTexture()
    {
        return _Texture;
    }

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        
        if (_Sensor != null) 
        {
            _Reader = _Sensor.ColorFrameSource.OpenReader();
            
            var frameDesc = _Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            ColorWidth = frameDesc.Width;
            ColorHeight = frameDesc.Height;
			m_pCoordinateMapper = _Sensor.CoordinateMapper;
			_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
			_Texture.hideFlags = HideFlags.DontSaveInEditor;
            _Data = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];
		//	Debug.Log ("_data length is "+_Data.Length);
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }
    
    void Update () 
    {
        if (_Reader != null) 
        {
            var frame = _Reader.AcquireLatestFrame();
            
            if (frame != null)
            {
				//Debug.Log ("get color image");
				MapCameraToImage ();
                frame.CopyConvertedFrameDataToArray(_Data, ColorImageFormat.Rgba);
                _Texture.LoadRawTextureData(_Data);
                _Texture.Apply();
                
                frame.Dispose();
                frame = null;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (_Reader != null) 
        {
            _Reader.Dispose();
            _Reader = null;
        }
        
        if (_Sensor != null) 
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            
            _Sensor = null;
        }
    }

	private void MapCameraToImage(){
		CameraSpacePoint[] CameraPoints = GameObject.Find ("BodyView").GetComponent<MineBodySourceView> ().getCameraPoints ();
		if (CameraPoints.Length > 0 && CameraPoints[0].X != 0 &&CameraPoints[0].Y != 0&&CameraPoints[0].Z != 0) {
//			Debug.Log ("head camera position is " + CameraPoints[0].X + ","+CameraPoints[0].Y+","+CameraPoints[0].Z);
			head = m_pCoordinateMapper.MapCameraPointToColorSpace (CameraPoints[0]);
			neck = m_pCoordinateMapper.MapCameraPointToColorSpace (CameraPoints[1]);
//			Debug.Log ("head image position is " + head.X + ","+head.Y);
			int distance = (int)Mathf.Max(Mathf.Ceil(Mathf.Abs ((head.X - neck.X) + (head.Y - neck.Y))),150);
//			Debug.Log ("distance is " + distance );
			int TX = (int)Mathf.Ceil(head.X+distance);
			int TY = (int)Mathf.Ceil(head.Y+distance);
			int BX = (int)Mathf.Ceil(head.X-distance);
			int BY = (int)Mathf.Ceil(head.Y-distance);
			_Key [0] = BX;
			_Key [1] = TX;
			_Key [2] = BY;
			_Key [3] = TY;
//			Debug.Log ("Top Right is " + TX+","+TY );				
//			Debug.Log ("Bottom Left is " + BX+","+BY );

		}
	}
		
	public int[] GetKey(){
		return _Key;
	}
}
