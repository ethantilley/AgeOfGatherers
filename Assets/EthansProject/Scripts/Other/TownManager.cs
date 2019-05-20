using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EthansProject
{
    public class TownManager : MonoBehaviour
    {
        ResourceSupply berrySupply, woodSupply;

        public GameObject gatherersPrefab, builderPrefab; // same VV

        [Header("Spawn Count")]
        public int gatherers, builders; //TODO: add builder and warter etc villagers when the work lol
        int newestGeneration = 0;
        public Dictionary<string, int> avaliableNames = new Dictionary<string, int>()
    {

        {"Ethan", 1}, {"Melvin", 1}, {"Jesse", 1}, {"Jackson", 1}, {"Riley", 1}, {"Kell", 1}, {"Ben", 1}, {"Melissa", 1}, {"Bethany", 1}, {"Kurtis Nerville", 1}

    };

        /// <summary>
        /// Returns a random name for a list 
        /// also changes that name so that the names never 
        /// </summary>
        /// <returns></returns>
        public string GetNewVillagerName()
        {
            string _villagerName = string.Empty, nameToSet = string.Empty;
            int index = UnityEngine.Random.Range(0, avaliableNames.Count);

            _villagerName = avaliableNames.Keys.ToList()[index]; ;

            if (avaliableNames[_villagerName] > 1)
                nameToSet = string.Format(_villagerName + " #{0}", avaliableNames[_villagerName].ToString());
            else
                nameToSet = _villagerName;

            avaliableNames[_villagerName] += 1;

            return nameToSet;
        }



        // Use this for initialization
        void Start()
        {
            FindResourceSupply();
            SpawnVillager(gatherersPrefab, gatherers);
            SpawnVillager(builderPrefab, builders);
        }

        private void FindResourceSupply()
        {
            foreach (var item in GetComponentsInChildren<ResourceSupply>())
            {
                if (item.storageType == ResourceSupply.StorageTypes.BerryStorage)
                    berrySupply = item;
                else if (item.storageType == ResourceSupply.StorageTypes.WoodStorage)
                    woodSupply = item;
            }
               
        }

        public void SpawnVillager(GameObject _villagerType, int _spawnAmount)
        {
            for (int i = 0; i < _spawnAmount; i++)
            {
                GameObject newVillager = Instantiate(_villagerType, transform.position, Quaternion.identity);
                newVillager.name = GetNewVillagerName();
                newVillager.GetComponent<Gatherer>().berrys = berrySupply;
                newVillager.GetComponent<Gatherer>().wood = woodSupply;
            }
            newestGeneration++;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
