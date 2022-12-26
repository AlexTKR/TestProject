using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoldersAndSettings
{
    [Serializable]
    public class SerializableEnum<T> where T : struct, IConvertible
    {
        [SerializeField] private string mEnumValueAsString;
        [SerializeField] private T mEnumValue;

        public T Value
        {
            get { return mEnumValue; }
            set { mEnumValue = value; }
        }
    }

    [Serializable]
    public class UpgradeEnum : SerializableEnum<UpgradeType>
    {
    }

    [Serializable]
    public enum UpgradeType
    {
        Speed,
        Damage,
        Radius
    }

    [Serializable]
    public class UpgradeData
    {
        public UpgradeEnum UpgradeType;
        public float Step;
        public float Chance;
    }

    [CreateAssetMenu(menuName = "Settings/UpgradeSettings")]
    public class UpgradeSettings : ScriptableObject
    {
        public UpgradeData[] UpgradeData;
    }
}