/*
using UnityEngine;
using System.Collections;
using System;
using SharpOSC;


public class Player : MonoBehaviour {

    public enum FieldType { REGULAR, APPLE, ORANGE };

    public int port;
    public string name;

    UDPListener listener;

    void Start() {
        mode = Mode.BLINK_TELEPATHY;
        // Callback function for received OSC messages. 
        // Prints EEG and Relative Alpha data only.
        HandleOscPacket callback = delegate(OscPacket packet)
        {
            var messageReceived = (OscMessage)packet;
            var addr = messageReceived.Address;
        }

        // Create an OSC server.
        listener = new UDPListener(port, callback);
    }

    void OnApplicationQuit() {
        listener.Close();
    }

}
*/