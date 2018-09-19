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