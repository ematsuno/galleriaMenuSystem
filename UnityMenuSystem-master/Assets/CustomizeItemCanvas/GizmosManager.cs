using System;
using System.Collections;
using System.Collections.Generic;
using RTG;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GizmosManager : MonoBehaviour
{
    public Slider xSlider;
    public Slider ySlider;
    public Slider zSlider;

    public Slider scaleSlider;
    public Slider rotaionSlider;


    public GameObject cubeObject;

    public float xSliderMinValue = -3;
    public float xSliderMaxValue = 3;

    public float ySliderMinValue = -2;
    public float ySliderMaxValue = 2;

    public float zSliderMinValue = -3;
    public float zSliderMaxValue = 3;

    public float scaleSliderMinValue = 1;
    public float scaleSliderMaxValue = 5;

    public float rotaionSliderMinValue = 0;
    public float rotaionSliderMaxValue = 360;

    private ObjectTransformGizmo objectMoveGizmo;
    private ObjectTransformGizmo objectRotationGizmo;
    private ObjectTransformGizmo objectScaleGizmo;
    private ObjectTransformGizmo objectUniversalGizmo;

    private ObjectTransformGizmo activeGizmo;




    private enum GizmoId
    {
        Move = 1,
        Rotate,
        Scale,
        Universal
    }
    private GizmoId _workGizmoId;

    private ObjectTransformGizmo _workGizmo;


    void Start()
    {
        objectMoveGizmo = RTGizmosEngine.Get.CreateObjectMoveGizmo();
        objectRotationGizmo = RTGizmosEngine.Get.CreateObjectRotationGizmo();
        objectScaleGizmo = RTGizmosEngine.Get.CreateObjectScaleGizmo();
        objectUniversalGizmo = RTGizmosEngine.Get.CreateObjectUniversalGizmo();


        //objectMoveGizmo.Gizmo.SetEnabled(false);
        objectRotationGizmo.Gizmo.SetEnabled(false);
        objectScaleGizmo.Gizmo.SetEnabled(false);
        objectUniversalGizmo.Gizmo.SetEnabled(false);

        objectMoveGizmo.SetTargetObject(cubeObject);
        objectRotationGizmo.SetTargetObject(cubeObject);
        objectScaleGizmo.SetTargetObject(cubeObject);
        objectUniversalGizmo.SetTargetObject(cubeObject);


        _workGizmo = objectMoveGizmo;
        _workGizmoId = GizmoId.Move;


        var res = new ObjectTransformGizmo.ObjectRestrictions();
        res.SetCanMoveAlongAxis(1, false);
        res.SetCanMoveAlongAxis(2, false);

        res.SetIsAffectedByHandle(GizmoHandleId.XRotationSlider, false);
        res.SetIsAffectedByHandle(GizmoHandleId.ZRotationSlider, false);
        res.SetIsAffectedByHandle(GizmoHandleId.CamZRotation, false);
        res.SetIsAffectedByHandle(GizmoHandleId.CamXYRotation, false);

        objectRotationGizmo.RegisterObjectRestrictions(cubeObject, res);



        objectMoveGizmo.Gizmo.PostDragUpdate += Gizmo_PostDragUpdatePos;
        objectMoveGizmo.Gizmo.PostDragEnd += Gizmo_PostDragEnd;
        objectRotationGizmo.Gizmo.PostDragUpdate += Gizmo_PostDragUpdateRotate;
        objectScaleGizmo.Gizmo.PostDragUpdate += Gizmo_PostDragUpdateScale;

        //Setting Values
        xSlider.minValue = xSliderMinValue;
        xSlider.maxValue = xSliderMaxValue;

        ySlider.minValue = ySliderMinValue;
        ySlider.maxValue = ySliderMaxValue;

        zSlider.minValue = zSliderMinValue;
        zSlider.maxValue = zSliderMaxValue;

        scaleSlider.minValue = scaleSliderMinValue;
        scaleSlider.maxValue = scaleSliderMaxValue;

        rotaionSlider.minValue = rotaionSliderMinValue;
        rotaionSlider.maxValue = rotaionSliderMaxValue;


    }

    private void Gizmo_PostDragEnd(Gizmo gizmo, int handleId)
    {
        //objectMoveGizmo.Gizmo.SetEnabled(false);
        //objectMoveGizmo.Gizmo.SetEnabled(true);
    }

    private void Gizmo_PostDragUpdateScale(Gizmo gizmo, int handleId)
    {
        Vector3 relativeScale = gizmo.RelativeDragScale;
        Vector3 totalScale = gizmo.TotalDragScale;
        float sValue = scaleSlider.value;
        float scaleValue = 1;

        if (GizmoHandleId.PZCap == handleId || GizmoHandleId.PZSlider == handleId)
        {
            scaleValue = Mathf.Abs(1 - relativeScale.z);
        }

        if (GizmoHandleId.PYCap == handleId || GizmoHandleId.PYSlider == handleId)
        {
            scaleValue = Mathf.Abs(1 - relativeScale.y);
        }

        if (GizmoHandleId.PXCap == handleId || GizmoHandleId.PXSlider == handleId || GizmoHandleId.MidScaleCap == handleId)
        {
            scaleValue = Mathf.Abs(1 - relativeScale.x);
        }

        if (relativeScale.z > 1 || relativeScale.y > 1 || relativeScale.x > 1)
        {
            sValue += scaleValue;
        }
        else if (relativeScale.z < 1 || relativeScale.y < 1 || relativeScale.x < 1)
        {
            sValue -= scaleValue;
        }

        scaleSlider.SetValueWithoutNotify(sValue);
        cubeObject.transform.SetWorldScale(new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value));
    }

    private void Gizmo_PostDragUpdateRotate(Gizmo gizmo, int handleId)
    {
        if (GizmoHandleId.YRotationSlider == handleId)
        {
            Quaternion relativeDrag = gizmo.RelativeDragRotation;
            float sValue = rotaionSlider.value;

            if (relativeDrag.y > 0)
            {
                sValue += relativeDrag.eulerAngles.y;
            }
            else if (relativeDrag.y < 0)
            {
                var absValue = 360 - relativeDrag.eulerAngles.y;
                sValue -= absValue;
                Debug.Log(absValue);
            }
            rotaionSlider.SetValueWithoutNotify(sValue);

            cubeObject.transform.rotation = Quaternion.Euler(0, rotaionSlider.value, 0);
        }
    }

    private void Gizmo_PostDragUpdatePos(Gizmo gizmo, int handleId)
    {
        GizmoDragChannel dragChannel = gizmo.ActiveDragChannel;

        Vector3 relativeOffset = gizmo.RelativeDragOffset;
        Vector3 totalOffset = gizmo.TotalDragOffset;

        SetSliderValue(xSlider, relativeOffset.x);
        SetSliderValue(ySlider, relativeOffset.y);
        SetSliderValue(zSlider, relativeOffset.z);

        cubeObject.transform.position = new Vector3(xSlider.value, ySlider.value, zSlider.value);



    }

    void Update()
    {
        if (activeGizmo != null)
        {
            activeGizmo.RefreshPositionAndRotation();
        }

        if (Input.GetKeyDown(KeyCode.W)) SetWorkGizmoId(GizmoId.Move);
        else if (Input.GetKeyDown(KeyCode.E)) SetWorkGizmoId(GizmoId.Rotate);
        else if (Input.GetKeyDown(KeyCode.R)) SetWorkGizmoId(GizmoId.Scale);

    }

    private void SetWorkGizmoId(GizmoId gizmoId)
    {
        if (gizmoId == _workGizmoId) return;

        objectMoveGizmo.Gizmo.SetEnabled(false);
        objectRotationGizmo.Gizmo.SetEnabled(false);
        objectScaleGizmo.Gizmo.SetEnabled(false);
        objectUniversalGizmo.Gizmo.SetEnabled(false);

        _workGizmoId = gizmoId;
        if (gizmoId == GizmoId.Move) _workGizmo = objectMoveGizmo;
        else if (gizmoId == GizmoId.Rotate) _workGizmo = objectRotationGizmo;
        else if (gizmoId == GizmoId.Scale) _workGizmo = objectScaleGizmo;
        else if (gizmoId == GizmoId.Universal) _workGizmo = objectUniversalGizmo;

        _workGizmo.Gizmo.SetEnabled(true);
        _workGizmo.RefreshPositionAndRotation();


    }

    public void SetSliderValue(Slider slider, float value)
    {
        float sValue = slider.value;
        sValue += value;
        slider.SetValueWithoutNotify(sValue);


        if (slider.value <= slider.minValue || slider.value >= slider.maxValue)
        {

            StartCoroutine(ObjectMoveGizmo());

        }

    }

    public IEnumerator ObjectMoveGizmo()
    {
        objectMoveGizmo.Gizmo.SetEnabled(false);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.2f);
        objectMoveGizmo.Gizmo.SetEnabled(true);
        objectMoveGizmo.RefreshPositionAndRotation();


    }


    public void OnScaleValueChanged(float value)
    {
        SetGizmos(objectScaleGizmo);

        cubeObject.transform.SetWorldScale(new Vector3(value, value, value));


    }

    public void OnRotationValueChanged(float value)
    {
        SetGizmos(objectRotationGizmo);

        cubeObject.transform.rotation = Quaternion.Euler(0, value, 0);

    }
    public void OnXMoveValueChanged(float value)
    {
        SetGizmos(objectMoveGizmo);
        cubeObject.transform.position = new Vector3(value, cubeObject.transform.position.y, cubeObject.transform.position.z);
    }
    public void OnYMoveValueChanged(float value)
    {
        SetGizmos(objectMoveGizmo);
        cubeObject.transform.position = new Vector3(cubeObject.transform.position.x, value, cubeObject.transform.position.z);
    }
    public void OnZMoveValueChanged(float value)
    {
        SetGizmos(objectMoveGizmo);
        cubeObject.transform.position = new Vector3(cubeObject.transform.position.x, cubeObject.transform.position.y, value);
    }


    private void SetGizmos(ObjectTransformGizmo objectTransformGizmo)
    {
        activeGizmo = objectTransformGizmo;

        objectMoveGizmo.Gizmo.SetEnabled(false);
        objectRotationGizmo.Gizmo.SetEnabled(false);
        objectScaleGizmo.Gizmo.SetEnabled(false);
        objectUniversalGizmo.Gizmo.SetEnabled(false);

        activeGizmo.Gizmo.SetEnabled(true);
    }


    private void OnDisable()
    {
        objectMoveGizmo.Gizmo.PostDragUpdate -= Gizmo_PostDragUpdatePos;
        objectMoveGizmo.Gizmo.PostDragEnd -= Gizmo_PostDragEnd;

        objectRotationGizmo.Gizmo.PostDragUpdate -= Gizmo_PostDragUpdateRotate;
        objectScaleGizmo.Gizmo.PostDragUpdate -= Gizmo_PostDragUpdateScale;

    }

    public void OnPointerDown(BaseEventData eventData)
    {
        Debug.Log(eventData);

    }
}
