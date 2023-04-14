# MediaPipe Hands
import mediapipe as mp
import cv2
import numpy as np
import threading
import time

DEBUG = True # significantly reduces performance
MODEL_COMPLEXITY = 0 # set to 1 to improve accuracy at the cost of performance

# the capture thread captures images from the WebCam on a separate thread (for performance)
class CaptureThread(threading.Thread):
    cap = None
    ret = None
    frame = None
    isRunning = False
    counter = 0
    timer = 0.0
    def run(self):
        #self.set_best_camera()
        self.cap = cv2.VideoCapture(1)
        if cv2.useOptimized():
            print("Optimized")
        else:
            print("not Optimized")

        print("Opened Capture")
        while(True):
            self.ret, self.frame = self.cap.read()
            self.isRunning = True
            if DEBUG:
                self.counter = self.counter+1
                if time.time()-self.timer>=3:
                    print("Capture FPS: ",self.counter/(time.time()-self.timer))
                    self.counter = 0
                    self.timer = time.time()
    def set_best_camera(self):
        index=0;
        camera_list=[]
        camera_list_resolution=[]
        index_out_of_range=False
        
        while not index_out_of_range:
            
            cap = cv2.VideoCapture(index)
          
            ret,frame = cap.read()
            if not ret:
                print(f"{index} out of range")
                index_out_of_range=True
                break
            else:
                cam_height,cam_width, channels = frame.shape
                camera_list.append(index)
                camera_list_resolution.append(cam_width*cam_height)
                index+=1
            cap.release()

        best_resolution=camera_list_resolution[0]
        index_camera_chosen=0
        for i in range(0,len(camera_list_resolution)):
            print(camera_list_resolution[i])
            if best_resolution < camera_list_resolution[i]:
                best_resolution = camera_list_resolution[i]
                index_camera_chosen=i
        print(f"risoluzione : {best_resolution}: {index_camera_chosen}")

            
      
          
        print(f" Trovate n:{len(camera_list)} camere")

# the hand thread actually does the processing of the captured images
class HandThread(threading.Thread):
    data=""
    dirty = True
    def run(self):
        mp_drawing = mp.solutions.drawing_utils
        mp_hands = mp.solutions.hands
        width=0

        capture = CaptureThread()
        capture.start()

        # Based Heavily on: https://github.com/nicknochnack/MediaPipeHandPose/blob/main/Handpose%20Tutorial.ipynb
        with mp_hands.Hands(min_detection_confidence=0.75, min_tracking_confidence=0.5, model_complexity = MODEL_COMPLEXITY) as hands: 
            while capture.isRunning==False:
                print("Waiting for capture")
                time.sleep(500/1000)
            print("beginning capture")
                
            while capture.cap.isOpened():
                #ret, frame = cap.read()
                ret = capture.ret
                frame = capture.frame
                width=frame.shape[1]
                # BGR 2 RGB
                image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                
                # Flip on horizontal
                image = cv2.flip(image, 1)
                
                # Set flag
                image.flags.writeable = DEBUG
                
                # Detections
                results = hands.process(image)
                
                # RGB 2 BGR
                image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
                
                # Rendering results
                if DEBUG:
                    if results.multi_hand_landmarks:
                        for num, hand in enumerate(results.multi_hand_landmarks):
                            mp_drawing.draw_landmarks(image, hand, mp_hands.HAND_CONNECTIONS, 
                                                    mp_drawing.DrawingSpec(color=(121, 22, 76), thickness=2, circle_radius=4),
                                                    mp_drawing.DrawingSpec(color=(250, 44, 250), thickness=2, circle_radius=2),
                                                    )
                            
                       
                # Set up data for piping
                self.data = ""
                i = 0
                
                if results.multi_hand_landmarks:
                    for j in range(len(results.multi_handedness)):
                        
                        hand_world_landmarks = results.multi_hand_landmarks[j]
                       
                        
                        for i in range(0,21):
                            self.data += "{}|{}|{}|{}|{}\n".format(results.multi_handedness[j].classification[0].label,i,hand_world_landmarks.landmark[i].x,hand_world_landmarks.landmark[i].y,hand_world_landmarks.landmark[i].z)
                   
                        #print(self.data)
                        self.dirty = True
                   # self.get_gesture(results.multi_hand_landmarks,width)
                
                if DEBUG:
                    cv2.imshow('Hand Tracking', image)

                    if cv2.waitKey(5) & 0xFF == ord('q'):
                        break

        capture.cap.release()
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


