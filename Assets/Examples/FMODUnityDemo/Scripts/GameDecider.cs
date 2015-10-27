using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Runtime.InteropServices;

public class GameDecider : MonoBehaviour
{
    //below is temporary code

    static float t1 = 0, t2 = 0;        // time
    static float v1 = 0, v2 = 0;        // velocity

    private RESULT pcmreadcallback(System.IntPtr sound_raw, System.IntPtr data, uint data_len)
    {
        int ofs = 0;
        uint count;
        float volume = 1;
        for (count = 0; count < (data_len >> 2); count++)
        {
            float f2 = .0572f * Mathf.Cos(4.667f * t1) + .0218f * Mathf.Cos(12.22f * t1);

            Marshal.WriteInt16(data, ofs, (short)(f2 * 32767.0f * volume));
            ofs += 2;
            Marshal.WriteInt16(data, ofs, (short)(f2 * 32767.0f * volume));
            ofs += 2;

            t1 += .1f;
            volume -= 1.0f / (data_len >> 2);
        }

        return RESULT.OK;
    }

    void Start() {
        MODE mode = MODE.OPENUSER | MODE.LOOP_NORMAL;

        CREATESOUNDEXINFO exinfo = new CREATESOUNDEXINFO();
        exinfo.cbsize = Marshal.SizeOf(exinfo);
        exinfo.numchannels = 2;
        exinfo.defaultfrequency = 44000;
        exinfo.decodebuffersize = 44000;
        exinfo.length = (uint)(exinfo.defaultfrequency * exinfo.numchannels * 2 * 4);
        exinfo.format = SOUND_FORMAT.PCM16;
        exinfo.pcmreadcallback = pcmreadcallback;

        Sound sound;
        RuntimeManager.LowlevelSystem.createSound("", mode, ref exinfo, out sound);

        //Channel channel;
        //RuntimeManager.LowlevelSystem.playSound(sound, null, false, out channel);
    }

    void Update() {

	}
}
