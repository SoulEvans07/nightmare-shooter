using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour {
	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;
	public AudioClip deathClip;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


	private Animator _animator;
	private AudioSource _playerAudio;

	private PlayerMovement _playerMovement;

	private PlayerShooting _playerShooting;
	private bool isDead;
	private bool damaged;


	private void Awake() {
		_animator = GetComponent<Animator>();
		_playerAudio = GetComponent<AudioSource>();
		_playerMovement = GetComponent<PlayerMovement>();
		_playerShooting = GetComponentInChildren<PlayerShooting>();
		currentHealth = startingHealth;
	}


	private void Update() {
		if (damaged) {
			damageImage.color = flashColour;
		} else {
			damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		damaged = false;
	}


	public void TakeDamage(int amount) {
		damaged = true;

		currentHealth -= amount;

		healthSlider.value = currentHealth;

		_playerAudio.Play();

		if (currentHealth <= 0 && !isDead) {
			Death();
		}
	}


	private void Death() {
		isDead = true;

		_playerShooting.DisableEffects();

		_animator.SetTrigger("Die");

		_playerAudio.clip = deathClip;
		_playerAudio.Play();

		_playerMovement.enabled = false;
		_playerShooting.enabled = false;
	}


	public void RestartLevel() {
		// SceneManager.LoadScene(0);
	}
}