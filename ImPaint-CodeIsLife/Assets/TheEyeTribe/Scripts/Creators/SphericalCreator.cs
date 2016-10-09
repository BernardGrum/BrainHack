/*
 * Copyright (c) 2013-present, The Eye Tribe. 
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the LICENSE file in the root directory of this source tree. 
 *
 */

using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace EyeTribe.Unity.Interaction
{
    /// <summary>
    /// Utility class the creates GameObjects in a spherical pattern around parent GO
    /// <para/>
    /// Classes wishing to create an arbitrary GameObject should extend this class and
    /// override CreateObject() method.
    /// </summary>
    public class SphericalCreator : MonoBehaviour
    {
        [SerializeField] private float _Radius = 10f;
        [SerializeField] private float _RandomRange;
        [SerializeField] private int _NumLongitude = 1;
        [SerializeField] private int _NumLatitude = 1;
        [SerializeField] private float _DuplicateDistance = .5f;
        [SerializeField] private bool _FaceCenter = true;

        protected virtual void Awake()
        {
            if (_Radius <= 0f)
                throw new Exception("_Radius must be a positive number");

            if (_RandomRange < 0f)
                throw new Exception("_RandomRange must be a positive number");

            if (_NumLongitude < 1f)
                throw new Exception("_NumLongitude must be a positive and non-zero number");

            if (_NumLatitude < 1f)
                throw new Exception("_NumLatitude must be a positive and non-zero number");

            if (_DuplicateDistance < 0f)
                throw new Exception("_DuplicateDistance must be a positive number");
        }

        void OnEnable() 
        {
            Initialize();
        }

        void OnDisable()
        {
            foreach (Transform child in transform)
                GameObject.Destroy(child.gameObject);
        }

        protected virtual void Initialize()
        {
            var pos = new Vector3(0, 0, 0);

            GameObject go = CreateObject(pos);
            go.transform.parent = transform;
        }

        protected virtual GameObject CreateObject(Vector3 position)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

            go.transform.position = position;

            return go;
        }
    }
}
