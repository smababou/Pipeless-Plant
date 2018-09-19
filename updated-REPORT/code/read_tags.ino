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