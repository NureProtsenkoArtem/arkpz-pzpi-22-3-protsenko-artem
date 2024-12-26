from utils import run_command


#Starts the server by running the specified .NET project.
def run_server():

    print("Starting the server...")
    if not run_command(["task","run_project"], use_shell=True):
        print("Failed to start the server.")
    else:
        print("Server started successfully.")
