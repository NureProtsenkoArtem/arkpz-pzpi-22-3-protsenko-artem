import os
from installer import check_and_install_postgresql, check_and_install_dotnet, check_and_install_chocolatey, check_and_install_task
from db_setup import setup_database
from server_runner import run_server
from config_updater import update_appsettings
from encrypt_utils import encrypt_file, decrypt_file
from dotenv import load_dotenv
import getpass

#Finds the most recent backup file in a specified directory.
def find_latest_backup(directory, extension=".sql"):
    """
    Finds the most recent backup file in the specified directory.

    Args:
        directory (str): Path to the directory containing backup files.
        extension (str): File extension to filter by (default is ".sql").

    Returns:
        str: Path to the most recent backup file, or None if no file is found.
    """
    try:
        files = [f for f in os.listdir(directory) if f.endswith(extension)]
        if not files:
            return None
        files.sort(key=lambda x: os.path.getmtime(os.path.join(directory, x)), reverse=True)
        return os.path.join(directory, files[0])
    except Exception as e:
        print(f"Error while finding backup files: {e}")
        return None
    
# Main function to deploy the server.
def main():
    """Main function to deploy the server."""
    print("Checking prerequisites...")

    load_dotenv()
    encryption_key = os.getenv("ENCRYPTION_KEY")

    if not encryption_key:
        print("Encryption key not found in .env file. Exiting.")
        return

    encryption_key = encryption_key.encode()

    check_and_install_chocolatey()
    check_and_install_postgresql()
    check_and_install_dotnet()
    check_and_install_task()
    
    db_name = "pethouseDB"

    script_dir = os.path.dirname(os.path.abspath(__file__))
    backup_directory = os.path.join(script_dir, "..\\PetHouse.Persistence\\Migration") 

    backup_directory = os.path.normpath(backup_directory)
    backup_file = find_latest_backup(backup_directory)
    if not backup_file:
        print(f"No backup files found in {backup_directory}!")
        return
    print(f"Using backup file: {backup_file}")

    user_key = getpass.getpass("Enter the encryption key to decrypt the migration file: ").encode()

    if user_key == encryption_key:
        print("Key is correct. Decrypting the file...")
        decrypt_file(backup_file, encryption_key)
    else:
        print("Invalid key. Exiting.")
        return

    if not setup_database(backup_file, db_name=db_name, username="postgres", password="root"):
        print("Failed to configure the database. Exiting.")
        return

    encrypt_file(backup_file, encryption_key)

    appsettings_path = os.path.join(script_dir, "../PetHouse.API/appsettings.Development.json")
    print(appsettings_path)
    if os.path.exists(appsettings_path):
        update_appsettings(appsettings_path, "postgres", "root", db_name=db_name)
    else:
        print(f"Configuration file {appsettings_path} not found. Skipping configuration update.")

    print("Server deployment completed successfully!")

if __name__ == "__main__":
    main()
