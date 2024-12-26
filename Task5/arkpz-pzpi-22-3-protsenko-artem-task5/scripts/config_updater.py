import json


#Updates the appsettings.Development.json file with the given database credentials.
def update_appsettings(file_path, db_username, db_password, db_name="pethousedb", host="localhost", port=5432):
    try:
        with open(file_path, "r") as file:
            config = json.load(file)
        
        connection_string = (
            f"Host={host};Port={port};Database={db_name};"
            f"Username={db_username};Password={db_password}"
        )
        config["ConnectionStrings"]["PetHouseDbContext"] = connection_string

        with open(file_path, "w") as file:
            json.dump(config, file, indent=4)
        
        print(f"Updated {file_path} successfully.")
    except FileNotFoundError:
        print(f"Configuration file {file_path} not found.")
    except json.JSONDecodeError:
        print(f"Error decoding JSON in {file_path}.")
    except Exception as e:
        print(f"An error occurred while updating {file_path}: {e}")
