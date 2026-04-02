import os
import pandas as pd

from services.database_service import get_db_connection

import pandas as pd

def load_knowledge():
   print("[System]: Fetching data from Database...")
   conn = get_db_connection()
   query = """
      SELECT J.JobPostID, J.Title, J.Description, J.Requirements, J.SalaryRange,
            J.Address, B.BusinessName, L.LocationName
      FROM JobPosts J
      JOIN Businesses B ON J.BusinessID = B.BusinessID
      JOIN Locations L ON J.LocationID = L.LocationID
      WHERE J.IsActive = 1
   """
   df = pd.read_sql(query, conn)
   conn.close()

   knowledge_chunks = []
   if os.path.exists("data/instruction.txt"):
      with open("data/instruction.txt", "r", encoding="utf-8") as f:
         knowledge_chunks.append(f.read())

   for _, row in df.iterrows():
      text_block = (
         f"[BẮT ĐẦU CÔNG VIỆC]\n"
         f"Tiêu đề: {row['Title']}\n"
         f"ID: {row['JobPostID']}\n"
         f"Công ty: {row['BusinessName']}\n"
         f"Địa điểm: {row['LocationName']} ({row['Address']})\n"
         f"[MÔ TẢ CÔNG VIỆC]: {row['Description']}\n"
         f"[YÊU CẦU CÔNG VIỆC]: {row['Requirements']}\n"
         f"Mức lương: {row['SalaryRange']}\n"
         f"[KẾT THÚC CÔNG VIỆC]"
      )
      knowledge_chunks.append(text_block)

   return knowledge_chunks

def split_knowledge(text):
   chunks = []
   current = ""

   for line in text.split("\n"):
      if line.startswith("[") and line.endswith("]"):
         if current:
               chunks.append(current.strip())
         current = line + "\n"
      else:
         current += line + "\n"

   if current:
      chunks.append(current.strip())

   return chunks
