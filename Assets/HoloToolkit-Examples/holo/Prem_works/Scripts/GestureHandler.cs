using System;
using UnityEngine;
#if UNITY_WSA
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR.WSA.Input;
#else
using UnityEngine.VR.WSA.Input;
#endif
using System.Collections.Generic;
#endif

namespace Assets.HoloToolkit_Examples.holo.Prem_works.Scripts
{

    public class GestureHandler : MonoBehaviour
    {
        private GestureRecognizer _gestureRecognizer;
        public GameObject objectToPlace;
        private void Start()
        {
            _gestureRecognizer = new GestureRecognizer();
            _gestureRecognizer.TappedEvent += GestureRecognizerOnTappedEvent;
            _gestureRecognizer.StartCapturingGestures();
        }

        private void GestureRecognizerOnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {        // Here we handle the events    
            if (objectToPlace == null) return;
            var distance = new System.Random().Next(2, 10);
            var location = transform.position + transform.forward * distance;
            Instantiate(objectToPlace, location, Quaternion.LookRotation(transform.up, transform.forward));
        }
    }
}
