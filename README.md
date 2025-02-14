# 🚗 DVLD Application

A comprehensive desktop application designed to manage the core functionalities of the Driver and Vehicle Licensing Directorate.

## ✨ Features

- **People Management**: 🧑‍🤝‍🧑 CRUD operations for individuals.
- **Driver Management**: 🚘 Handle driver-specific information and licensing details.
- **User Management**: 👥 Manage system users with role-based access.
- **Application Processing**: 📄 Submit and track applications for licenses.
- **Test Management**: 📝 Schedule and record results for driving tests.
- **License Issuance**: 🪪 Generate and manage driver licenses.

## 🛠️ Technologies Used

- **Programming Language**: 💻 C#
- **Framework**: 🏗️ .NET Framework 4.7.2
- **Architecture**: 🏛️ 3-Tier (Presentation, Business Logic, Data Access)
- **Database**: 🗄️ SQL Server
- **Data Access**: 🔗 ADO.NET
- **IDE**: 🖥️ Visual Studio 2022

## 🗃️ Database Setup

1. **Restore from Backup (`.bak`):**
   - 📥 Download `DVLD.bak` from the `Database/` folder.
   - 📂 Place it in a preferred location (e.g., `C:\`).
   - 🛠️ Open **SQL Server Management Studio (SSMS)**.
   - 📑 Right-click on **Databases** → **Restore Database**.
   - 📌 Select **Device** → **Browse** → Choose `DVLD.bak`.
   - ✅ Click **OK** to restore.

2. **Update Connection String in `App.config`:**
   - 📝 Open `App.config` in the **Presentation Layer** project.
   - 🔧 Modify the connection string to match your SQL Server configuration:

     ```xml
     <connectionStrings>
         <add name="DVLD"
              connectionString="Server=YOUR_SERVER_NAME;Database=DVLD;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;"
              providerName="System.Data.SqlClient"/>
     </connectionStrings>
     ```
