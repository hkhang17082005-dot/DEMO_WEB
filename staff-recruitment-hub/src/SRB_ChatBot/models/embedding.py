from sentence_transformers import SentenceTransformer

print("--- Loading Model Vector ---")
embed_model = SentenceTransformer('all-MiniLM-L6-v2')

def encode(texts):
   return embed_model.encode(texts, normalize_embeddings=True)
