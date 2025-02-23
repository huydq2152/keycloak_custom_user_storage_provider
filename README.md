## Quick Start

### Setup Local Keycloak with Custom Provider in Windows

This section provides a concise guide to setting up a local Keycloak instance and deploying a custom provider. For detailed explanations, refer to the Keycloak documentation.

**Steps:**

1.  **Install Keycloak:** Download the Keycloak ZIP distribution from [https://www.keycloak.org/getting-started/getting-started-zip](https://www.keycloak.org/getting-started/getting-started-zip) and extract it.

2.  **Install JDK 17:** Download and install Java SE Development Kit 21 (JDK 21, _not_ just the JRE).

3.  **Set Environment Variables (Windows):**

    - `JAVA_HOME`: Set to your JDK 21 installation directory (e.g., `C:\Program Files\Java\jdk-21`). This directory _must_ contain `bin`, `lib`, etc.
    - `PATH`: Add `%JAVA_HOME%\bin` to your `PATH` environment variable. It's best to place this at the _beginning_ of the `PATH` to prioritize this JDK.

4.  **Build Custom Provider (JAR):** Use Gradle to build your custom provider project. The resulting JAR file will be in the `build/libs` directory.

5.  **Deploy Provider:** Copy the JAR file to the `providers` directory within your Keycloak installation.

6.  **Restart & Rebuild Keycloak:**

    - **Windows:**
      ```bash
      cd <keycloak_installation_dir>\bin
      kc.bat build
      kc.bat start-dev
      ```

    Replace `<keycloak_installation_dir>` with the actual path. `start-dev` is for development; use `start` for production (requires more configuration).

7.  **Verify:** Access the Keycloak Admin Console (usually `http://localhost:8080/admin`) and check if your provider is available in User Federation menu.

**Important Notes:**

- These instructions assume a Windows environment. Adjust paths and commands accordingly for other operating systems.
- This is a _development_ setup. For production, refer to the Keycloak documentation for proper configuration (database, HTTPS, etc.).
- Ensure your `JAVA_HOME` and `PATH` are set correctly, and open a _new_ terminal/command prompt after making changes.
- If using an IDE, ensure it's also configured to use JDK 17.

### Configuration Keycloak Storage Database Using MSSQL

1. **Create Database:**

   - Open SQL Server Management Studio (SSMS) and create a new database named `keycloak`.

2. **Edit Keycloak Configuration:**

   - Open the `keycloak.conf` file located in the `conf` folder of your local Keycloak installation.
   - Change the following configurations:
     - **Database Vendor:** `db=mssql`
     - **Database Username:** `username` (replace with your SQL Server username)
     - **Database Password:** `password` (replace with your SQL Server password)
     - **JDBC URL:**
       ```plaintext
       db-url=jdbc:sqlserver://<your_sql_server_hostname>:<your_sql_server_port>;databaseName=keycloak;encrypt=false;trustServerCertificate=true;
       ```
     - **Disable Distributed Transaction:** `transaction-xa-enabled=false`

3. **Enable Transaction Recovery:**

   - Create a new file named `quarkus.properties` in the `conf` folder.
   - Add the following line to enable the transaction recovery feature:
     ```plaintext
     quarkus.transaction-manager.enable-recovery=true
     ```

4. **Enable TCP/IP Protocol:**

   - Open SQL Server Configuration Manager.
   - Navigate to `SQL Server Network Configuration` -> `Protocols for MSSQLSERVER`.
   - Enable the `TCP/IP` protocol.

5. **Restart & Rebuild Keycloak:**

   - **Windows:**
     ```bash
     cd <keycloak_installation_dir>\bin
     kc.bat build --db=mssql
     kc.bat start-dev
     ```

   Replace `<keycloak_installation_dir>` with the actual path. `start-dev` is for development; use `start` for production (requires more configuration).

**References:**

- [YouTube: Keycloak MSSQL Configuration](https://www.youtube.com/watch?v=QeTLzVEchk4)
- [Stack Overflow: JDBC Connection Failed Error](https://stackoverflow.com/questions/18841744/jdbc-connection-failed-error-tcp-ip-connection-to-host-failed)

### Reference Documentation

For further reference, please consider the following sections:

- [Official Gradle documentation](https://docs.gradle.org)
- [Spring Boot Gradle Plugin Reference Guide](https://docs.spring.io/spring-boot/docs/3.2.6/gradle-plugin/reference/html/)
- [Create an OCI image](https://docs.spring.io/spring-boot/docs/3.2.6/gradle-plugin/reference/html/#build-image)

### Additional Links

These additional references should also help you:

- [Gradle Build Scans – insights for your project's build](https://scans.gradle.com#gradle)
