using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string ID { get { return _id; } }
    [SerializeField] private string _id;

    public string DisplayName { get { return _displayName; } }
    [SerializeField] private string _displayName;

    public Sprite Icon { get { return _icon; } }
    [SerializeField] private Sprite _icon;

    public int MaxStackSize { get { return _maxStackSize; } }
    [SerializeField] private int _maxStackSize;
}
