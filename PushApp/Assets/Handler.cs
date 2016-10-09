using UnityEngine;
using System.Collections;
using System;
using SharpOSC;



public class Handler : MonoBehaviour {

    public GameObject body;
    public GameObject fruit1;
    public GameObject fruit2;
    public GameObject fruit3;

    int fruitCounter=0;

    private enum Direction { UP, DOWN };
    private enum FieldType { NORMAL, ORANGE, APPLE };
    
    public int playerProgress = 0;
    private float[][] coordinates = new float [][] {
        new float[] {-8.42f, 0.07f},
        new float[] {-6.5f, 0.07f},
        new float[] {-5.12f, -0.63f},
        new float[] {-3.85f,-2.02f},
        new float[] {-3.85f, -0.93f},
        new float[] {-1.61f, 0.07f},
        new float[] {0.43f, 0.07f},
        new float[] {1.78f, -2.02f},
        new float[] {1.78f, -0.63f},
        new float[] {4.22f, -0.63f},
        new float[] {5.96f, -0.63f},
        new float[] {5.96f, 0.07f},
        new float[] {8.6f, 0.07f}
    };
    
    private FieldType[] fieldTypes = {
        FieldType.NORMAL,
        FieldType.NORMAL,
        FieldType.NORMAL,
        FieldType.APPLE,
        FieldType.NORMAL,
        FieldType.NORMAL,
        FieldType.NORMAL,
        FieldType.ORANGE,
        FieldType.NORMAL,
        FieldType.NORMAL,
        FieldType.APPLE,
        FieldType.NORMAL,
        FieldType.NORMAL,
    };

    public float y; //Player1 = -7.45, Player2 = -3.37

    private int[] canSee = new int[]{1,1,1};
    public int port;
    public string name;

    UDPListener listener;

    DateTime beginCommand = DateTime.Now;
    int blinkThreshold = 1300;


    int blinkCounter = 0;
    // Init to down since fronts always activate downwards first
    Direction previousFront = Direction.DOWN;
    // Not used
    DateTime lastBlinkTime = DateTime.Now;
    // The front changes when the threshold is not active, so we have to save the switch
    bool frontNotRegistered = false;
    // Command timeouts
    DateTime commandStartTime = DateTime.Now;
    const int commandTimeout = 3000;


    //int PUcounter = 0;
    // Init to down since fronts always activate upwards first
    Direction PUpreviousFront = Direction.DOWN;
    // The front changes when the threshold is not active, so we have to save the switch
    bool PUfrontNotRegistered = false;
    // Store the last action time to make sure we don't count things more than once
    DateTime lastActionTime = DateTime.Now;

    // Use this for initialization
    void Start () {
        // Callback function for received OSC messages. 
        // Prints EEG and Relative Alpha data only.
        HandleOscPacket callback = delegate(OscPacket packet)
        {
            // Get the current field type based on player progress
            FieldType currentFieldType = fieldTypes[playerProgress];

            var messageReceived = (OscMessage)packet;
            var addr = messageReceived.Address;
            
            /**
             * BLINK CONTROLLER
             */
            if (currentFieldType == FieldType.APPLE || currentFieldType == FieldType.ORANGE) {
                const int BLINK_AVERAGE = 890;
                const int BLINK_UPPER_THRESHOLD = 1400;
                const int BLINK_LOWER_THRESHOLD = 200;
                const int MIN_BLINK_TIME = 750;

                if (addr == "/muse/eeg") {
                    int current = Convert.ToInt32(messageReceived.Arguments[1]);

                    Direction currentFront = current > BLINK_AVERAGE ? Direction.UP : Direction.DOWN;

                    bool thresholdPassed = current > BLINK_UPPER_THRESHOLD || current < BLINK_LOWER_THRESHOLD;
                    bool blinkChangingFront = previousFront != currentFront;

                    if (blinkChangingFront) {
                        frontNotRegistered = true;
                    }

                    // How much time has passed from the start of the command
                    TimeSpan timeFromCommandStart = DateTime.Now - commandStartTime;

                    // Make sure enough time has passed from last action to not count the same one twice
                    TimeSpan timeFromLastAction = DateTime.Now - lastActionTime;
                    bool enoughTimeFromLastAction = timeFromLastAction > TimeSpan.FromMilliseconds(MIN_BLINK_TIME);

                    // Register only if the current front direction is downwards
                    if (thresholdPassed && frontNotRegistered && currentFront == Direction.DOWN && enoughTimeFromLastAction) {
                        if (timeFromCommandStart < TimeSpan.FromMilliseconds(commandTimeout)) {
                            // If still part of the same command sequence
                            blinkCounter += 1;
                        } else {
                            // The command has timed out and we start a new command
                            blinkCounter = 1;
                            commandStartTime = DateTime.Now;
                        }
                        // In any event we have to reset the front registered
                        frontNotRegistered = false;
                        lastActionTime = DateTime.Now;
                    }

                    // When the time from command start is larger than timeout, end the command
                    if (timeFromCommandStart > TimeSpan.FromMilliseconds(commandTimeout) && blinkCounter > 1) {
                        Debug.Log(name + " blinked " + blinkCounter + " times.");

                        if (
                            blinkCounter >= 2 && currentFieldType == FieldType.APPLE ||
                            blinkCounter >= 3 && currentFieldType == FieldType.ORANGE
                        ) {
                            nextMove();
                            if(fruitCounter == 0) {
                               canSee[0] = 0;
                            } else if(fruitCounter == 1) {
                               canSee[1] = 0;
                            } else {
                               canSee[2] = 0;
                            }
                            fruitCounter++;
                        }

                        blinkCounter = 0;
                    }

                    previousFront = currentFront;
                    lastBlinkTime = DateTime.Now;
                }
            }
            /**
             * PUSHUP CONTROLLER
             */
            else if (currentFieldType == FieldType.NORMAL) {
                const int PU_AVERAGE = 850;
                const int PU_UPPER_THRESHOLD = 1200;
                const int PU_LOWER_THRESHOLD = 200;
                const int MIN_PU_TIME = 1200;

                if (addr == "/muse/acc") {
                    int current = Convert.ToInt32(messageReceived.Arguments[0]);

                    Direction currentFront = current > PU_AVERAGE ? Direction.UP : Direction.DOWN;

                    bool thresholdPassed = current > PU_UPPER_THRESHOLD || current < PU_LOWER_THRESHOLD;
                    bool blinkChangingFront = PUpreviousFront != currentFront;

                    if (blinkChangingFront) {
                        PUfrontNotRegistered = true;
                    }

                    // Make sure enough time has passed from last PU to not count the same one twice
                    TimeSpan timeFromLastPU = DateTime.Now - lastActionTime;
                    bool enoughTimeFromLastPU = timeFromLastPU > TimeSpan.FromMilliseconds(MIN_PU_TIME);

                    // Register only if the current front direction is downwards
                    if (thresholdPassed && PUfrontNotRegistered && currentFront == Direction.DOWN) {
                        if (enoughTimeFromLastPU) {
                            //PUcounter += 1;
                            nextMove();
                            Debug.Log(name + " did a push up.");
                            lastActionTime = DateTime.Now;
                        }
                        PUfrontNotRegistered = false;
                    }

                    previousFront = currentFront;
                }
            }
        };

        // Create an OSC server.
        listener = new UDPListener(port, callback);
    }

    void nextMove() {
        if (playerProgress + 1 < fieldTypes.Length) {
            playerProgress++;
        }
    }
    
    // Update is called once per frame
    void Update () {
        if(canSee[0] == 0) {
            fruit1.transform.position = new Vector3(100, 100, 100);
        } 
        if(canSee[1] == 0){
            fruit2.transform.position = new Vector3(100, 100, 100);
        } 
        if(canSee[2] == 0) {
            fruit3.transform.position = new Vector3(100, 100, 100);
        }

        body.transform.position =  new Vector3(coordinates[playerProgress][0], y, coordinates[playerProgress][1]);
    }

    void OnApplicationQuit() {
        listener.Close();
    }
}
