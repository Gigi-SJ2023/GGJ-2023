using UnityEditor;
using PlayerHorde;

namespace SerializableUtils
{
    [CustomPropertyDrawer(typeof(SerializableHordeIntTypeDictionary))]
    public class SerializableHordeIntDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}