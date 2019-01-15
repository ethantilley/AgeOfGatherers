using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EthansProject
{
    public class ResourceSupply : MonoBehaviour
    {

        public enum StorageTypes
        {
            BerryStorage,
            WoodStorage
        }
        public StorageTypes storageType = StorageTypes.BerryStorage;

        public int resourceCount = 0;
        public int resourceCapacity = 100;
        public float upgradeScale = 1.2f;
        public float berryExpiryTime = 500;
        private float currExpiryTime ;
        bool isFilled = false;


        public Queue<int> resourceFillLog;
        public int UpgradeCost
        {
            get { return resourceCapacity; }
        }

        public void StoreResource(int _amount)
        {
            resourceCount += _amount;
            resourceFillLog.Enqueue(_amount);
        }

        [ContextMenu("Display Storage Log")]
        void PrintStorageLog()
        {
            
            foreach (var item in resourceFillLog)
            {
                Debug.Log(item);

            }
        }

        private void Update()
        {
            if (resourceCount >= resourceCapacity && !isFilled)
            {
                isFilled = true;
                //RemoveStorage();
                WorldInfo.filledStorage.Add(this);
            }

            if (currExpiryTime > 0)
            {
                currExpiryTime -= Time.deltaTime;

            }
            else
            {
                TakeResource(resourceFillLog.Peek());
                resourceFillLog.Dequeue();
                currExpiryTime = berryExpiryTime;
            }

        }

        public int TakeResource(int _amount)
        {
            resourceCount -= _amount;

            return _amount;
        }

        public void UpgradeResources()
        {
            resourceCapacity += Mathf.RoundToInt(resourceCapacity * upgradeScale);

            Debug.Log("Removed " + this.gameObject);

            WorldInfo.filledStorage.Remove(this);
            isFilled = false;

        }

        void AddStorage()
        {
            switch (storageType)
            {
                case StorageTypes.BerryStorage:
                    WorldInfo.globalBerryAmount = this.resourceCount;
                    WorldInfo.berrySorages = (this);
                    break;
                case StorageTypes.WoodStorage:
                    WorldInfo.globalLogsAmount = this.resourceCount;
                    WorldInfo.treeStorages = (this);
                    break;
                default:
                    break;
            }
        }


        void RemoveStorage()
        {
            switch (storageType)
            {
                case StorageTypes.BerryStorage:
                    WorldInfo.berrySorages = (null);
                    break;
                case StorageTypes.WoodStorage:
                    WorldInfo.treeStorages = (null);
                    break;
                default:
                    break;
            }
        }

        public float ReturnFilledPercentage()
        {
            float percent = resourceCount / (float)resourceCapacity;
            return percent;
        }

        private void Start()
        {
            AddStorage();
            currExpiryTime = berryExpiryTime;
        }
    }
}
