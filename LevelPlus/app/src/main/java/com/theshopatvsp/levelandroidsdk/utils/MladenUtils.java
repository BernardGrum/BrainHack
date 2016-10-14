package com.theshopatvsp.levelandroidsdk.utils;

import android.util.Log;

import com.contentful.java.cma.CMACallback;
import com.contentful.java.cma.CMAClient;
import com.contentful.java.cma.model.CMAEntry;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.theshopatvsp.levelandroidsdk.ble.BleManager;

import java.util.Random;

import static java.util.Locale.US;

/**
 * Created by Mladen on 10/9/2016.
 */

public abstract class MladenUtils {


    private static final String TAG = "UTIL";

    public static String printIntArr(int[] param) {

        String result = "@@ Data array: \n";
        for (int i = 0; i < param.length; i++) {
            result += ("" + param[i] + "  ");
        }
        return result;
    }

    public static void sendToContentful(String message) {
        Log.d(TAG, "@@@ Sending SMS!!! Message: " + message);

        try {

            final CMAClient client = new CMAClient.Builder()
                    .setAccessToken("c820d11d940ac4a471277134d6236036473846b3cd3f3d3f17dfdc0e72b23da6")
                    .build();

            Random random = new Random();

            client.entries().async().create("ro1y8co2kk99", "trigger",
                    new CMAEntry().setField("id", random.nextInt(50000), "en-US").setField("actionType", message, "en-US"),
                    new CMACallback<CMAEntry>() {
                        @Override
                        protected void onSuccess(CMAEntry result) {


                            client.entries().async().publish(result, new CMACallback<CMAEntry>() {
                                @Override
                                protected void onSuccess(CMAEntry result) {
                                    Log.d(TAG, "Sms sent!");
                                }

                                @Override
                                protected void onFailure(RuntimeException exception) {
                                    super.onFailure(exception);
                                    Log.d(TAG, "Error while sending SMS! Message: " + exception.getMessage(), exception);
                                }
                            });
                        }

                        @Override
                        protected void onFailure(RuntimeException exception) {
                            super.onFailure(exception);
                            Log.d("BleManager", "kresa " + exception.getMessage());
                        }
                    });
        } catch (Exception ex) {
            Log.e(TAG, "Error while processing data! Message: " + ex.getMessage(), ex);
        }
    }
}
