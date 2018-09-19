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