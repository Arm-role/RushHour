using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class myExtention
{
    public static T findNearObject<T>(T[] colliders, Vector3 myPosition, string Tag = null) where T : Component
    {
        T closest = null;
        float NearDistance = Mathf.Infinity;

        //Don't Ignor Collider
        foreach (T collider in colliders)
        {

            if (Tag == string.Empty)
            {
                Vector3 direction = collider.transform.position - myPosition;

                if (direction.magnitude < NearDistance)
                {
                    closest = collider;
                    NearDistance = direction.magnitude;
                }
            }
            else
            {
                if (collider.CompareTag(Tag))
                {
                    Vector3 direction = collider.transform.position - myPosition;

                    if (direction.magnitude < NearDistance)
                    {
                        closest = collider;
                        NearDistance = direction.magnitude;
                    }
                }
            }
        }
        return closest;
    }
    public static T findNearObject<T>(T[] colliders, Vector3 myPosition, T MyCollInColls, string Tag = null) where T : Component
    {
        T closest = null;
        float NearDistance = Mathf.Infinity;

        foreach (T collider in colliders)
        {
            //Nothig Tag
            if (Tag == string.Empty || Tag == null)
            {
                Vector3 direction = collider.transform.position - myPosition;

                if (direction.magnitude < NearDistance && collider != MyCollInColls)
                {
                    closest = collider;
                    NearDistance = direction.magnitude;
                }
            }
            //Have Tag
            else
            {
                if (collider.CompareTag(Tag))
                {
                    Vector3 direction = collider.transform.position - myPosition;

                    if (direction.magnitude < NearDistance && collider != MyCollInColls)
                    {
                        closest = collider;
                        NearDistance = direction.magnitude;
                    }
                }
            }

        }
        return closest;
    }
    public static T findObjectSortTag<T>(T[] colliders, Vector3 myPosition, string[] Tag, T MyCollInColls = null) where T : Component
    {
        //Don't Ignor Collider
        if (MyCollInColls)
        {
            foreach (string tag in Tag)
            {
                foreach (T collider in colliders)
                {
                    if (collider.CompareTag(tag) && MyCollInColls != collider)
                    {
                        return collider;
                    }
                }
            }
        }
        else
        {
            foreach (string tag in Tag)
            {
                foreach (T collider in colliders)
                {
                    if (collider.CompareTag(tag))
                    {
                        return collider;
                    }
                }
            }
        }
        return null;
    }
    public static List<T> CheckSame<T>(List<T> data)
    {
        if (data.Count > 0)
        {
            var dul = data.GroupBy(x => x).Select(g => g.Key);

            return dul.ToList();
        }
        return data;
    }
    public static float SmoothFloat(float floatInput, float floatOutput, float TimeLine)
    {
        if (floatInput < floatOutput)
        {
            if (floatInput <= floatOutput)
            {
                floatInput += TimeLine * Time.deltaTime;
                if (floatInput >= floatOutput) floatInput = floatOutput;
            }
            return floatInput;
        }
        else if (floatInput > floatOutput)
        {
            if (floatInput >= floatOutput)
            {
                floatInput -= TimeLine * Time.deltaTime;
                if (floatInput <= floatOutput) floatInput = floatOutput;
            }
            return floatInput;
        }
        return floatInput;
    }
    public static bool ContainsMethod(Delegate del, string methodName)
    {
        if (del != null)
        {
            foreach (var d in del.GetInvocationList())
            {
                if (d.Method.Name == methodName)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static Transform GetChildByName(string Name, Transform transform)
    {
        Transform child = null;

        foreach (Transform t in transform)
        {
            if (t.gameObject.name == Name)
            {
                return t;
            }
        }
        return child;
    }
    public static float GridPosition(float mousePosition, float Grid)
    {
        float position = mousePosition % Grid;
        mousePosition -= position;

        if (position > (Grid / 2))
        {
            mousePosition += Grid;
        }
        return mousePosition;
    }
}