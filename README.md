## Ev3 emulator  

#### The main goal of the project is to create an emulator that would be very similar to the original Ev3 brick  

![image](https://github.com/user-attachments/assets/f8aaddaa-cc58-4faf-a702-9ca7a4124e3f)


### How to use?

If you have Visual Studio with .NET tools:
-  Download the repository;
-  Select the externals that you want to be imported [here](https://github.com/CrackAndDie/Ev3Emulator/blob/main/Ev3LowLevelLib/Ev3LowLevelLib.csproj#L13);
-  Rebuild and run *Ev3Emulator* project.

If you do not have any tools to compile the app:
- The app has no releases yet. Sorry :) It will be released in [SoftHub](https://softv.su/resources/Apps/SoftHub/installers/win/softhub_x64.exe).

### Clarifications  

- For now, the EV3 firmware library is only tested on Windows machines, but when I was rewriting it, I assumed it would be cross-platform;
- It can be compiled and tested only targeting x86 because of some problems with pointer size. I've tried to deal with it but understood that there is too much work;
- Output/input fuction implementations are still under development;
- There are still some small bugs like I can't rename the brick;
- Communication with the brick is also not done yet. I couldn't find any resources about emulating USB or Bluetooth. So probably only WiFi would be supported.
