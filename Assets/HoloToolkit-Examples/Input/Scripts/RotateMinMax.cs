// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Examples.GazeRuler;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace HoloToolkit.Unity.Tests
{
    public class RotateMinMax : MonoBehaviour
    {
        public Transform cursor;
        Camera referenceCamera;

        private void Start()
        {
            // if no camera referenced, grab the main camera
            if (!referenceCamera)
                referenceCamera = Camera.main;

        }


        private void Update()
        {
            transform.LookAt(transform.position + referenceCamera.transform.rotation * Vector3.back, referenceCamera.transform.rotation * Vector3.up);

            //     [SerializeField] private float _minAngle;

            //[SerializeField] private float _maxAngle;

            //[SerializeField] private float _step;



            //private void Update()

            //{

            //    transform.Rotate(Vector3.up, _step);

            //    if (transform.localRotation.eulerAngles.y < _minAngle || transform.localRotation.eulerAngles.y > _maxAngle)

            //    {

            //        _step *= -1;

            //    }

            //}
            //Vector3 hitPoint = GazeManager.Instance.HitPosition;
            //    Vector3 centerPos = hitPoint * 0.5f;
            //    Vector3 cameraPosition = CameraCache.Main.transform.position;
            //    Vector3 directionFromCamera = centerPos - cameraPosition;
            //    float distanceA = Vector3.Distance(hitPoint, cameraPosition);
            //    float distanceB = Vector3.Distance(transform.position, cameraPosition);

            //    Vector3 direction;
            //    if (distanceB > distanceA || (distanceA > distanceB && distanceA - distanceB < 0.1))
            //    {
            //        direction = hitPoint - transform.position;
            //    }
            //    else
            //    {
            //        direction = transform.position - hitPoint;
            //    }
            //    Vector3 normalV = Vector3.Cross(direction, directionFromCamera);
            //    Vector3 normalF = Vector3.Cross(direction, normalV) * -1;

            //    Debug.Log("A: " + normalF);
            //    transform.rotation = Quaternion.LookRotation(normalF);

            //    if (transform.localRotation.eulerAngles.y < _minAngle || transform.localRotation.eulerAngles.y > _maxAngle)
            //    {
            //        _step *= -1;
            //    }
        }
    }
}
