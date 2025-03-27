using EmployeeManagementSystem.DTOs;

public interface IAuthService
{
    Task<EmployeeResponseDTO> Register(EmployeeRegisterDTO dto);
    Task<EmployeeResponseDTO> Login(EmployeeLoginDTO dto);
}
