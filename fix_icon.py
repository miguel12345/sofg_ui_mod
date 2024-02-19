import cv2
from PIL import Image

# img = cv2.imread('eye.png', cv2.IMREAD_UNCHANGED)
# img[:,:,:3] = 255 - img[:,:,:3]
# pil_image = Image.fromarray(img)

pil_image = Image.open('eye.png')

if pil_image.mode != 'RGBA':
    pil_image = pil_image.convert('RGBA')


new_width = 200
aspect_ratio = pil_image.height / pil_image.width
new_height = int(new_width * aspect_ratio)

pil_image = pil_image.resize((new_width, new_height), Image.Resampling.LANCZOS)

pil_image.save('eye_fixed.png')