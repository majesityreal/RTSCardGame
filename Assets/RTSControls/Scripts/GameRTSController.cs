﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameRTSController : MonoBehaviour {

    [SerializeField] private Transform selectionAreaTransform;

    private Vector3 startPosition;
    public List<Unit> selectedUnitList;

    private void Awake() {
        selectedUnitList = new List<Unit>();
        selectionAreaTransform.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // Left Mouse Button Pressed
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButton(0)) {
            // Left Mouse Button Held Down
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0)) {
            // Left Mouse Button Released
            selectionAreaTransform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

            // Deselect all Units
            foreach (Unit unit in selectedUnitList) {
                unit.SetSelectedVisible(false);
            }
            selectedUnitList.Clear();

            // Select Units within Selection Area
            foreach (Collider2D collider2D in collider2DArray) {
                Unit unit = collider2D.GetComponent<Unit>();
                if (unit != null) {
                    unit.SetSelectedVisible(true);
                    selectedUnitList.Add(unit);
                }
            }

            Debug.Log(selectedUnitList.Count);
        }
/*
        if (Input.GetMouseButtonDown(1)) {
            // Right Mouse Button Pressed
            Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();

            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 10f, 20f, 30f }, new int[] { 5, 10, 20 });

            int targetPositionListIndex = 0;

            foreach (Unit unit in selectedUnitList) {
                unit.MoveTo(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }*/
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray) {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++) {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount) {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++) {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

}
