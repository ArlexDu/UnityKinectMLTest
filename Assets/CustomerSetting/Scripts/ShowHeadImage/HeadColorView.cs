using UnityEngine;
using System.Collections;
using Windows.Kinect;
using UnityEngine.UI;
public class HeadColorView : MonoBehaviour
{
    public GameObject ColorSourceManager;
    private ColorSourceManager _ColorManager;
	private ComputeBuffer keybuffer;
	private int[] keys = { 0, 0, 0, 0 };
	private float[] computekeys= {0f,0f,0f,0f};
    void Start ()
    {
		ReleaseBuffers ();
		Texture2D renderTexture = ColorSourceManager.GetComponent<HeadShowManager>().GetColorTexture();
		if (renderTexture != null)
		{
			gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", renderTexture);
			//GameObject.Find("HeadImage").GetComponent<RawImage>().material.SetTexture("_MainTex", renderTexture);
		} else {
			Debug.LogError ("Texture is null !");
		}
		keys = ColorSourceManager.GetComponent<HeadShowManager> ().GetKey();
		if (keys != null) {
			keybuffer = new ComputeBuffer (keys.Length, sizeof(float));
			gameObject.GetComponent<Renderer> ().material.SetBuffer ("key", keybuffer);
			//gameObject.GetComponent<Renderer> ().material.SetFloatArray("key",computekeys);
		}

     
    }

	void Update()
	{

		// ComputeBuffers do not accept bytes, so we need to convert to float.
		updateKey ();
	}
		

	void updateKey(){
		computekeys[0] = (float)(keys[1]-keys[0])/1920;
		computekeys [1] = (float)keys [0] / 1920;
		computekeys [2] = (float)(keys [3] - keys [2]) / 1080;
		computekeys [3] = (float)keys [2] / 1080;
		if (computekeys [0] < 0 || computekeys [1] < 0 || computekeys [2] < 0 || computekeys [3] < 0) {
			computekeys [0] = 0f;
			computekeys [1] = 0f;
			computekeys [2] = 0f;
			computekeys [3] = 0f;
		}
	//	Debug.Log ("area is "+ computekeys[0]+","+computekeys[1]+","+computekeys[2]+","+computekeys[3]);
		keybuffer.SetData(computekeys);
	}

	private void ReleaseBuffers() 
	{

		if (keybuffer != null) keybuffer.Release();
		keybuffer = null;

		keys = null;
	}

	void OnDisable() 
	{
		ReleaseBuffers ();
	}
}
