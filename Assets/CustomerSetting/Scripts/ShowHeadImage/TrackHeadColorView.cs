using UnityEngine;
using System.Collections;
using Windows.Kinect;
using UnityEngine.UI;
public class TrackHeadColorView : MonoBehaviour
{
    public GameObject ColorSourceManager;
    private ColorSourceManager _ColorManager;
	private ComputeBuffer maskbuffer;
	private byte[] mask;
    void Start ()
    {
		ReleaseBuffers ();
		Texture2D renderTexture = ColorSourceManager.GetComponent<HeadShowManager>().GetColorTexture();
		if (renderTexture != null) {
			gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainTex", renderTexture);
		} else {
			Debug.LogError ("Texture is null !");
		}
		mask = ColorSourceManager.GetComponent<HeadShowManager> ().GetMask ();
	//	Debug.Log ("mask length is "+mask.Length);
		if (mask != null) {
			maskbuffer = new ComputeBuffer (mask.Length, sizeof(int));
			gameObject.GetComponent<Renderer> ().material.SetBuffer ("_Mask", maskbuffer);
		}
     
    }

	void Update()
	{

		// ComputeBuffers do not accept bytes, so we need to convert to float.
		updateMask();
	}

	void updateMask(){
		int[] buffer = new int[1920 * 1080];
		for (int i = 0; i < mask.Length; i++)
		{
			buffer[i] = (int)mask[i];
		}
		maskbuffer.SetData(buffer);
		buffer = null;
	}

	private void ReleaseBuffers() 
	{
		if (maskbuffer != null) maskbuffer.Release();
		maskbuffer = null;

		mask = null;
	}

	void OnDisable() 
	{
		ReleaseBuffers ();
	}
}
