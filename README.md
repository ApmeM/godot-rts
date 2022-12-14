# godot-template

## Overview

A template repository to create godot project with the following features:
- Automatic deploy to github pages and google play store on each commit to main branch
- Generate C# code for scenes as a partial class (see https://github.com/ApmeM/GodotFieldsGenerator)
- Incude C# implementations for achievements and popups 
- CommonSignals class contains names for godot signals

## Install

1. Clone repository from template

Replace all occurances of "godotRts" to your name

2. Create keystores for android deployment

Go to "keystore" folder and execute following commands:

```
keytool -genkey -v -keystore debug.keystore -alias debg_user -storepass debug_password -keyalg RSA -keysize 2048 -validity 10000
keytool -genkey -v -keystore release.keystore -alias release_user -keyalg RSA -keysize 2048 -validity 10000
```

And then add release keystore password to github secrets with the name RELEASE_KEYSTORE_PASSWORD

3. Create new project in google play 

To add permissions for the github to automatically deploy new versions to play store please check https://github.com/ApmeM/ApmeM/blob/main/github_actions_autodeploy_android_apk.md

Service account json key generated by google console should be copied to github secret with the name SERVICE_ACCOUNT_JSON

4. Enjoy

