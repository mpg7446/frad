using UnityEngine;

[CreateAssetMenu(menuName = "FRAD/Item (Scriptable Item)", fileName = "New Item")]
public class ScriptableItem : ScriptableObject
{
    // TODO - I really dont know how to set this up
    // possibly need some more time to sketch up ideas/specifics of what I want
    // items to be and do

    public enum ItemType {
        Modifier,
        Action
    }

    [Tooltip("ID is set automatically by InventoryManager, there is no need to change this")]
    [HideInInspector] public int ID;

    [Space]
    public string name;
    [TextArea]
    public string description;
    public ItemType type = ItemType.Modifier;

    [Space]
    public float speedMultiplier = 1;
    public float sprintSpeedMultiplier = 1;
    public float sprintDurationMultiplier = 1;

}
