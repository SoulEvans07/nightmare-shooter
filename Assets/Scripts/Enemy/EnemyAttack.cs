using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	public float timeBetweenAttacks = 0.5f;
	public int attackDamage = 10;

	private Animator _animator;
	private GameObject player;

	private PlayerHealth playerHealth;

	private EnemyHealth _enemyHealth;
	private bool playerInRange;
	private float timer;


	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		_enemyHealth = GetComponent<EnemyHealth>();
		_animator = GetComponent<Animator>();
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject == player) {
			playerInRange = true;
		}
	}


	void OnTriggerExit(Collider other) {
		if (other.gameObject == player) {
			playerInRange = false;
		}
	}


	void Update() {
		timer += Time.deltaTime;

		if (timer >= timeBetweenAttacks && playerInRange && _enemyHealth.currentHealth > 0) {
			Attack();
		}

		if (playerHealth.currentHealth <= 0) {
			_animator.SetTrigger("PlayerDead");
		}
	}


	void Attack() {
		timer = 0f;

		if (playerHealth.currentHealth > 0) {
			playerHealth.TakeDamage(attackDamage);
		}
	}
}