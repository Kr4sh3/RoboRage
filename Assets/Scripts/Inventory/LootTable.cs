using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "LootTable")]
public class LootTable : SerializedScriptableObject
{
    [OdinSerialize] public Dictionary<ItemStack, float> Items { get { return _items; } }
    [OdinSerialize] public Dictionary<ItemStack, float> _items;
}

