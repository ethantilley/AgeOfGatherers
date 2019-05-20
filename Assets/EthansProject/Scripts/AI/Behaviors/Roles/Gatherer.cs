using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EthansProject
{
    /// <summary>
    /// Inherates from villager, the common class all role dirive from.
    /// 
    /// </summary>
    public class Gatherer : Villager
    {
        [HideInInspector]
        public ResourceSupply berrys, wood;

        public void GetActions()
        {
            GetComponent<GatherResourceAction>().SwitchRoles();
            GetComponent<StoreResourceAction>().SwitchRoles();
            role = GetComponent<StoreResourceAction>().CurrGatherType.ToString();
            GetComponent<RoleIcon>().SwitchIcons();
        }

        
        public override HashSet<KeyValuePair<string, object>> CreateGoalState()
        {
            goal.Clear();
            
            if (needFood)
                goal.Add(new KeyValuePair<string, object>("dontDieOfHunger", needFood));
            else
                goal.Add(new KeyValuePair<string, object>("collectResource", (!needFood)));

            return goal;
        }
    }
}
