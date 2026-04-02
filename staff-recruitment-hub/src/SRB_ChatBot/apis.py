import os
import sys

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from fastapi.middleware.cors import CORSMiddleware

sys.path.append(os.path.dirname(os.path.abspath(__file__)) + "/..")

from models.llm import load_model
from rag.knowledge import load_knowledge
from rag.retriever import Retriever
from services.chat_service import ChatService

app = FastAPI(title="Chatbot API")

app.add_middleware(
   CORSMiddleware,
   allow_origins=["http://localhost:8000"],
   allow_credentials=True,
   allow_methods=["*"],
   allow_headers=["*"],
)

retriever = Retriever(knowledge_list=[], db_path="./data/chroma_db")
if retriever.collection.count() == 0:
   knowledge = load_knowledge()
   retriever._add_to_db(knowledge)

llm = load_model()
chat_service = ChatService(llm, retriever)

class ChatRequest(BaseModel):
   message: str

@app.post("/chat")
async def chat_endpoint(request: ChatRequest):
   try:
      response = chat_service.chat(request.message, is_stream = False)

      return {"status": "success", "reply": response}
   except Exception as e:
      raise HTTPException(status_code=500, detail=str(e))

if __name__ == "__main__":
   import uvicorn

   print("Chat Bot Start...")

   uvicorn.run(app, host="0.0.0.0", port=8080)
