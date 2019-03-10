using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float range = 100f;


	private float timer;
	private Ray shootRay;
	private RaycastHit shootHit;
	private int shootableMask;
	private ParticleSystem _gunParticles;
	private LineRenderer _gunLine;
	private AudioSource _gunAudio;
	private Light _gunLight;
	private float effectsDisplayTime = 0.2f;


	private void Awake() {
		shootableMask = LayerMask.GetMask("Shootable") | LayerMask.GetMask("Furniture");
		_gunParticles = GetComponent<ParticleSystem>();
		_gunLine = GetComponent<LineRenderer>();
		_gunAudio = GetComponent<AudioSource>();
		_gunLight = GetComponent<Light>();
	}


	private void Update() {
		timer += Time.deltaTime;

		if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) {
			Shoot();
		}

		if (timer >= timeBetweenBullets * effectsDisplayTime) {
			DisableEffects();
		}
	}


	public void DisableEffects() {
		_gunLine.enabled = false;
		_gunLight.enabled = false;
	}


	private void Shoot() {
		timer = 0f;

		_gunAudio.Play();

		_gunLight.enabled = true;

		_gunParticles.Stop();
		_gunParticles.Play();

		_gunLine.enabled = true;
		_gunLine.SetPosition(0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if (Physics.Raycast(shootRay, out shootHit, range, shootableMask)) {
			EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
			if (enemyHealth != null) {
				enemyHealth.TakeDamage(damagePerShot, shootHit.point);
			}

			_gunLine.SetPosition(1, shootHit.point);
		} else {
			_gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
		}
	}
}