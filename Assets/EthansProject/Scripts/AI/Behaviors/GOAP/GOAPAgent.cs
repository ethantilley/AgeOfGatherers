using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EthansProject
{

    public class GOAPAgent : MonoBehaviour
    {

        private FSM stateMachine;

        private FSM.FSMState idleState,
            moveToState,
            prefromActionState;

        private HashSet<GOAPAction> availableActions;
        private Queue<GOAPAction> currentActions;

        private IGOAP dataProvider;
        private GOAPPlanner planner;

        // Use this for initialization
        void Start()
        {
            stateMachine = new FSM();
            availableActions = new HashSet<GOAPAction>();
            currentActions = new Queue<GOAPAction>();
            planner = new GOAPPlanner();
            FindDataProvider();
            CreateIdleState();
            CreateMoveToState();
            CreatePerformActionState();
            stateMachine.PushState(idleState);
            LoadActions();
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update(gameObject);
        }

        public void AddAction(GOAPAction a)
        {
            availableActions.Add(a);
        }

        /// <summary>
        /// Seaches for action in the agents available actions
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public GOAPAction GetAction(Type action)
        {
            foreach (GOAPAction g in availableActions)
            {
                if (g.GetType().Equals(action))
                    return g;
            }
            return null;
        }

        /// <summary>
        /// Removes the action from the available actions
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction(GOAPAction action)
        {
            availableActions.Remove(action);
        }

        /// <summary>
        /// Check to see if the agent has any actions to preform a plan
        /// </summary>
        /// <returns></returns>
        private bool HasActionPlan()
        {
            return currentActions.Count > 0;
        }

        /// <summary>
        /// Formulates the plan based on availble actions, world state, etc
        /// </summary>
        private void CreateIdleState()
        {
            idleState = (fsm, gameObj) =>
            {
                // GOAP planning

                // get the world state and the goal we want to plan for
                HashSet<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
                HashSet<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();
                if (goal.Contains(new KeyValuePair<string, object>("expandStorage", true)))
                {
                    Debug.Log("found builder");
                }
                // Plan
                Queue<GOAPAction> plan = planner.Plan(gameObject, availableActions, worldState, goal);
                if (plan != null)
                {
                    // we have a plan, hooray!
                    currentActions = plan;
                    dataProvider.PlanFound(goal, plan);

                    fsm.PopState(); // move to PerformAction state
                    fsm.PushState(prefromActionState);

                }
                else
                {
                    // ugh, we couldn't get a plan
//                    Debug.Log("<color=orange>Failed Plan:</color> " + prettyPrint(goal));
                    dataProvider.PlanFailed(goal, currentActions);
                    fsm.PopState(); // move back to IdleAction state
                    fsm.PushState(idleState);
                }

            };
        }

        /// <summary>
        /// Starts acting on the plan by moving the agen
        /// </summary>
        private void CreateMoveToState()
        {
            moveToState = (fsm, gameObj) =>
            {
                // move the game object

                GOAPAction action = currentActions.Peek();
                if (action.RequiresInRange() && action.target == null)
                {
                    Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                    fsm.PopState(); // move
                    fsm.PopState(); // perform
                    fsm.PushState(idleState);
                    return;
                }

                // get the agent to move itself
                if (dataProvider.MoveAgent(action))
                {
                    fsm.PopState();
                }               
            };
        }

        /// <summary>
        /// Preforms the action if the agent is in range 
        /// </summary>
        private void CreatePerformActionState()
        {

            prefromActionState = (fsm, gameObj) =>
            {
                // perform the action

                if (!HasActionPlan())
                {
                    // no actions to perform
                    Debug.Log("<color=red>Done actions</color>");
                    fsm.PopState();
                    fsm.PushState(idleState);
                    dataProvider.ActionFinished();
                    return;
                }

                GOAPAction action = currentActions.Peek();
                if (action.IsDone())
                {
                    // the action is done. Remove it so we can perform the next one
                    currentActions.Dequeue();
                }

                if (HasActionPlan())
                {
                    // perform the next action
                    action = currentActions.Peek();
                    bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

                    if (inRange)
                    {
                        // we are in range, so perform the action
                        bool success = action.Preform(gameObj);

                        if (!success)
                        {
                            // action failed, we need to plan again
                            fsm.PopState();
                            fsm.PushState(idleState);
                            Debug.LogWarning(gameObj.name + " could not prefrom the action, " + action + ". The agent ("+gameObject.name+") is now going to abort plan.");
                            dataProvider.PlanAborted(action);
                        }
                    }
                    else
                    {
                        // we need to move there first
                        // push moveTo state
                        fsm.PushState(moveToState);
                    }

                }
                else
                {
                    // no actions left, move to Plan state
                    fsm.PopState();
                    fsm.PushState(idleState);
                    dataProvider.ActionFinished();
                }

            };
        }


        private void FindDataProvider()
        {
            foreach (Component comp in gameObject.GetComponents(typeof(Component)))
            {
                if (typeof(IGOAP).IsAssignableFrom(comp.GetType()))
                {
                    dataProvider = (IGOAP)comp;
                    return;
                }
            }
        }

        /// <summary>
        /// Loads all the avaliable actions on the gameobject
        /// </summary>
        private void LoadActions()
        {
            GOAPAction[] actions = gameObject.GetComponents<GOAPAction>();
            foreach (GOAPAction a in actions)
            {
                availableActions.Add(a);
            }
            
        }

        #region Print Functions

        public static string prettyPrint(HashSet<KeyValuePair<string, object>> state)
        {
            string s = "";
            foreach (KeyValuePair<string, object> kvp in state)
            {
                s += kvp.Key + ":" + kvp.Value.ToString();
                s += ", ";
            }
            return s;
        }

        public static string prettyPrint(Queue<GOAPAction> actions)
        {
            string s = "";
            foreach (GOAPAction a in actions)
            {
                s += a.GetType().Name;
                s += "-> ";
            }
            s += "GOAL";
            return s;
        }

        public static string prettyPrint(GOAPAction[] actions)
        {
            string s = "";
            foreach (GOAPAction a in actions)
            {
                s += a.GetType().Name;
                s += ", ";
            }
            return s;
        }

        public static string prettyPrint(GOAPAction action)
        {
            string s = "" + action.GetType().Name;
            return s;
        }

#endregion
    }
}