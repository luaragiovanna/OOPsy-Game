using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Color, Method, MethodValue }
    public ItemType itemType;
    public string value;
    private ItemCollectionManager itemCollectionManager;

    void Start()
    {
        itemCollectionManager = FindObjectOfType<ItemCollectionManager>();
    }

    public void OnMouseDown()
    {
        Collect();
    }

    public void Collect()
    {
        if (itemCollectionManager != null)
        {
            itemCollectionManager.ItemCollected(itemType, value);
        }

        Destroy(gameObject);
    }
}
