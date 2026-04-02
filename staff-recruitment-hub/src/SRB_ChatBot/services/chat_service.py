import re

from utils.prompt import build_prompt

class ChatService:
   def __init__(self, llm, retriever, max_history=1500):
      self.llm = llm
      self.retriever = retriever
      self.chat_history = ""
      self.max_history = max_history

   def chat(self, user_input, is_stream=True):
      context = self.retriever.get_context(user_input)
      prompt = build_prompt(context, self.chat_history, user_input)

      response = ""
      chunks = self.llm(
         prompt,
         max_tokens=512,
         stop=["<|im_end|>"],
         temperature=0.3,
         top_p=0.9,
         repeat_penalty=1.2,
         stream=True,
      )

      for chunk in chunks:
         token = chunk["choices"][0]["text"]
         response += token

         if is_stream:
               print(token, end="", flush=True)

      if is_stream:
         print() # Xuống dòng cho CLI

      self._update_history(user_input, response)

      return response

   def _update_history(self, user, bot):
      self.chat_history += f"<|im_start|>user\n{user}<|im_end|>\n<|im_start|>assistant\n{bot}<|im_end|>\n"

      if len(self.chat_history) > self.max_history:
         self.chat_history = self.chat_history[-self.max_history:]

def normalize_salary(salary_text):
   salary_text = salary_text.lower().replace(",", "").replace(".", "")

   if any(x in salary_text for x in ["thoả thuận", "thỏa thuận", "negotiable"]):
      return None, None

   numbers = list(map(int, re.findall(r"\d+", salary_text)))

   if not numbers:
      return None, None

   if "triệu" in salary_text:
      numbers = [n * 1_000_000 for n in numbers]

   if "usd" in salary_text:
      numbers = [n * 24000 for n in numbers]

   if len(numbers) == 1:
      return numbers[0], numbers[0]

   return min(numbers), max(numbers)

def parse_query(query):
   query = query.lower()

   location = None
   salary_min = None

   # Location
   if any(x in query for x in ["hồ chí minh", "hcm", "sài gòn"]):
      location = "Hồ Chí Minh"
   elif any(x in query for x in ["hà nội", "thủ đô", "Hà Nội"]):
        location = "Hà Nội"

   # Salary
   match = re.search(r"(trên|>=|từ)\s*(\d+)", query)
   if match:
      salary_min = int(match.group(2))

   match2 = re.search(r"(\d+)\s*-\s*(\d+)", query)
   if match2:
      salary_min = int(match2.group(1))

   return {
      "location": location,
      "salary_min": salary_min
   }

def parse_job(doc):
   data = {}

   for line in doc.split("\n"):
      line = line.strip()

      if line.lower().startswith("Địa điểm:"):
         data["location"] = line.split(":", 1)[-1].strip()

      if line.startswith("Mức lương:"):
         data["salary"] = line.split(":", 1)[-1].strip()

   return data
