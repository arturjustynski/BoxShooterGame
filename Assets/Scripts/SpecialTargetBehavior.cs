using UnityEngine;
using System.Collections;

public class SpecialTargetBehavior : MonoBehaviour {

	public GameObject explosionPrefab;

	void OnCollisionEnter (Collision newCollision) {
		
		if (GameManager.gm) {
			if (GameManager.gm.gameIsOver) {
				return;
			}
		}
		if (newCollision.gameObject.tag != "Projectile") {
			return;
		}
		if (explosionPrefab) {
			Instantiate (explosionPrefab, transform.position, transform.rotation);
		}
		if (GameManager.gm) {
			GameManager.gm.specialTargetHit ();
		}

		Destroy (newCollision.gameObject);
		Destroy (gameObject);
	}
}
