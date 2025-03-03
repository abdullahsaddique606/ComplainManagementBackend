# Complaint Management System

A web-based complaint management system built with .NET that allows users to register, submit complaints, and track their status. Administrators can manage complaints efficiently usingcommunication tools.

## Features

### 1. User Registration and Login
- Users can create an account and log in.
- A simple and minimal registration process ensures a smooth user experience.

### 2. Complaint Creation Functionality
- Users can submit complaints through a form.
- The form includes fields for a short description, category selection, and other necessary details.

### 3. Complaint Listing and Closing Functionality
- Users can view a list of submitted complaints.
- Users can close a complaint once it is resolved, ensuring better tracking.

### 4. Communication Between Users and Administrators
- Email notifications are used to keep users and administrators informed.
- A notification system ensures effective communication.

### 5. Complaint Management for Administrators
- Admins can view, sort, and filter complaints based on parameters like status or category.
- An intuitive interface makes it easy for administrators to manage complaints.

## Tech Stack
- **Backend:** .NET Core / .NET 6+
- **Database:** SQL Server 
- **Authentication:** Identity Framework / JWT Auth

## Installation

### 1. Clone the Repository
```sh
git clone https://github.com/yourusername/complaint-management.git
cd complaint-management
```

### 2. Setup Database
- Update `appsettings.json` with your database connection string.
- Run migrations:
```sh
dotnet ef database update
```

### 3. Run the Application
```sh
dotnet run
```

### 4. Access the Application
Open your browser and navigate to `http://localhost:5000` or the assigned port.

## Contribution
- Fork the repository.
- Create a new branch (`feature/your-feature`).
- Commit changes and push to GitHub.
- Create a Pull Request.

## License
This project is licensed under the MIT License.

## Contact
For any inquiries, please reach out to **abdullahsaddique6061@gmail.com**.
