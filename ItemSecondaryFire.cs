﻿using UnityEngine;
using ThunderRoad;
using static ModularFirearms.FirearmFunctions;

namespace ModularFirearms
{
    public class ItemSecondaryFire : MonoBehaviour
    {
        private float prevShot;
        //ThunderRoad references
        protected Item item;
        protected ItemModuleSecondaryFire module;
        private Handle gunGrip;
        //Unity references
        private AudioSource fireSound;
        private ParticleSystem MuzzleFlash;
        private Transform muzzlePoint;

        public void Awake()
        {
            item = this.GetComponent<Item>();
            item.OnHeldActionEvent += OnHeldAction;
            module = item.data.GetModule<ItemModuleSecondaryFire>();
            if (!string.IsNullOrEmpty(module.mainGripID)) gunGrip = item.definition.GetCustomReference(module.mainGripID).GetComponent<Handle>();
            if (!string.IsNullOrEmpty(module.fireSoundRef)) fireSound = item.definition.GetCustomReference(module.fireSoundRef).GetComponent<AudioSource>();
            if (!string.IsNullOrEmpty(module.muzzleFlashRef)) MuzzleFlash = item.definition.GetCustomReference(module.muzzleFlashRef).GetComponent<ParticleSystem>();
            if (!string.IsNullOrEmpty(module.muzzlePositionRef)) muzzlePoint = item.definition.GetCustomReference(module.muzzlePositionRef);
            else muzzlePoint = item.transform;
        }

        private void Start()
        {
            prevShot = Time.time;
        }

        public void OnHeldAction(Interactor interactor, Handle handle, Interactable.Action action)
        {
            if (handle.Equals(gunGrip) && action == Interactable.Action.UseStart && ((Time.time - prevShot) > module.fireDelay))
            {
                prevShot = Time.time;
                Fire();
            }
        }

        public void PreFireEffects()
        {
            if (MuzzleFlash != null) MuzzleFlash.Play();
            if (fireSound != null) fireSound.Play();
        }

        private void Fire()
        {
            PreFireEffects();
            ShootProjectile(item, module.projectileID, muzzlePoint, null, module.forceMult, module.throwMult);
            //TODO: Apply recoil
        }

    }
}