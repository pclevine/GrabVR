using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserScript : MonoBehaviour {

	private GameObject VRacMan;
	public Material Material1;
	public Material Material2;
	public Material Material3;
	public Material Material4;
	private Material[] MaterialList;
	private Component[] CompList;
	public float StartTime;

	void Start () { 
		VRacMan = GameObject.Find ("VRacMan");
		StartTime = VRacMan.GetComponent<VRacManScript>().GetStartTime();
		MaterialList = new Material[4];
		MaterialList [0] = Material1;
		MaterialList [1] = Material2;
		MaterialList [2] = Material3;
		MaterialList [3] = Material4;
		Material RandomMaterial = MaterialList[Random.Range(1, 4)];
		CompList = new Component[2];
		CompList = this.gameObject.GetComponentsInChildren<Renderer> ();
		foreach (Component comp in CompList) {
			comp.gameObject.GetComponent<Renderer> ().material = RandomMaterial;
		}
		Invoke ("DestroyThis", (Time.realtimeSinceStartup / 10.0f) + 5.0f);
	}

	void Update () {
		this.transform.position = Vector3.MoveTowards (transform.position, VRacMan.transform.position, ((Time.realtimeSinceStartup - StartTime) / 100.0f * Time.deltaTime));
	}

	void DestroyThis(){
		Destroy (this.gameObject);
	}
}
