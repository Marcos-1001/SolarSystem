using UnityEngine;
using UnityEditor;

public class PlanetEditorWindow : EditorWindow {
    private Vector3 position;
    private Vector3 velocity;
    private float mass = 1.0f;
    private float radius = 1.0f;
    
    [MenuItem("Window/Planet Creator")]
    public static void ShowWindow() {
        GetWindow<PlanetEditorWindow>("Planet Creator");
    }

    void OnGUI() {
        GUILayout.Label("Add a New Planet", EditorStyles.boldLabel);

        // Input fields for planet parameters
        position = EditorGUILayout.Vector3Field("Position", position);
        velocity = EditorGUILayout.Vector3Field("Velocity", velocity);
        mass = EditorGUILayout.FloatField("Mass", mass);
        radius = EditorGUILayout.FloatField("Radius", radius);
        
        // Add planet button
        if (GUILayout.Button("Add Planet")) {
            AddPlanet();
        }
    }

    private void AddPlanet() {
        // Find an active Universe object in the scene
        Universe universe = FindObjectOfType<Universe>();
        if (universe != null) {
            universe.CreatePlanet(position, velocity, mass, radius, new Color(Random.value, Random.value, Random.value));
        } else {
            Debug.LogError("Universe object not found in the scene!");
        }
    }
}
