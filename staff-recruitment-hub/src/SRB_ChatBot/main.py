import os
import sys

sys.path.append(os.path.dirname(os.path.abspath(__file__)) + "/..")

from models.llm import load_model
from rag.knowledge import load_knowledge
from rag.retriever import Retriever
from services.chat_service import ChatService

def main():
   print("\n[System]: Chatbot đang khởi động...")

   retriever = Retriever(knowledge_list=[], db_path="./data/chroma_db")

   if retriever.collection.count() == 0:
      knowledge = load_knowledge()

      retriever._add_to_db(knowledge)
   else:
      print(f"[System]: Đã nạp {retriever.collection.count()} bài tuyển dụng từ Vector DB!")

   llm = load_model()
   chat_service = ChatService(llm, retriever)

   while True:
      user_input = input("Người dùng: ")

      if user_input.lower() in ["exit", "quit", "thoát"]:
         break

      chat_service.chat(user_input)

if __name__ == "__main__":
   main()
