// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Persistence;
using System;
using UnityEngine.VR.WSA;
using System.Collections.Generic;

namespace HoloToolkit.Examples.GazeRuler
{
    /// <summary>
    /// mananger all lines in the scene
    /// </summary>
    public class LineManager : Singleton<LineManager>, IGeometry
    {
        public GameObject PointPrefabReSpawn;
        public GameObject LinePrefabReSpawn;
        public GameObject TipPrefabReSpawn;
        // save all lines in scene        
        private Stack<Line> Lines = new Stack<Line>();
        private Stack<Line> LinesPoint = new Stack<Line>();
        private Stack<Line> TipsText = new Stack<Line>();

        private Point lastPoint;

        private const float defaultLineScale = 0.005f;
        private static int _pointCount, _lineCount, tipCount;
        //to store anchors
        private WorldAnchorStore _store;

        public void Start()
        {
            WorldAnchorStore.GetAsync(WorldAnchorStoreLoaded);
        }


        private void WorldAnchorStoreLoaded(WorldAnchorStore store)
        {
            _store = store;
            LoadAllAnchors();
        }

        void LoadAllAnchors()
        {
            var allIds = _store.GetAllIds();
            _pointCount = allIds.Length;
            foreach (var id in allIds)
            {
                if (id.ToString().Contains("Point"))
                {
                    var newLine = Instantiate(PointPrefabReSpawn);
                    _store.Load(id, newLine);
                }
                //if (id.ToString().Contains("Line"))
                //{
                //    var newGuitar = Instantiate(LinePrefabReSpawn);
                //    _store.Load(id, newGuitar);
                //}
                //if (id.ToString().Contains("Tip"))
                //{
                //    var newGuitar = Instantiate(TipPrefabReSpawn);
                //    _store.Load(id, newGuitar);
                //}
            }
        }

        // place point and lines
        public void AddPoint(GameObject LinePrefab, GameObject PointPrefab, GameObject TextPrefab)
        {
            if (GazeManager.Instance.HitObject.tag != "UI")
            {
                Vector3 hitPoint = GazeManager.Instance.HitPosition;

                GameObject point = (GameObject)Instantiate(PointPrefab, hitPoint, Quaternion.identity);
                
                if (lastPoint != null && lastPoint.IsStart)
                {
                    Vector3 centerPos = (lastPoint.Position + hitPoint) * 0.5f;
                    Vector3 cameraPosition = CameraCache.Main.transform.position;
                    Vector3 directionFromCamera = centerPos - cameraPosition;

                    float distanceA = Vector3.Distance(lastPoint.Position, cameraPosition);
                    float distanceB = Vector3.Distance(hitPoint, cameraPosition);

                    Debug.Log("A: " + distanceA + ",B: " + distanceB);
                    Vector3 direction;
                    if (distanceB > distanceA || (distanceA > distanceB && distanceA - distanceB < 0.1))
                    {
                        direction = hitPoint - lastPoint.Position;
                    }
                    else
                    {
                        direction = lastPoint.Position - hitPoint;
                    }

                    float distance = Vector3.Distance(lastPoint.Position, hitPoint);
                    GameObject line = (GameObject)Instantiate(LinePrefab, centerPos, Quaternion.LookRotation(direction));
                    line.transform.localScale = new Vector3(distance, defaultLineScale, defaultLineScale);
                    line.transform.Rotate(Vector3.down, 90f);
                    //var worldLineAnchor = point.AddComponent<WorldAnchor>();
                    //_lineCount++;
                    //var anchorLineName = string.Format("Point{0:000}", _lineCount);
                    //if (!Application.isEditor)
                    //{
                    //    _store.Save(anchorLineName, worldLineAnchor);
                    //}

                    Vector3 normalV = Vector3.Cross(direction, directionFromCamera);
                    Vector3 normalF = Vector3.Cross(direction, normalV) * -1;
                    GameObject tip = (GameObject)Instantiate(TextPrefab, centerPos, Quaternion.LookRotation(normalF));

                    //unit is meter
                    tip.transform.Translate(Vector3.up * 0.05f);
                    tip.GetComponent<TextMesh>().text = distance + "m";

                    GameObject root = new GameObject();
                    if (lastPoint.Root.transform.parent)
                    {
                        lastPoint.Root.transform.parent = root.transform;
                    }
                    line.transform.parent = root.transform;
                    point.transform.parent = root.transform;
                    tip.transform.parent = root.transform;
                
                    //Add World Anchors to save position
                    var worldAnchor = root.AddComponent<WorldAnchor>();
                    _pointCount++;
                    var anchorName = string.Format("Point{0:000}", _pointCount);
                    if (!Application.isEditor)
                    {
                        _store.Save(anchorName, worldAnchor);
                        root.name = anchorName;
                    }
                    
                    Lines.Push(new Line
                    {
                        Start = lastPoint.Position,
                        End = hitPoint,
                        Root = root,
                        Distance = distance
                    });

                    lastPoint = new Point
                    {
                        Position = hitPoint,
                        Root = point,
                        IsStart = true
                    };

                }
                else
                {
                    lastPoint = new Point
                    {
                        Position = hitPoint,
                        Root = point,
                        IsStart = true
                    };
                    
                    LinesPoint.Push(new Line { Root = point });
                }
            }
        }

        //Remove worl anchor component
        private void ClearAnchor(GameObject theGameObjectIWantAnchored)
        {
            var anchor = theGameObjectIWantAnchored.GetComponent<WorldAnchor>();
            if (anchor)
            {
                // remove any world anchor component from the game object so that it can be moved
                _store.Delete(theGameObjectIWantAnchored.name);
                DestroyImmediate(anchor);
            }
        }

        // delete latest placed lines
        public void Delete()
        {
            if (Lines != null && Lines.Count > 0)
            {
                Line lastLine = Lines.Pop();
                if (!Application.isEditor)
                {
                    ClearAnchor(lastLine.Root);
                }
                Destroy(lastLine.Root);
                int index = Lines.Count -1;
                if (Lines.Count > 0)
                {
                    lastLine = Lines.Peek();
                    GameObject point = (GameObject)Instantiate(PointPrefabReSpawn, lastLine.End, Quaternion.identity);
                    lastPoint = new Point
                    {
                        Position = lastLine.End,
                        Root = point,
                        IsStart = true
                    };
                    lastPoint.Root.transform.parent = lastLine.Root.transform;
                }
                else
                {
                    lastPoint = null; 
                    lastLine =LinesPoint.Peek();
                    Destroy(lastLine.Root);
                }
                
                //Reset();
            }

        }

        // delete all lines in the scene
        public void Clear()
        {
            if (Lines != null && Lines.Count > 0)
            {
                while (Lines.Count > 0)
                {
                    Line lastLine = Lines.Pop();
                    if (!Application.isEditor)
                    {
                        ClearAnchor(lastLine.Root);
                    }
                    Destroy(lastLine.Root);
                }
            }
        }

        // reset current unfinished line
        public void Reset()
        {
            if (lastPoint != null && lastPoint.IsStart)
            {
                Destroy(lastPoint.Root);
                if (!Application.isEditor)
                {
                    ClearAnchor(lastPoint.Root);
                }
                lastPoint = null;
            }
        }
        // reset current unfinished line
        public void EndLine()
        {
            if (lastPoint != null && lastPoint.IsStart)
            {               
                lastPoint.IsStart = false;
            }
            //Delete();
        }
    }


    public struct Line
    {
        public Vector3 Start { get; set; }

        public Vector3 End { get; set; }

        public GameObject Root { get; set; }

        public float Distance { get; set; }

        public GameObject LastPoint { get; set; }
    }
}