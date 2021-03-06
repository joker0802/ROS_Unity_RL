
//AI_Traffic_Pack//
var colorRoad: Color = Color(0,1,0, 0.5);


																														
function OnDrawGizmos () {
	var wayroads = gameObject.GetComponentsInChildren( Transform );

    
	for ( var wayroad : Transform in wayroads ) {

	    Gizmos.color = colorRoad;
		Gizmos.DrawWireCube (wayroad.position, Vector3 (5,5,5));
		//Handles.color = colorRoad;
        //Handles.Label(wayroad.position, wayroad.gameObject.name);
	}
	
}
