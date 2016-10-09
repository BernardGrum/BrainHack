# -*- coding: utf-8 -*-
"""
Created on Sat Oct 08 19:45:01 2016

@author: Primoz Kocevar
"""

"""Example program to show how to read a multi-channel time series from LSL."""

from pylsl import StreamInlet, resolve_stream
import numpy as np
import winsound
import socket

HOST='localhost'
PORT=8080


s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))
s.listen(1)
conn, addr = s.accept()
print 'Connected by', addr




# first resolve an EEG stream on the lab network
print("looking for an EEG stream...")
streams = resolve_stream('name', 'BR8-001AFF090C91')
print(streams)
# create a new inlet to read from the stream

inlet = StreamInlet(streams[0])

#samples = []
#for _ in xrange(10000):
#    sample, timestamp = inlet.pull_sample()
#    samples.append(sample)   
#samples = np.asarray(samples)
#
#print("min: " + str(np.amin(samples, axis=0)))
#print("max: " + str(np.amax(samples, axis=0)))
#print("avg: " + str(np.average(samples, axis=0)))

timediff = 0.500
state = 0
timestamp_last =0
while True:
    sample, timestamp = inlet.pull_sample()
    if sample[0] > 400 and ( timestamp-timestamp_last ) > timediff:
        if state == 0:
            state = 1
            print("blink    @ " + str(sample[0]))
            conn.sendall("1")
            timestamp_last = timestamp
    elif state == 1 and (timestamp-timestamp_last) > timediff:
        state = 0
        print("no blink @ " + str(sample[0]))
        conn.sendall("0")
        timestamp_last = timestamp
