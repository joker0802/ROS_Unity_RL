
var Red : GameObject;
var Green : GameObject;
var Yellow : GameObject;
private var GreenTime : float = 15; 
private var RedTime : float = 10; 
var Pos1 : boolean = true;  
var Pos2 : boolean = false; 
var Pos3 : boolean = false; 
var Pos4 : boolean = false; 
 

function Start()

{
 if (Pos1)

 {
 Pos_1() ;

 }
 if (Pos2)

 {
 Pos_2() ;

 } 
  if (Pos3)

 {
 Pos_3() ;

 }
 if (Pos4)

 {
 Pos_4() ;

 }
} 


function Pos_1()

{


  Red.SetActive(false); 
  
  Yellow.SetActive(false);  

  Green.SetActive(true);   

    yield WaitForSeconds(GreenTime);

    Green.SetActive(false);   
       yield WaitForSeconds(1);
    Green.SetActive(true);   
       yield WaitForSeconds(1); 
    Green.SetActive(false);   
       yield WaitForSeconds(1);   
    Green.SetActive(true);   
       yield WaitForSeconds(1);
    Yellow.SetActive(true);   
       yield WaitForSeconds(1) ;
  Red.SetActive(true);   
 
  Yellow.SetActive(true);   

  Green.SetActive(false); 

  Red.SetActive(true);   

  Yellow.SetActive(false);   

  Green.SetActive(false);  
  yield WaitForSeconds (30); 
    
Start() ;

}
function Pos_2()

{
  Red.SetActive(true);   

  Yellow.SetActive(false);  

  Green.SetActive(false); 

  yield WaitForSeconds (7.5f);

  Red.SetActive(true);   
 
  Yellow.SetActive(true);   

  Green.SetActive(false); 

  Red.SetActive(false);   

  Yellow.SetActive(false);   

  Green.SetActive(true);   
  
  yield WaitForSeconds (GreenTime);
 
    Green.SetActive(false);   
       yield WaitForSeconds(1);
    Green.SetActive(true);   
       yield WaitForSeconds(1); 
    Green.SetActive(false);   
       yield WaitForSeconds(1);   
    Green.SetActive(true);   
       yield WaitForSeconds(1);
    Yellow.SetActive(true);   
       yield WaitForSeconds(1);
  Red.SetActive(true);   

  Yellow.SetActive(false);  

  Green.SetActive(false);      

  yield WaitForSeconds (22.5);      
Start() ;

}

function Pos_3()
{
  Red.SetActive(true);   

  Yellow.SetActive(false);  

  Green.SetActive(false); 

  yield WaitForSeconds (15);

  Red.SetActive(true);   
 
  Yellow.SetActive(true);   

  Green.SetActive(false); 

  Red.SetActive(false);   

  Yellow.SetActive(false);   

  Green.SetActive(true);   
  
  yield WaitForSeconds (GreenTime);

    Green.SetActive(false);   
       yield WaitForSeconds(1);
    Green.SetActive(true);   
       yield WaitForSeconds(1); 
    Green.SetActive(false);   
       yield WaitForSeconds(1);   
    Green.SetActive(true);   
       yield WaitForSeconds(1);
    Yellow.SetActive(true);   
       yield WaitForSeconds(1);
  Red.SetActive(true);   

  Yellow.SetActive(false);  

  Green.SetActive(false);      

  yield WaitForSeconds (15);      
Start() ;

}
function Pos_4()
{
  Red.SetActive(true);   

  Yellow.SetActive(false);  

  Green.SetActive(false); 

  yield WaitForSeconds (22.5f);

  Red.SetActive(true);   
 
  Yellow.SetActive(true);   

  Green.SetActive(false); 

  Red.SetActive(false);   

  Yellow.SetActive(false);   

  Green.SetActive(true);   
  
  yield WaitForSeconds (GreenTime);

    Green.SetActive(false);   
       yield WaitForSeconds(1);
    Green.SetActive(true);   
       yield WaitForSeconds(1); 
    Green.SetActive(false);   
       yield WaitForSeconds(1);   
    Green.SetActive(true);   
       yield WaitForSeconds(1);
    Yellow.SetActive(true);   
       yield WaitForSeconds(1);
  Red.SetActive(true);   

  Yellow.SetActive(false);  

  Green.SetActive(false);      

  yield WaitForSeconds (7.5);      
Start() ;

}