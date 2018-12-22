using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EthansProject
{
    public class AIDirector : MonoBehaviour
    {

        public static AIDirector instance;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(this);
            }
            else
            {
                instance = this;
            }
        }
   
        public int globalBerryGatherers, globalWoodGatherers; 

        public AnimationCurve scaleRoles;

        /// <summary>
        /// Check To See if the gatherer should switch roles
        /// </summary>
        /// <param name="gatherType"> // pass through weather im a berry or wood gatherer.</param>
        /// <returns></returns>
        public bool SwitchRolesCheck(StoreResourceAction.GatherType gatherType)
        {
            float berryPercent = WorldInfo.berrySorages.ReturnFilledPercentage();
            float goalRolePercent = scaleRoles.Evaluate(berryPercent);
            float currentRolePercent = WorldInfo.RolePercentage();

            globalBerryGatherers = WorldInfo.berryGatherers.Count;
            globalWoodGatherers = WorldInfo.woodGatherers.Count;

            // return out that we dont need a role switch if goalRolePercent  & currentRolePercent are the same.
            if (currentRolePercent == goalRolePercent)
            {
                return false;
            }
           

            // if  there's way more berry gatherers than i need then we need and im a berry gatherer i need to switch else if im a wood gatherer i need to not switch
            if (gatherType == StoreResourceAction.GatherType.BerryGatherer && currentRolePercent < goalRolePercent)
            {
                return false;
            }
            else if (gatherType == StoreResourceAction.GatherType.WoodGatherer && currentRolePercent < goalRolePercent)
            {
                return true;
            }

            // same with way less but oposite switching outcomes.
            // have a extra berry gatherer threshold of like 20%.
            if (gatherType == StoreResourceAction.GatherType.BerryGatherer && currentRolePercent > goalRolePercent * 1.2f)
            {
                return true;
            }
            else if (gatherType == StoreResourceAction.GatherType.WoodGatherer && currentRolePercent > goalRolePercent * 1.2f)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Call to spawn a new agent, primarly if its to replace an old one
        /// </summary>
        public void SpawnNewAgent(bool _spawnGatherer)
        {
            TownManager tm = FindObjectOfType<TownManager>();
            tm.SpawnVillager(_spawnGatherer ? tm.gatherersPrefab : tm.builderPrefab, 1);
        }


    }
}