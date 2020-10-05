﻿using UnityEngine;
using ThunderRoad;
using static ModularFirearms.FirearmFunctions;

namespace ModularFirearms.Shared
{
    public class InteractiveAmmo : MonoBehaviour
    {
        protected Item item;
        protected AmmoModule module;
        private GameObject bulletMesh;
        private Handle ammoHandle;
        private AmmoType thisAmmoType;
        private int capacity;
        public bool isLoaded = true;

        protected void Awake()
        {
            item = this.GetComponent<Item>();
            module = item.data.GetModule<AmmoModule>();
            thisAmmoType = module.GetSelectedType();
            capacity = module.numberOfRounds;
            if (module.handleRef != null) ammoHandle = item.definition.GetCustomReference(module.handleRef).GetComponent<Handle>();
            if (module.bulletMeshID != null) bulletMesh = item.definition.GetCustomReference(module.bulletMeshID).gameObject;
            Refill();
        }


        public AmmoType GetAmmoType()
        {
            return thisAmmoType;
        }

        public int GetAmmoTypeInt()
        {
            return module.ammoType;
        }

        public string GetAmmoID()
        {
            return item.data.id;
        }

        public int GetAmmoCount()
        {
            return capacity;
        }

        public void Consume(int i = 1)
        {
            capacity -= i;
            SetMeshState(bulletMesh);
            if (capacity <= 0) isLoaded = false;
            if (ammoHandle != null) ammoHandle.data.allowTelekinesis = false;
        }

        public void Refill()
        {
            capacity = module.numberOfRounds;
            SetMeshState(bulletMesh, true);
            isLoaded = true;
            if (ammoHandle != null) ammoHandle.data.allowTelekinesis = true;
            return;
        }

        protected void SetMeshState(GameObject ammoMesh, bool newState = false)
        {
            if (ammoMesh != null) { ammoMesh.SetActive(newState); }
            return;
        }
    }
}