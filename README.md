# cmcs_
 POE PART 2
 
# Claims Management System

## Overview
The Claims Management System (CMS) is a web application developed to streamline the process of managing claims. It provides an intuitive interface for users to submit claims, upload supporting documents, and track their status. The application supports role-based access control, ensuring that only authorized users can verify and manage claims. Built using ASP.NET Core MVC, this application is designed for scalability and ease of use.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Directory Structure](#directory-structure)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features
- **User Authentication**: Secure login system with role-based access for different user types (Lecturer, Coordinator, Manager).
- **Claim Submission**: Users can submit claims with necessary details and upload supporting documents.
- **Claim Tracking**: View the status of submitted claims and any related actions.
- **Claims Verification**: Authorized users can verify, approve, or reject claims, ensuring a streamlined management process.
- **File Uploads**: Supports secure file uploads with validation for accepted file types.
- **Responsive Design**: Built with Bootstrap to ensure compatibility across various devices.

## Technologies Used
- **ASP.NET Core MVC**: Framework for building the web application.
- **Entity Framework Core**: Future integration for database management.
- **Microsoft SQL Server**: Database management system (for when database integration is implemented).
- **HTML/CSS/JavaScript**: Frontend technologies for building the user interface.
- **Bootstrap**: CSS framework for responsive design.
- **jQuery Validation**: Client-side validation for forms.

## Prerequisites
Before running the application, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 5.0 or later).
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/).
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (if using a database).

## Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/ClaimsManagementSystem.git
   cd ClaimsManagementSystem
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run database migrations** (if using a database):
   ```bash
   dotnet ef database update
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```

5. **Open your web browser** and navigate to `https://localhost:5001` (or `http://localhost:5000`).

## Configuration
- Update the **connection string** in the `appsettings.json` file for database connectivity:
  ```json
  "ConnectionStrings": {
      "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
  }
  ```
- Configure your **authentication settings** in the same file as needed.

## Usage
1. **Login**: Use the predefined credentials for various roles:
   - **Lecturer**: `john.doe@example.com` / `password1`
   - **Coordinator**: `harry.brodersen@example.com` / `password2`
   - **Manager**: `manager@example.com` / `password3`

2. **Submit Claim**: Navigate to the "Submit Claim" page, complete the form, and upload supporting documents.

3. **Track Claim**: Go to the "Track Claim" page to view the status of your submitted claims.

4. **Verify Claims**: Coordinators and Managers can access the "Verify Claims" page to manage claims.

5. **Logout**: Click on the logout option to securely end your session.

## Directory Structure Summary
```
ClaimsManagementSystem/
├── Controllers/
│   ├── ClaimsController.cs                # Manages claims operations
│   └── HomeController.cs                  # Manages home and authentication operations
├── Models/
│   ├── Claim.cs                           # Represents a claim
│   ├── ClaimDocument.cs                   # Represents a document related to a claim
│   └── Person.cs                           # Represents a user in the system
├── Services/
│   ├── IClaimRepository.cs                 # Interface for claim repository operations
│   ├── ClaimRepository.cs                  # Implementation of claim repository for data management
│   ├── IClaimService.cs                    # Interface for claim service operations
│   └── ClaimService.cs                     # Implementation of claim service
├── Views/
│   ├── Claims/
│   │   ├── SubmitClaim.cshtml             # View for submitting a new claim
│   │   ├── UploadSupportingDocuments.cshtml# View for uploading supporting documents
│   │   ├── TrackClaim.cshtml               # View for tracking claims status
│   │   ├── VerifyClaims.cshtml             # View for verifying claims (authorized users only)
│   │   └── Success.cshtml                  # View for success message after submission
│   └── Home/
│       ├── Index.cshtml                   # Welcome page view
│       ├── Privacy.cshtml                 # Privacy policy view
│       └── Login.cshtml                   # Login page view
├── wwwroot/
│   ├── css/
│   │   └── site.css                       # Custom styles
│   ├── js/
│   │   └── site.js                        # Custom scripts
│   └── images/
│       └── logo.png                       # Application logo
├── appsettings.json                        # Configuration file for application settings
├── Program.cs                             # Entry point for the application
└── Startup.cs                             # Configures services and the app's request pipeline
```

## Testing
- You can run unit tests and integration tests (if any) using the following command:
  ```bash
  dotnet test
  ```

## Contributing
Contributions are welcome! To contribute to this project:
1. **Fork the repository**.
2. **Create your feature branch** (`git checkout -b feature/YourFeature`).
3. **Commit your changes** (`git commit -m 'Add some feature'`).
4. **Push to the branch** (`git push origin feature/YourFeature`).
5. **Open a pull request**.

