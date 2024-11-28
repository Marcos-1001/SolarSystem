

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
    public float length = 50;

    public Canvas infoCanvas;
    private bool delayB = false;


    


    private Planet temporaryPlanet;

    public Transform rightHandAnchor; 
    public LineRenderer lineRenderer;

    public GameObject explosionPrefab;
    public void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        // Configure LineRenderer properties
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;

        // Set position count for a simple ray (2 points: start and end)
        lineRenderer.positionCount = 2;

        explosionPrefab = Resources.Load<GameObject>("Explosion");
        Debug.Log(explosionPrefab.name);
    }
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
        Debug.Log(position);
    }
    public void Update(){

        Vector3 startPoint = rightHandAnchor.position;
        Vector3 endPoint = startPoint + rightHandAnchor.forward * length;

        
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        UpdatePlanets(planets);
        CheckRayPlanetCollision();

        if (OVRInput.GetDown(OVRInput.Button.Two) && !delayB){
            delayB = true;

            // render a temporary planet that follows the controller
            Vector3 rayOrigin = rightHandAnchor.position;
            Vector3 rayDirection = rightHandAnchor.forward;
            Vector3 endpoint = rayOrigin + rayDirection * length;


            Vector3 velocity = Vector3.zero;
            float mass = 1;
            float radius = 20f;
            Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));                    
            temporaryPlanet = new GameObject("Planet").AddComponent<Planet>();
            temporaryPlanet.Initialize(endpoint, velocity, radius, color);
            
                

        } else if (OVRInput.GetDown(OVRInput.Button.Two) && delayB){
            delayB = false;
            // create the planet
            Vector3 rayOrigin = rightHandAnchor.position;
            Vector3 rayDirection = rightHandAnchor.forward;
            Vector3 endpoint = rayOrigin + rayDirection * length;

            Vector3 velocity = Vector3.zero;
            float mass = 1;                        
            Destroy(temporaryPlanet.gameObject);
            CreatePlanet(endpoint, velocity, mass, temporaryPlanet.radius, temporaryPlanet.color);
        }

        if (temporaryPlanet != null){
            temporaryPlanet.transform.position = rightHandAnchor.position + rightHandAnchor.transform.forward * length;
        }

        AdjustRayLength();

    }

    public void Remove_Planet(int idx){
        Instantiate(explosionPrefab, planets[idx].transform.position, Quaternion.identity);
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
                if (Vector3.Distance(planets[i].transform.position, planets[j].transform.position) <= planets[i].radius + planets[j].radius){
                    // Collision detected
                    // Combine the two planets
                    float newMass = planets[i].mass + planets[j ].mass;
                    Vector3 newPosition = (planets[i].transform.position * planets[i].mass + planets[j].transform.position * planets[j].mass) / newMass;
                    Vector3 newVelocity = (planets[i].velocity * planets[i].mass + planets[j].velocity * planets[j].mass) / newMass;
                    float newRadius = Mathf.Pow(newMass * 3 / (4 * Mathf.PI * 5.5f), 1f/3f);

                    

                    // remove the planet with less mass
                    int remove_idx = 0, not_remove_idx = 0;
                    if (planets[i].mass <= planets[j].mass){
                        remove_idx = i;
                        not_remove_idx = j;
                    } else {
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

                    // Set the initial properties
        
                    planets[not_remove_idx].Rigidbody.mass = newMass;      
                    planets[not_remove_idx].Rigidbody.velocity = newVelocity;
                    planets[not_remove_idx].Rigidbody.position = newPosition;        

                    // add a tag

                    //gameObject.tag = "Planet";

                    // Set up SphereCollider
                    
                    planets[not_remove_idx].sphereCollider.radius = 1;
                    planets[not_remove_idx].sphereCollider.isTrigger = true;
                    // Initialize sphere collider position 
                    planets[not_remove_idx].sphereCollider.center = Vector3.zero;
                    

                    // add vfx explosion is in Mizra Beig/Particle Systems/Inferno VFX/Prefabs/Loop/pf_vfx-inf_psys_demo_loop_ultranova2

                    //Instantiate(Resources.Load("pf_vfx-inf_psys_demo_loop_ultranova2"), newPosition, Quaternion.identity);
                   
                }
            }
        }
    }

    private void CheckRayPlanetCollision(){
        Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);

        for(int i  =0; i< planets.Length; i++)
        {
            planets[i].selectionHighlight_deactivate(); 
        }
        // unable the canvas image

        

        if (Physics.Raycast(ray, out RaycastHit hit, 10000)){

            Planet planet  = hit.collider.gameObject.GetComponent<Planet>();

            
            if (planet == null ){                
                return;
            }

            planet.selectionHighlight_activate();



            TextMeshProUGUI infoText = infoCanvas.GetComponentInChildren<TextMeshProUGUI>();
            infoText.text = "Mass: " + planet.mass + "\n" + "Radius: " + planet.radius + "\n" + "Velocity: " + planet.velocity + "\n" + "Position: " + planet.transform.position + "\n" + "Name: " + planet.name;
            Image image = infoCanvas.GetComponentInChildren<Image>();
            image.enabled = true;
            
                        

            if(OVRInput.GetDown(OVRInput.Button.One)){// Remove the planet
                for (int i = 0; i < planets.Length; i++){
                    if (planets[i].gameObject == hit.collider.gameObject && planets[i].isRemovable){
                        Remove_Planet(i);
                        return;
                    }
                }                
            }

        }  else{
            TextMeshProUGUI infoText = infoCanvas.GetComponentInChildren<TextMeshProUGUI>();
            infoText.text = "";
            Image image = infoCanvas.GetComponentInChildren<Image>();
            image.enabled = false;
        }
    }

      private void AdjustRayLength()
    {
        float joystickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        length += joystickY * 10f * Time.deltaTime;
        length = Mathf.Clamp(length, 20, 200);
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
