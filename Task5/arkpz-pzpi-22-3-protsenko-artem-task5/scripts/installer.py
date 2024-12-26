import os
import subprocess
import shutil

# Executes a command in the shell and captures its output and errors
def run_command(command):
    try:
        print(f"Executing command: {' '.join(command)}")
        subprocess.run(command, shell=True, check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        return True
    except FileNotFoundError:
        print(f"FileNotFoundError: Command not found - {' '.join(command)}")
        return False
    except subprocess.CalledProcessError as e:
        print(f"Command failed: {e}")
        return False
    
# Adds a directory to the system PATH environment variable
def add_to_path(directory):
    """Adds a directory to the system PATH environment variable."""
    if directory not in os.environ["PATH"]:
        os.environ["PATH"] += os.pathsep + directory
        print(f"Added {directory} to PATH.")
    else:
        print(f"{directory} is already in PATH.")


# Checks if Chocolatey is installed, and installs it if missing
def check_and_install_chocolatey():
    """Checks if Chocolatey is installed, and installs it if missing."""
    print("Checking for Chocolatey...")
    if not shutil.which("choco"):
        print("Chocolatey is not installed. Installing...")
        install_command = (
            'powershell -NoProfile -ExecutionPolicy Bypass '
            '-Command "Set-ExecutionPolicy Bypass -Scope Process; '
            '[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; '
            'iex ((New-Object System.Net.WebClient).DownloadString(\'https://community.chocolatey.org/install.ps1\'))"'
        )
        if run_command(install_command, shell=True):
            print("Chocolatey installed successfully.")
        else:
            print("Failed to install Chocolatey. Please install it manually.")
            return False
    else:
        print("Chocolatey is already installed.")
    return True

# Checks if PostgreSQL is installed, installs it if missing, and configures a default user
def check_and_install_postgresql():
    """Checks if PostgreSQL is installed, installs it if missing, and configures a default user."""
    print("Checking for PostgreSQL...")
    if not shutil.which("psql"):
        print("PostgreSQL is not installed. Installing...")
        if run_command(["choco", "install", "postgresql", "-y"]):
            print("PostgreSQL installed successfully.")
        else:
            print("Failed to install PostgreSQL. Please install it manually.")
            return
    else:
        print("PostgreSQL is already installed.")

    default_username = "postgres"
    default_password = "root"
    os.environ['PGPASSWORD'] = default_password
    
    create_user_command = f"""
    DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = '{default_username}') THEN
            CREATE ROLE {default_username} WITH LOGIN PASSWORD '{default_password}';
        END IF;
    END $$;
    """
    
    print(f"Checking if user '{default_username}' exists...")
    if not run_command(["psql", "-U", "postgres", "-c", create_user_command]):
        print(f"Failed to create user '{default_username}'. Ensure you have superuser access.")
    else:
        print(f"User '{default_username}' exists or has been created successfully.")
    
    print(f"Granting CREATEDB privilege to user '{default_username}'...")
    grant_privilege_command = f"ALTER ROLE {default_username} CREATEDB;"
    if not run_command(["psql", "-U", "postgres", "-c", grant_privilege_command]):
        print(f"Failed to grant CREATEDB privilege to user '{default_username}'. Ensure you have superuser access.")
    else:
        print(f"CREATEDB privilege granted to user '{default_username}'.")

# Checks if .NET SDK is installed, installs it if missing, and ensures it is in PATH
def check_and_install_dotnet():
    """Checks if .NET SDK is installed, installs it if missing, and ensures it is in PATH."""
    print("Checking for .NET SDK...")

    if not run_command(["dotnet","--version"]):
        print(".NET SDK is not installed. Installing...")

        if run_command(["choco", "install", "dotnet-8.0-sdk", "--version=8.0.404", "-y"]):
            print(".NET SDK installed successfully.")
        else:
            print("Failed to install .NET SDK. Please install it manually.")
            return
    else:
        print(".NET SDK is already installed.")

    dotnet_path = "C:\\Program Files\\dotnet"
    if os.path.isdir(dotnet_path):
        add_to_path(dotnet_path)
    else:
        print(f"Directory {dotnet_path} does not exist. Ensure .NET SDK is installed correctly.")

# Checks if Task is installed, installs it if missing, and adds it to PATH
def check_and_install_task():
    if not run_command(["task", "--version"]):
        print("Task command not found, installing...")
        subprocess.run(["choco", "install", "go-task", "-y"], check=True)
        taskfile_path = os.path.join(os.environ.get('TASKPROFILE', ''), 'taskfile', 'bin')
            
        if taskfile_path and taskfile_path not in os.environ['PATH']:
            os.environ['PATH'] += os.pathsep + taskfile_path
            print(f"Added {taskfile_path} to PATH.")
            print("Task installed successfully.")
        else:
            print("Failed to install Task using Chocolatey.")
    else:
        print("Task is already installed.")
    