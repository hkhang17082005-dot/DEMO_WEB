import pyodbc

def get_db_connection():
   conn_str = (
      "Driver={ODBC Driver 17 for SQL Server};"
      "Server=localhost;"
      "Database=srb_project_v3;"
      "UID=SA;"
      "PWD=Anhhung1@;"
   )
   return pyodbc.connect(conn_str)
