import subprocess

# Run the command and capture stdout and stderr with specified encoding
def run_command(command, use_shell=False):
    """Executes a command and returns the result as a boolean."""
    try:
        result = subprocess.run(command, capture_output=True, text=True, shell=use_shell, encoding='cp1251')
        
        if result.returncode != 0:
            print(f"Error running command: {command}\n{result.stderr}")
            return False
        print(result.stdout)
        return True
    except Exception as e:
        print(f"Error executing command: {e}")
        return False