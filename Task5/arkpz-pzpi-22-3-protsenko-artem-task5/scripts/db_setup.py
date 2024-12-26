import subprocess
import os
import sys


#Sets up the database by creating it and restoring it from a backup file.
def setup_database(backup_file, db_name="pethouseDB", username="postgres", password="root"):
    print("Setting up the database...")

    os.environ["PGPASSWORD"] = password 

    try:
        check_db_command = ["psql", "-U", username, "-lqt"]
        check_db_result = subprocess.run(check_db_command, capture_output=True, text=True, check=True, shell=True)
        if db_name in check_db_result.stdout:
            print(f"Database {db_name} already exists.")
        else:
            create_db_command = ["createdb", "-U", username, db_name]
            subprocess.run(create_db_command, capture_output=True, text=True, check=True, shell=True)
            print(f"Database {db_name} created successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Failed to check or create the database {db_name}. Error: {e.stderr}")
        return

    try:
        restore_db_command = ["pg_restore", "-U", username, "-d", db_name, backup_file]
        restore_db_result = subprocess.run(restore_db_command, capture_output=True, text=True, check=True, shell=True)
        print("Database restored from backup successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Failed to restore the database from {backup_file}.")
        print(f"stdout: {e.stdout}")
        print(f"stderr: {e.stderr}")
        return
    
    print("Database setup completed successfully.")
    return True
