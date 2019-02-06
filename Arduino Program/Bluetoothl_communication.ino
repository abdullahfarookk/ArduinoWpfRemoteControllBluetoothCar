#include<SoftwareSerial.h>
const int trigPin = 11;
const int echoPin = 10;
SoftwareSerial mySerial(12,13);
int Wheel1Fwd=3;
int Wheel1Bwd=4;
int Wheel2Fwd=8;
int Wheel2Bwd=9;
/*int enable1Pin = 8;
int enable2Pin = 9;*/


void MoveFwd(){

  digitalWrite(Wheel1Fwd,LOW);
    digitalWrite(Wheel1Bwd,HIGH);
      digitalWrite(Wheel2Fwd,LOW);
        digitalWrite(Wheel2Bwd,HIGH);
}
void MoveBwd(){
  
  digitalWrite(Wheel1Fwd,HIGH);
    digitalWrite(Wheel1Bwd,LOW);
      digitalWrite(Wheel2Fwd,HIGH);
        digitalWrite(Wheel2Bwd,LOW);
}
void MoveLeft(){
  digitalWrite(Wheel1Fwd,HIGH);
    digitalWrite(Wheel1Bwd,LOW);
      digitalWrite(Wheel2Fwd,LOW);
        digitalWrite(Wheel2Bwd,LOW);
}

void MoveRight(){
  digitalWrite(Wheel1Fwd,LOW);
    digitalWrite(Wheel1Bwd,LOW);
      digitalWrite(Wheel2Fwd,HIGH);
        digitalWrite(Wheel2Bwd,LOW);
}
void Stop(){
  
  digitalWrite(Wheel1Fwd,LOW);
    digitalWrite(Wheel1Bwd,LOW);
      digitalWrite(Wheel2Fwd,LOW);
        digitalWrite(Wheel2Bwd,LOW);  
       
}


void setup() {
   pinMode(7, OUTPUT);
  Serial.begin(9600);
   pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
pinMode(Wheel1Fwd,OUTPUT);
pinMode(Wheel1Bwd,OUTPUT);
pinMode(Wheel2Fwd,OUTPUT);
pinMode(Wheel2Bwd,OUTPUT);
   // sets enable1Pin and enable2Pin high so that motor can turn on:
//    digitalWrite(enable1Pin, HIGH);
   // digitalWrite(enable2Pin, HIGH);
  mySerial.begin(128000);
}
long duration, distance;
int t1,t2;
void loop(){
   //Serial.println("Forward is Pressed");
while(mySerial.available()){
  t1=millis();
  char bt=mySerial.read();
  Serial.println(bt);
  mySerial.println("Hello World");
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);  
  duration = pulseIn(echoPin, HIGH);
  distance = duration/58.2;
 // mySerial.write(distance);
  //mySerial.println(distance);
  if(bt=='F')
  {
     Serial.println("Forward is Pressed");
    if(distance>30){
    MoveFwd();
     
    }
    else{
    Stop();
   digitalWrite(7, HIGH);   
  delay(500);              
  digitalWrite(7, LOW);   
  delay(500);  
  
    
      }
    
  }
  if(bt=='B')
  {
    MoveBwd();
    }
  if(bt=='R')
  {
    MoveRight();
     
     }
  if(bt=='L')
  {
    MoveLeft();
   
    
  }

  }
 t2=millis();
 if((t2-t1)>150)
  {
    Stop();
    }
  
}

