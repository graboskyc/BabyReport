# BabyReport

# Pieces
* [Backend code is stored in MongoDB Atlas using MongoDB Stitch as a REST API endpoint](StitchApp/README.md)
* [Front end headless buttons using GPIO pins on a Pi to call Stitch REST APOIs from UWP app on Windows 10 IOT Core](PiUWP/README.md)
* [Simple buttons in a Xamarin app for Android to call same REST APIs from Stitch](AndroidXamarin/README.md)
* MongoDB Charts to report on data visually

# Configuration
GPIO pinout can be found [here](https://pinout.xyz/)

The C# projects each have a `AppConstants.cs` file which is omitted here due to security for the strings. The format should be:

```
using System.Collections.Generic;

    namespace BabyReport
{
    public static class AppConstants
    {
        public static string newEventEndpoint = ""; // trailing slash
        public static string eventEndpointSecret = "";
        public static string chartsEmbed = "";
        public static int refreshSecDelay = 90;

        // pin to push 3.3v gpio pin out
        public static int pinAlwaysOn = 5;
        // pee, poo, milk, formula gpio input pins
        public static int[] pinList = new int[] { 6, 13, 19, 26 };
    }
}
```
