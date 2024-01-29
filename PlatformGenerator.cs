using UnityEditor;
using UnityEngine;


public class PlatformGenerator : EditorWindow
{
    // currently this works with a basic plane. plans to include disc and custom flat meshes will be the next step
    GameObject parentObject; // plane object to create bounds for
    GameObject prefabChild; // plane object to create bounds for
    int numberOfPrefabs; // number of child objects to spawn
    bool visibleBounds; // determines if bounds are visable
    bool includeChildren;
    int boundHeight; // How high does the user want the bounds

    [MenuItem("Tools/Plane Bound Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PlatformGenerator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate Input", EditorStyles.boldLabel);
        // we meed an input for a plane
        parentObject = EditorGUILayout.ObjectField("Plane to give bounds", parentObject, typeof(GameObject), true) as GameObject;
        // we need an input for prefabs
        includeChildren = EditorGUILayout.Toggle("Include Children", includeChildren);

        if(includeChildren)
        {
            prefabChild = EditorGUILayout.ObjectField("Child Objects", prefabChild, typeof(GameObject), true) as GameObject;
        }
       
        // we need an input for a number of prefabs we want to instantiate

        GUILayout.Label("Generate params", EditorStyles.boldLabel);
        // we need a check box for visible bounds
        visibleBounds = EditorGUILayout.Toggle("Visible Bounds", visibleBounds);
        // we need a button for generating
        boundHeight = EditorGUILayout.IntField("Bound Height", boundHeight);

        if(GUILayout.Button("Generate Bounds"))
        {
            CreateBounds();
        }
    }
     
    private void CreateBounds()
    {
        int buffer = 1;
        Renderer parentRenderer = parentObject.GetComponent<Renderer>();

        float planeSizeZ = parentObject.GetComponent<Renderer>().bounds.size.z;
        float edgeZ = parentObject.transform.position.z + planeSizeZ / 2;
        float edgeZNeg = parentObject.transform.position.z - planeSizeZ / 2;

        float planeSizeX = parentObject.GetComponent<Renderer>().bounds.size.x;
        float edgeX = parentObject.transform.position.z + planeSizeX / 2;
        float edgeXNeg = parentObject.transform.position.z - planeSizeX / 2;

        GameObject top = GameObject.CreatePrimitive(PrimitiveType.Cube);
        top.transform.parent = parentObject.transform;
        top.transform.position = new Vector3(top.transform.position.x, top.transform.position.y, edgeZ);
        Vector3 topScale = top.transform.localScale;
        topScale.x = planeSizeX+buffer;
        topScale.y = boundHeight;
        top.transform.localScale = topScale;
        Renderer topCubeRenderer = top.GetComponent<Renderer>();
        topCubeRenderer.sharedMaterial = parentRenderer.material;

        GameObject bottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottom.transform.parent = parentObject.transform;
        bottom.transform.position = new Vector3(bottom.transform.position.x, bottom.transform.position.y, edgeZNeg);
        Vector3 bottomScale = top.transform.localScale;
        bottomScale.x = planeSizeX+buffer;
        bottomScale.y = boundHeight;
        bottom.transform.localScale = bottomScale;
        Renderer bottomCubeRenderer = bottom.GetComponent<Renderer>();
        bottomCubeRenderer.sharedMaterial = parentRenderer.material;

        GameObject left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.parent = parentObject.transform;
        left.transform.position = new Vector3(edgeX, left.transform.position.y, left.transform.position.z);
        Vector3 leftScale = left.transform.localScale;
        leftScale.z = planeSizeZ+buffer;
        leftScale.y = boundHeight;
        left.transform.localScale = leftScale;
        Renderer leftCubeRenderer = left.GetComponent<Renderer>();
        leftCubeRenderer.sharedMaterial = parentRenderer.material;

        GameObject right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.parent = parentObject.transform;
        right.transform.position = new Vector3(edgeXNeg, right.transform.position.y, right.transform.position.z);
        Vector3 rightScale = right.transform.localScale;
        rightScale.z = planeSizeZ+buffer;
        rightScale.y = boundHeight;
        right.transform.localScale = rightScale;
        Renderer rightCubeRenderer = right.GetComponent<Renderer>();
        rightCubeRenderer.sharedMaterial = parentRenderer.material;
    }
}
