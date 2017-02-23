using UnityEngine;
using System.Collections;
using Steerings;

namespace FSM
{

	public class FSM_BEE_POLLINATE : FiniteStateMachine
	{

		public enum State {INITIAL, WANDERING, REACHING_FLOWER, POLLINATING, REACHING_RUSC};

		public State currentState = State.INITIAL; 

		private BEE_Blackboard blackboard;

		private GameObject flower; // the trash can being approached or rummaged
		private GameObject honey; // the honey being transported
		private Arrive arrive; // steering
		private Wander wander; // steering
		private float elapsedTime; // time elapsed

		void Start () {

			// get the steerings
			arrive = GetComponent<Arrive>();
			if (arrive == null)
				Debug.LogError (gameObject +" has no Arrive attached in "+this);

			wander = GetComponent<Wander>();
			if (wander == null)
				Debug.LogError (gameObject +" has no Wander attached in "+this);

			// get the blackboard
			blackboard = GetComponent<BEE_Blackboard>();
			if (blackboard == null) {
				blackboard = gameObject.AddComponent<BEE_Blackboard>();
			}
		}


		public override void Exit () {
			arrive.enabled = false;
			wander.enabled = false;
			base.Exit ();
		}

		public override void ReEnter() {
			currentState = State.INITIAL;
			base.ReEnter ();
		}

		void Update ()
		{
			switch (currentState) {
			case State.INITIAL:
				ChangeState (State.WANDERING);
				break;

			case State.REACHING_FLOWER:
				if (SensingUtils.DistanceToTarget (gameObject, flower) < blackboard.placeReachedRadius) { // trashcan reached?
					ChangeState(State.POLLINATING);
					break;
				}
				// do nothing while in this state
				break;

			case State.REACHING_RUSC:
				if (SensingUtils.DistanceToTarget (gameObject, blackboard.rusc) < blackboard.placeReachedRadius) { //hideout reached?
					ChangeState (State.WANDERING);
                        Destroy(honey);
					break;
				}
				// do nothing while in this state
				break;

			case State.POLLINATING:
				if (elapsedTime >= blackboard.rummageTime) {// food found? 
					ChangeState (State.REACHING_RUSC);
                        Bite(flower);
                        break;
				}
				elapsedTime += Time.deltaTime;
                    // remember, cheese is highly volatile. it may "disappear"
                    if (flower == null || flower.Equals(null))
                    {
                        // if it has vanished just forget about it wander and wander again
                        ChangeState(State.WANDERING);
                        break;
                    }
       
                    break;

			case State.WANDERING:
                    flower = SensingUtils.FindInstanceWithinRadius (gameObject, "Flower", blackboard.flowerDetectableRadius);
				if (flower != null) { // trash can detected? 
					ChangeState(State.REACHING_FLOWER);
					break;
				}
				// do nothing while in this state.
				break;

			} // end of switch
		}



		private void ChangeState (State newState) {
			
			// EXIT STATE LOGIC. Depends on current state
			switch (currentState) {


			case State.REACHING_FLOWER:
				arrive.enabled = false;
				break;

			case State.REACHING_RUSC:
				arrive.enabled = false;
				break;

			case State.POLLINATING:
				// when exiting rummaging create a honey and "hold" it
				honey = Instantiate (blackboard.honeyPrefab);
                honey.transform.parent = gameObject.transform;
                honey.transform.position = gameObject.transform.position;
                honey.transform.localRotation = Quaternion.Euler(0,0,gameObject.transform.rotation.z+90);
				break;

			case State.WANDERING:
				wander.enabled = false;
				break;

			} // end exit switch

			// ENTER STATE LOGIC. Depends on newState
			switch (newState) {

			case State.REACHING_FLOWER:
				arrive.target = flower;
				arrive.enabled = true;
				break;

			case State.REACHING_RUSC:
				arrive.target = blackboard.rusc;
				arrive.enabled = true;
				break;

			case State.POLLINATING:
				elapsedTime = 0;
				break;

			case State.WANDERING:
				wander.enabled = true;
				break;

			} // end of enter switch
			currentState = newState;

		} // end of method ChangeState

        private void Bite(GameObject Flower)
        {
            Flower.SendMessage("BeBitten");
        }
    }
}