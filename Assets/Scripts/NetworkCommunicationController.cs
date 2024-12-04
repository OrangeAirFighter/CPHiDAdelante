using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.IO;
using UnityEngine.Networking;

public class NetworkCommunicationController : MonoBehaviour
{

    public class State
    {
        public byte[] buffer = new byte[bufSize];
    }

    public class HandPositionAndOrientation
    {
        public Vector3 handPosition;
        public Quaternion handOrientation;
    }

    public Vector3 handpos;
    public Vector3 speedVector;
    double[] msgUI = new double[30];

    public int boardNr = 0; 
    public float F1 = 0;
    public float F2 = 0;
    public float F3 = 0;
    public float F4 = 0;
    public float Fweight = 0;
    public float Ftot = 0;
    public float BatteryStatus = 0;

    public int boardNr_B = 0;
    public float F1_B = 0;
    public float F2_B = 0;
    public float F3_B = 0;
    public float F4_B = 0;
    public float Fweight_B = 0;
    public float BatteryStatus_B = 0;

    //private Socket _socket_client; //Sends the Force information to DEXTER
    //private Socket _socket_client_UI;

    private const int bufSize = 8 * 1024;

    private Socket _socket_server; //Listens to the position information delivered by Balance Board!!
    private State state = new State();
    private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback recv = null;

    //Second Balance Board UDP
    private Socket _socket_server_B;
    private State state_B = new State();
    private EndPoint epFrom_B = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback recv_B = null;

    


    // Start is called before the first frame update
    void Start()
    {

        _socket_server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _socket_server_B = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
        
        state = new State();
        epFrom = new IPEndPoint(IPAddress.Any, 0);
        recv = null;

        state_B = new State();
        epFrom_B = new IPEndPoint(IPAddress.Any, 0);
        recv_B = null;

        
        Server("127.0.0.1", 11111); // Balance Board A (left)
        Server_B("127.0.0.1", 22222); // Balance Board B (right)


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        closeConnection();
    }

    public void Server(string address, int port)
    {
        _socket_server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _socket_server.Bind(new IPEndPoint(IPAddress.Parse(address), port));
        Receive();
    }
    
    public void Server_B(string address, int port)
    {
        _socket_server_B.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _socket_server_B.Bind(new IPEndPoint(IPAddress.Parse(address), port));
        ;
        Receive_B();
    }
    
    public void closeConnection()
    {

        _socket_server.Close();
        _socket_server_B.Close();
        

    }

    private void Receive()
    {
        _socket_server.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
        {
            State so2 = (State)ar.AsyncState;
            int bytes = _socket_server.EndReceiveFrom(ar, ref epFrom);
            _socket_server.BeginReceiveFrom(so2.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so2);
            parseMessage(Encoding.ASCII.GetString(so2.buffer, 0, bytes));
        }, state);
    }

    
    private void Receive_B()
    {
        _socket_server_B.BeginReceiveFrom(state_B.buffer, 0, bufSize, SocketFlags.None, ref epFrom_B, recv_B = (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = _socket_server_B.EndReceiveFrom(ar, ref epFrom_B);
            _socket_server_B.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom_B, recv_B, so);
            parseMessage_B(Encoding.ASCII.GetString(so.buffer, 0, bytes));
        }, state_B);
    }

    
    void parseMessage(string receivedMsg)
    {
        //print("Wii A receive: " + receivedMsg);
        //Debug.Log(receivedMsg);

        string[] msgParts = receivedMsg.Split(';');
        boardNr = int.Parse(msgParts[0]);
        F1 = float.Parse(msgParts[1]);
        F2 = float.Parse(msgParts[2]);
        F3 = float.Parse(msgParts[3]);
        F4 = float.Parse(msgParts[4]);
        Fweight = float.Parse(msgParts[5]);
        BatteryStatus = float.Parse(msgParts[6]);

        //Ftot = F1 + F2 + F3 + F4;
        //Debug.Log("board: " + boardNr + " - F1: " + F1 + " - F2: " + F2 + " - F3: " + F3 + " - F4: " + F4 + " - Ftot: " + Ftot);


    }

    void parseMessage_B(string receivedMsg)
    {
        //print("Wii B receive: " + receivedMsg);
        //Debug.Log(receivedMsg);
        string[] msgParts = receivedMsg.Split(';');
        boardNr_B = int.Parse(msgParts[0]);
        F1_B = float.Parse(msgParts[1]);
        F2_B = float.Parse(msgParts[2]);
        F3_B = float.Parse(msgParts[3]);
        F4_B = float.Parse(msgParts[4]);
        Fweight_B = float.Parse(msgParts[5]);
        BatteryStatus_B = float.Parse(msgParts[6]);
        
        //Ftot = F1 + F2 + F3 + F4;
        //Debug.Log("board: " + boardNr + " - F1: " + F1 + " - F2: " + F2 + " - F3: " + F3 + " - F4: " + F4 + " - Ftot: " + Ftot);


    }

    /*
    public Vector3 getHandTracking_Position()
    {

        if (handpos != null)
        {
            return handpos;
        }
        return new Vector3(0.0f, 0.0f, 0.0f);
    }
    */

    public double[,] GetForceData()
    {
        double[,] result = new double[2, 7];

        result[0, 0] = boardNr;
        result[0, 1] = F1;
        result[0, 2] = F2;
        result[0, 3] = F3;
        result[0, 4] = F4;
        result[0, 5] = Fweight;
        result[0, 6] = BatteryStatus;

        result[1, 0] = boardNr_B;
        result[1, 1] = F1_B;
        result[1, 2] = F2_B;
        result[1, 3] = F3_B;
        result[1, 4] = F4_B;
        result[1, 5] = Fweight_B;
        result[1, 6] = BatteryStatus_B;


        return result;

    }
   
    /*
    public Vector3 getSpeedUpdates()
    {
        if (speedVector != null)
        {
            return speedVector;
        }
        return new Vector3(0.0f, 0.0f, 0.0f);
    }
    */








}

