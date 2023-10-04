using Sirenix.OdinInspector;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private float _collectableTimer;
    [SerializeField] private float _collectableMagnetSpeed;

    public ItemStack Item { get { return _item; } set { _item = value; } }
    [ShowInInspector] private ItemStack _item;
    private bool _collectable;
    private bool _collecting;
    private GameObject _justDroppedBy;
    private float _justDroppedByTimer;
    [SerializeField] private float _collectionRadius;

    private Rigidbody2D _rb;
    private GameObject _collector;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponentsInChildren<Collider2D>()[1]);
    }

    private void Update()
    {
        if (_collector == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _collectionRadius);
            foreach (Collider2D c in colliders)
            {
                if (c.CompareTag("Player"))
                    SetCollector(c.gameObject);
            }
        }
        else
        {
            bool collecting = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _collectionRadius);
            foreach (Collider2D c in colliders)
            {
                if (c.CompareTag("Player"))
                    collecting = true;
            }
            if (!collecting)
                SetCollector(null);
        }


        #region Timers
        if (_collectableTimer >= 0)
            _collectableTimer -= Time.deltaTime;

        if (_collectableTimer < 0)
            _collectable = true;

        if (_justDroppedByTimer >= 0)
            _justDroppedByTimer -= Time.deltaTime;

        if (_justDroppedByTimer < 0)
            _justDroppedBy = null;
        #endregion

        if (!_collecting)
            return;

        //Stop collecting if player inventory is full
        if (_collector != null && _collector.GetComponent<InventorySystem>().IsFull(_item))
        {
            _collecting = false;
            _collector = null;
        }
        //Remove the player that dropped the
        //item from next collector
        if (_collector == _justDroppedBy)
            _collecting = false;

        //Don't do anything if there is no target collector
        if (_collector == null || _collector == _justDroppedBy)
            return;

        //Move to collector
        Vector2 targetVelocity = (_collector.transform.position - transform.position).normalized * 10000;
        _rb.velocity = Vector2.ClampMagnitude(targetVelocity, _collectableMagnetSpeed);

        //Rotate to collector
        if (_rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { CollectItem(other.gameObject); }

    private void OnTriggerStay2D(Collider2D other) { CollectItem(other.gameObject); }

    private void CollectItem(GameObject collectableItem)
    {
        if (collectableItem.GetComponent<InventorySystem>() == null)
            return;
        if (collectableItem == _justDroppedBy)
            return;

        if (_collectable && !collectableItem.GetComponent<InventorySystem>().IsFull(_item))
        {
            int prevSize = Item.StackSize;
            ItemStack item = collectableItem.GetComponent<InventorySystem>().Add(_item);
            //Play sound if item has been increased
            if (item != null && item.StackSize < prevSize)
                VolatileSound.Create(AssetManager.Instance.PickupSound);
            if (item == null)
            {
                VolatileSound.Create(AssetManager.Instance.PickupSound);
                Destroy(this.gameObject);
            }
            else
                _item = item;
        }
    }

    public void DroppedBy(GameObject obj)
    {
        _justDroppedBy = obj;
        _justDroppedByTimer = 2f;
    }

    public void SetCollector(GameObject entity)
    {
        if (entity == null)
        {
            _collecting = false;
            _collector = null;
            return;
        }
        if (_collectable)
        {
            _collecting = true;
            _collector = entity;
        }
    }

    public bool IsCollectable() { return _collectable; }

    public bool IsCollecting() { return _collecting; }

    public static GameObject Create(ItemStack itemStack)
    {
        itemStack = new ItemStack(itemStack);
        GameObject itemObject = Instantiate(AssetManager.Instance.CollectableItemPrefab);
        itemObject.GetComponent<SpriteRenderer>().sprite = itemStack.ItemData.Icon;
        itemObject.GetComponent<CollectableItem>()._item = itemStack;
        return itemObject;
    }
}
