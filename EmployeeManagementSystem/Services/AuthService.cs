using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IEmployeeRepository employeeRepository, JwtTokenGenerator jwtTokenGenerator)
    {
        _employeeRepository = employeeRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<EmployeeResponseDTO> Register(EmployeeRegisterDTO dto)
    {
        if (await _employeeRepository.GetByEmailAsync(dto.Email) != null)
            throw new Exception("Email already in use");

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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            isDeleted = false
        };

        await _employeeRepository.AddAsync(employee);
        return new EmployeeResponseDTO
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            DepartmentId = employee.DepartmentId,
            RoleId = employee.RoleId
        };
    }

    public async Task<EmployeeResponseDTO> Login(EmployeeLoginDTO dto)
    {
        var employee = await _employeeRepository.GetByEmailAsync(dto.Email);
        if (employee == null || !BCrypt.Net.BCrypt.Verify(dto.Password, employee.PasswordHash))
            throw new Exception("Invalid credentials");

        var token = _jwtTokenGenerator.GenerateToken(employee.EmployeeId, employee.Email, employee.RoleId);
        return new EmployeeResponseDTO
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Token = token
        };
    }
}
