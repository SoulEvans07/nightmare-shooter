using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	public static int score;

	private Text _text;

	void Awake() {
		_text = GetComponent<Text>();
		score = 0;
	}


	void Update() {
		_text.text = "Score: " + score;
	}
}