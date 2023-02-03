using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HordeMemberType
{
    Carrot,
    Potato,
    Radish,
    Onion
}
public class HordeController : MonoBehaviour
{
    public SerializableHordeTypeDictionary HordeMembers;
    public SerializableHordeTypeDictionary HordeMembersDamage;

}
