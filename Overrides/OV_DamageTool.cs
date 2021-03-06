﻿using Binjector.Other;
using Binjector.Utilities;
using SDG.Framework.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Binjector.Overrides
{
    public class OV_DamageTool
    {
        public static RaycastInfo OV_raycast(Ray ray, float range, int mask)
        {
            ItemWeaponAsset weapon = (ItemWeaponAsset)Player.player.equipment.asset;
            /*if (MenuGUI.instance.LongRangeMelee && weapon == null)
            {
                range = MenuGUI.instance.MeleeRange;
            }*/
            range = Mathf.Infinity;
            RaycastHit hit;
            PhysicsUtility.raycast(ray, out hit, range, mask, 0);
            RaycastInfo raycastInfo = new RaycastInfo(hit);
            if (MenuGUI.instance.silentAim)
            {
                if (hit.transform == null && Functions.GetDistance(Functions.GetNearestPlayer().transform.position) >= Functions.GetGunDistance())
                {
                    return raycastInfo;
                }
            }

            if (hit.transform.CompareTag("Zombie"))
            {
                raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);
            }
            if (hit.transform.CompareTag("Animal"))
            {
                raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);
            }
            raycastInfo.direction = new Vector3(0, MenuGUI.instance.launchAmount, 0);

            if (MenuGUI.instance.silentAim && !hit.transform.CompareTag("Zombie"))
            {
                if (Functions.GetDistFrom(Functions.GetNearestPlayer().transform.position, Player.player.look.aim.position) <= 15.5)
                {
                    raycastInfo.point = Player.player.transform.position;
                }
                else
                {
                    raycastInfo.point = Functions.GetLimbPosition(Functions.GetNearestPlayer().transform, "Skull");
                }
                if (MenuGUI.instance.randomLimb)
                {
                    ELimb[] array = (ELimb[])Enum.GetValues(typeof(ELimb));
                    raycastInfo.limb = array[Random.Next(0, array.Length)];
                }
                else
                {
                    raycastInfo.limb = ELimb.SKULL;
                }
                raycastInfo.player = Functions.GetNearestPlayer();
            }
            else
            {
                raycastInfo.limb = DamageTool.getLimb(hit.transform);
                if (hit.transform.CompareTag("Enemy"))
                {
                    raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);
                }
            }

            if (hit.transform.CompareTag("Vehicle"))
            {
                raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);
            }
            if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
            {
                raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
            }
            else
            {
                raycastInfo.material = EPhysicsMaterial.NONE;
            }
            return raycastInfo;
        }

        public static System.Random Random;
    }
}
