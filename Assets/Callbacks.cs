using UnityEngine;
using System.Collections;
using FMOD.Studio;
using FMOD;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Collections.Generic;

public class Callbacks
{
    public class EventCallbackWrapper
    {
        public Action<string> callback;
        public EVENT_CALLBACK delcall;

        public EventCallbackWrapper(Action<string> _callback)
        {
            callback = _callback;
            delcall = call;
        }

        public RESULT call(EVENT_CALLBACK_TYPE type, IntPtr eventInstance, IntPtr parameters)
        {
            try
            {
                callback(getMarkerName(parameters));
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Wow, this is not supposed to happen. It took a day to try and fix this. IF THIS HAPPENS THEN GOD HELP US ALL. Error: " + ex.Message);
            }

            return RESULT.OK;
        }
    }

    //used to make sure the callback wrappers aren't GC'd
    private static List<EventCallbackWrapper> callbackWrappers = new List<EventCallbackWrapper>();

    public static void setMarkerCallback(EventInstance instance, Action<string> callback)
    {
        //create a new wrapper and add it to the list (o it doesn't get GC'd)
        EventCallbackWrapper wrapper = new EventCallbackWrapper(callback);
        callbackWrappers.Add(wrapper);
        //set callback to delegate call of wrapper
        instance.setCallback(wrapper.delcall, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }

    public static string getMarkerName(IntPtr parameters)
    {
        //reads the marker name from inputted parameters
        TIMELINE_MARKER_PROPERTIES marker = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
        IntPtr namePtr = marker.name;
        int nameLen = 0;
        while (Marshal.ReadByte(namePtr, nameLen) != 0)
        {
            ++nameLen;
        }
        byte[] buffer = new byte[nameLen];
        Marshal.Copy(namePtr, buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, nameLen);
    }
}
