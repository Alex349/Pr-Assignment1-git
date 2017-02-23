using UnityEngine;
using System.Collections;
using Steerings;

namespace FSM
{

	public class FSM_BEE_FINAL: FiniteStateMachine
	{

		public enum State {INITIAL, POLLINATING, DEFEND};

		public State currentState = State.INITIAL; 
	
		private BEE_Blackboard blackboard;
		private FSM_BEE_POLLINATE fsmBeePollinate;
        private FSM_BEE_DEFEND fsmBeeDefend;
        private GameObject mosquito;


        void Start () {
			
			blackboard = GetComponent<BEE_Blackboard>();
			if (blackboard == null) {
				blackboard = gameObject.AddComponent<BEE_Blackboard>();
			}

            fsmBeePollinate = GetComponent<FSM_BEE_POLLINATE> ();
			if (fsmBeePollinate == null) {
                fsmBeePollinate = gameObject.AddComponent<FSM_BEE_POLLINATE>();
			}

            fsmBeeDefend = GetComponent<FSM_BEE_DEFEND>();
            if (fsmBeePollinate == null)
            {
                fsmBeeDefend = gameObject.AddComponent<FSM_BEE_DEFEND>();
            }

            fsmBeePollinate.enabled = true;
            fsmBeeDefend.enabled = false;

        }


        public override void Exit () {
            fsmBeePollinate.enabled = false;
            fsmBeeDefend.enabled = true;

            base.Exit ();
		}

		public override void ReEnter() {
			base.ReEnter ();
		}

		void Update ()
		{
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.POLLINATING);
                    break;
                case State.POLLINATING:
                    mosquito = SensingUtils.FindInstanceWithinRadius(gameObject, "BOID", blackboard.perilDetectableRadius);
                    if (mosquito != null)
                    {
                        ChangeState(State.DEFEND);
                        break;
                    }
                    // do nothing while in this state
                    break;
                case State.DEFEND:
                    if (tag == "QueenBee")
                    {
                        ChangeState(State.POLLINATING);
                    }
                    // do nothing while in this state
                    break;
            } // end of switch
        }

		private void ChangeState (State newState) {
			// EXIT STATE LOGIC. Depends on current state
			switch (currentState) {
			case State.POLLINATING:
				fsmBeeDefend.Exit ();
				break;
			} // end of EXIT SWITCH

			// ENTER STATE LOGIC. Depends on newState
			switch (newState) {
			case State.POLLINATING:
				fsmBeePollinate.ReEnter ();
				break;
			} // end of ENTER switch

			currentState = newState;
		}

	}
}