#  Employee Management System API  

## 📌 Overview  
The **Employee Management System API** is designed to streamline employee management, timesheet tracking, and leave management. It supports two main user roles: **Employees** and **Admins**, ensuring efficient workforce management.  

---

## 🚀 Functional Requirements  

### 🔹 1. User Roles  
The system includes two user roles with distinct permissions:  
- **Employee**: Can log work hours, update profile details, and request leave.  
- **Admin**: Manages employees, views reports, and exports timesheet data.  

### 🔹 2. Employee Details  
Each employee profile contains the following attributes:  
- **Employee ID** (Unique Identifier)  
- **First Name**  
- **Last Name**  
- **Email**  
- **Phone Number**  
- **Department**  
- **Tech Stack** (Skills & Technologies known)  
- **Date of Birth** *(Optional)*  
- **Address** *(Optional)*  

### 🔹 3. Timesheet Management  
Employees can log their working hours daily, with each entry including:  
- **Date**  
- **Start Time**  
- **End Time**  
- **Total Hours Worked**  
- **Description of Work Done** *(Optional)*  
Employees can also view and edit their timesheets as needed.  

### 🔹 4. Admin Functionalities  
Administrators have the following capabilities:  
- Log in using email and password.  
- View all employees and their timesheets.  
- Export timesheet data to Excel for reporting.  
- Manage employee profiles (edit details, activate/deactivate accounts).  

### 🔹 5. Profile Management  
- Employees have a **profile page** displaying their details.  
- They can update their **phone number, tech stack, and address**.  
- Password reset functionality is available.  

### 🔹 6. Leave Management *(Additional Feature)*  
Employees can request leave by specifying:  
- **Start Date & End Date**  
- **Type of Leave** (Sick Leave, Casual Leave, Vacation, etc.)  
- **Reason** *(Optional)*  

### 🔹 7. Reports & Analytics *(Additional Feature)*  
Admins can generate reports for:  
- **Employee Work Hours** (weekly, monthly).  

### 🔹 8. Authentication & Security  
- Secure login for employees and admins using **email & password**.  
- Passwords are stored securely using **hashing**.  
- Users can reset their passwords via **email verification**.  

---

## 📌 API Endpoints  

## 🚀 API Endpoints  

### 🔹 Admin APIs  

| Method  | Endpoint                          | Description                   |
|---------|-----------------------------------|-------------------------------|
| **POST**   | `/api/admin/create-admin`        | Create a new admin            |
| **GET**    | `/api/admin/all-admins`         | Retrieve all admins           |
| **GET**    | `/api/admin/admin/{id}`         | Retrieve admin by ID          |
| **PUT**    | `/api/admin/update/{id}`        | Update admin details          |
| **DELETE** | `/api/admin/delete/{id}`        | Delete an admin               |
| **PUT**    | `/api/admin/restore/{id}`       | Restore a deleted admin       |

---

### 🔹 Admin Authentication APIs  

| Method  | Endpoint                   | Description                  |
|---------|----------------------------|------------------------------|
| **POST**   | `/api/AdminAuth/register` | Register a new admin        |
| **POST**   | `/api/AdminAuth/login`    | Login as admin              |

---

### 🔹 Department APIs  

| Method  | Endpoint                                  | Description                     |
|---------|-------------------------------------------|---------------------------------|
| **GET**    | `/api/Department/GetAllDepartment`       | Retrieve all departments        |
| **GET**    | `/api/Department/GeDepartmenttById/{id}` | Retrieve department by ID       |
| **POST**   | `/api/Department/CreateDepartment`       | Create a new department         |
| **PUT**    | `/api/Department/UpdateDepartment/{id}`  | Update department details       |
| **DELETE** | `/api/Department/DeleteDepartment/{id}`  | Delete a department            |

---

### 🔹 Employee APIs  

| Method  | Endpoint                               | Description                   |
|---------|----------------------------------------|-------------------------------|
| **POST**   | `/api/Employee/CreateEmployee`        | Create a new employee        |
| **GET**    | `/api/Employee/GetEmployeeById/{id}`  | Retrieve employee by ID      |
| **PUT**    | `/api/Employee/UpdateEmployee/{id}`   | Update employee details      |
| **DELETE** | `/api/Employee/DeleteEmployee/{id}`   | Delete an employee           |
| **GET**    | `/api/Employee/GetAllEmployees`       | Retrieve all employees       |
| **PUT**    | `/api/Employee/RestoreEmployee/{id}`  | Restore a deleted employee   |
| **POST**   | `/api/Employee/send`                 | Password Reset and Verification  |

---

### 🔹 Employee Authentication APIs  

| Method  | Endpoint             | Description              |
|---------|----------------------|--------------------------|
| **POST**   | `/api/auth/register` | Register a new employee |
| **POST**   | `/api/auth/login`    | Login as an employee    |

---

### 🔹 Leave Management APIs  

| Method  | Endpoint                                  | Description                      |
|---------|-------------------------------------------|----------------------------------|
| **POST**   | `/api/leaves/ApplyForLeave`             | Apply for leave                  |
| **GET**    | `/api/leaves/GetLeaveById/{id}`        | Retrieve leave details by ID     |
| **PUT**    | `/api/leaves/UpdateLeaveStatus/{id}`   | Update leave status              |
| **DELETE** | `/api/leaves/DeleteLeave/{id}`         | Delete a leave request           |
| **GET**    | `/api/leaves/GetPendingLeaves`         | Retrieve all pending leave       |
| **GET**    | `/api/leaves/GetLeavesByEmployee/{employeeId}` | Retrieve leave by employee ID  |

---

### 🔹 Timesheet Management APIs  

| Method  | Endpoint                                             | Description                         |
|---------|------------------------------------------------------|-------------------------------------|
| **POST**   | `/api/timesheets/CreateTimesheet`                 | Create a new timesheet entry      |
| **GET**    | `/api/timesheets/GetTimesheetById/{id}`          | Retrieve timesheet by ID          |
| **GET**    | `/api/timesheets/GetTimesheetsByEmployeeId/{employeeId}` | Retrieve timesheets by employee ID |
| **PUT**    | `/api/timesheets/UpdateTimesheet/{id}`          | Update a timesheet entry         |
| **DELETE** | `/api/timesheets/DeleteTimesheet/{id}`          | Delete a timesheet entry         |
| **GET**    | `/api/timesheets/ExportTimesheets`              | Export timesheets to Excel       |

---

## 📌 Installation & Setup  

Here’s the **Markdown** file for your **Setup Instructions** so you can directly paste it:  


## 🛠️ Setup Instructions

### Clone the repository:
```sh
git clone https://github.com/Fatema-MithaiwalaB/EmployeeManagementSystem.git
```

### Navigate to the project directory:
```sh
cd EmployeeManagementSystem
```

### Restore dependencies:
```sh
dotnet restore
```

### Configure the database:
Modify `appsettings.json` to configure the database connection.

### Run database migrations:
```sh
dotnet ef database update
```

### Start the API:
```sh
dotnet run
```

### Access Swagger UI:
```
https://localhost:7278/swagger/index.html
```
