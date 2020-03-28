using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseManager : MonoBehaviour
{
    public Collider selectedCollider = null;

    public Vector3 desiredDirection;
    public const int minDirectionMagnitude = 3;         //min specified direction for mouse when setting facing. Prevents "ghost clicks". Anything less than this will get thrown out.

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public bool isDragging = false;

    public GameObject cam;

    //mouse button stand-ins
    int left = 0;
    int right = 1;
    int middle = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //mouse buttons
        if (Input.GetMouseButtonUp(left) && isDragging == false)        //we only want to select a unit if we haven't been dragging the camera around
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit))
            {
                //unselect objects if necessary
                if (selectedCollider != null && selectedCollider != rayHit.collider)
                {
                    MouseDeselect(selectedCollider);
                }

                //select new object
                Collider hitCollider = rayHit.collider;
                selectedCollider = hitCollider;
                MouseSelect(hitCollider);
            }
            else
            {
                //we are assuming we are trying to deselect unit
                MouseDeselect(selectedCollider);
                selectedCollider = null;
            }
        }

        //move units
        if (Input.GetMouseButtonDown(right))            
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(right))                //is user dragging mouse w/ right button selected? if so this will align the unit if selected
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                desiredDirection = dragCurrentPosition - dragStartPosition;
            }
        }

        if (Input.GetMouseButtonUp(right))          //right mouse button up
        {
            if(selectedCollider != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit))
                {
                    if (desiredDirection.magnitude >= minDirectionMagnitude)
                    {
                        Move(selectedCollider, desiredDirection, dragStartPosition);
                    }
                    else
                    {
                        Move(selectedCollider, Vector3.zero, dragStartPosition);
                    }
                }
            }
        }
    }

    public event Action<Collider> OnMouseSelect;
    public event Action<Collider> OnMouseDeselect;
    public event Action<Collider, Vector3, Vector3> OnMove;

    void MouseSelect(Collider collider)
    {
        if (OnMouseSelect != null)
        {
            OnMouseSelect(collider);
        }
    }
    void MouseDeselect(Collider collider)
    {
        if (OnMouseDeselect != null)
        {
            OnMouseDeselect(collider);
        }
    }

    void Move(Collider collider, Vector3 direction, Vector3 destination)
    {
        if (OnMove != null)
        {
            OnMove(collider, direction, destination);
        }
    }
}
