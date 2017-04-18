using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//[ExecuteInEditMode]
public class HistogramTexture : MonoBehaviour {

	public Material material = null;
	public bool iscanvas = false;
	#region Material properties
	[SerializeField,SetProperty("textureWidth")]
	private int m_textureWidth = 100;
	public int textureWidth{
		get{
			return m_textureWidth;
		}
		set{
			m_textureWidth = value;
			_UpdateMaterial ();
		}
	}
	[SerializeField,SetProperty("rectangleColor")]
	private Color m_rectangaleColor = Color.yellow;
	public Color rectangleColor{
		get{
			return m_rectangaleColor;
		}
		set{
			m_rectangaleColor = value;
			_UpdateMaterial ();
		}
	}
	[SerializeField,SetProperty("Count")]
	private int m_count = 30;
	public int Count{
		get{
			return m_count;
		}
		set{
			m_count = value;
			_UpdateMaterial ();
		}
	}

	#endregion

	private Texture2D m_generatedTexture = null;
	private int base_width;
	private Queue heights = new Queue();
	private float height = 0f;
	private float maxScore = 0;
	// Use this for initialization
	void Start () {
		if (material == null) {
			Renderer renderer = gameObject.GetComponent<Renderer> ();
			if (renderer == null) {
				Debug.LogWarning ("Cannot find a renderer.");
				return;
			}
			material = renderer.sharedMaterial;
		}
		_UpdateMaterial ();
	}
	
	// Update is called once per frame
	void _UpdateMaterial () {
		if (material != null) {
			base_width = (int)Mathf.Ceil(textureWidth / (float)m_count);
//			Debug.Log ("base_with is "+base_width);
			heights.Clear ();
			for (int i = 0; i < m_count; i++) {
				heights.Enqueue (1);
			}
			m_generatedTexture = _GenerateproduceTexture ();
			if (iscanvas) {
				GameObject.Find ("Diagram").GetComponent<RawImage> ().texture = m_generatedTexture; 
			} else {
				material.SetTexture ("_MainTex", m_generatedTexture);
			}
		}
	}
		
	private Texture2D _GenerateproduceTexture(){
		Texture2D proceduralTexture = new Texture2D (textureWidth,textureWidth);
		//draw histogram
		object[] hs = heights.ToArray();
		for (int w = 0; w < textureWidth; w++) {
			int it = (int)Mathf.Floor(w / base_width);
			int distogram = int.Parse (hs [it].ToString ());
//			Debug.Log ("distogram is "+distogram);
			for (int h = 0; h < distogram; h++) {
				Color pixel = m_rectangaleColor;
				proceduralTexture.SetPixel (w,h,pixel);
			}
		}
		proceduralTexture.Apply ();
		return proceduralTexture;
	}
	//update by frame
/*	void Update(){
		height = (int)(Random.value * 100);
//		Debug.Log ("new hieght is "+height);
		heights.Dequeue ();
		heights.Enqueue (height);
		m_generatedTexture = _GenerateproduceTexture ();
		if (iscanvas) {
			GameObject.Find ("RawImage").GetComponent<RawImage> ().texture = m_generatedTexture; 
		} else {
			material.SetTexture ("_MainTex", m_generatedTexture);
		}
	}*/
	// combine with gesture detector
	public void setHeight(float value){
		maxScore = value > maxScore ? value:maxScore;
		height = (int)(value*100);
		heights.Dequeue ();
		heights.Enqueue (height);
		m_generatedTexture = _GenerateproduceTexture ();
		Debug.Log ("maxScore is "+maxScore);
		if (iscanvas) {
			GameObject.Find ("Diagram").GetComponent<RawImage> ().texture = m_generatedTexture; 
		} else {
			material.SetTexture ("_MainTex", m_generatedTexture);
		}
		height = 0f;
	}
}
