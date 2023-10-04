using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "CraftingRequirement")]
public class CraftingRequirement : SerializedScriptableObject
{
    [OdinSerialize] public ItemStack[] Items { get { return _items; } }
    [OdinSerialize] public ItemStack[] _items;
}
