from paddleocr import PaddleOCR
import fitz
import requests
import cv2
import logging
import numpy as np

logging.disable(logging.DEBUG)

ocr = PaddleOCR(
   use_textline_orientation=True,
   lang='vi'
)

def extract_text_from_cdn_url(url):
   response = requests.get(url)
   pdf_content = response.content

   doc = fitz.open(stream=pdf_content, filetype="pdf")
   full_text = ""

   for page in doc:
      # Ưu tiên lấy text gốc trước
      text = page.get_text()
      if text.strip():
         full_text += text + "\n"
         continue

      # Nếu không có thì mới OCR
      pix = page.get_pixmap(dpi=300)
      img_bytes = pix.tobytes("png")

      nparr = np.frombuffer(img_bytes, np.uint8)

      img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

      if img is None:
         continue

      img = cv2.resize(img, None, fx=1.5, fy=1.5)

      # preprocess
      gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
      thresh = cv2.adaptiveThreshold(
         gray, 255,
         cv2.ADAPTIVE_THRESH_GAUSSIAN_C,
         cv2.THRESH_BINARY, 11, 2
      )

      # result = ocr.predict(thresh)
      result = ocr.predict(img)

      print("Shape:", img.shape)
      print("Result raw:", result)

      if result:
         for res in result:
            texts = res.get('rec_texts', [])
            for text in texts:
                  full_text += text + " "

   return full_text

pdf_url = "https://zeionzone.b-cdn.net/cv-folders/019d3f66-56b7-7da4-ad9e-e66b6290d9a2.pdf"

print("--- Đang bắt đầu quá trình OCR, vui lòng đợi... ---")

try:
   # Gọi hàm xử lý
   result_text = extract_text_from_cdn_url(pdf_url)

   print("\n--- NỘI DUNG TRÍCH XUẤT ĐƯỢC ---")
   print(result_text)
   print("\n--- HOÀN TẤT ---")

except Exception as e:
   print(f"Có lỗi xảy ra: {e}")
