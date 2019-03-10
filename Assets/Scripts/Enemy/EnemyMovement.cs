using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
	Transform player;

	PlayerHealth playerHealth;
	EnemyHealth _enemyHealth;
	NavMeshAgent _nav;


	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = player.GetComponent<PlayerHealth>();
		_enemyHealth = GetComponent<EnemyHealth>();
		_nav = GetComponent<NavMeshAgent>();
	}


	void Update() {
		if (_enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0) {
			_nav.SetDestination(player.position);
		} else {
			_nav.enabled = false;
		}
	}
}