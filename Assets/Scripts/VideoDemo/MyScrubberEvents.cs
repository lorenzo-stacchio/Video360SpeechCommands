//-----------------------------------------------------------------------
// <copyright file="ScrubberEvents.cs" company="Google Inc.">
// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// An object for handling pointer interactions with the Video Scrubber (time selection slider).
    /// </summary>
    public class MyScrubberEvents : MonoBehaviour
    {
        private GameObject newPositionHandle;

        private Vector3[] corners;
        private Slider slider;

        private MyVideoControlsManager mgr;

        /// <summary>Sets the control manager.</summary>
        /// <value>The control manager.</value>
        public MyVideoControlsManager ControlManager
        {
            set
            {
                mgr = value;
            }
        }

        /// <summary>Called when a pointer enter event occurs.</summary>
        /// <param name="data">The EventData for the triggering pointer enter event.</param>
        public void OnPointerEnter(BaseEventData data)
        {
            if (GvrPointerInputModule.Pointer != null)
            {
                RaycastResult r = GvrPointerInputModule.Pointer.CurrentRaycastResult;
                if (r.gameObject != null)
                {
                    newPositionHandle.transform.position = new Vector3(
                        r.worldPosition.x,
                        newPositionHandle.transform.position.y,
                        newPositionHandle.transform.position.z);
                }
            }

            newPositionHandle.SetActive(true);
        }

        /// <summary>Called when a pointer exit event occurs.</summary>
        /// <param name="data">The EventData for the triggering pointer exit event.  Unused.</param>
        public void OnPointerExit(BaseEventData data)
        {
            newPositionHandle.SetActive(false);
        }

        /// <summary>Called when a pointer click event occurs.</summary>
        /// <param name="data">The EventData for the triggering pointer click event. Unused.</param>
        public void OnPointerClick(BaseEventData data)
        {
            float minX = corners[0].x;
            float maxX = corners[3].x;

            float pct = (newPositionHandle.transform.position.x - minX) / (maxX - minX);

            if (mgr != null)
            {
                long p = (long)(slider.maxValue * pct);
                mgr.Player.frame = p;
            }
        }

        private void Start()
        {
            foreach (Image im in GetComponentsInChildren<Image>(true))
            {
                if (im.gameObject.name == "newPositionHandle")
                {
                    newPositionHandle = im.gameObject;
                    break;
                }
            }

            corners = new Vector3[4];
            GetComponent<Image>().rectTransform.GetWorldCorners(corners);
            slider = GetComponentInParent<Slider>();
        }

        private void Update()
        {
            bool setPos = false;
            if (GvrPointerInputModule.Pointer != null)
            {
                RaycastResult r = GvrPointerInputModule.Pointer.CurrentRaycastResult;
                if (r.gameObject != null)
                {
                    newPositionHandle.transform.position = new Vector3(
                        r.worldPosition.x,
                        newPositionHandle.transform.position.y,
                        newPositionHandle.transform.position.z);
                    setPos = true;
                }
            }

            if (!setPos)
            {
                newPositionHandle.transform.position = slider.handleRect.transform.position;
            }
        }
    }
}
