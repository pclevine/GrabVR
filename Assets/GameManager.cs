using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManager : MonoBehaviour {

	public GameObject Plane;
	public GameObject Orb;
	public GameObject Chaser;
	public GameObject PowerOrb;
	public GameObject ScoreText;
	public GameObject LobbyTextParent;
	public GameObject GameOverText;
	public GameObject StartButton;
	public GameObject ReturnButton;
	public AudioClip StartSound;
	public AudioClip Music;
	public AudioClip PowerMusic;
	public Material Logo;
	public Material Grid;
	private GameObject Platform;
	private Bounds bounds;
	private AudioSource[] allAudioSources;
	private float minx;
	private float maxx;
	private float miny;
	private float maxy = 2.5f;
	private float minz;
	private float maxz;
	private float score;
	private float Highscore;
	private Vector3 VectorZero = new Vector3 (0.0f, 0.0f, 0.0f);

	void Start () {
		Platform = Instantiate (Plane, VectorZero, Quaternion.identity);
		resizePlatform ();
		CreateLobby ();
	}
		
	void Update () {
	}

	public void CreateLobby(){ // TODO: Finish
		Platform.GetComponentInChildren<Renderer> ().material = Logo;
		GameObject[] UIList;
		UIList = GameObject.FindGameObjectsWithTag ("EndGameUI");
		foreach (GameObject obj in UIList) {
			Destroy (obj);
		}
		StopAllSounds ();
		score = 0;
		Instantiate (StartButton, new Vector3 (0.0f, 1.0f, -0.75f), Quaternion.identity);
		if (!GameObject.Find("LobbyTextParent")){
			Instantiate (LobbyTextParent, VectorZero, Quaternion.identity);
		}
	}

	public void resizePlatform () {
		var rect = new HmdQuad_t();
		if (!SteamVR_PlayArea.GetBounds (SteamVR_PlayArea.Size.Calibrated, ref rect)) {
			Debug.LogError ("VRACMAN ERROR: Failed to get Calibrated Play Area bounds!  Make sure you have tracking first, and that your space is calibrated.");
			return;
		}
		var corners = new HmdVector3_t[] { rect.vCorners0, rect.vCorners1, rect.vCorners2, rect.vCorners3 };
		Vector3 chapScale = transform.localScale;
		chapScale.x = Mathf.Abs(corners[0].v0 - corners[1].v0)/10;
		chapScale.z = Mathf.Abs(corners[0].v2 - corners[3].v2)/10;
		transform.localScale = chapScale;
		Platform.transform.localScale = chapScale;
		bounds = Platform.GetComponentInChildren<MeshFilter>().mesh.bounds;
		minx = bounds.min.x;
		maxx = bounds.max.x;
		miny = bounds.min.y;
		minz = bounds.min.z;
		maxz = bounds.max.z;
	}

	public void StartGame(){ // TODO: Finish
		GameObject[] UIList;
		UIList = GameObject.FindGameObjectsWithTag ("LobbyUI");
		foreach (GameObject obj in UIList) {
			Destroy (obj);
		}
		Instantiate (ScoreText, new Vector3 (0.75f, 2.0f, -2.0f), Quaternion.Euler(new Vector3 (0.0f, -180.0f, 0.0f)));
		AudioSource.PlayClipAtPoint (StartSound, this.gameObject.transform.position);
		AudioSource.PlayClipAtPoint (Music, this.gameObject.transform.position);
		Platform.GetComponentInChildren<Renderer> ().material = Grid;
		InvokeRepeating ("SpawnOrb", 0.0f, 1.0f);
		InvokeRepeating ("SpawnChaser", 5.0f, 5.0f);
		InvokeRepeating ("SpawnPowerOrb", 10.0f, 15.0f);
	}

	private void SpawnOrb () {
		Instantiate(Orb, new Vector3(Random.Range(minx/5, maxx/5), Random.Range((miny + 0.5f)/5, maxy), Random.Range(minz/5, maxz/5)), Quaternion.identity);
	}

	private void SpawnChaser(){
		int i = Random.Range (0, 3);
		switch (i){
			case 0:
			Instantiate (Chaser, new Vector3 (minx / 5, Random.Range ((miny + 0.5f)/5, maxy), Random.Range (minz / 5, maxz / 5)), Quaternion.identity);
				break;
			case 1:
			Instantiate (Chaser, new Vector3 (maxx / 5, Random.Range ((miny + 0.5f)/5, maxy), Random.Range (minz / 5, maxz / 5)), Quaternion.identity);
				break;
			case 2: 
			Instantiate (Chaser, new Vector3 (Random.Range (minx / 5, maxx / 5), Random.Range ((miny + 0.5f)/5, maxy), minz / 5), Quaternion.identity); 
				break;
			case 3: 
			Instantiate (Chaser, new Vector3 (Random.Range (minx / 5, maxx / 5), Random.Range ((miny + 0.5f)/5, maxy), maxz / 5), Quaternion.identity); 
				break;
		}
	}

	private void SpawnPowerOrb(){
		Instantiate (PowerOrb, new Vector3 (Random.Range (minx / 5, maxx / 5), Random.Range ((miny + 0.5f) / 5, maxy), Random.Range (minz / 5, maxz / 5)), Quaternion.identity);
	}

	public void IncreaseScore(float inc){
		GameObject ScoreText = GameObject.Find ("ScoreText");
		score += inc;
		ScoreText.GetComponent<TextMesh>().text = "Score:" + "\n" + "\t" + score;
	}

	public void Powered(bool status){
		StopAllSounds();
		if (status) {
			AudioSource.PlayClipAtPoint (PowerMusic, this.gameObject.transform.position);
		} else {
			AudioSource.PlayClipAtPoint (Music, this.gameObject.transform.position);
		}
	}

	public void EndGame(){ 
		CancelInvoke ();
		GameObject[] Objlist;
		Objlist = GameObject.FindGameObjectsWithTag ("GameplayObject");
		foreach (GameObject obj in Objlist) {
			Destroy (obj);
		}
		Instantiate (GameOverText, VectorZero, Quaternion.Euler(VectorZero));
		Instantiate (ReturnButton, new Vector3 (0.0f, 1.0f, 0.75f), Quaternion.identity);
		Debug.Log ("Score: " + score);
		if (score > Highscore) {
			Highscore = score;
			Debug.Log ("NEW HIGH SCORE: " + Highscore);
			GameObject HighScoreText = GameObject.Find ("HighScoreText");
			HighScoreText.GetComponentInChildren<TextMesh> ().text = "HighScore:" + "\n" + "\t" + Highscore;
		} else {
			Debug.Log ("HighScore: " + Highscore);
		}
	}

	public void StopAllSounds(){
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource audioS in allAudioSources) {
			audioS.Stop ();
		}
	}
}
