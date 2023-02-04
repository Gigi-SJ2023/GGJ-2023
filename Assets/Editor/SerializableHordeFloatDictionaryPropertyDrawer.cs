using UnityEditor;
using PlayerHorde;

namespace SerializableUtils
{
    [CustomPropertyDrawer(typeof(SerializableHordeFloatTypeDictionary))]
    public class SerializableHordeFloatDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}