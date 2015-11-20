using UnityEngine;
using System.Collections;
using FMOD.Studio;
using FMOD;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Collections.Generic;

/*
* This script simplifies a bit of the process of event callbacks.
* The following is an example on how this class can be used:
*
* EventCallback.setCallback(my_callback_function);
*
* public void my_callback_function(EventCallbackData data) {
*   //all callback types can be handled here
*   //beware: depending on the type, the callback may not run on Unity's main thread.
*   //which means you are unable to modify the GUI on that thread.
*   
*   if (data.type == EVENT_CALLBACK_TYPE.MARKER) {          //a marker has been reached in the event
*       string markerName = data.createMarker();
*   }else if (data.type == EVENT_CALLBACK_TYPE.STARTED) {   //event has started
*       //code
*   }
* }
*
* Note: This is not part of the FMOD API or Unity integration, it is just a helper class for this demo.
*/

public class MarkerProperties
{
    public string name;
    public int position;
};

public class EventCallbackData
{
    public EVENT_CALLBACK_TYPE type;
    public IntPtr instance;
    public IntPtr parameters;

    public EventCallbackData(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameters)
    {
        this.type = type;
        this.instance = instance;
        this.parameters = parameters;
    }

    /*
    * Creates a MarkerProperties instance if the callback is a marker
    */
    public MarkerProperties createMarker()
    {
        if (type != EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            UnityEngine.Debug.LogError("Cannot create a marker object. Callback type is not a marker (" + type + ")");
            return null;
        }

        // Loads the marker timeline properties from FMOD
        TIMELINE_MARKER_PROPERTIES marker = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
        IntPtr namePtr = marker.name;
        int nameLen = 0;
        while (Marshal.ReadByte(namePtr, nameLen) != 0) ++nameLen;
        byte[] buffer = new byte[nameLen];
        Marshal.Copy(namePtr, buffer, 0, buffer.Length);

        // Create a custom MarkerProperties object and store the marker name and position in them
        MarkerProperties markerProps = new MarkerProperties();
        markerProps.name = Encoding.UTF8.GetString(buffer, 0, nameLen);
        markerProps.position = marker.position;

        return markerProps;
    }
};

/*
* Simplified functions for fmod event callbacks
*/
public class EventCallbackHelper
{
    //List used to make sure event callbacks are not GC'd
    private static List<EVENT_CALLBACK> evCallbacks = new List<EVENT_CALLBACK>();

    /*
    * Used to set a callback on an event instance that returns a EventCallbackData instance.
    * EventCallbackData can be used to simplify the process  of getting data from the callbacks.
    */
    public static void setCallback(
        EventInstance instance, 
        Action<EventCallbackData> callback, 
        EVENT_CALLBACK_TYPE callbackMask = EVENT_CALLBACK_TYPE.ALL)
    {
        EVENT_CALLBACK evCallback = (EVENT_CALLBACK_TYPE type, IntPtr eventInstance, IntPtr parameters) =>
        {
            try
            {
                EventCallbackData data = new EventCallbackData(type, eventInstance, parameters);
                callback(data);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Error occurred during event callback: " + ex.Message);
            }
            return RESULT.OK;
        };
        evCallbacks.Add(evCallback);

        //set callback to lambda instance (whose lifetime SHOULD be the same as this class (forever))
        instance.setCallback(evCallback, callbackMask);
    }
}
