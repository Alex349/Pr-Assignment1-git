using UnityEngine;
using System.Collections;
using Steerings;

public class SpawnerPlus : MonoBehaviour {

	public GameObject sample;

	public int numInstances = 10;
	public float interval = 5f; // one ant every interval seconds
	public float variationRatio = 0.25f;

	private int generated = 0;
	private float elapsedTime = 0f; // time elapsed since last generation


	// Update is called once per frame
	void Update () {
		if (generated == numInstances)
			return;

		GameObject clone;
		if (elapsedTime >= interval) {
			// spawn creating an instance...
			clone = Instantiate(sample);
			clone.transform.position = this.transform.position;

			KinematicState ks = clone.GetComponent<KinematicState> ();
			if (ks != null) {
				ks.maxSpeed = ks.maxSpeed + Utils.binomial()*variationRatio*ks.maxSpeed;
				ks.maxAcceleration = ks.maxAcceleration + Utils.binomial()*variationRatio*ks.maxAcceleration;
			}

			Flocking fk = clone.GetComponent<Flocking> ();
			if (fk != null) {
				fk.cohesionThreshold += Utils.binomial () * variationRatio * fk.cohesionThreshold;
				fk.repulsionThreshold += Utils.binomial () * variationRatio * fk.repulsionThreshold;
				fk.wanderRate += Utils.binomial () * variationRatio * fk.wanderRate;
			}

			generated++;
			elapsedTime = 0;
		} else {
			elapsedTime += Time.deltaTime;
		}

	}
}
