void setup() {
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