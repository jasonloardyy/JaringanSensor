char inData[24];
byte index;
boolean started = false;
boolean ended = false;

void setup()
{
Serial.begin(9600);
Serial.println("Temperature & Humidity");
Serial.println();
}

void loop()
{
  while(Serial.available() > 0)
  {
  char aChar = Serial.read();
  if(aChar == '<')
  {
      started = true;
      index = 0;
      inData[index] = '\0';
  }
  else if(aChar == '>')
  {
      ended = true;
  }
  else if(started)
  {
      inData[index] = aChar;
      index++;
      inData[index] = ',';
  }
  }

  if(started && ended)
  {
  // Use the value
  if(inData[0] == 'T')
  {
     inData[0] = ':';
     inData[1] = ' ';
     inData[7] = ' ';
     int windVal = atoi(inData);
           Serial.print("Temp ");
           Serial.print(inData);
           Serial.println("C");
  }
  else if(inData[0] == 'H')
     {
     inData[0] = ':';
     inData[1] = ' ';
     inData[7] = ' ';
           int temp = atoi(inData);
           Serial.print("Humidity ");
           Serial.print(inData);
           Serial.println("%");
     }

  started = false;
  ended = false;

  index = 0;
  inData[index] = '\0';
  }
}
