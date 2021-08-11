#include <ArduinoJson.h>
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
    // Initialize serial port
    Serial.begin(19200);
}
void loop()
{
    while (Serial.available() <= 0)
        ;
    playAnimation();
}

void playAnimation()
{
    StaticJsonDocument<200> doc;
    String json = Serial.readStringUntil('\n');
    
    // Deserialize the JSON document
    DeserializationError error = deserializeJson(doc, json);

    // Test if parsing succeeds.
    if (error)
    {
        Serial.print(F("deserializeJson() failed: "));
        Serial.println(error.f_str());
        return;
    }
    
        Serial.println(doc["pins"][0].as<int>());
    
    switch (char(doc["type"].as<char>()))
    {
    case 'a':
        //modify analog actuator
        
        changeAnalog(doc["pins"][0], doc["values"][0]);
        break;
    case 'b':
        //modify buzzer
        changeBuzzer(doc["pins"][0], doc["values"][0], doc["duration"]);
        break;
    case 'd':
        //modify digital actuator
        changeDigital(doc["pins"][0], doc["values"][0]);
        break;
    case 'l':
        //modify rgb actuator
        changeRGB(doc["pins"].as<JsonArray>(), doc["values"].as<JsonArray>());
        break;
    case 's':
        //modify servo
        changeServo(doc["pins"][0], doc["values"][0]);
        break;
    default:
        //default
        Serial.println("default");
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

void changeServo(int pin, int value)
{
    //search the servo index in the array
    int index = searchServo(pin);
    if (index < 0)
        //do nothing cause is out of the array
        return;
    //modify servo value
    servos[index].write(value);
}

void changeAnalog(int pin, int value)
{
    pinMode(pin, OUTPUT);
    analogWrite(pin, value);
}

void changeDigital(int pin, int value)
{
    pinMode(pin, OUTPUT);
    digitalWrite(pin, value);
}

void changeBuzzer(int pin, int value, float duration)
{
    pinMode(pin, OUTPUT);
    tone(pin, value, duration);
    noTone(pin);
}

void changeRGB(JsonArray pins, JsonArray values)
{
    for(int i = 0; i < 3; i++)
    {
        pinMode(pins[i].as<int>(), OUTPUT);
        analogWrite(pins[i].as<int>(), values[i].as<int>());
    }
}
