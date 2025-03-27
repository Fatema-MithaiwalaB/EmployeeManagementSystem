using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using System.Security.Claims;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IEmployeeRepository employeeRepository, IAdminRepository adminRepository, JwtTokenGenerator jwtTokenGenerator, ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _adminRepository = adminRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<string> CreateEmployeeAsync(EmployeeRegisterDTO dto)
    {
        var newEmployee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Phone = dto.Phone,
            TechStack = dto.TechStack,
            Address = dto.Address,
            DepartmentId = dto.DepartmentId,
            RoleId = 1,
            isDeleted = false
        };
        await _employeeRepository.AddAsync(newEmployee);
        await _employeeRepository.SaveChangesAsync();
        return "Employee registered successfully";
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id, ClaimsPrincipal user)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null || employee.isDeleted)
            return null;

        var loggedInUserId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        var userRole = user.FindFirstValue(ClaimTypes.Role);
        if (userRole != "Admin" && loggedInUserId != id)
            throw new UnauthorizedAccessException();

        return employee;
    }

    public async Task<string> UpdateEmployeeAsync(int id, EmployeeRegisterDTO dto, ClaimsPrincipal user)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null || employee.isDeleted)
            return "Employee not found";

        var loggedInUserId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        var userRole = user.FindFirstValue(ClaimTypes.Role);
        if (userRole != "Admin" && loggedInUserId != id)
            throw new UnauthorizedAccessException();

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.TechStack = dto.TechStack;
        employee.Address = dto.Address;
        employee.DepartmentId = dto.DepartmentId;

        await _employeeRepository.UpdateAsync(employee);
        return "Employee updated successfully";
    }

    public async Task<string> DeleteEmployeeAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null || employee.isDeleted)
            return "Employee not found";

        employee.isDeleted = true;
        await _employeeRepository.UpdateAsync(employee);
        return "Employee deleted successfully";
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _employeeRepository.GetAllActiveAsync();
    }

    public async Task<string> RegisterAsync(EmployeeRegisterDTO dto)
    {
        if (await _employeeRepository.GetByEmailAsync(dto.Email) != null)
            return "Email already in use";

        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            TechStack = dto.TechStack,
            Address = dto.Address,
            DepartmentId = dto.DepartmentId,
            RoleId = dto.RoleId,
            PasswordHash = PasswordHasher.HashPassword(dto.Password)
        };
        await _employeeRepository.AddAsync(employee);
        return "Employee registered successfully";
    }

    public async Task<EmployeeResponseDTO?> LoginAsync(EmployeeLoginDTO dto)
    {
        var employee = await _employeeRepository.GetByEmailAsync(dto.Email);
        if (employee == null || !PasswordHasher.VerifyPassword(dto.Password, employee.PasswordHash))
            return null;

        var token = _jwtTokenGenerator.GenerateToken(employee.EmployeeId, employee.Email, employee.RoleId);
        return new EmployeeResponseDTO
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            DepartmentId = employee.DepartmentId,
            RoleId = employee.RoleId,
            Token = token
        };
    }
}