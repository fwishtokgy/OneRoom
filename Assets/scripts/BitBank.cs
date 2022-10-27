using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitBank : MonoBehaviour {
	public GameObject [] rubits;
	public GameObject ParentBase;

	/*toBe Public Static Const later*/
	int cX = 0;	
	int cY = 1;	
	int cZ = 2;	
	// Use this for initialization
	void Start () {
		rubits = new GameObject[27];
		for (var i=0; i<27; i++) {
			rubits[i] = ParentBase.transform.GetChild(i).gameObject;
			rubits[i].GetComponent<RubitBlock>().label.text = i+"";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void returnChildren(GameObject[] blocks) {

	}
	public void returnChild(GameObject block) {
		block.transform.parent = ParentBase.transform;
	}

	public void setToPivot(int cardinal, int valence, Transform obj) {
		for (var i=0; i<27; i++) {
			float val = maskedValue(rubits[i].transform.position,cardinal);
			if (matchesValence(val,valence)) {
				rubits[i].transform.parent = obj;
			}
		}
	}
	public void returnToBase(Transform obj) {
		int count = obj.childCount;
		for (var i=0; i<9; i++) {
			obj.GetChild(0).transform.parent = ParentBase.transform;
		}
	}
	public float maskedValue(Vector3 subject, int cardinal) {
		if (cardinal == cX) {
			return subject.x;
		}
		else if (cardinal == cY) {
			return subject.y;
		}
		else if (cardinal == cZ) {
			return subject.z;
		}
		return 0f;
	}
	public bool matchesValence(float subject, int valence) {
		if (valence==2 && subject>5f) return true;
		if (valence==1 && subject<5f && subject>-5f) return true;
		if (valence==0 && subject<-5f) return true;
		return false;
	}
}
