using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureController : MonoBehaviour {

	private bool Righthand = false;
	private bool Lefthand = false;
	private GestureSourceManager manager;

		
	public void AddGesture(){
		manager = GestureSourceManager.Instance;
		manager.OnGesture += wave_Righthand;
		manager.OnGesture += wave_Lefthand;
	} 

	private void wave_Righthand (object sender,KinectGestureEvent e){
		Debug.Log ("wavehand Right!!!");
		if (e.name.Equals ("wavehand_Right")) {
			Debug.Log ("wavehand Right!!!");
			Righthand = true;
		} else {
			Righthand = false;
		}
	}

	private void wave_Lefthand (object sender,KinectGestureEvent e){
		if (e.name.Equals ("wavehand_Left")) {
			Debug.Log ("wavehand Left!!!");
			Lefthand = true;
		} else {
			Lefthand = false;
		}
	}


	public bool IsWaingRightHand(){
		return Righthand;
	}
		
	public bool IsWavingLeftHand(){
		return Lefthand;
	}
}
