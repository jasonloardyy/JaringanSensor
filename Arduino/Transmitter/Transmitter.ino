#include "DHT.h"
#define DHTPIN 2     
#define DHTTYPE DHT11   
DHT dht(DHTPIN, DHTTYPE);

void setup()
{
Serial.begin(9600);
}

void loop()
{

float h = dht.readHumidity();
float t = dht.readTemperature();

Serial.print("<T:");
Serial.print(t); 
Serial.print(">");

Serial.print("<H:");
Serial.print(h); 
Serial.print(">");

delay(5000);
}
