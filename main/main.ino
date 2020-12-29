#include <Servo.h>
//define the pin used for debug
#define debugPin 2
int debug = 1;
//saved animation
String anim;
//index for the saved animation
int index = 0;
//define lenght of the servo array
#define maxNServo 12
//the servo array 
Servo servos[maxNServo];
//the array with the pin value of the servos
int servosPin[maxNServo];
//number of servos actually used
int nServos = 0;
//variables for controlling the animation flow
unsigned long elapsedTime = 0, nextTiming = 0, delta = 0, last = 0;

void setup()
{
    //init serial connection
    Serial.begin(19200);
}

void loop()
{
    //reset variables
    index = 0;
    elapsedTime = 0;
    nextTiming = 0;
    //play animation
    playAnimation();
}

void playAnimation()
{
    switch (waitAndReadChar())
    {
    case 'n':
        //for initializing new 
        break;
    case 's':
        //select one of the actuators
        selectActuator();
        break;
    case 't':
        //modify the timing
        nextTiming = waitAndReadFloat() * 1000;
        break;
    case 'r':
        //end of animation
        return;
    default:
        //default
        return;
    }
}

void playSavedAnimation()
{
    while (index < anim.length())
    {
        //check if 
        if (elapsedTime >= nextTiming)
        {
            Serial.print("-----time: ");
            Serial.print(elapsedTime);
            Serial.print(" nextTiming: ");
            Serial.println(elapsedTime);
            playAnimation();
        }

        delta = millis() - last;
        elapsedTime += delta;
        last = elapsedTime;
    }
}

char waitAndReadChar()
{
    if (debug == 1)
    {
        //wait for serial message
        while (Serial.available() <= 0)
            ;
        //read the byte
        return Serial.read();
    }

    if (anim[index] == ' ')
        index++;
    index++;
    return anim[index - 1];
}

float waitAndReadFloat()
{
    if (debug == 1)
    {
        //wait for serial message
        while (Serial.available() <= 0)
            ;
        //read the float
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
        //wait for serial message
        while (Serial.available() <= 0)
            ;
        //read the int
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
        //modify analog actuator
        changeAnalog(waitAndReadInt());
        break;
    case 'b':
        //modify buzzer
        changeBuzzer(waitAndReadInt());
        break;
    case 'd':
        //modify digital actuator
        changeDigital(waitAndReadInt());
        break;
    case 'l':
        //modify rgb actuator
        changeRGB();
        break;
    case 's':
        //modify servo
        changeServo();
        break;
    default:
        //default
        break;
    }
}

int searchServo(int pin)
{
    //search for the servo in the array
    for (int i = 0; i < nServos; i++)
        if (servosPin[i] == pin)
            //servo founded
            return i;
    //servo not found so 
    if (nServos == maxNServo)
        return -1;
    //servo not found so add it to the array
    servosPin[nServos] = pin;
    servos[nServos].attach(pin);
    return nServos++;
}

void changeServo()
{
    //search the servo index in the array
    int index = searchServo(waitAndReadInt());
    if (index < 0)
        //do nothing cause is out of the array
        return;
    //modify servo value
    servos[index].write(waitAndReadInt());
}

void changeAnalog(int num)
{
    //set pinMode
    pinMode(num, OUTPUT);
    //write to analog
    analogWrite(num, waitAndReadInt());
}

void changeDigital(int num)
{
    //set pinMode
    pinMode(num, OUTPUT);
    //write to digital
    digitalWrite(num, waitAndReadInt());
}

void changeBuzzer(int num)
{
    int timing = waitAndReadInt();
    int note = waitAndReadInt();
    if (note == 0)
    {
        //almost like doing nothing to not crash if the frequence is set to 0
        waitAndReadInt();
        tone(num, note, 1);
        return;
    }
    tone(num, note, timing);
}

void changeRGB()
{
    //get pins value
    int r = waitAndReadInt();
    int g = waitAndReadInt();
    int b = waitAndReadInt();
    //set pinsMode
    pinMode(r, OUTPUT);
    pinMode(g, OUTPUT);
    pinMode(b, OUTPUT);
    //analog write the values to the pins
    analogWrite(r, waitAndReadInt());
    analogWrite(g, waitAndReadInt());
    analogWrite(b, waitAndReadInt());
}
