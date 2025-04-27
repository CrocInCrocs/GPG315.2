using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class CameraControllerWindow : EditorWindow
{
    [MenuItem("Tools/Camera Controller")]

    static void ShowWindow()
    {
        GetWindow<CameraControllerWindow>("Camera Controller");
    }

   
    public Camera cameraMain;
    public GameObject objectToAttachTo;
    public Vector3 selectedObjPosition;
    public bool cameraHasObj;
    public Vector2 scrollBarPosition;
    
    //camera zoom variables
    public float cameraZoom = 60f;
    // had variables for min and max values that just did not work 
    
    public CameraShake cameraShakeScript;
    public float cameraShakeIntensity = 1f;
    public float cameraShakeDuration = 1f;
   

    public void OnEnable()
    {
        
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollBarPosition = EditorGUILayout.BeginScrollView(scrollBarPosition, true, false);
        
        GUIStyle titleStyle = GUIStyle.none;
        titleStyle.normal.textColor = Color.white;
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        
        cameraMain = (Camera)EditorGUILayout.ObjectField("Camera",cameraMain,typeof(Camera), true);

        objectToAttachTo = (GameObject)EditorGUILayout.ObjectField("Object To Follow", objectToAttachTo, typeof(GameObject), true);

        cameraShakeScript = (CameraShake)EditorGUILayout.ObjectField("Camera Shake Script", cameraShakeScript, typeof(CameraShake), true);
        
        cameraZoom = EditorGUILayout.Slider("Camera Zoom", cameraZoom, 1f, 100f);
        if (cameraMain.orthographic)
        {
            cameraMain.orthographicSize = cameraZoom;
        }
        else
        {
            cameraMain.fieldOfView = cameraZoom;
        }
        
        
        if (GUILayout.Button("Clear selection"))
        {
            objectToAttachTo = null;
        }
        
        EditorGUILayout.Space(10);
        GUILayout.Label("Camera Positions", titleStyle);
        EditorGUILayout.Space(10);

        if (GUILayout.Button("Reset Camera"))
        {
            ResetCameraPosition();
        }

        if (GUILayout.Button("Top Down View"))
        {
            TopDownView();
        }

        if (GUILayout.Button("Side Scroll View"))
        {
            SideScroll();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Camera Perspectives", titleStyle);
        GUILayout.Space(10);

        if (GUILayout.Button("Set Camera to Orthographic"))
        {
            SetCameraToOrthographic();
        }

        if (GUILayout.Button("Set Camera to Perspective"))
        {
            SetCameraToPerspective();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Camera Shake", titleStyle);
        GUILayout.Space(10);

        cameraShakeIntensity = EditorGUILayout.Slider("Camera Shake Intensity", cameraShakeIntensity, 0.0f, 1.0f);
        cameraShakeDuration = EditorGUILayout.Slider("Camera Shake Duration", cameraShakeDuration, 0.0f, 1.0f);
        if (GUILayout.Button("Camera Shake Test"))
        {
            cameraShakeScript.CameraShakeTest(cameraShakeIntensity, cameraShakeIntensity);
        }
        
        EditorGUILayout.Space(10);
       
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
    
    public void Update()
    {
        if (objectToAttachTo != null)
        {
            if (!cameraHasObj)
            {
                cameraMain.transform.SetParent(objectToAttachTo.transform);
                cameraHasObj = true;
                
                cameraMain.transform.position = objectToAttachTo.transform.position;
                cameraMain.transform.localPosition = new Vector3(0f,0f,-5f);
            }
        }
        else if (objectToAttachTo == null)
        {
            if (cameraHasObj)
            {
                cameraMain.transform.SetParent(null);
                cameraHasObj = false;
            }
           
        }
        
    }

    public void TopDownView()
    {
        selectedObjPosition = objectToAttachTo.transform.position;
        cameraMain.transform.rotation = Quaternion.Euler(90f,0f,0f);
        cameraMain.transform.localPosition = new Vector3(selectedObjPosition.x, selectedObjPosition.y = 6, selectedObjPosition.z);
        
    }

    public void SideScroll()
    {
        selectedObjPosition = objectToAttachTo.transform.position;
        cameraMain.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        cameraMain.transform.localPosition = new Vector3(selectedObjPosition.x, selectedObjPosition.y, selectedObjPosition.z = -10f);
    }

    public void ResetCameraPosition()
    {
        selectedObjPosition = objectToAttachTo.transform.position;
        cameraMain.transform.rotation = objectToAttachTo.transform.rotation;
        cameraMain.transform.position = objectToAttachTo.transform.position;
    }

    public void SetCameraToOrthographic()
    {
        cameraMain.orthographic = true;
    }

    public void SetCameraToPerspective()
    {
        cameraMain.orthographic = false;
    }
    
   
}
