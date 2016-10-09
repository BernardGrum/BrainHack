using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class SphereControls : MonoBehaviour {

    public float speed = 10.0f;
    public float jump_speed = 0.01f;
    private Rigidbody rb;
    //private TCPConnection myTCP;
    private bool jumping = false;

    void Awake()
    {
        //myTCP = gameObject.AddComponent<TCPConnection>();
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        //myTCP.setupSocket();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool moveUp = Input.GetButtonDown("Fire1");
        bool moveDown = Input.GetButtonUp("Fire1");

        //string serverSays = myTCP.readSocket();
        //if (serverSays.StartsWith("1"))
        //{
        //    moveUp = true;
        //}
        //print(serverSays);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //float jump = (moveUp ? rb.velocity.y + jump_speed : 0.0f) - (moveDown ? rb.velocity.y + jump_speed : 0.0f);
        if (moveUp) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jump_speed, rb.velocity.z);
        }
        else if (moveDown) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - jump_speed, rb.velocity.z);
        }

        rb.AddForce(movement * speed);
    }   
    
}


/*
public class TCPConnection : MonoBehaviour
{

    //the name of the connection, not required but better for overview if you have more than 1 connections running
    public string conName = "Localhost";

    //ip/address of the server, 127.0.0.1 is for your own computer
    public string conHost = "192.168.43.191";

    //port for the server, make sure to unblock this in your router firewall if you want to allow external connections
    public int conPort = 8080;

    //a true/false variable for connection status
    public bool socketReady = false;

    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;

    //try to initiate connection
    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient(conHost, conPort);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            print("Socket error:" + e);
        }
    }

    //send message to server
    public void writeSocket(string theLine)
    {
        if (!socketReady)
            return;
        String tmpString = theLine + "\r\n";
        theWriter.Write(tmpString);
        theWriter.Flush();
    }

    //read message from server
    public string readSocket()
    {
        String result = "";
        if (theStream.DataAvailable)
        {
            Byte[] inStream = new Byte[mySocket.SendBufferSize];
            theStream.Read(inStream, 0, inStream.Length);
            result += System.Text.Encoding.UTF8.GetString(inStream);
        }
        return result;
    }

    //disconnect from the socket
    public void closeSocket()
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }

    //keep connection alive, reconnect if connection lost
    public void maintainConnection()
    {
        if (!theStream.CanRead)
        {
            setupSocket();
        }
    }


}
*/
