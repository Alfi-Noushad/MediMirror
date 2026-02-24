import cv2
import mediapipe as mp
import socket
import json

UDP_IP = "127.0.0.1"
UDP_PORT = 5052
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

mp_pose = mp.solutions.pose
mp_draw = mp.solutions.drawing_utils

pose = mp_pose.Pose(
    static_image_mode=False,
    model_complexity=1,
    min_detection_confidence=0.6,
    min_tracking_confidence=0.6
)

cap = cv2.VideoCapture(0)

alpha = 0.3
smooth = {}

def ema(k, v):
    if k not in smooth:
        smooth[k] = v
    smooth[k] += alpha * (v - smooth[k])
    return smooth[k]

while True:
    ret, img = cap.read()
    if not ret:
        break

    img = cv2.flip(img, 1)
    rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

    result = pose.process(rgb)

    data = {}

    if result.pose_landmarks:
        lm = result.pose_landmarks.landmark

        # Right shoulder / elbow / wrist
        rs = lm[12]
        rw = lm[16]

        # Left shoulder
        ls = lm[11]

        # Nose (head)
        nose = lm[0]

        # ---------------- ARM ----------------
        side = (rw.x - rs.x) * 300
        forward = (rs.y - rw.y) * 300

        side = max(-90, min(90, side))
        forward = max(-90, min(90, forward))

        data["shoulderX"] = ema("shoulderX", forward)
        data["shoulderY"] = ema("shoulderY", side)

        # ---------------- CHEST ----------------
        chestX = (ls.y - rs.y) * -80
        chestY = (ls.x - rs.x) * 120

        data["chestX"] = ema("chestX", chestX)
        data["chestY"] = ema("chestY", chestY)

        # ---------------- HEAD ----------------
        headX = (rs.y - nose.y) * 150
        headY = (rs.x - nose.x) * 150

        data["headX"] = ema("headX", headX)
        data["headY"] = ema("headY", headY)

        mp_draw.draw_landmarks(
            img,
            result.pose_landmarks,
            mp_pose.POSE_CONNECTIONS
        )

    sock.sendto(json.dumps(data).encode(), (UDP_IP, UDP_PORT))

    cv2.imshow("Simple Body Tracking", img)

    if cv2.waitKey(1) & 0xFF == 27:
        break

cap.release()
cv2.destroyAllWindows()
