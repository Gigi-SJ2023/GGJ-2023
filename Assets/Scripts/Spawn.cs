
using PlayerHorde;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "Spawnables/Standard")]
public class Spawn : ScriptableObject
{
    [field: SerializeField]
    public GameObject Prefab
    {
        get;
        protected set;
    }

    public HordeMemberType type;

    public virtual GameObject SpawnUnit()
    {
        return Instantiate(Prefab);
    }
}
