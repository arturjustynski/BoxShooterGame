using UnityEngine;
using System.Collections;

public class TargetBehavior : MonoBehaviour
{

	// target impact on game
	public int scoreAmount = 0;
	public float timeAmount = 0.0f;

	// explosion when hit?
	public GameObject explosionPrefab;

	public bool isTargetExplosive = false;
	public float explosionRadius = 4.0f;
	private bool alreadyDestroyed;
	public bool isTargetSpecial = false;

	// when collided with another gameObject
	void OnCollisionEnter (Collision newCollision)
	{
		// exit if there is a game manager and the game is over
		if (GameManager.gm) {
			if (GameManager.gm.gameIsOver)
				return;
		}

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "Projectile") {
			if (explosionPrefab) {
				// Instantiate an explosion effect at the gameObjects position and rotation
				Instantiate (explosionPrefab, transform.position, transform.rotation);
			}
				
			// destroy the projectile
			Destroy (newCollision.gameObject);
			DestroyTargets ();
		}
	}

	void DestroyTargets()
	{
		if (alreadyDestroyed)
			return;

		alreadyDestroyed = true;

		if (explosionPrefab)
		{
			// Instantiate an explosion effect at the gameObjects position and rotation
			Instantiate(explosionPrefab, transform.position, transform.rotation);
		}

		// if game manager exists, make adjustments based on target properties
		if (GameManager.gm) {
			if (!isTargetSpecial) {
				GameManager.gm.targetHit(scoreAmount, timeAmount);
			} else {
				GameManager.gm.specialTargetHit ();
			}
		}

		if (isTargetExplosive)
		{
			Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);
			foreach (Collider collider in colliders) {
				if (collider.tag == "Target") {
					collider.SendMessage ("DestroyTargets");
				}
			}
		}

		Destroy (gameObject);

	}
}
