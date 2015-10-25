# DashboardVR
The goal of this project is to visualize data from Magento in a virtual reality environment

##Requirements
- Magento
- Unity3d

##Installation

### Magento
- Provide real or mock orders
- run shell/generate-journey.php

### Unity
Requirements:

- Built for Samsung GearVR
- Unity 5.2.1

Important setting: 

Android > Player settings > Other settings > Virtual Reality Supported

Output on GearVR: enabled 
Run in Unity Editor: disabled

Adjust Data url: 
VR/UnityProject/Assets/Scripts/HostInfo.cs

Currently it uses https://raw.githubusercontent.com/magento-hackathon/DashboardVR/master/VR/unity_data.json