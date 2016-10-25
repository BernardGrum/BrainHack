# Team IcingOnCake - data gathering and correlation

All of the sensor devices come with their own software and data gathering mechanisms.
To really find correlation and possible causation we decided to level the data playing field
by collecting ALL of the data in a singular platform, while still preserving specific data of each platform.

This tears down the walled garden of each individual interface and allows for broader data analysis.

In our demo we collect openBCI data with a modified version of their GUI, and we have a script that controls VLC to playback specific videos while capturing data.
All of these event are also streamed via OSC (open sound control) and mangled into correct format for Maya to interpret and show actual brain activity in real time.

Other devices can be added easily, from temperature sensor, heart monitors (ecg) and specialised equiptment.

# Contact info
* Slavko Glamočanin (slavko@naprave.net)
* Jan Košir (jan.kosir@gmail.com)
* Andrej Zadnik (andrej_zadnik@hotmail.com)
* Vesna Šketa (vesna.sketa@hotmail.com)
