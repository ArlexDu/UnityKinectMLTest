using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class GestureController : MonoBehaviour {

	private KinectGestureEvent point_door = new KinectGestureEvent("point_right",0);
	private KinectGestureEvent helmet = new KinectGestureEvent("helmet",0.5f);   
	private GestureSourceManager manager;
    public GameObject bodymanager;
	private static GestureController instance = null;
	public static GestureController Instance{
		get{
			return instance;
		}
	}

	//initial manager
	public void Start(){
		manager = GestureSourceManager.Instance;
		manager.RemoveDetectors();
		manager.AddDetector ("recognize");
		manager.OnGesture += recognize;
		instance = this;
	}

    //judge recognize
    private void recognize (object sender,KinectGestureEvent e){
		if (e.name.Equals ("recognize")) {
			//GameObject.Find ("CreateDiagram").GetComponent<HistogramTexture> ().setHeight (e.confidence);
			if (e.confidence > 0.8) {
			//	PlayerPrefs.SetString("BodyId",e.getOpt("BodyId"));
				//judge event done
				ulong id = Convert.ToUInt64(e.getOpt ("BodyId"));
				Debug.Log ("detect bodyid is "+id);
				manager.confirmBody (id);
				DontDestroyOnLoad (bodymanager);
				SceneManager.LoadScene ("Game");
				manager.RemoveDetectors();
				/*manager.AddDetector ("walk");
				manager.OnGesture += walking;
				Debug.Log ("Add Walk");*/
			}
		}
	}
    //judge walking
	private void walking (object sender,KinectGestureEvent e){
        Debug.Log("==================" + e.name+"======="+e.confidence);
		if (e.name.Equals ("walk")) {
			if (e.confidence > 0.02) {
				//	PlayerPrefs.SetString("BodyId",e.getOpt("BodyId"));
				//judge event done
				manager.OnGesture -= walking; 
				manager.RemoveDetectors();
				//GameObject.Find ("Player").GetComponent<PlayerController>().StartTraning();
            }
		}
	}
    #region redesign the gestures of lesson1
    private void Bowing(object sender,KinectGestureEvent e)
    {
        if(e.name.Equals("bowing"))
        {
            if(e.confidence > 0.3)
            {
                manager.OnGesture -= Bowing;
                manager.RemoveDetectors();
                //GameObject.Find("Player").GetComponent<PlayerController>().StartTraning();
            }
        }
    }
    private void Guide(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("guide"))
        {
            if (e.confidence > 0.3)
            {
                manager.OnGesture -= Guide;
                manager.RemoveDetectors();
                //GameObject.Find("Player").GetComponent<PlayerController>().StartTraning();
            }
        }
    }
    private void Squat(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("squat"))
        {
            if (e.confidence > 0.3)
            {
                manager.OnGesture -= Squat;
                manager.RemoveDetectors();
                //GameObject.Find("Player").GetComponent<PlayerController>().StartTraning();
            }
        }
    }
    private void WelcomeCustomer(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("welcomeCustomer"))
        {
            if (e.confidence > 0.3)
            {
                manager.OnGesture -= WelcomeCustomer;
                manager.RemoveDetectors();
                //GameObject.Find("Player").GetComponent<PlayerController>().StartTraning();
            }
        }
    }
    private void Service(object sender,KinectGestureEvent e)
    {
        if(e.name.Equals("service"))
        {
            if(e.confidence > 0.3)
            {
                manager.OnGesture -= Service;
                manager.RemoveDetectors();
                //GameObject.Find("Player").GetComponent<PlayerController>().StartTraning();
            }
        }
    }
    private void SetDiagramBowing(object sender,KinectGestureEvent e)
    {
        if(e.name.Equals("bowing"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void SetDiagramSquat(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("squat"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void SetDiagramGuide(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("guide"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void SetDiagramWelcomeCustomer(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("welcomeCustomer"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void SetDiagramService(object sender,KinectGestureEvent e)
    {
        if(e.name.Equals("service"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    #endregion

    private void starting(object sender,KinectGestureEvent e){
        //Debug.Log("==================" + e.name + "=======" + e.confidence);
        if (e.name.Equals("walk"))
        {
            if (e.confidence > 0.02)
            {
                //	PlayerPrefs.SetString("BodyId",e.getOpt("BodyId"));
                //judge event done
                manager.OnGesture -= walking;
                manager.RemoveDetectors();
               // GameObject.Find("Player").GetComponent<PlayerController2>().StartTraning();
            }
        }
    }

	//judge point door
	private void pointingDoor (object sender,KinectGestureEvent e){
		if (e.name.Equals ("point_right")) {
			GameObject.Find ("CreateDiagram").GetComponent<HistogramTexture> ().setHeight (e.confidence);
		}
	}
	//judge helmet
	private void Helmet (object sender,KinectGestureEvent e){
		if (e.name.Contains ("helmet")) {
			GameObject.Find ("CreateDiagram").GetComponent<HistogramTexture> ().setHeight (e.confidence);
		}
	}

    private void goingStraight(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("goStraight"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void stop(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("stop"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void turnLeft(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("turnLeft"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void turnRight(object sender, KinectGestureEvent e)
    {
        if (e.name.Equals("turnRight"))
        {
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void simulate(object sender,KinectGestureEvent e){
        if (e.name.Equals("deiverExercise")){
            GameObject.Find("CreateDiagram").GetComponent<HistogramTexture>().setHeight(e.confidence);
        }
    }
    private void startLessonThree(object sender, KinectGestureEvent e){
        if (e.name.Equals("recognize")){
            if (e.confidence > 0.8){
                //GameObject.Find("Hand").GetComponent<HandController>().startPosition = GameObject.Find("BodyView").GetComponent<MineBodySourceView>().handposition;
                //GameObject.Find("Canvas").GetComponent<CanvasController3>().StartTraining();
                manager.RemoveDetectors();
            }
        }
    }
    // load scene form menu to Lesson One	
    public void LoadLessonOne(){
		SceneManager.LoadScene ("LessonOne");
        manager.RemoveDetectors();
		manager.AddDetector ("walk");
		manager.OnGesture += walking;
	}
    public void LoadLessonTwo()
    {
        SceneManager.LoadScene("LessonTwo");
        manager.RemoveDetectors();
        manager.AddDetector("walk");
        manager.OnGesture += starting;
    }
    public void LoadLessonThree()
    {
        SceneManager.LoadScene("LessonThree");
        manager.RemoveDetectors();
        manager.AddDetector("recognize");
        manager.OnGesture += startLessonThree;
    }

    // load pointer door gesture
    public void LoadPointer(){
		manager.RemoveDetectors();
		manager.AddDetector ("point_right");
		manager.OnGesture += pointingDoor; 
	}

    #region redesign for the lesson1
    public void LoadSquat()
    {
        manager.RemoveDetectors();
        manager.AddDetector("squat");
        manager.OnGesture += SetDiagramSquat;
    }
    public void LoadGuide()
    {
        manager.RemoveDetectors();
        manager.AddDetector("guide");
        manager.OnGesture += SetDiagramGuide;
    }
    public void LoadBowing()
    {
        manager.RemoveDetectors();
        manager.AddDetector("bowing");
        manager.OnGesture += SetDiagramBowing;
    }
    public void LoadWelcomeCustomer()
    {
        manager.RemoveDetectors();
        manager.AddDetector("welcomeCustomer");
        manager.OnGesture += SetDiagramWelcomeCustomer;
    }
    public void LoadService()
    {
        manager.RemoveDetectors();
        manager.AddDetector("service");
        manager.OnGesture += SetDiagramService;
    }
    #endregion


    // add helmetevent
    public void LoadHelmetP1(){
		manager.RemoveDetectors();
		manager.AddDetector ("helmet_p1");
		manager.OnGesture += Helmet;
	}
	public void LoadHelmetP2(){
		manager.RemoveDetectors();
		manager.AddDetector ("helmet_p2");
		manager.OnGesture += Helmet;
	}
	public void LoadHelmetP3(){
		manager.RemoveDetectors();
		manager.AddDetector ("helmet_p3");
		manager.OnGesture += Helmet;
	}
	public void LoadHelmetP4(){
		manager.RemoveDetectors();
		manager.AddDetector ("helmet_p4");
		manager.OnGesture += Helmet;
	}
    
    public void LoadGoStraight()
    {
        manager.RemoveDetectors();
        manager.AddDetector("goStraight");
        manager.OnGesture += goingStraight;
    }
    public void LoadStop()
    {
        manager.RemoveDetectors();
        manager.AddDetector("stop");
        manager.OnGesture += stop;

        //manager.AddDetector("bowing");
        //manager.OnGesture += SetDiagramBowing;
    }
    public void LoadTurnLeft()
    {
        manager.RemoveDetectors();
        manager.AddDetector("turnLeft");
        manager.OnGesture += turnLeft;
        //manager.AddDetector("bowing");
        //manager.OnGesture += SetDiagramBowing;
    }
    public void LoadTurnRight()
    {
        manager.RemoveDetectors();
        manager.AddDetector("turnRight");
        manager.OnGesture += turnRight;
        //manager.AddDetector("bowing");
        //manager.OnGesture += SetDiagramBowing;
    }
    public void LoadSimulation(){
        manager.RemoveDetectors();
        manager.AddDetector("deiverExercise");
        manager.OnGesture += simulate;
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Press Esc Key!");
            SceneManager.LoadScene("Menu");
        }
        //manual test
        /*	if (Input.GetKeyDown (KeyCode.A)) {
                manager.RemoveDetectors ();
            }
            if (Input.GetKeyDown (KeyCode.B)) {
                manager.OnGesture += walking;
            }*/
    }
}
