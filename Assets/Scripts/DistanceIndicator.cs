﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Portalble {
    /**
     * An distance indicator, to measure distance and manage display even self eliminate
     * Actually, the elimination should be handled in IndicatorManager.
     * However, the distance calculation is different from Manager and indicator. They get different result.
     * For manager it's hard to handle which one isn't in range. For indicator, it's hard to directly use distance to deactive
     * itself since the manager may immediately reactive it (they got different distance result)
     */
    public class DistanceIndicator : IDistanceIndicator {
        public Text distanceText;
        public LineRenderer distanceLine;
        public AudioSource notifySound;
        /*
        private Transform handLT;
        private Transform handRT;

        private Transform trackingObject;*/

        /*
        // Here, find Hand we need
        protected override void Start() {
            base.Start();
            GameObject gobj = GameObject.Find("Hand_l");
            if (gobj != null)
                handLT = gobj.transform.Find("palm");
            gobj = GameObject.Find("Hand_r");
            if (gobj != null)
                handRT = gobj.transform.Find("palm");
        }*/

        // Update is called once per frame
        protected override void Update() {
            // Make it look towards camera
            Camera cam = Camera.main;
            if (cam != null) {
                transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
            }

            // If it's tracking, find distance
            if (trackingObject != null && distanceText != null) {
                // Debug.Log("tracking:" + trackingObject.position + "&" + handLT.position);
                float minDis = 99999f;
                bool isLeftNear = true;
                if (handLT != null) {
                    minDis = (trackingObject.position - handLT.position).magnitude;
                }
                if (handRT != null) {
                    float tmpDis = (trackingObject.position - handRT.position).magnitude;
                    // Same logic, since if handLT doesn't exist, minDis is -1, where tmpDis is impossible to be negative
                    if (tmpDis < minDis) {
                        minDis = tmpDis;
                        isLeftNear = false;
                    }
                }

                // pitch varies depends on distance
                notifySound.pitch = 4f / (1f + minDis * 10f);

                // Debug.Log(minDis);
                minDis *= 1000f;
                distanceText.text = Mathf.RoundToInt(minDis).ToString();

                // Draw lines
                distanceLine.positionCount = 2;
                distanceLine.SetPosition(0, trackingObject.position);
                if (isLeftNear) {
                    distanceLine.SetPosition(1, handLT.position);
                }
                else {
                    distanceLine.SetPosition(1, handRT.position);
                }
            }

            base.Update();
        }

        /*
        public override void SetToAnInteractiveObject(Transform t, float activeTime = 1.2f) {
            transform.SetParent(null, false);
            // Set position

            trackingObject = t;

            outofdateValue = activeTime;

            // Set size
            MeshFilter mf = t.GetComponent<MeshFilter>();
            if (mf != null) {
                Bounds bds = mf.mesh.bounds;
                // Find the max one
                float maxvalue = 0f;
                for (int i = 0; i < 3; ++i) {
                    if (maxvalue < bds.size[i])
                        maxvalue = bds.size[i];
                }

                transform.SetParent(t);
                transform.localPosition = bds.center;
                // Adjust size
                float factor = 0.012f * maxvalue;
                transform.localScale = new Vector3(factor, factor, factor);
            }
            else {
                transform.position = t.position;
                transform.SetParent(t);
            }
            gameObject.SetActive(true);
        }*/

        /*
        public void RefreshActiveTime(float time = 1.0f) {
            outofdataValue = time;
        }*/

        public override void UpdateConfig(IndicatorManager.DI_CONFIG config) {
            transform.Find("Indicator").gameObject.SetActive(config.useSphereText);
            distanceText.gameObject.SetActive(config.useSphereText);
            distanceLine.gameObject.SetActive(config.useLine);
            notifySound.gameObject.SetActive(config.useSound);
        }
    }
}