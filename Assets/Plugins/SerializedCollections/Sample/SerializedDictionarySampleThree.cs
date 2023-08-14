using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    public class SerializedDictionarySampleThree : MonoBehaviour
    {
        [SerializeField]
        [SerializedDictionary("Element Type", "Description")]

        private SerializedDictionary<ScriptableObject, string> _nameOverrides;
    }
}