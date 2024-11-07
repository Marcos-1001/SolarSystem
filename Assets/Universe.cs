

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour {
    public static float G=  0.001f;
    public static float timeStep= 0.05f;

    public Planet[] planets;
    

    public void CreatePlanet(Vector3 position, Vector3 velocity, float mass, float radius){
        // Instantiate a new Planet GameObject
        GameObject planetObject = new GameObject("Planet");
        Planet planet = planetObject.AddComponent<Planet>();
        
        // Initialize the planet with position, velocity, and mass
        planet.Initialize(position, velocity, mass,radius);

        // Set up Rigidbody component for the planet

        
        // Set up sphere shape for the planet
        MeshFilter meshFilter = planetObject.AddComponent<MeshFilter>();
        meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx"); // Unity's built-in sphere mesh

        MeshRenderer meshRenderer = planetObject.AddComponent<MeshRenderer>();
        // URP
        meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"))
        {
            // set a random color 
            color = new Color(Random.value, Random.value, Random.value)
        };

        // Set the scale to match the planet's radius
        planetObject.transform.localScale = Vector3.one * radius;

        // Expand the planets array to include the new planet
        Planet[] newPlanets = new Planet[planets.Length + 1];
        for (int i = 0; i < planets.Length; i++) {
            newPlanets[i] = planets[i];
        }
        newPlanets[planets.Length] = planet;
        planets = newPlanets;

        velocity = Vector3.zero;
        for (int i = 0; i < planets.Length-1; i++){
            float sqrDst = (planets[i].transform.position - planet.transform.position).magnitude;
            
            planet.transform.LookAt(planets[i].transform);
            // circular orbit
            velocity += planet.transform.right * Mathf.Sqrt(G * planets[i].mass / sqrDst);
        }
        planet.velocity = velocity;
    }
    public void Update(){
        UpdatePlanets(planets);
    }

    public void UpdatePlanets(Planet[] planets){
        for (int i = 0; i < planets.Length; i++){
            planets[i].UpdateVelocity(planets, timeStep);
        }

        
        
        for (int i = 0; i < planets.Length; i++){
            planets[i].UpdatePosition(timeStep);
        }
    }    

    
}
