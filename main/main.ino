#include <Servo.h>

#define e 2.71828182846

#define maxNServo 12
#define number <= '9' >= '0' 
#define debugPin 2
int debug = 1;

String anim;

int index = 0;
Servo servos[maxNServo];
int servosPin[maxNServo];
int nServos = 0;

Servo servo;
char readByte;


unsigned long timee = 0, timing = 0, delta = 0, last= 0;

void setup()
{
    debug = 1;
    pinMode(21, OUTPUT);
    
    digitalWrite(21, 1);
    delay(500);
    digitalWrite(21, 0);
    Serial.begin(9600);
}

void loop()
{
    index = 0;
    timee = 0;
    timing = 0;
    playAnimation();
}

void playAnimation()
{
  switch (waitAndReadChar()) 
    {
        case 'n':
            break;
        case 's':
            selectActuator();
            break;
        case 't':
            timing = waitAndReadFloat() * 1000;
            //delay((int)timing - (int)timee);
            break;
        case 'r':
            return;
        default:
            // statements
            return;
    }
}

void playSavedAnimation()
{
  while (index < anim.length())
  {
    if (timee >= timing)
    {
      Serial.print("-----time: ");
  Serial.print(timee);
  Serial.print(" timing: ");
  Serial.println(timee);
  
        playAnimation();
    }

    delta = millis() - last;
    timee += delta;
    last = timee;
  }
}

char waitAndReadChar()
{
    if (debug == 1){
    while (Serial.available() <= 0)
    ;
    return Serial.read();
    }
    if (anim[index] == ' ')
    {
        index++;
    }
    index++;
    return anim[index-1];
}

float waitAndReadFloat()
{
  if (debug == 1)
  {
      while (Serial.available() <= 0)
      ;
      return Serial.parseFloat();
  }
    if (anim[index] == ' ')
            index++;
    String val = "";
    while (anim[index] != ' ')
    {
        val += anim[index];
        index++;
    }
    //index++;
    return val.toFloat();
}

int waitAndReadInt()
{
    if (debug == 1)
    {
      while (Serial.available() <= 0)
      ;
      return Serial.parseInt();
    }
    if (anim[index] == ' ')
        index++;
    String val = "";
    while (anim[index] != ' ')
    {
        val += anim[index];
        index++;
    }
    return val.toInt();
}

void selectActuator()
{
    switch (waitAndReadChar()) 
    {
        case 'a':
            changeAnalog(waitAndReadInt());
            break;
        case 'b':
            changeBuzzer(waitAndReadInt());
            break;
        case 's':
            changeServo();
            break;
        case 'd':
            changeDigital(waitAndReadInt());
            break;
        case 'l':
            changeRGB();
        default:
            // statements
            break;
    }
}

int searchServo(int pin)
{
    for (int i = 0; i < nServos; i++)
        if (servosPin[i] == pin)   
            return i;
    if (nServos == maxNServo)
        return -1;
    
    servosPin[nServos] = pin;
    servos[nServos].attach(pin);
    return nServos++;
}

void changeServo()
{
    int index = searchServo(waitAndReadInt());
    if (index < 0)
        return;
    servos[index].write(waitAndReadInt());
}

void changeAnalog(int num)
{
    pinMode(num, OUTPUT);
    analogWrite(num, waitAndReadInt());
}

void changeDigital(int num)
{
    pinMode(num, OUTPUT);
    int a = waitAndReadInt();
    digitalWrite(num, a);
}

void changeBuzzer(int num)
{
  int timing = waitAndReadInt();
  int note = waitAndReadInt();
  if (note == 0)
  {
    waitAndReadInt();
    tone(num, note, 1);
    return;
  }
    tone(num, note, timing);
}

void changeRGB()
{
  int r = waitAndReadInt();
  int g = waitAndReadInt();
  int b = waitAndReadInt();
  pinMode(r, OUTPUT);
  pinMode(g, OUTPUT);
  pinMode(b, OUTPUT);
  analogWrite(r, waitAndReadInt());
  analogWrite(g, waitAndReadInt());
  analogWrite(b, waitAndReadInt());
}
