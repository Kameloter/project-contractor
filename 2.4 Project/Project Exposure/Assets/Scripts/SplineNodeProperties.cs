/**
 * STUFF DOWNLOADED FROM http://wiki.unity3d.com/index.php/Hermite_Spline_Controller
 * AUTHOR: F. Montorsi
 */

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// properties for a node in a spline camera path
/// </summary>
public class SplineNodeProperties : MonoBehaviour
{
    //time to wait at this noce
	public float BreakTime = 0f;
    //if it should skip to next node instead of interpolation
    public bool SkipToNext = false;

    public BaseActivatable activatable;

    [HideInInspector]
	public string Name = "";


}
