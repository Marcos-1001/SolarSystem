

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class Universe : MonoBehaviour {
    public static float G=  0.001f;
    public static float timeStep= 0.05f;

    public Planet[] planets;
    public XRRayInteractor rayInteractor;


    public Canvas infoCanvas;

    public void CreatePlanet(Vector3 position, Vector3 velocity, float mass, float radius, Color color){
        // Instantiate a new Planet GameObject
        //GameObject planetObject = new GameObject("Planet");
        GameObject planetObject = new GameObject("Planet");
        Planet planet = planetObject.AddComponent<Planet>();        

        planet.Initialize(position, velocity, radius, color);

        
        
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
        planet.isRemovable = true;
    }
    public void Update(){
        UpdatePlanets(planets);
        CheckRayPlanetCollision();
    }

    public void Remove_Planet(int idx){
        
        Destroy(planets[idx].gameObject);
        Planet[] newPlanets = new Planet[planets.Length - 1];
        for (int i = 0; i < idx; i++){
            newPlanets[i] = planets[i];
        }
        for (int i = idx + 1; i < planets.Length; i++){
            newPlanets[i - 1] = planets[i];
        }
        planets = newPlanets;
        
    }

    

    public void checkPlanets_collisions(Planet[] planets){
        for (int i = 0; i < planets.Length; i++){
            for (int j = i + 1; j < planets.Length; j++){
                if (Vector3.Distance(planets[i].transform.position, planets[j].transform.position) < planets[i].radius + planets[j].radius){
                    // Collision detected
                    // Combine the two planets
                    float newMass = planets[i].mass + planets[j ].mass;
                    Vector3 newPosition = (planets[i].transform.position * planets[i].mass + planets[j].transform.position * planets[j].mass) / newMass;
                    Vector3 newVelocity = (planets[i].velocity * planets[i].mass + planets[j].velocity * planets[j].mass) / newMass;
                    float newRadius = Mathf.Pow((planets[i].mass + planets[j].mass) * 3 / (4 * Mathf.PI * 5.5f), 1/3);

                    // remove the planet with less mass
                    int remove_idx = 0, not_remove_idx = 0;
                    if (planets[i].mass < planets[j].mass){
                        Destroy(planets[i].gameObject);
                        remove_idx = i;
                        not_remove_idx = j;
                    } else {
                        Destroy(planets[j].gameObject);
                        remove_idx = j;
                        not_remove_idx = i;
                    }


                    Remove_Planet(remove_idx);
                    // update the remaining planet
                    planets[not_remove_idx].mass = newMass;
                    planets[not_remove_idx].transform.position = newPosition;
                    planets[not_remove_idx].velocity = newVelocity;
                    planets[not_remove_idx].radius = newRadius;
                    planets[not_remove_idx].transform.localScale = Vector3.one * newRadius;                    

                    

                    // add vfx explosion is in Mizra Beig/Particle Systems/Inferno VFX/Prefabs/Loop/pf_vfx-inf_psys_demo_loop_ultranova2

                    //Instantiate(Resources.Load("pf_vfx-inf_psys_demo_loop_ultranova2"), newPosition, Quaternion.identity);
                   
                }
            }
        }
    }

    private void CheckRayPlanetCollision(){
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)){

            Planet planet  = hit.collider.gameObject.GetComponent<Planet>();


            if (planet == null ){                
                return;
            }
            TextMeshProUGUI infoText = infoCanvas.GetComponentInChildren<TextMeshProUGUI>();
            infoText.text = "Mass: " + planet.mass + "\n" + "Radius: " + planet.radius + "\n" + "Velocity: " + planet.velocity + "\n" + "Position: " + planet.transform.position;

            
                        

            if(isPressed()){// Remove the planet
                for (int i = 0; i < planets.Length; i++){
                    if (planets[i].gameObject == hit.collider.gameObject && planets[i].isRemovable){
                        Remove_Planet(i);
                        return;
                    }
                }                
            }

        }
    }

    private bool isPressed(){
        UnityEngine.XR.InputDevice rhand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool isPressed = false;
        if (rhand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out isPressed) && isPressed)
        {
            Debug.Log("Primary button pressed");
            return true;
        }
        return false;
                
    }

    public void UpdatePlanets(Planet[] planets){
        checkPlanets_collisions(planets);

        for (int i = 0; i < planets.Length; i++){
            planets[i].UpdateVelocity(planets, timeStep);
        }

        
        
        for (int i = 0; i < planets.Length; i++){
            planets[i].UpdatePosition(timeStep);
        }
    }    

    
}
