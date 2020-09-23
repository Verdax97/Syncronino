#include <Servo.h>

Servo servo;
int lastServo = -1;
char readByte;



void setup()
{
    Serial.begin(9600);
}

void loop()
{
    switch (waitAndReadChar()) 
    {
        case 'n':
            createNew();
            break;
        case 's':
            selectActuator();
            break;
        default:
            // statements
            break;
    }
}

char waitAndReadChar()
{
    while (Serial.available() <= 0)
    ;
    return Serial.read();
}

int waitAndReadInt()
{
    while (Serial.available() <= 0)
    ;
    return Serial.parseInt();
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

void createNew()
{
    switch (waitAndReadChar()) 
    {
        case 'a':
            createAnalog(waitAndReadInt());
            break;
        case 's':
            createServo(waitAndReadInt());
            break;
        case 'd':
            createDigital(waitAndReadInt());
            break;
        default:
            // statements
            break;
    }
}

void changeServo()
{
    servo.detach();
    servo.attach(waitAndReadInt());
    servo.write(waitAndReadInt());
}

void changeAnalog(int num)
{
    analogWrite(num, waitAndReadInt());
}

void changeDigital(int num)
{
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
}

void createServo(int num)
{
    //servos[num].attach(waitAndReadInt());
}

void createAnalog(int num)
{
    pinMode(num, OUTPUT);
}

void createDigital(int num)
{
    pinMode(num, OUTPUT);
}
