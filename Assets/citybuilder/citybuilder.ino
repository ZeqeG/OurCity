const int buttonPin = 2;
const int randomizerPin = 3;
const int variablePin = A3;

int buttonState = 0;
int randomBtnState = 0;
int variableValue = 0;
int lastVariableValue = -1;

void setup() {
  pinMode(buttonPin, INPUT);
  pinMode(randomizerPin, INPUT);
  pinMode(variablePin, INPUT);
  Serial.begin(9600);
}

void loop() {
  buttonState = digitalRead(buttonPin);
  randomBtnState = digitalRead(randomizerPin);
  variableValue = analogRead(variablePin);

  // Send potentiometer value only if it changes significantly
  if (abs(variableValue - lastVariableValue) > 5) {
    Serial.print("VALUE:");
    Serial.println(variableValue);
    lastVariableValue = variableValue;
  }

  // Send button states
  if (buttonState == HIGH) {
    Serial.println("BUTTON:1");
    delay(20);
  }

  if (randomBtnState == HIGH) {
    Serial.println("BUTTON:2");
    delay(20);
  }

  delay(50);  // General delay to prevent flooding
}