//AI_Traffic_Pack//
var F_L_Wheel : WheelCollider;
var F_R_Wheel : WheelCollider;
var B_L_Wheel : WheelCollider;
var B_R_Wheel : WheelCollider;
private var SteeringWheel : int = 12;
var BreakeLight : GameObject;
var waypoint : GameObject;
var EngineMoment  : float = 100.0;
private var EngineMomentBcz  : float = 100.0;
var Speed : float = 0;
var MaxSpeed : float = 100.0;
private var vehicleCenterOfMass : float = 0.0;
private var side_ray : float = 12.0;
private var breake_far : float = 10.0;
private var currentGear: int = 0;
private var EngineRPM : float = 0.0;
private var waypoints : Array;
private  var currentWaypoint : int = 0;
var BrakeForce = 800 ;
private var inputSteer : float = 0.0;
private var inputTorque : float = 0.0;
private var RayDistance = 1;
private var MaxEngineRPM : float = 3000.0;
private var MinEngineRPM : float = 1000.0;
var CarStatus = "Drive";

   
    

	
function Start () {
GetComponent.<Rigidbody>().centerOfMass = Vector3 (0, 2, 0);
BreakeLight.SetActive(false);  	
currentWaypoint ++;

}

function FixedUpdate () {
GetComponent.<Rigidbody>().centerOfMass.y = -1.5;
GetComponent.<Rigidbody>().drag = GetComponent.<Rigidbody>().velocity.magnitude / 250;
GetComponent.<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent.<Rigidbody>().velocity, MaxSpeed);
//rigidbody.AddForce(-transform.up * rigidbody.velocity.magnitude);
Speed = Mathf.Round (GetComponent.<Rigidbody>().velocity.magnitude );
CarStatus == "Drive";
Adv() ;

}

function Update()
{
breake_far = Speed /2 +15 ; 
side_ray = Speed /2 ;
//\\\\\\\\\\\\\\\\\\\\\\STATUS //////////////////////////////

if(CarStatus == "Stop")
    {
            BreakeLight.SetActive(true); 
            EngineMoment  = 0; 
            F_L_Wheel.brakeTorque = BrakeForce;
            F_R_Wheel.brakeTorque = BrakeForce;
}        
      
if(CarStatus == "Drive")
    {
           
            BreakeLight.SetActive(false); 
            F_L_Wheel.brakeTorque = 0;
            F_R_Wheel.brakeTorque = 0;
            EngineMoment  = EngineMomentBcz; 
            
}   



F_L_Wheel.motorTorque = EngineMoment ;
F_R_Wheel.motorTorque = EngineMoment ;
F_L_Wheel.steerAngle = 40 * inputSteer;
F_R_Wheel.steerAngle = 40 * inputSteer;

Navigate_to_waypoint();	

}



function Adv() 
{

        var fwd = transform.TransformDirection (Vector3.forward) * side_ray;
        var fwd2 = transform.TransformDirection (Vector3.forward) * breake_far;
        
        		
        Debug.DrawRay (transform.position, Quaternion.AngleAxis(15, transform.up) * fwd2 , Color.yellow);     
        if (Physics.Raycast (transform.position, Quaternion.AngleAxis(15, transform.up) * fwd2 , breake_far))
        {
			Debug.DrawRay (transform.position, Quaternion.AngleAxis(15, transform.up) * fwd2 , Color.red);
                        CarStatus = "Stop";
                        F_L_Wheel.brakeTorque = BrakeForce;
                        F_R_Wheel.brakeTorque = BrakeForce; 
                        
        } 
        
          
        Debug.DrawRay (transform.position, Quaternion.AngleAxis(-15, transform.up) * fwd2 , Color.yellow);     
        if (Physics.Raycast (transform.position, Quaternion.AngleAxis(-15, transform.up) * fwd2 , breake_far))
        {
			Debug.DrawRay (transform.position, Quaternion.AngleAxis(-15, transform.up) * fwd2 , Color.red);
                        CarStatus = "Stop";
                        F_L_Wheel.brakeTorque = BrakeForce;
                        F_R_Wheel.brakeTorque = BrakeForce; 
                        
        }
        
        
		Debug.DrawRay (transform.position, fwd2, Color.yellow);		
 		if (Physics.Raycast (transform.position, fwd2, breake_far)) {
			Debug.DrawRay (transform.position, fwd2, Color.red);
			
			CarStatus = "Stop";
	} 		
		 else{ 
		    
            CarStatus = "Drive";
            Navigate_to_waypoint();	
    }   
}

             
function GotWaypoints () {

    var potentialWaypoints : Array = waypoint.GetComponentsInChildren( Transform );

    waypoints = new Array();

    

    for ( var potentialWaypoint : Transform in potentialWaypoints ) {

        if ( potentialWaypoint != waypoint.transform ) {

            waypoints[ waypoints.length ] = potentialWaypoint;

        }
    }
}

function Navigate_to_waypoint () {
if(CarStatus == "Drive")
  {
var waypoints : Transform[];
var closest: GameObject;
var closestDist = Mathf.Infinity;
waypoints = waypoint.GetComponentsInChildren.<Transform>();
var RPWaypoint : Vector3 = transform.InverseTransformPoint( Vector3(waypoints[currentWaypoint].position.x,transform.position.y,waypoints[currentWaypoint].position.z ));

var dist = (transform.position - waypoint.transform.position).sqrMagnitude;
 
    if (dist < closestDist) {
        closestDist = dist;
        closest = waypoint;
}
    inputSteer = RPWaypoint.x / RPWaypoint.magnitude;
 
    if ( Mathf.Abs( inputSteer ) < 1.0 ) {
        inputTorque = RPWaypoint.z / RPWaypoint.magnitude - Mathf.Abs( inputSteer );
   }
    else
    {
        inputTorque = 0.0;
        
    }
    
    if ( RPWaypoint.magnitude < 10 ) {
    
            F_L_Wheel.brakeTorque = 100;
            F_R_Wheel.brakeTorque = 100;
            currentWaypoint ++;
            
            
        if ( currentWaypoint >= waypoints.length ) {
            currentWaypoint = 0;

      }
    }
  }                 
}                        
                            
                                
                                    
                                        
                                                
          
                        
    

 




   

