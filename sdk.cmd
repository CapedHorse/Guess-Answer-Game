set UNITY_VERSION=2021.3.0f1
 
c:
set JAVA_HOME=D:\Program Files\Unity Editors\2021.3.0f1\Editor\Data\PlaybackEngines\AndroidPlayer\OpenJDK\
set ANDROID_HOME=D:\Program Files\Unity Editors\2021.3.0f1\Editor\Data\PlaybackEngines\AndroidPlayer
cd %ANDROID_HOME%SDK\tools\bin\
echo.> %USERPROFILE%\.android\repositories.cfg
 
cmd /C sdkmanager --update
cmd /C sdkmanager "platform-tools" "platforms;android-29"
cmd /C sdkmanager "platform-tools" "platforms;android-30"
cmd /C sdkmanager "platform-tools" "platforms;android-31"