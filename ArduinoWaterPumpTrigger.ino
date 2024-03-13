char incomingByte;   // for incoming serial data

void setup() {
        Serial.begin(9600);     // opens serial port, sets data rate to 9600 bps
        pinMode(2,OUTPUT);
        digitalWrite(2, LOW);
}

void loop() {

        // send data only when you receive data:
    if (Serial.available() > 0)
    {
        // read the incoming byte:
        incomingByte = Serial.read();

        // say what you got:
        Serial.print("I received: ");
        Serial.println(incomingByte);

        if (incomingByte == '1')
        {
            digitalWrite(2, HIGH);
        }
        else
        {
            digitalWrite(2, LOW);
        } 
    }
}