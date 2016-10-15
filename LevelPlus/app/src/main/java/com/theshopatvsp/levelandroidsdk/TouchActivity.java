package com.theshopatvsp.levelandroidsdk;

import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.theshopatvsp.levelandroidsdk.ble.model.DeviceClient;
import com.theshopatvsp.levelandroidsdk.ble.model.DeviceObserverCallbacks;
import com.theshopatvsp.levelandroidsdk.ble.model.ReporterConfig;
import com.theshopatvsp.levelandroidsdk.ble.model.constants.BatteryState;
import com.theshopatvsp.levelandroidsdk.ble.model.constants.ReporterError;
import com.theshopatvsp.levelandroidsdk.ble.model.constants.ReporterType;
import com.theshopatvsp.levelandroidsdk.ble.model.constants.reporter.DependentDataScale;
import com.theshopatvsp.levelandroidsdk.ble.model.response.RecordData;
import com.theshopatvsp.levelandroidsdk.utils.MladenUtils;

import java.util.Set;

/**
 * Created by admin on 10/8/2016.
 */

public class TouchActivity extends AppCompatActivity {

    int magnitudeDifference;
    boolean smsSent;

    private static long lastCheckTime = 0;
    private TextView log, logTxt2, dataReceived, debug3;
    private static final String TAG = TouchActivity.class.getSimpleName();
    private static final String TAG_XYZ = "Location";
    private DeviceClient deviceClient;
    private boolean dataOn = false;
    private Handler handler;
    private Button reInitiateSms;
    private DeviceObserverCallbacks callbacks = new DeviceObserverCallbacks() {
        @Override
        public void onBluetoothNotAvailable() {
            Log.v(TAG, "onBluetoothNotAvailable");
        }

        @Override
        public void onBluetoothNotOn() {
            Log.v(TAG, "onBluetoothNotOn");
        }

        @Override
        public void onInputLedCode() {
            Log.v(TAG, "onInputLedCode");
        }

        @Override
        public void onLedCodeAccepted() {
            Log.v(TAG, "onLedCodeAccepted");
        }

        @Override
        public void onLedCodeFailed() {
            Log.v(TAG, "onLedCodeFailed");
        }

        @Override
        public void onLedCodeNotNeeded() {
            Log.v(TAG, "onLedCodeNotNeeded");
        }

        @Override
        public void onDisconnect() {
            Log.v(TAG, "onDisconnect");
            log.post(new Runnable() {
                @Override
                public void run() {
                    log.setText("onDisconnect");
                }
            });

        }

        @Override
        public void onData(final RecordData data) {
            Log.v(TAG, "onData ########" + MladenUtils.printIntArr(data.getData()));

            lastCheckTime = System.currentTimeMillis();
            dataReceived.post(new Runnable() {
                @Override
                public void run() {

                    final StringBuffer asdf = new StringBuffer("");
                    // int broj = 0;
                    int dataSum = 0;
                    magnitudeDifference = 0;

                    for (int i = 0; i < data.getData().length - 3; i += 3) {
                        int x = data.getData()[i];
                        int y = data.getData()[i + 1];
                        int z = data.getData()[i + 2];

                        /*
                        double total = Math.sqrt(x * x + y * y + z * z);
                        broj += 1;
                        dataSum += data.getData()[i];

                        if (broj % 20 == 0) {
                            asdf.append("" + dataSum / 2000);// + " t: " + new Date(data.getOriginalTimestamp()) + "\n");
                            dataSum = 0;
                        }
                        broj += data.getData()[i];

                        dataReceived.setText(x + "  " + y + "  " + z + "\n");

                        magnitudeDifference = Math.abs(data.getData()[i] - data.getData()[i - 1]);

                        */
                        int diffX;
                        int diffY;
                        int diffZ;

                        if (x != 0 && y != 0 && z != 0 && i != 0) {
                            diffX = Math.abs(x - data.getData()[i - 3]);
                            diffY = Math.abs(y - data.getData()[i - 2]);
                            diffZ = Math.abs(z - data.getData()[i - 1]);

                            Log.d(TAG_XYZ, "X " + x + " Y " + y + " Z " + z + "dX " + diffX + " dY " + diffY + " dZ " + diffZ);


                            String axis = "";
                            if (diffX > diffY && diffX > diffZ) {

                                axis = "X";

                            } else if (diffY > diffZ) {
                                axis = "Y";
                            } else {
                                axis = "Z";
                            }

                            magnitudeDifference = Math.max(Math.max(diffX, diffY), diffZ);
                            dataReceived.setText("mag.diff.:  " + magnitudeDifference + "; Axis: " + axis);
                        }

                    }

                    //dataReceived.setText(dataReceived.getText().toString() + asdf.toString() + "\n");

                    handler.postDelayed(new Runnable() {
                        @Override
                        public void run() {

                            if (lastCheckTime != 0 && (System.currentTimeMillis() - lastCheckTime) > (2 * 1000)) {


                                /**
                                 * this is just example (proof of concept.) gesture detection algorithms are yet to be implemented;
                                 * Example implementation:  tap can be detected as quick major ghance in one of the axes where
                                 * (for example X) changes sequential minus and plus axis in short period of time.
                                 * Explanation of tap: https://mylifewithandroid.blogspot.rs/2013/06/tap-detection-supported-by-gyroscope.html
                                 *
                                 *
                                 **/

                                if (magnitudeDifference > 35000 && !smsSent) {
                                    debug3.setText("Max diff: " + magnitudeDifference);
                                    smsSent = true;
                                    logTxt2.setText("Sending SMS...");
                                    Log.d(TAG, "##########  Sending SMS Message! ");
                                    MladenUtils.sendToContentful("Help!");
                                } else {
                                    if (smsSent) {
                                        logTxt2.setText("SMS ALREADY SENT!");
                                        Log.d(TAG, "Skipping sms! SMS SENT! ");
                                    }
                                }
                                dataReceived.setText(/*dataReceived.getText().toString() + */"0");
                            }
                        }
                    }, 2000);
                }
            });
        }

        @Override
        public void onDataDeleted() {
            Log.v(TAG, "onDataDeleted");
            log.post(new Runnable() {
                @Override
                public void run() {
                    log.setText("Data deleted");
                }
            });
        }

        @Override
        public void onLedCodeDone() {
            Log.v(TAG, "onLedCodeDone");
        }

        @Override
        public void onDeviceReady() {
            Log.v(TAG, "onDeviceReady");
            deviceClient.getBatteryLevel();
            deviceClient.getBatteryState();
            log.post(new Runnable() {
                @Override
                public void run() {
                    log.setText("Device ready.");
                }
            });
        }

        @Override
        public void onSetupSuccess() {
            Log.v(TAG, "onSetupSuccess");
            log.post(new Runnable() {
                @Override
                public void run() {
                    log.setText("Setup success");
                }
            });
        }

        @Override
        public void onSetupFailed(final ReporterError error) {
            Log.v(TAG, "onSetupFailed " + error);
            log.post(new Runnable() {
                @Override
                public void run() {
                    log.setText("Setup failed. " + error);
                }
            });
            ;
        }

        @Override
        public void onReporterQueried(ReporterConfig config) {
            Log.v(TAG, "onReporterQueried ");
        }

        @Override
        public void onReportersEnabled(Set<ReporterType> activeReporters) {
            Log.v(TAG, "onReportersEnabled");
            log.post(new Runnable() {
                @Override
                public void run() {
                    log.setText("Reporters enabled.");
                }
            });

            for (final ReporterType type : activeReporters) {
                Log.v(TAG, "Reporter " + type + " active.");
                log.post(new Runnable() {
                    @Override
                    public void run() {
                        log.setText("Reporter " + type + " active.");
                    }
                });
            }
        }

        @Override
        public void onBatteryLevel(final int level) {
            Log.v(TAG, "onBatteryLevel");
//            batteryLevel.post(new Runnable() {
//                @Override
//                public void run() {
            //                    batteryLevel.setText("" + level);
//                }
//            });
        }

        @Override
        public void onBatteryState(final BatteryState state) {
            Log.v(TAG, "onBatteryState");
            /*
            batteryState.post(new Runnable() {
                @Override
                public void run() {
                    batteryState.setText(state.name());
                }
            });
            batteryState.post(new Runnable() {
                @Override
                public void run() {
                    batteryState.setText(state.name());
                }

            });
            */
        }

        @Override
        public void onResponseData(String data) {
            Log.v(TAG, "onResponseData");
            setTextInRunnable(log, "Receiving data...");
            setTextInRunnable(dataReceived, data);
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.touch_activity);
        handler = new Handler();
        deviceClient = new DeviceClient();

        log = (TextView) findViewById(R.id.log);
        logTxt2 = (TextView) findViewById(R.id.log2);
        dataReceived = (TextView) findViewById(R.id.data_received);

        debug3 = (TextView) findViewById(R.id.debug3);

        reInitiateSms = (Button) findViewById(R.id.reInitiateSms);
        reInitiateSms.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Log.d(TAG, "Re-enabling SMS");
                logTxt2.setText("Re-initialized and waiting...");
                debug3.setText("...");
                smsSent = false;
            }
        });

        Button setUp = (Button) findViewById(R.id.setUpReporter);
        setUp.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                sendReporterData();
            }
        });

        Button unpairDevice = (Button) findViewById(R.id.disconnect);

        unpairDevice.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                deviceClient.disconnet();
            }
        });

        //  doIt.setOnClickListener(new View.OnClickListener() {
        //            @Override
        //            public void onClick(View v) {
        //                String type = (String) queryReporterSpinner.getSelectedItem();
        //                String action = (String) reporterActionSpinner.getSelectedItem();
        //
        //                if (action.equalsIgnoreCase("Query")) {
        //                    deviceClient.queryReporter(ReporterType.getByName(type));
        //                } else if (action.equalsIgnoreCase("Enable")) {
        //                    deviceClient.enableReporter(ReporterType.getByName(type));
        //                } else if (action.equalsIgnoreCase("Disable")) {
        //                    deviceClient.disableReporter(ReporterType.getByName(type));
        //                }
        //            }
        //        });                                                                   // execute button


        final Button enableData = (Button) findViewById(R.id.enableData);
        enableData.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (!dataOn) {
                    dataOn = true;
                    enableData.setText("Disable Data Stream");
                    deviceClient.enableDataStream();
                } else {
                    dataOn = false;
                    enableData.setText("Enable Data Stream");
                    deviceClient.disableDataStream();
                }
            }
        });
        Button nukeData = (Button) findViewById(R.id.nukeData);
        nukeData.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                deviceClient.deleteAllStoredData();
            }
        });

        Button queryReportControl = (Button) findViewById(R.id.queryReportControl);
        queryReportControl.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                deviceClient.queryEnabledReporters();
            }
        });
    }

    private void sendReporterData() {
        ReporterConfig.Builder configBuilder = new ReporterConfig.Builder();

        configBuilder = configBuilder.accel();

        int samplingFrequency = 20; // as high as glasses could handle
        DependentDataScale scale = DependentDataScale.getById(6); // ONE_TO_ONE_BIT auto 2 - 100 to 1bit
        int samplesPerRec = 1;
        int maxRecs = 0;

        configBuilder = configBuilder
                .samplingFrequency(samplingFrequency)
                .dependentDataScale(scale)
                .samplesPerRecord(samplesPerRec)
                .maxRecordsPerReport(maxRecs);

        configBuilder = configBuilder.includeXAxis();
        configBuilder = configBuilder.includeYAxis();
        configBuilder = configBuilder.includeZAxis();
        //configBuilder = configBuilder.includeMagnitude();

        deviceClient.setUpReporter(configBuilder.build());
    }

    @Override
    public void onPause() {
        super.onPause();

        deviceClient.unregisterDeviceCallbacks();
    }

    @Override
    public void onResume() {
        super.onResume();
        deviceClient.registerDeviceCallbacks(callbacks);
    }

    private void setTextInRunnable(final TextView textView, final String text) {
        textView.post(new Runnable() {
            @Override
            public void run() {
                textView.setText(text);
            }
        });
    }
}
