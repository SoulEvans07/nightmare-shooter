using UnityEngine;

public class EnemyHealth : MonoBehaviour {
	public int startingHealth = 100;
	public int currentHealth;
	public float sinkSpeed = 2.5f;
	public int scoreValue = 10;
	public AudioClip deathClip;

	private Animator _animator;
	private AudioSource _enemyAudio;
	private ParticleSystem _hitParticles;
	private CapsuleCollider _capsuleCollider;
	private bool isDead;
	private bool isSinking;


	private void Awake() {
		_animator = GetComponent<Animator>();
		_enemyAudio = GetComponent<AudioSource>();
		_hitParticles = GetComponentInChildren<ParticleSystem>();
		_capsuleCollider = GetComponent<CapsuleCollider>();

		currentHealth = startingHealth;
	}


	private void Update() {
		if (isSinking) {
			transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
		}
	}


	public void TakeDamage(int amount, Vector3 hitPoint) {
		if (isDead) return;

		_enemyAudio.Play();

		currentHealth -= amount;

		_hitParticles.transform.position = hitPoint;
		_hitParticles.Play();

		if (currentHealth <= 0) {
			Death();
		}
	}


	private void Death() {
		isDead = true;

		_capsuleCollider.isTrigger = true;

		_animator.SetTrigger("Dead");

		_enemyAudio.clip = deathClip;
		_enemyAudio.Play();
	}


	public void StartSinking() {
		GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
		isSinking = true;
		ScoreManager.score += scoreValue;
		Destroy(gameObject, 2f); // 2 second
	}
}