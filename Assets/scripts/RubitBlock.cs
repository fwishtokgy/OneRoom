using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubitBlock : MonoBehaviour {
	public Transform center;
	public GameObject guide;
	private GameObject [] planes;
	private Face [] faces;	
	private Vector3 pos;

	public bool debug;
	public bool inMotion;  
	public TextMesh label;

	/*toBe Public Static Const later*/
	int NGA = 0;	// Negative
	int EQL = 1;	// Equal: 0
	int GRT = 2;	// Greater

	/*toBe Public Static Const later*/
	int RT_CNTR = 0;	// center piece: 0 sides
	int RT_FACE = 1;	// face piece: 1 side
	int RT_EDGE = 2;	// edge piece: 2 sides
	int RT_CORN = 3;	// corner piece: 3 sides
	public int type;

	// Use this for initialization
	void Start () {
		planes = new GameObject[6];
		pos = this.transform.position;
		if (guide.activeSelf) guide.SetActive(false);

		Transform planeHolder = this.transform.GetChild(1);
		Vector3 curPos;
		int differences = 0;
		for (var i=0; i<6; i++) {
			planes[i] = planeHolder.GetChild(i).gameObject;
			if (planes[i].activeSelf) planes[i].SetActive(false);
		}
		differences += activePlanePairs(planes[4],planes[5],relativity(pos.x));
		differences += activePlanePairs(planes[0],planes[1],relativity(pos.y));
		differences += activePlanePairs(planes[3],planes[2],relativity(pos.z));
		type = differences;
	}

	public void highlight() {
		guide.SetActive(true);
	}
	public void normalState() {
		guide.SetActive(false);
	}

	int activePlanePairs(GameObject NGA_plane, GameObject GRT_plane, int code) {
		int activations = 1;
		if (code==NGA) {
			NGA_plane.SetActive(true);
		} else if (code == GRT) {
			GRT_plane.SetActive(true);
		} else {
			activations = 0;
		}
		return activations;
	}

	int relativity(float subject) {
		int answer = EQL;
		if (subject < 0) answer = NGA;
		else if (subject > 0) answer = GRT;
		if (debug) {
			Debug.Log("center: "+center+" ... subject: "+subject);
		}
		return answer;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
