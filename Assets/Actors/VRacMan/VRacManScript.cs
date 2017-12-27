using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRacManScript : MonoBehaviour {

	public Material OriginalMaterial;
	public Material PoweredMaterial;
	public Material DeadMaterial;
	public AudioClip Point;
	public AudioClip Death;
	public AudioClip PowerPoint;
	public AudioClip Chomp;
	public AudioClip EatGhost1;
	public AudioClip EatGhost2;
	public AudioClip EatGhost3;
	public AudioClip EatGhost4;
	public AudioClip EatGhost5;
	private AudioClip[] AudioClips;
	public GameObject GameManager;
	private bool Powered;
	private int PowerDuration;
	private bool anim;
	private GameObject bodytop;
	private GameObject bodybot;
	public float StartTime;

	// Use this for initialization
	void Start () {
		// TODO: Assign to an active controller, warn if none exist.
		Powered = false;
		GameManager = GameObject.Find ("GameManager");
		AudioClips = new AudioClip[5];
		AudioClips [0] = EatGhost1;
		AudioClips [1] = EatGhost2;
		AudioClips [2] = EatGhost3;
		AudioClips [3] = EatGhost4;
		AudioClips [4] = EatGhost5;
		bodytop = GameObject.Find ("Top");
		bodybot = GameObject.Find ("Bottom");
		bodytop.GetComponent<Renderer> ().material = OriginalMaterial;
		bodybot.GetComponent<Renderer> ().material = OriginalMaterial;
		InvokeRepeating ("PowerDurationDecrement", 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name == "Orb") {
			AudioSource.PlayClipAtPoint (Point, this.gameObject.transform.position);
			GameManager.GetComponent<GameManager> ().IncreaseScore (1.0f);
			Destroy (other.gameObject); 
		} else if (other.gameObject.name == "Chaser") {
			if (Powered) {
				AudioClip RandomClip = AudioClips [Random.Range (0, 4)];
				AudioSource.PlayClipAtPoint (Chomp, this.gameObject.transform.position);
				AudioSource.PlayClipAtPoint (RandomClip, this.gameObject.transform.position);
				Destroy (other.gameObject);
				GameManager.GetComponent<GameManager> ().IncreaseScore (5.0f);
			} else {
				AudioSource.PlayClipAtPoint (Death, this.gameObject.transform.position);
				bodytop.GetComponent<Renderer> ().material = DeadMaterial;
				bodybot.GetComponent<Renderer> ().material = DeadMaterial;
				GameManager.GetComponent<GameManager> ().EndGame ();
			}
		} else if (other.gameObject.name == "PowerOrb") {
			PowerDuration += 5;
			Destroy (other.gameObject); 
			if (!Powered) {
				Powered = true;
				AudioSource.PlayClipAtPoint (PowerPoint, this.gameObject.transform.position);
				GameManager.GetComponent<GameManager> ().Powered (Powered);
				InvokeRepeating ("PowerAnimation", 0.0f, 0.5f);
			}
		} else if (other.gameObject.name == "StartButton") {
			StartTime = Time.realtimeSinceStartup;
			GameManager.GetComponent<GameManager> ().StartGame ();
			Destroy (other.gameObject);
		} else if (other.gameObject.name == "ReturnButton") {
			GameManager.GetComponent<GameManager> ().CreateLobby ();
			bodytop.GetComponent<Renderer> ().material = OriginalMaterial;
			bodybot.GetComponent<Renderer> ().material = OriginalMaterial;
			Destroy (other.gameObject);
		}
	}

	void PowerDurationDecrement (){
		if (Powered) {
			if (PowerDuration > 0) {
				PowerDuration -= 1;
			} else if (PowerDuration == 0) {
				Powered = false;
				CancelInvoke ();
				bodytop.GetComponent<Renderer> ().material = OriginalMaterial;
				bodybot.GetComponent<Renderer> ().material = OriginalMaterial;
				GameManager.GetComponent<GameManager>().Powered(Powered); // For music change
				InvokeRepeating ("PowerDurationDecrement", 0.0f, 1.0f);
			}
		}
	}
	void PowerAnimation(){
		if (anim) {
			anim = false;
			bodytop.GetComponent<Renderer> ().material = PoweredMaterial;
			bodybot.GetComponent<Renderer> ().material = PoweredMaterial;
		} else {
			anim = true;
			bodytop.GetComponent<Renderer> ().material = OriginalMaterial;
			bodybot.GetComponent<Renderer> ().material = OriginalMaterial;
		}
	}
	public float GetStartTime(){
		return StartTime;
	}
}
