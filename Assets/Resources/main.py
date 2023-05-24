#pipe server
from hands import HandThread
import time
import struct
from socket import *
from hands import setCamera

serverName = '127.0.0.1'

serverPort = 6790

def main():
# Create a UDP socket
    
    
    clientSocket = socket(AF_INET, SOCK_DGRAM)
    server_address = (serverName, serverPort)
    server_address_dataWorld = (serverName, 6770)
    server_address_gesture = (serverName, 6794)
    thread = HandThread()
    thread.start()

    # Piping method based heavily on: https://gist.github.com/JonathonReinhart/bbfa618f9ad19e2ca48d5fd10914b069
    #f = open(r'\\.\pipe\UnityMediaPipeHands', 'r+b', 0)



    while not thread.haveFinished:
        
        if thread.dirty:
            #in data ci sono i dati ottenuti dal thread hand
            s = thread.data.encode('ascii') 
            # s2 = thread.dataWorld.encode('ascii') 
            sent = clientSocket.sendto(s, server_address)
           # sent = clientSocket.sendto(s, server_address_dataWorld)
                        

            thread.dirty = False
        if thread.gesture_rilevata:
            d= thread.last_gesture.encode('ascii')
            sentGesture=clientSocket.sendto(d,server_address_gesture)
            thread.gesture_rilevata=False


        time.sleep(16/1000) # enforces a hard limit on the speed of sending data
    print("end")
    quit()




if __name__ == "__main__":
    main()

