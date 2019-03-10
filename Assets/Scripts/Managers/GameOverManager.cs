using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
	public PlayerHealth playerHealth;
	
	private Animator _animator;

	private void Awake() {
		_animator = GetComponent<Animator>();
	}


	private void Update() {
		if (playerHealth.currentHealth <= 0) {
			_animator.SetTrigger("GameOver");
		}
	}
	
	public void RestartLevel() {
		SceneManager.LoadScene(0);
	}
}