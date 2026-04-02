import re
import chromadb

from services.chat_service import parse_job, parse_query, normalize_salary
from models.embedding import encode

class Retriever:
   def __init__(self, knowledge_list, db_path="./data/chroma_db"):
      self.client = chromadb.PersistentClient(path=db_path)

      self.collection = self.client.get_or_create_collection(name="job_posts")

      if self.collection.count() == 0:
         print("[System]: Đang nạp dữ liệu vào Vector Database...")

         if not knowledge_list:
            return

         self._add_to_db(knowledge_list)

   def _add_to_db(self, knowledge_list):
      ids = [item["metadata"]["job_id"] for item in knowledge_list]

      documents = [item["text"] for item in knowledge_list]
      metadatas = [item["metadata"] for item in knowledge_list]

      embeddings = encode(documents)

      self.collection.add(
         ids=ids,
         embeddings=embeddings.tolist(),
         documents=documents,
         metadatas=metadatas
      )

      print(f"[System]: Đã nạp thành công {len(documents)} bài đăng...")

   def filter_jobs(self, raw_docs, user_query):
      conditions = parse_query(user_query)

      filtered = []

      for doc in raw_docs:
         job = parse_job(doc)

         # FILTER LOCATION
         if conditions["location"]:
            if conditions["location"] not in job.get("location", ""):
               continue

         # FILTER SALARY
         if conditions["salary_min"]:
            _, max_sal = normalize_salary(job.get("salary", ""))

            if max_sal is None:
               continue

            if max_sal < conditions["salary_min"] * 1_000_000:
               continue

         filtered.append(doc)

      return filtered

   def get_context(self, query, top_n=10):
      query_vec = encode([query]).tolist()

      conditions = parse_query(query)

      where_clause = {}

      if conditions["location"]:
         where_clause["location"] = conditions["location"]

      if where_clause:
         results = self.collection.query(
            query_embeddings=query_vec,
            n_results=top_n,
            where=where_clause
         )
      else:
         results = self.collection.query(
            query_embeddings=query_vec,
            n_results=top_n
         )

      documents = results['documents'][0]
      documents = self.filter_jobs(documents, query)

      return "\n---\n".join(documents)

   def add_single_job(self, job_text, job_id):
      """Thêm 1 bài mới ngay lập tức khi nhà tuyển dụng đăng bài"""
      emb = encode([job_text]).tolist()
      self.collection.add(
         ids=[str(job_id)],
         embeddings=emb,
         documents=[job_text]
      )
