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

namespace EyeTribe.Unity.Interaction
{

    public class SphericalCreatorInteraction : SphericalCreator
    {
        [SerializeField]private Transform _ReticleTransform;

        protected override void Awake()
        {
            base.Awake();

            if (null == _ReticleTransform)
                throw new Exception("_ReticleTransform is not set");
        }

        protected override GameObject CreateObject(Vector3 position)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = position;

            VRInteractiveItem vrii = go.AddComponent<VRInteractiveItem>();
            InteractiveColorInterpolator ici  = go.AddComponent<InteractiveColorInterpolator>();

            ici.InteractiveItem = vrii;
            ici.Initialize();

            return go;
        }
    }
}
