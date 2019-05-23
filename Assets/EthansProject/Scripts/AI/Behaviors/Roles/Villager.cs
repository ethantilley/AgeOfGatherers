using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace EthansProject
{
    public abstract class Villager : MonoBehaviour, IGOAP
    {
        [Header("Movement")]
        public List<PathingNode> agentPath = new List<PathingNode>();
        public float moveSpeed = 10;
        Vector3 destination;
        bool atDestination;

        public Stopwatch sw;
        public string role;

        public HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        [Header("Hunger")]
        public float hungerCap = 100;
        public float currentHungerLevel = 100;
        public float foodNeededLevel = 20; 
        public float apattiteLevel = .2f;
        public bool needFood;
        public float avg_Hunger = 100;

        [Header("Aging")]
        public int villagerAge = 0;    
        [Range(0f,5f)]
        public float agingRate = 0.2f;
        public int ageOfDeath = 100; 
        public int generation;
        public AnimationCurve deathCurve; 
        float currentAge;
        int agentBirthDate, agentDeathDate;

        public GameObject graveStone;
        public GameObject agentBody;
        public AgentStorage Storage
        {
            get { return GetComponent<AgentStorage>(); }
        }

        public void CalculateNewDeathAge(float _newValue)
        {
            avg_Hunger = (avg_Hunger + _newValue) / 2;
            float deathPercent = avg_Hunger / hungerCap;
            ageOfDeath = Mathf.RoundToInt(deathCurve.Evaluate(deathPercent) * 100);
        }

        // Use this for initialization
        void Start()
        {
            sw = new Stopwatch();
            currentHungerLevel = hungerCap;
            agentBirthDate = GameManager.instance.daysPast;
            apattiteLevel *= Random.Range(0.8f, 1.2f);
            WorldInfo.glogalApititeConsumtionthing += apattiteLevel;
            sw.Start();
        }
       
        void KillAgent(string _deathCause)
        {
            sw.Stop();
            agentDeathDate = GameManager.instance.daysPast;
            //UnityEngine.Debug.LogWarning(gameObject.name + " Just died of "+ _deathCause + " after, " + sw.Elapsed.Minutes + " minutes");

            //Spawn grave stone and write epitaph on it.
            GameObject newGStone = Instantiate(graveStone, transform.position, graveStone.transform.rotation);
            newGStone.GetComponent<GraveGenerator>().WriteEpitaph(gameObject.name, agentBirthDate.ToString(), agentDeathDate.ToString(), _deathCause);
                      
            AIDirector.instance.SpawnNewAgent(GetComponent<Gatherer>());

            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            currentAge += Time.deltaTime * agingRate;

            villagerAge = Mathf.RoundToInt(currentAge);

            if (villagerAge > ageOfDeath)
                KillAgent("Old Age");

            if (currentHungerLevel > 0)
                currentHungerLevel -= Time.deltaTime * apattiteLevel;
            else
            {             
                KillAgent("Stravation");
            }
            if (currentHungerLevel <= foodNeededLevel)
                needFood = true;
            else
                needFood = false;


        }

        public void ActionFinished()
        {
            atDestination = false;

        }

        public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();


        /// <summary>
        /// Core world data set of the villager
        /// </summary>
        /// <returns></returns>
        public HashSet<KeyValuePair<string, object>> GetWorldState()
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
            worldData.Add(new KeyValuePair<string, object>("hasResource", (Storage.resourceHolding > 0)));
            worldData.Add(new KeyValuePair<string, object>("expandNeeded", (WorldInfo.filledStorage.Count > 0)));
            worldData.Add(new KeyValuePair<string, object>("dontDieOfHunger", needFood));
            worldData.Add(new KeyValuePair<string, object>("needsFood", needFood));
          
            return worldData;
        }

        int index = 0;
        Vector3 currentPos;

        Vector3[] path;
        void CheckPoint()
        {

            currentPos = path[index];
        }

        void StepAgent()
        {
            if (path == null)
            {
                AssignPath();
                return;
            }

            float step = moveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, currentPos) <= 0.2f)
            {
                if (index < path.Length - 1)
                {
                    index++;
                    CheckPoint();
                }
            }
            else
            {
                if (currentPos != Vector3.zero)
                    transform.position = Vector3.MoveTowards(transform.position, currentPos, step);
            }
        }

        bool needPath = true;
        public bool MoveAgent(GOAPAction nextAction)
        {
            //destination
            //has the destination been reached
            if (!NodeManager.instance._initialized)
            {
                UnityEngine.Debug.LogWarning("Has not initialized yet");

                return false;
            }

            destination = nextAction.target.transform.position;

            AssignPath();

            // Don't touch this.
            if (Vector3.Distance(gameObject.transform.position, nextAction.target.transform.position) <= 3.5f)
            {
                // we are at the target location, we are done
                nextAction.SetInRange(true);
                needPath = true;
                index = 0;
                atDestination = true;
                return true;
            }
            else
            {
                StepAgent();

                return false;
            }
        }

        void AssignPath()
        {
            if (!needPath || agentPath == null)
                return;

            needPath = false;
            agentPath = PathingManager.FindPath(gameObject.transform.position, destination);
            path = new Vector3[agentPath.Count + 1];
            for (int i = 0; i < agentPath.Count; i++)
            {
                path[i] = agentPath[i].node.spacialInfo;
            }
            CheckPoint();
        }



        public void PlanAborted(GOAPAction aborter)
        {
            UnityEngine.Debug.LogWarning("Plan aborted by the action: " + aborter);
        }

        public void PlanFailed(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions)
        {
            UnityEngine.Debug.LogWarning("Plan failed! Thanks to, " + gameObject);
        }

        public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions)
        {
            UnityEngine.Debug.LogWarning("Plan found!");
        }
    }
}
