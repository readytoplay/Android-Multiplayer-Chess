﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Program
    {

        // Data types to represent what state a board block could be in
        // chessmanColour repreents the colour of the piece, or in case of no piece; empty
        public enum chessmanColour { empty, white, black }

        // chesman represents the type of pieces, or if none; empty
        public enum chessman { empty, King, Queen, Rook, Bishop, Knight, Pawn }

        // What is in a square
        public struct gameSquare
        {
            public chessmanColour colour; // colour of the piece on the square 
            public chessman piece; // content of the game piece
        }

        // Basic structure of all the gamedata for each game
        public struct gameObject
        {
            public gameSquare[,] boardGame;
            public List<String> chatRoom;
            public string playerOne;
            public string playerTwo;
        }

        // List of all games that are going on
        public static List<gameObject> gamesInPlay;

        // List of usernames to make sure new connections have unique usernames
        public static List<String> userNames;
        
        // delegate init.
        public delegate void Del(TcpClient client);

        // Genearte a standard board
        public static gameSquare[,] generateDefaultBoard()
        {
            return
                new gameSquare[8, 8]
                { 
                    // ROW 1
                    {
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Rook },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Knight },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Bishop },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.King },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Queen },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Bishop },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Knight },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Rook }
                    },
                    // ROW 2
                    {
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn }
                    },
                    // ROW 3
                    {
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
                    },
                    //ROW 4
                    {
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
                    },
                    // ROW 5
                    {
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
                    },
                    // ROW 6
                    {
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
                        new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
                    },
                    // ROW 7
                    {
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn }
                    },
                    // ROW 8
                    {
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Rook },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Knight },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Bishop },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Queen },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.King },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Bishop },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Knight },
                        new gameSquare() { colour = chessmanColour.white, piece = chessman.Rook }
                    }
                };
        }
        
        public static void handleNewConnection(TcpClient client)
        {
            try
            {
                // get streams
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());

                Console.Write("New Delegate created to handle new connection.\n");

                gameObject myGame = new gameObject();

                // Setup where the command data will be stored
                string[] command;

                // Read in from the socket here
                string recieve;

                // While logged in, recieve and process data
                bool loggedIn = true; 

                // Recieve input
                do {
                    // Read in from socket
                    recieve = reader.ReadLine();

                    command = recieve.Split(' ');

                    // Switch based on command recieved
                    switch (command[0])
                    {
                        // Login to the server
                        case "LOGIN":
                            
                            break;
                        // Start New Game
                        case "NEW":
                            myGame.boardGame = generateDefaultBoard();
                            break;
                        // Move a chess piece
                        case "MOVE":

                            break;
                        // Get new moves that have been made
                        case "GETMOVE":

                            break;
                        // logout from the server
                        case "LOGOUT":
                            loggedIn = false;
                            break;
                        // Get new messages that are in the chatroom
                        case "GET":

                            break;
                        // default, any other string sent will be posted to the chatroom
                        default:

                            break;

                    }
                } while (loggedIn);


                // do the work of a server instance


            }
            catch (Exception E) { Console.WriteLine("Exception " + E); }
            
        }

        static void Main(string[] args)
        {
            Console.Write("Server Starting Up\n");

            // Initalize the lists
            userNames = new List<string>();
            gamesInPlay = new List<gameObject>();

            // create server, on port 8180
            TcpListener server = new TcpListener(8180);

            // start server
            server.Start();

            // Setup delegates
            List<Del> handleConnection = new List<Del>();
            List<IAsyncResult> results = new List<IAsyncResult>();

            // How many clients
            int connected = 0;

            //Allow connections
            while (true)
            {

                // Wait for client to connect
                TcpClient client = server.AcceptTcpClient();

                // Add blank field to list so we can use them for new connections
                results.Add(null);
                handleConnection.Add(null);

                Console.Write("Creating new Delegate to handle connection.\n");
                // get streams
                handleConnection[connected] = handleNewConnection;
                // Start new delegate
                results[connected] = handleConnection[connected].BeginInvoke(client, null, null);
                

                connected++;

            }
        }
    }
}