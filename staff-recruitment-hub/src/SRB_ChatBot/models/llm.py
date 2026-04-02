from llama_cpp import Llama
from huggingface_hub import hf_hub_download

from SRB_ChatBot.config import MODEL_REPO, MODEL_FILE

def load_model():
   print(f"--- Loading model: {MODEL_FILE} ---")

   model_path = hf_hub_download(
      repo_id=MODEL_REPO,
      filename=MODEL_FILE
   )

   print("--- Loading LLM ---")

   return Llama(
      model_path=model_path,
      n_gpu_layers=-1,
      n_ctx=4096,
      verbose=False
   )
