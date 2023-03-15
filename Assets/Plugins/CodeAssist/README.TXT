Code Assist improves your coding experience with Visual Studio. Coding for Unity projects becomes easier and more efficient with fewer bugs.
Connect your game editor and IDE, let Unity and Visual Studio share data between each other. Enrich your coding experience with the new data available.


Contact:
merryyellow@outlook.com

Website:
https://unitycodeassist.netlify.app/

Community:
https://discord.gg/2CgKHDq

Asset Store:
http://u3d.as/2N2H


Restrictions:
* Unity 2020.3 or newer.
* Visual Studio 2022 or newer. Does not work with Visual Studio Code, Visual Studio for Mac, Rider or any other IDE or text editor.


How to Setup:
* Import the asset under folder "Assets"
* Install UnityCodeAssistVSIX.vsix under folder "Assets/Plugins/CodeAssist" (You can skip this step if you have installed it for another project)
* Open your Unity project with Visual Studio. Code Assist loads in background with low priority, so it will be ready in a minute when Visual Studio first starts up.
* To check if setup is successfull, in Unity, choose "Tools"->"Code Assist"->"Status" from menu. The windows should display "Code Assist is working!".


How to Use:
Code Assist is always active while you are coding. The following samples will give you a glimpse of Code Assist.
* Code Completion: In a class which derives MonoBehaviour, type "CompareTag(". When you press double quotes("), code completion popup should appear with the available tags of the scene.
* Inline Visuals: Type a color in hex code with quotation marks, like "#FF8800". A box with the matching color should appear left of the color code.
* Code Analyzers: Define a Vector3 (let's say vec3). When used like "if (vec3.magnitude < 10)", a warning will appear on that line. Press Alt+Enter to convert it to "if (vec3.sqrMagnitude < 10 * 10)", which has better performance.


Troubleshooting:
* Make sure in Unity, "Tools"->"Code Assist"->"Status" is available from the menu bar. If not, reimport the asset. Also confirm package "Newtonsoft.Json" is installed in the project (Check it through "Window"->"Package Manager" from the menu bar).
* Make sure in Visual Studio, "View"->"Other Windows"->"Unity Code Assist" is available from the menu bar. If not, reinstall the UnityCodeAssistVSIX.vsix file.
* If Visual Studio has just been started, just wait; it usually takes around one minute to fully load Code Assist.
* If still not working, in Unity try "Tools"->"Code Assist"->"Synchronize" from the menu bar. This will attempt a new connection with Visual Studio.
* If none works above, try restarting both Unity and Visual Studio; sometimes an unclosed background thread blocks communication channels (network ports).


Error Reporting:
* In Unity, choose "Tools"->"Code Assist"->"Report Error" from the menu bar, to open feedback window.
* Or in Visual Studio, choose "View"->"Other Windows"->"Unity Code Assist" from the menu bar, then click "Report Error" button, to open feedback window.
* In the feedback window, you can view the most recent logs, or view full log of Unity or Visual Studio.
* You can report the error via email ( merryyellow@outlook.com ) or Discord ( https://discord.gg/2CgKHDq )
* Attaching full logs is recommended when reporting an error.


Thank you for using Code Assist. We hope this will help you in your crazy journey called game development :)