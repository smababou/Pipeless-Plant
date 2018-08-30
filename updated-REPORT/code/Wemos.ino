#include <SoftwareSerial.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <string.h>
#include <ArduinoJson.h>


//how many clients should be able to telnet to this ESP8266
#define MAX_SRV_CLIENTS 2

// Definig Wifi address, password, host and port
//const char* ssid     = "UPC113C854 A.Abouelkhair";   // write SSID between "( here )"
//const char* password = "rnU3cu6dzpkA";               // write Password between "(case sensitive)"
//const char* ssid     = "iPhone";                   // write SSID between "( here )"
//const char* password = "boody123";                 // write Password between "(case sensitive)"
const char* ssid  = "iRobot";                      // write SSID between "( here )"
const char* password = "nopipes123";               // write Password between "(case sensitive)"
//const char* ssid  = "DORTMUND-DEMO-AP";                      // write SSID between "( here )"
//const char* password = "R0b0tn1K";               // write Password between "(case sensitive)"

// Starting Wifi Server and client with Port 8883
WiFiServer server(8883);
WiFiClient serverClients[MAX_SRV_CLIENTS];

// Variables Declaration
unsigned long previousMillis = 0;
const long interval = 10;
unsigned long currentTime;

/* RFID Intialization */
SoftwareSerial RFID(14, 12, false, 256);    //RX,TX = D5,D6 (Wemos UART1)




void setup() {
  // put your setup code here, to run once:

  /* Beginning Serial Communication with RFID with baud rate 115200 */
  RFID.begin(115200);
//  delay(10);
  // Beginning Serial Communication with baud rate 115200
  Serial.begin(115200);
//  delay(10);
  Serial.println();


  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  // Time is used so the device does not stuck in
  // connecting to Wifi forever
  currentTime = millis();
  unsigned long previousTime = currentTime;
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
    currentTime = millis();
    if ((currentTime - previousTime) > 12000) {
      break;
    }
  }
  
  
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  uint8_t i = 0;
  while (WiFi.status() != WL_CONNECTED && i++ < 20) delay(500);
  if (i == 21) {
    Serial.print("Could not connect to"); Serial.println(ssid);
    while (1) delay(500);
  }
  server.begin();
  server.setNoDelay(true);

 
  //   Getting the MAC address and saving it
  if (WiFi.status() == WL_CONNECTED) {
    Serial.println("");
    Serial.println("WiFi connected");
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());
    Serial.println("Port: 8883");
  }

  /* AUTO Send AT Command to the RFID */
  RFID.write("AT+Scan=OFF\r");
//  RFID.write("AT+Scan=AC,RSSI\r");
  RFID.write("AT+Scan=AC,RSSI\r");

//    delay(10);


}


void loop() {
  // put your main code here, to run repeatedly

//  while(RFID.available())
//  {
//     char read= RFID.read();
//     Serial.print(read);
//     delay(10);
//    } 

  /*Send commands Through Wifi*/
  sendCommandsWiFi();

  /* send AT Commands to RFID through serial monitor */
  //Example: AT+Scan=AC,RSSI            "Without /r"
  // Comment the next command if you are using AUTO command send
  sendCommandsRFID();


}



void sendCommandsWiFi()
{
 
  unsigned long currentMillis = millis();
  uint8_t i;
  //check if there are any new clients
  if (server.hasClient()) {
    for (i = 0; i < MAX_SRV_CLIENTS; i++) {
      //find free/disconnected spot
      if (!serverClients[i] || !serverClients[i].connected()) {
        if (serverClients[i]) serverClients[i].stop();
        serverClients[i] = server.available();
        continue;
      }
    }
    //no free/disconnected spot so reject
    WiFiClient serverClient = server.available();
    serverClient.stop();
  }

  //do every 2ms edit from time interval in Declaration
//  if (currentMillis - previousMillis >= interval)    // 
//  {  //
//    previousMillis = currentMillis;  //
    for (i = 0; i < MAX_SRV_CLIENTS; i++) 
    {
      if (serverClients[i] && serverClients[i].connected()) 
      {
//        delay(20);
//        RFID.write("AT+A\r");
        while(RFID.available()>0)
        {
          char read = RFID.read();
          serverClients[i].print(read);
//          delay(1);   //
          }
      }
    }
//  }  //

}


void sendCommandsRFID()
{
    /* send AT Commands to RFID through serial monitor */
    //Example: AT+Scan=AC,RSSI            "Without /r"
  while (Serial.available() > 0) {
    RFID.write(Serial.read());
  }

  }

