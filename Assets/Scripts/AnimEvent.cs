using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;

public class AnimEvent : MonoBehaviour {	
	public void StartGame () {
		SceneManager.LoadScene("Game", LoadSceneMode.Additive);
	}
}
