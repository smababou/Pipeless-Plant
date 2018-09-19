void sendCommandsRFID()
{
    /* send AT Commands to RFID through serial monitor */
    //Example: AT+Scan=AC,RSSI            "Without /r"
  while (Serial.available() > 0) {
    RFID.write(Serial.read());
  }

  }

