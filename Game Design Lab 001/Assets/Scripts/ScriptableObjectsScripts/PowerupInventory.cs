using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupInventory", menuName = "ScriptableObjects/PowerupInventory", order = 6)]
public class PowerupInventory : Inventory<Powerup> //inherits from Inventory base class with Powerup as object type
{
}