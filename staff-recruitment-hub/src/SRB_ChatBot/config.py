import os
import logging

os.environ["TOKENIZERS_PARALLELISM"] = "false"
os.environ["HF_HUB_DISABLE_SYMLINKS_WARNING"] = "1"
os.environ["GGML_METAL_LOG_LEVEL"] = "0"

logging.getLogger("transformers").setLevel(logging.ERROR)

MAX_HISTORY = 1500

MODEL_REPO = "Qwen/Qwen2.5-1.5B-Instruct-GGUF"
MODEL_FILE = "qwen2.5-1.5b-instruct-q4_k_m.gguf"
