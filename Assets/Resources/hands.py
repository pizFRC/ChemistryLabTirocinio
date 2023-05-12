# MediaPipe Hands
import mediapipe as mp
import mediapipe.modules.selfie_segmentation as mp2
import cv2
import numpy as np
import threading
import time
from socket import *
import time
from threading import RLock,Condition
import os


DEBUG =  True# significantly reduces performance
MODEL_COMPLEXITY = 0 # set to 1 to improve accuracy at the cost of performance
CAMERA_INDEX=0


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
        
        
        self.stringa=""

    def run(self):
        self.timer=time.time()
        while True:
           
            
            
            self.clientSocket.sendto(self.img,self.server_address)

           
            
               # sent = self.clientSocket.sendto(, self.server_address)
            time.sleep(0.08)
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
                    print("Capture FPS: ",self.counter/(time.time()-self.timer),self.timer)
                    self.counter = 0
                    self.timer = time.time()
        print("capture thread finished")
    def set_have_finished(self,booleanValue):
        self.haveFinished=True
    

# the hand thread actually does the processing of the captured images
class HandThread(threading.Thread):
    
    data=""
    dataWorld=""
    last_gesture=""
    gesture_rilevata=False
    dirty = True
    haveFinished=False
    imageSender=ImageSender()
    capture = CaptureThread()
   # gestureSender=GestureSender()
    def is_finger_raised(self,tip,pip,mcp,wrist):
        return tip.y < pip.y and pip.y < mcp.y and mcp.y > wrist.y
    def run(self):
        mp_drawing = mp.solutions.drawing_utils
        self.mp_hands = mp.solutions.hands
        width=0
        lastTimestamp=int(time.monotonic()*1000)

        # disegna i cerchi sui due frame
        radius = 100
        color = (0, 255, 0)
        thickness = 2
        
        self.capture.start()
        self.imageSender.start()
       # self.gestureSender.start()
        def print_result(result: mp.tasks.vision.GestureRecognizerResult, output_image: mp.Image, timestamp_ms: int):
          #  self.gestureSender.set_gesture(f"<{result.gestures}|{result.handedness}>")
            
           
            
            resultHand1=resultGesture1=resultGesture=resultHand=final=final2=""
            if(len(result.gestures)>0 and len(result.handedness)>0 ):
                #print(result.gestures," <----->",result.handedness)
                resultGesture1=f"{result.gestures[0]}"
                resultHand1=f"{result.handedness[0]}"
                final=f"[{resultGesture1.split(',')[3]}  [{resultHand1.split(',')[3]}"
            if( len(result.gestures)>1 and len(result.handedness)>1 ):
                resultGesture=f"{result.gestures[1]}"
                resultHand=f"{result.handedness[1]}"
         
            if(resultGesture!= "" and resultHand != ""):
                final2=f"[{resultGesture.split(',')[3]}  [{resultHand.split(',')[3]}"
            self.last_gesture=final+"---"+final2
            self.gesture_rilevata=True
            print(final ,"-----",final2)
               
                
        gesture_file = os.path.join(os.path.dirname(__file__), 'gesture_recognizer.task')
        with self.mp_hands.Hands(min_detection_confidence=0.5, min_tracking_confidence=0.6, model_complexity = MODEL_COMPLEXITY) as hands:
            with open(gesture_file, 'rb') as f:
                model = f.read()
                options = mp.tasks.vision.GestureRecognizerOptions(
                    base_options=mp.tasks.BaseOptions(model_asset_buffer=model),
                    num_hands=2,
                    min_hand_detection_confidence=0.4,
                    min_hand_presence_confidence = 0.5,
                    min_tracking_confidence = 0.5,
                    running_mode=mp.tasks.vision.RunningMode.LIVE_STREAM,
                    result_callback=print_result)
                recognizer = mp.tasks.vision.GestureRecognizer.create_from_options(options)
           
            while self.capture.isRunning==False:
                
                print(f"Waiting for capture t:{threading.get_ident()}")
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
               
                # Detections
                results = hands.process(image)
                
                
                segmenter = mp.solutions.selfie_segmentation.SelfieSegmentation()
                # Estrai la maschera binaria dell'hand
                results2 = segmenter.process(image)
                image_h, image_w, _ = frame.shape
                mask2 = results2.segmentation_mask
                
                mask2 = (results2.segmentation_mask > 0).astype(np.uint8) *255
                x1, y1 = int(image_w/4)  , int(image_h/2)
                x2, y2 = int(image_w*(3/4)) ,int(image_h/2)

              
                
                image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
                #gray_image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
                kernel = np.ones((8, 8), np.uint8)
               
               
                dilated_image = cv2.erode(mask2, kernel, iterations=4)
                masked_image3 = cv2.bitwise_and(image, image, mask=dilated_image)
                '''TEST 
                kernel = np.ones((8,8), np.uint8)
                eroded_mask = cv2.erode(mask2, kernel, iterations=3)
                masked_image3 = cv2.bitwise_and(image, image, mask=eroded_mask)

               
                
                kernel = np.ones((10, 10), np.uint8)
                dilated_mask = cv2.dilate(mask2, kernel, iterations=5)
                masked_image3 = cv2.bitwise_and(image, image, mask=dilated_mask)
                
                

                '''


                background = np.zeros_like(masked_image3)


                #result3 = cv2.add(masked_image3, background)

              
              
                if results.multi_hand_landmarks:
                    
                    for num, hand in enumerate(results.multi_hand_landmarks):
                        mp_drawing.draw_landmarks(masked_image3, hand, self.mp_hands.HAND_CONNECTIONS, 
                                                mp_drawing.DrawingSpec(color=(121, 22, 76), thickness=2, circle_radius=4),
                                                mp_drawing.DrawingSpec(color=(250, 44, 250), thickness=2, circle_radius=2),
                                                )

                   
                    
                
                    #if len(recognition_result.handedness)>0:
                     #   print(recognition_result.gestures)
                      #  print(recognition_result.handedness)
                       # print(len(recognition_result.handedness))
                        #print(recognition_result.handedness[0])
                        #print(recognition_result.gestures[0][0])
               
                   
                timestamp = int(time.monotonic() * 1000)
                
                if timestamp-lastTimestamp>180 :
                    
                    mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=frame)
                    
                    lastTimestamp=timestamp
                    
                    recognizer.recognize_async(mp_image,timestamp)

                # Set up data for piping
                self.data = ""
                
                self.dataWorld=""
                
                if results.multi_hand_landmarks:

                    for j in range(len(results.multi_handedness)):
                        
                        hand_landmarks = results.multi_hand_landmarks[j]
                        
                        hand_world_landmarks = results.multi_hand_world_landmarks[j]
                        for i in range(0,21):
                            self.data += "{}|{}|{}|{}|{}\n".format(results.multi_handedness[j].classification[0].label,i,hand_landmarks.landmark[i].x,hand_landmarks.landmark[i].y,hand_landmarks.landmark[i].z)
                            self.dataWorld += "{}|{}|{}|{}|{}\n".format(results.multi_handedness[j].classification[0].label,i,hand_world_landmarks.landmark[i].x,hand_world_landmarks.landmark[i].y,hand_world_landmarks.landmark[i].z)
                        #self.data += "{}".format(self.gesture_rilevata)
                        
                        self.dirty = True
                        
                        
                        
                        
                        
                       # euclidean_distance_to_camera = math.sqrt(hand_world_landmarks.landmark[j].x ** 2 + hand_world_landmarks.landmark[0].y ** 2 + hand_world_landmarks.landmark[0].z ** 2)
                        y_coord = (hand_landmarks.landmark[0].y *image_h)
    
                        # Verifica se la coordinata y del punto medio si trova più o meno nel centro dell'immagine lungo l'asse y
                        if y_coord < image_h / 2:
                            pass
                            #print(f"{results.multi_handedness[j].classification[0].label} si più in alto rispetto  {y_coord} - { image_h / 2}")
                        elif y_coord > image_h / 2:
                            pass
                           # print(f"{results.multi_handedness[j].classification[0].label}  più in basso rispetto{y_coord}  { image_h / 2}");
                       # print(f"{results.multi_handedness[j].classification[0].label}: {euclidean_distance_to_camera}")       
                
           
                # mostra l'immagine sovrapposta alla maschera
            
                encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 50]
                compressed_img, _ = cv2.imencode('.jpg', masked_image3, encode_param)
          
               
                
                
              
                self.imageSender.img=_.tobytes()
                
                   
                
                if DEBUG:
                    #cv2.imshow('Segmentation Mask', image)
                    
                    cv2.imshow('result3',masked_image3)
                    #cv2.imshow('sef',dilated_image)
                    
                    
                if cv2.waitKey(5) & 0xFF == ord('q'):
                    self.haveFinished=True
                    self.capture.set_have_finished(self.haveFinished) 
                    break

                if self.capture.haveFinished:
                    break
                    
                    

        self.capture.cap.release()
        self.capture.join()
        cv2.destroyAllWindows()
