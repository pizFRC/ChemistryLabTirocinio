#pipe server
from hands import HandThread
import time
import struct
from socket import *


serverName = '127.0.0.1'
serverPort = 6790


# Create a UDP socket
clientSocket = socket(AF_INET, SOCK_DGRAM)
server_address = (serverName, serverPort)



thread = HandThread()
thread.start()

# Piping method based heavily on: https://gist.github.com/JonathonReinhart/bbfa618f9ad19e2ca48d5fd10914b069
#f = open(r'\\.\pipe\UnityMediaPipeHands', 'r+b', 0)



while True:
    
    if thread.dirty:
        #in data ci sono i dati ottenuti dal thread hand
        s = thread.data.encode('ascii')
    
        sent = clientSocket.sendto(s, server_address)
      
       # f.write(struct.pack('I', len(s)) + s)   
       # f.seek(0)                          

        thread.dirty = False

    time.sleep(16/1000) # enforces a hard limit on the speed of sending data
 
quit()
