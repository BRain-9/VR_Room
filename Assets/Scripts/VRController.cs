using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VRController : MonoBehaviour
{
	LineRenderer line;

	private static Vector3 startPoint, endPoint;
	[SerializeField]private static Transform my_Transform;
	public static Transform ControllerPosition{
		get{return my_Transform;}
	}

    [SerializeField] GameObject reticle;
    [SerializeField] VREyeRaycaster mainEyeRaycaster;
    Color reticleColor;
	RaycastHit hitPoint;
    public float rayDistance = 1.5f;

	public static bool controller;
	public static event System.Action<Transform> controllerEnable, controllerDisable;

	private void Awake()
	{
		
	}

	private IEnumerator Start(){
		yield return new WaitForSeconds(1f);
		my_Transform = this.transform;
        if (controllerEnable != null)
            controllerEnable.Invoke(this.transform);
	}

	private void OnEnable()
    {
		if (controllerEnable != null)
		    controllerEnable.Invoke(this.transform);		
		reticle.gameObject.GetComponent <Image>().enabled = false;
        reticleColor = reticle.GetComponent<UnityEngine.UI.Image>().color;
        line = this.gameObject.GetComponent<LineRenderer>();
		controller = true;
		VREyeRaycaster.OnRaycasthit += ReticleOn;
		VREyeRaycaster.OnRaycastOut += ReticleOff;
    }

    void OnDisable()
    {
		if (controllerDisable != null)
		    controllerDisable.Invoke(this.transform);        
		reticle.gameObject.GetComponent<Image>().enabled = true;
		controller = false;
		VREyeRaycaster.OnRaycasthit -= ReticleOn;
		VREyeRaycaster.OnRaycastOut -= ReticleOff;
        
    }
	
	void Update () {
		my_Transform = this.transform;
        startPoint = transform.position;

		if (!reticle.GetComponent<Image>().enabled)
		{
			endPoint = (transform.forward * rayDistance) + transform.position;
		}
		else
		{
			endPoint = hitPoint.point;
			reticle.transform.position = hitPoint.point;
		}

		
        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);

    }

	private void ReticleOn(RaycastHit hit){
		hitPoint = hit;
		if (!reticle.GetComponent<Image>().enabled)
			reticle.gameObject.GetComponent<Image>().enabled = true;
	}

	private void ReticleOff(){
		if (reticle.GetComponent<Image>().enabled)
			reticle.gameObject.GetComponent<Image>().enabled = false;
	}

}
