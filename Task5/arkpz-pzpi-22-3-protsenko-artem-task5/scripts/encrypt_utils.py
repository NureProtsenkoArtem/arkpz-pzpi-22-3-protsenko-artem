from dotenv import load_dotenv
import os


#Encrypts or decrypts data using XOR.
def xor_encrypt_decrypt(data, key):
    return bytes([data[i] ^ key[i % len(key)] for i in range(len(data))])

#Encrypts the specified file.
def encrypt_file(file_path, key):
    try:
        key_bytes = key.encode()
        with open(file_path, 'rb') as file:
            data = file.read()
        encrypted_data = xor_encrypt_decrypt(data, key_bytes)
        with open(file_path, 'wb') as file:
            file.write(encrypted_data)
        print(f"File {file_path} encrypted successfully.")
    except Exception as e:
        print(f"Error encrypting file {file_path}: {e}")


#Decrypts the specified file.

def decrypt_file(file_path, key):
    try:
        key_bytes = key
        with open(file_path, 'rb') as file:
            encrypted_data = file.read()
        decrypted_data = xor_encrypt_decrypt(encrypted_data, key_bytes)
        with open(file_path, 'wb') as file:
            file.write(decrypted_data)
        print(f"File {file_path} decrypted successfully.")
    except Exception as e:
        print(f"Error decrypting file {file_path}: {e}")

#Finds the most recent backup file in the specified directory.

def find_latest_backup(directory, extension=".sql"):
    try:
        files = [f for f in os.listdir(directory) if f.endswith(extension)]
        if not files:
            return None
        files.sort(key=lambda x: os.path.getmtime(os.path.join(directory, x)), reverse=True)
        return os.path.join(directory, files[0])
    except Exception as e:
        print(f"Error while finding backup files: {e}")
        return None

if __name__ == "__main__":
    load_dotenv() 
    script_dir = os.path.dirname(os.path.abspath(__file__))
    backup_directory = os.path.join(script_dir, "..\PetHouse.Persistence\Migration")
    backup_directory = os.path.normpath(backup_directory)
    file_path = find_latest_backup(backup_directory)
    key = os.getenv('ENCRYPTION_KEY')
    if not key:
        print("ENCRYPTION_KEY not found in .env file. Exiting.")
        exit()

    action = input("Do you want to (e)ncrypt or (d)ecrypt the file? ").lower()
    if action == 'e':
        encrypt_file(file_path, key)
    elif action == 'd':
        decrypt_file(file_path, key)
    else:
        print("Invalid input. Exiting.")