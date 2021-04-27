using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;



public class ClientScript : MonoBehaviour {
    public String host = "80.182.0.246";
    public Int32 port = 6073;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;

    public string received_message = "";

    StreamWriter socket_writer;
    StreamReader socket_reader;
    bool canSend = true;
    private void Start() {

        setupSocket();
        //Thread t = new Thread(new ThreadStart(ReadFromServer));
        //t.Start();
    }

    void ReadFromServer() {
        while (true) {
            received_message = readSocket();
            if (received_message != null) return;
        }
    }



    public bool Send(string s) {
        //r//eturn true ;
        if (canSend) {
            print("Sending: " + s);
            if (s.Length == 0) {
                return true;
            }
            //writeSocket(s.Length);
            writeSocket(s);
            canSend = false;
            StartCoroutine(Timing());
            return true;
        }
        else return false;
    }


    IEnumerator Timing() {
        yield return new WaitForSeconds(60);
        canSend = true;
    }


    void Update() {
        var temp = readSocket();
        if (temp != "") {
            received_message = temp;
        }

        //switch (received_data) {
        //  case "pong":
        //    Debug.Log("Python controller sent: " + (string)received_data);


        if (received_message.Length > 0) {
            print(received_message);
        }

    }

    void OnApplicationQuit() {
        closeSocket();
    }

    // Helper methods for:
    //...setting up the communication
    public void setupSocket() {
        try {
            tcp_socket = new TcpClient(host, port);
            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);
            socket_ready = true;
        } catch (Exception e) {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    //... writing to a socket...
    public void writeSocket(string line) {
        if (!socket_ready)
            return;

        socket_writer.Write(line + "\n");
        socket_writer.Flush();
    }

    public void writeSocket(int line) {
        if (!socket_ready)
            return;
        //var x = BitConverter.GetBytes(line);

        socket_writer.Write(line);
        socket_writer.Flush();
    }

    //... reading from a socket...
    public String readSocket() {
        if (!socket_ready)
            return "";

        if (net_stream.DataAvailable) {
            return socket_reader.ReadLine();
        }

        return "";
    }

    //... closing a socket...
    public void closeSocket() {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
    }
}
