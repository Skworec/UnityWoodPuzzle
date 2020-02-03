using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private static Dictionary<GameObject, IRaycastable> raycastableObjects = new Dictionary<GameObject, IRaycastable>();

    private RaycastHit2D hitObjects;
    public static void AddToRaycasttable(Collider2D colliderToAdd)
    {
        IRaycastable raycastInterface = colliderToAdd.gameObject.GetComponent<IRaycastable>();
        if (!raycastableObjects.ContainsKey(colliderToAdd.gameObject))
        {
            raycastableObjects.Add(colliderToAdd.gameObject, raycastInterface);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hitObjects = Physics2D.Raycast(ray, Vector2.zero);
            if (hitObjects.transform != null)
            {
                if (raycastableObjects.ContainsKey(hitObjects.collider.gameObject))
                {
                    raycastableObjects[hitObjects.collider.gameObject].OnRaycastHit();
                }
            }

        }
    }
}

