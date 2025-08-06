using UnityEngine;

[CreateAssetMenu(menuName = "FRAD/Item (Scriptable Item)", fileName = "New Item")]
public class ScriptableItem : ScriptableObject
{
    // TODO - I really dont know how to set this up
    // possibly need some more time to sketch up ideas/specifics of what I want items to be and do

    public enum ItemType { // might use this later to dictate if this is a "passive" or "action" based item
        Modifier,
        Action
    }

    [Space]
    public new string name;
    [Tooltip("Name referenced by internal methods, must be unique")]
    public string internalName;
    [TextArea]
    public string description;
    public ItemType type = ItemType.Modifier;

    [Space]
    public float speedMultiplier = 1;
    public float sprintSpeedMultiplier = 1;
    public float sprintDurationMultiplier = 1;

}
