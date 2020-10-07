#include <Servo.h>

#define e 2.71828182846

#define maxNServo 12
#define number <= '9' >= '0' 
#define debugPin 2
int debug;

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
    debug = digitalRead(debugPin);
    pinMode(21, OUTPUT);
    digitalWrite(21, debug);
    Serial.begin(9600);
    anim = "t0 sa3 0 t0.03 sa3 8 t0.06 sa3 17 t0.1 sa3 25 t0.13 sa3 34 t0.16 sa3 42 t0.2 sa3 51 t0.23 sa3 59 t0.26 sa3 68 t0.3 sa3 76 t0.33 sa3 85 t0.36 sa3 93 t0.4 sa3 102 t0.43 sa3 110 t0.46 sa3 119 t0.5 sa3 127 t0.53 sa3 136 t0.56 sa3 144 t0.6 sa3 153 t0.63 sa3 161 t0.66 sa3 170 t0.7 sa3 178 t0.73 sa3 187 t0.76 sa3 195 t0.8 sa3 204 t0.83 sa3 212 t0.86 sa3 221 t0.9 sa3 229 t0.93 sa3 238 t0.96 sa3 246 t1 sa3 255 t1.03 sa3 246 t1.06 sa3 238 t1.09 sa3 229 t1.13 sa3 221 t1.16 sa3 212 t1.19 sa3 204 t1.23 sa3 195 t1.26 sa3 187 t1.29 sa3 178 t1.33 sa3 170 t1.36 sa3 161 t1.39 sa3 153 t1.43 sa3 144 t1.46 sa3 136 t1.49 sa3 127 t1.53 sa3 119 t1.56 sa3 110 t1.59 sa3 102 t1.63 sa3 93 t1.66 sa3 85 t1.69 sa3 76 t1.73 sa3 68 t1.76 sa3 59 t1.79 sa3 51 t1.83 sa3 42 t1.86 sa3 34 t1.89 sa3 25 t1.93 sa3 17 t1.96 sa3 8 t1.99 sa3 0 t2 sa3 0 r";
    
    
}

void loop()
{
    index = 0;
    timee = 0;
    timing = 0;
    if (debug == 1)
    {
      playAnimation();
    }
    else
    {
      playSavedAnimation();
    }
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
