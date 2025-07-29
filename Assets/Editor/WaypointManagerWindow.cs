using UnityEngine;
using UnityEditor;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Window/Waypoint Editor Tools")]

    public static void ShowWindow()
    {
        GetWindow<WaypointManagerWindow>("Waypoint Editor Tools");
    }

    public Transform wayPointOrigin;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("wayPointOrigin"));

        if(wayPointOrigin == null)
        {
            EditorGUILayout.HelpBox("Please assign a Waypoint origin transform", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            CreateButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    void CreateButtons()
    {
        if(GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
    }

    void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + wayPointOrigin.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(wayPointOrigin, false);

        WayPoint waypoint = waypointObject.GetComponent<WayPoint>();

        if(wayPointOrigin.childCount > 1)
        {
            waypoint.previousWayPoint = wayPointOrigin.GetChild(wayPointOrigin.childCount - 2).GetComponent<WayPoint>();
            //for example we have 2 waypoint, 2-2 = 0. This indicates first index of waypoint
            waypoint.previousWayPoint.nextWayPoint = waypoint;
            //this will refer to the next first index which is index 1

            waypoint.transform.position = waypoint.previousWayPoint.transform.position;
            waypoint.transform.forward = waypoint.previousWayPoint.transform.forward;

        }

        Selection.activeGameObject = waypoint.gameObject; //automatically select the last created wayPoint
    }
}
