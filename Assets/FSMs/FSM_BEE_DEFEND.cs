using UnityEngine;
using System.Collections;
using Steerings;

namespace FSM
{

    public class FSM_BEE_DEFEND : FiniteStateMachine
    {

        public enum State { INITIAL, NORMAL, DEFEND };

        public State currentState = State.INITIAL;

        private BEE_Blackboard blackboard;

        private float elapsedTime; // time elapsed in EATING or RUMMAGING states


        private KeepDistanceVersatile defend;
        private KinematicState KS;

        private GameObject mosquito, queenBee;

        void Start()
        {
            defend = GetComponent<KeepDistanceVersatile>();
            if (defend == null)
                Debug.LogError(gameObject + " has no KeepDistanceVersatile attached in " + this);

            KS = GetComponent<KinematicState>();
            if (KS == null)
                Debug.LogError(gameObject + " has no Kinematic state attached in " + this);

            blackboard = GetComponent<BEE_Blackboard>();
            if (blackboard == null)
            {
                blackboard = gameObject.AddComponent<BEE_Blackboard>();
            }

            defend.enabled = false;
        }

        public override void Exit()
        {
            defend.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            base.ReEnter();
        }

        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.NORMAL);
                    break;
                case State.NORMAL:

                    // do nothing in particular while in this state
                    break;
                case State.DEFEND:
                    queenBee = SensingUtils.FindInstanceWithinRadius(gameObject, "QueenBee", blackboard.perilDetectableRadius);
                    mosquito = SensingUtils.FindInstanceWithinRadius(gameObject, "BOID", blackboard.perilDetectableRadius);
                    if (SensingUtils.DistanceToTarget(gameObject, mosquito) >= blackboard.perilSafetyRadius)
                    {
                        ChangeState(State.NORMAL);
                    }
                    // do nothing in particular, while in this state
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.NORMAL:
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.NORMAL:
                    break;

                case State.DEFEND:
                    defend.enabled = true;
                    defend.target = queenBee;
                    
                    break;
            }

            currentState = newState;
        }
    }
}