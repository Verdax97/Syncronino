#include <ArduinoJson.h>

void setup()
{
    // Initialize serial port
    Serial.begin(19200);
    while (!Serial)
        continue;
    // Allocate the JSON document
    //
    // Inside the brackets, 200 is the capacity of the memory pool in bytes.
    // Don't forget to change this value to match your JSON document.
    // Use arduinojson.org/v6/assistant to compute the capacity.
    //StaticJsonDocument<400> doc;

    // StaticJsonDocument<N> allocates memory on the stack, it can be
    // replaced by DynamicJsonDocument which allocates in the heap.
    //
    // DynamicJsonDocument doc(200);

    // JSON input string.
    //
    // Using a char[], as shown here, enables the "zero-copy" mode. This mode uses
    // the minimal amount of memory because the JsonDocument stores pointers to
    // the input buffer.
    // If you use another type of input, ArduinoJson must copy the strings from
    // the input to the JsonDocument, so you need to increase the capacity of the
    // JsonDocument.
}

void loop()
{
  StaticJsonDocument<400> doc;
    String json;
    while (Serial.available() <= 0)
            ;
    json = Serial.readStringUntil('\n');
    
    // Deserialize the JSON document
    DeserializationError error = deserializeJson(doc, json);

    // Test if parsing succeeds.
    if (error)
    {
        Serial.print(F("deserializeJson() failed: "));
        Serial.println(error.f_str());
        digitalWrite(12, 1);
        return;
        
    }
    
    digitalWrite(12, 0);
    // Fetch values.
    //
    // Most of the time, you can rely on the implicit casts.
    // In other case, you can do doc["time"].as<long>();
    //const char *sensor = doc["keyframes"][0];
    float timing = doc["timing"];
    int val = doc["values"][0];
    digitalWrite(13, val);
    // Print values.
    Serial.println(timing);
    Serial.println(val);
}
