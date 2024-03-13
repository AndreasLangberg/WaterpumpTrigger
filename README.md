This project simply connects windows to an arduino and triggers a pin to high at the press of a button

Hardware:
- An Arduino (Or other controller that runs on Arduino, such as https://www.aliexpress.com/item/1005001621886380.html?spm=a2g0o.order_list.order_list_main.138.409718025JST52)
- A Peristaltic Pump (https://www.aliexpress.com/item/1005005617915976.html?spm=a2g0o.order_list.order_list_main.73.409718025JST52)
- A PWM controller (https://www.aliexpress.com/item/4000002811175.html?spm=a2g0o.order_list.order_list_main.33.409718025JST52)
- A 36W 24V Power supply (For 240V/EU: https://www.aliexpress.com/item/1005005595508452.html?spm=a2g0o.order_list.order_list_main.63.409718025JST52)
- Enough foodsafe silicon tubing (ID 6mm x 9mm OD) to reach from your drinks bottle to your mouth (https://www.aliexpress.com/item/1005002185082384.html?spm=a2g0o.order_list.order_list_main.28.409718025JST52)
- Something to mount it all to (https://www.printables.com/model/804409-waterpump-holder-for-sim-racing)
- You'll also need a regular power cable for your country, and a USB-C cable for your Arduino.

Install Arduino IDE, run the file ArduinoWaterPumpTrigger.ino, and upload the code to your controller. The code does not need changing.
Install Visual Studio, run WaterPump.sln, and open Program.cs from within the solution.

Things to note:
- The program runs in the background, yet it still collects every keyboard press you make. And altough it tosses everything away immediately, this is in essence a keylogger. It could cause trouble with various anti virus softwares.
- The program looks for the key "Oem2". What this key actually is varies between keyboards. On mine it is \*. If you want to use any other key, you can find the list of Virtual keys here https://learn.microsoft.com/en-us/windows/iot/iot-enterprise/customize/keyboardfilter-key-names
- The program looks for the Arduino on "COM9". Change this to the actual comport it connects itself to, which will be semi-random. You can find it in Windows Device Manager
- You can run the application directly from Visual Studio, or to save a few frames while driving, right click on the solution, click "build", and find the respective .exe file in /bin/Debug/net8.0-windows/WaterPump.exe

Wire up PIN2 on your arduino board to the "HIGH/PWM" port on your PWN controller, and the "GND" back to an GND pin on the Arduino
Wire up the VIN+ and VIN- to their respectice + and - outputs on the power supply, and the OUT- and OUT+ to the Peristaltic Pump.

Finally, through SimHub or other prefered software, bind a button on your steering wheel to \* (Or what you rebound it to)

And you are ready to go! Enjoy your completely pointless, but infinitely cool, waterpump, for those warm F1 races in the middle east!
