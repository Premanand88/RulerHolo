using UnityEngine;
using HoloToolkit.Sharing.SyncModel;
using System;

namespace HoloToolkit.Sharing.Tests
{
    /// <summary>
    /// A SpawnedObject contains all the information needed for another device to spawn an object in the same location
    /// as where it was originally created on this device.
    /// </summary>
    [SyncDataClass]
    public class SyncSpawnedObject : SyncObject
    {
        /// <summary>
        /// Transform (position, rotation, and scale) for the object.
        /// </summary>
        [SyncData] public SyncTransform Transform;

        /// <summary>
        /// Name of the object.
        /// </summary>
        [SyncData] public SyncString Name;

        /// <summary>
        /// Path to the parent object in the game object.
        /// </summary>
        [SyncData] public SyncString ParentPath;

        /// <summary>
        /// Path to the object
        /// </summary>
        [SyncData] public SyncString ObjectPath;


        public GameObject GameObject { get; set; }

        public virtual void Initialize(string name, string parentPath)
        {
            Name.Value = name;
            ParentPath.Value = parentPath;

            ObjectPath.Value = string.Empty;
            if (!string.IsNullOrEmpty(ParentPath.Value))
            {
                ObjectPath.Value = ParentPath.Value + "/";
            }

            ObjectPath.Value += Name.Value;
        }
    }

}