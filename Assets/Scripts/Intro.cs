using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;

public class Intro : MonoBehaviour {
	void Start () {
		SceneManager.LoadScene("Intro", LoadSceneMode.Additive);
	}
}
