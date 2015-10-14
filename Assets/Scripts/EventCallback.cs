using UnityEngine;
using System.Collections;
using FMOD.Studio;
using FMOD;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Collections.Generic;

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
    * Creates a MarkerProperties instance
    */
    public MarkerProperties createMarker()
    {
        if (type != EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            UnityEngine.Debug.LogError("Cannot create a marker object. Callback type is not a marker (" + type + ")");
            return null;
        }

        TIMELINE_MARKER_PROPERTIES marker = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
        IntPtr namePtr = marker.name;
        int nameLen = 0;
        while (Marshal.ReadByte(namePtr, nameLen) != 0) ++nameLen;
        byte[] buffer = new byte[nameLen];
        Marshal.Copy(namePtr, buffer, 0, buffer.Length);

        MarkerProperties markerProps = new MarkerProperties();
        markerProps.name = Encoding.UTF8.GetString(buffer, 0, nameLen);
        markerProps.position = marker.position;

        return markerProps;
    }
};

/*
* Simplified functions for fmod event callbacks
*/
public class EventCallback
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
            EventCallbackData data = new EventCallbackData(type, eventInstance, parameters);
            try
            {
                callback(data);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("This is not supposed to happen. It took a day to try and fix this. IF THIS HAPPENS THEN GOD HELP US ALL. Error: " + ex.Message);
            }
            return RESULT.OK;
        };
        evCallbacks.Add(evCallback);

        //set callback to lambda instance (whose lifetime SHOULD be the same as this class (forever))
        instance.setCallback(evCallback, callbackMask);
    }
}
