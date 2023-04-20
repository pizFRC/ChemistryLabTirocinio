# MediaPipe Hands
import mediapipe as mp
import cv2
import numpy as np
import threading
import time
from socket import *
import time
from threading import RLock
DEBUG = False # significantly reduces performance
MODEL_COMPLEXITY = 1 # set to 1 to improve accuracy at the cost of performance
CAMERA_INDEX=0
print( mp.__file__)

def setCamera(index):
    CAMERA_INDEX=index
class ImageSender(threading.Thread):
    def __init__(self,):
        super().__init__()
        print("start")
        self.serverName="127.0.0.1"
        self.serverPort=6792
        self.clientSocket = socket(AF_INET, SOCK_DGRAM)
        self.server_address = (self.serverName, self.serverPort)
        self.haveFinished=False
        self.dirty=False
        self.lock =RLock()
        self.img=bytearray()

    def run(self):
        while True:
            print("first")
           
            print(" invio")
            
        
            self.clientSocket.sendto(self.img,self.server_address)
               # sent = self.clientSocket.sendto(, self.server_address)
            time.sleep(0.05)
            #in data ci sono i dati ottenuti dal thread hand
            
    def setDirty(self):
        with self.lock:
            self.dirty=True
    def setImg(self,data):
        with self.lock:
            self.img=data
         #   
            
                        

          #  thread.dirty = False
# the capture thread captures images from the WebCam on a separate thread (for performance)
class CaptureThread(threading.Thread):
    cap = None
    ret = None
    frame = None
    isRunning = False
    counter = 0
    timer = 0.0
    haveFinished=False
    def run(self):
        #self.set_best_camera()
        self.cap = cv2.VideoCapture(CAMERA_INDEX)
        if cv2.useOptimized():
            print("Optimized")
        else:
            print("not Optimized")

        print("Opened Capture")
        while(not self.haveFinished):
            self.ret, self.frame = self.cap.read()
            self.isRunning = True
            if DEBUG:
                self.counter = self.counter+1
                if time.time()-self.timer>=3:
                    print("Capture FPS: ",self.counter/(time.time()-self.timer))
                    self.counter = 0
                    self.timer = time.time()
        print("capture thread finished")
    def set_have_finished(self,booleanValue):
        self.haveFinished=True
    

# the hand thread actually does the processing of the captured images
class HandThread(threading.Thread):
    
    data=""
    dirty = True
    haveFinished=False
    capture = CaptureThread()
    imageSender=ImageSender()
    def is_finger_raised(self,tip,pip,mcp,wrist):
        return tip.y < pip.y and pip.y < mcp.y and mcp.y > wrist.y
    def run(self):
        mp_drawing = mp.solutions.drawing_utils
        self.mp_hands = mp.solutions.hands
        width=0

        
        self.capture.start()
        self.imageSender.start()
        print("started")
        # Based Heavily on: https://github.com/nicknochnack/MediaPipeHandPose/blob/main/Handpose%20Tutorial.ipynb
        with self.mp_hands.Hands(min_detection_confidence=0.5, min_tracking_confidence=0.6, model_complexity = MODEL_COMPLEXITY) as hands: 
            while self.capture.isRunning==False:
                print("Waiting for capture")
                time.sleep(500/1000)
            print("beginning capture")
                
            while self.capture.cap.isOpened():
                #ret, frame = cap.read()
                ret = self.capture.ret
                frame = self.capture.frame
                width=frame.shape[1]
                # BGR 2 RGB
                image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                
                # Flip on horizontal
                image = cv2.flip(image, 1)
                
               
                image.flags.writeable = True
                # RGB 2 BGR
                image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
                # Detections
                results = hands.process(image)



              
                
               
                    
                
                if results.multi_hand_landmarks:
                    for num, hand in enumerate(results.multi_hand_landmarks):
                        mp_drawing.draw_landmarks(image, hand, self.mp_hands.HAND_CONNECTIONS, 
                                                mp_drawing.DrawingSpec(color=(121, 22, 76), thickness=2, circle_radius=4),
                                                mp_drawing.DrawingSpec(color=(250, 44, 250), thickness=2, circle_radius=2),
                                                )
                        
                       
                # Set up data for piping
                self.data = ""
         
                if results.multi_hand_landmarks:
                    for j in range(len(results.multi_handedness)):
                        
                        hand_world_landmarks = results.multi_hand_landmarks[j]
                       
                        
                        for i in range(0,21):
                            self.data += "{}|{}|{}|{}|{}\n".format(results.multi_handedness[j].classification[0].label,i,hand_world_landmarks.landmark[i].x,hand_world_landmarks.landmark[i].y,hand_world_landmarks.landmark[i].z)
                   
                            
                        self.dirty = True
                        print("change")
                        
                        encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 50]
                        compressed_img, _ = cv2.imencode('.jpg', image, encode_param)
                        
                      #  resized_frame = cv2.resize(frame, (new_width, new_height))
                        
                        
                        
                        self.imageSender.img=_.tobytes()
                        
                        
                       # self.imageSender.setImg()
            
                        #print(results.multi_hand_landmarks[0])
                        #self.count_raised_fingers(results.multi_hand_landmarks)
                   # self.get_gesture(results.multi_hand_landmarks,width)
                
                    if DEBUG:
                        cv2.imshow('Hand Tracking', image)
                        
                    if cv2.waitKey(5) & 0xFF == ord('q'):
                        self.haveFinished=True
                        self.capture.set_have_finished(self.haveFinished)

                        
                        break
                if self.capture.haveFinished:
                    break
                    
                    

        self.capture.cap.release()
        self.capture.join()
        cv2.destroyAllWindows()



    def get_gesture(self,multi_hand_landmarks,width):
        hand_landmarks = multi_hand_landmarks[0]

        landmarks = np.array([[lmk.x, lmk.y] for lmk in hand_landmarks.landmark])

            # Calcola la distanza tra il pollice e l'indice
        thumb_tip = landmarks[4]
        index_tip = landmarks[8]
        dist_thumb_index = np.linalg.norm(thumb_tip - index_tip)

        # Rileva il gesto del pollice in su
        if dist_thumb_index > 0.32:
            # Il pollice è in su
            print(dist_thumb_index /width)
            print("Il pollice è in su.")
            print(dist_thumb_index )
        else:
            # Il pollice non è in su
            print(dist_thumb_index /width)
            print("Il pollice non è in su.")
            print(dist_thumb_index )
    
    
    def count_raised_fingers(self,hand_landmarks_pa):
        count = 0
        i=0
        hand_tmp = self.mp_hands.HandLandmark
        print(hand_tmp)
        for j, hand_landmarks in enumerate(hand_landmarks_pa):
            
            
            for point in self.mp_hands.HandLandmark:
                i+=1
                print(point)
                normalized_landmark = hand_landmarks.landmark[point]

              #  print(f"cord:{normalized_landmark}")
                
          #  print(f"{hand_tmp.landmark[0]} ")
                
        return count        # Pollice
    '''if hand_landmarks[4].x < hand_landmarks[3].x:
           count += 1
        
        
        # Indice
        if self.is_finger_raised(hand_landmarks[self.mp_hands.HandLandmark.INDEX_FINGER_TIP],
                            hand_landmarks[self.mp_hands.HandLandmark.INDEX_FINGER_PIP],
                            hand_landmarks[self.mp_hands.HandLandmark.INDEX_FINGER_MCP],
                            hand_landmarks[self.mp_hands.HandLandmark.WRIST]):
            count += 1

        # Medio
        if self.is_finger_raised(hand_landmarks[self.mp_hands.HandLandmark.MIDDLE_FINGER_TIP],
                            hand_landmarks[self.mp_hands.HandLandmark.MIDDLE_FINGER_PIP],
                            hand_landmarks[self.mp_hands.HandLandmark.MIDDLE_FINGER_MCP],
                            hand_landmarks[self.mp_hands.HandLandmark.WRIST]):
            count += 1

        # Anulare
        if  self.is_finger_raised(hand_landmarks[self.mp_hands.HandLandmark.RING_FINGER_TIP],
                            hand_landmarks[self.mp_hands.HandLandmark.RING_FINGER_PIP],
                            hand_landmarks[self.mp_hands.HandLandmark.RING_FINGER_MCP],
                            hand_landmarks[self.mp_hands.HandLandmark.WRIST]):
            count += 1

        # Mignolo
        if  self.is_finger_raised(hand_landmarks[self.mp_hands.HandLandmark.PINKY_TIP],
                            hand_landmarks[self.mp_hands.HandLandmark.PINKY_PIP],
                            hand_landmarks[self.mp_hands.HandLandmark.PINKY_MCP],
                            hand_landmarks[self.mp_hands.HandLandmark.WRIST]):
            count += 1
        print(f"{count} dita alzate")'''
         



       

        #time.sleep(16/1000)
        