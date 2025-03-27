using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public LeaveService(ILeaveRepository leaveRepository, IEmployeeRepository employeeRepository)
        {
            _leaveRepository = leaveRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<LeaveResponseDTO> ApplyForLeaveAsync(LeaveCreateDTO dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
                return null; // Handle this properly in the controller

            if (dto.StartDate > dto.EndDate)
                return new LeaveResponseDTO { Reason = "Start date cannot be after the end date." };

            var overlappingLeave = await _leaveRepository.GetOverlappingLeaveAsync(dto.EmployeeId, dto.StartDate, dto.EndDate);
            if (overlappingLeave != null)
                return new LeaveResponseDTO { Reason = "You already have a leave request for this period." };

            var leave = new Leave
            {
                EmployeeId = dto.EmployeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                LeaveType = dto.LeaveType,
                Reason = dto.Reason,
                Status = "Pending"
            };

            await _leaveRepository.ApplyLeaveAsync(leave);

            return new LeaveResponseDTO
            {
                LeaveId = leave.LeaveId,
                EmployeeId = leave.EmployeeId,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                LeaveType = leave.LeaveType,
                Reason = leave.Reason,
                Status = leave.Status
            };
        }


        public async Task<LeaveResponseDTO> GetLeaveByIdAsync(int leaveId)
        {
            var leave = await _leaveRepository.GetLeaveByIdAsync(leaveId);
            if (leave == null) return null;

            return new LeaveResponseDTO
            {
                LeaveId = leave.LeaveId,
                EmployeeId = leave.EmployeeId,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                LeaveType = leave.LeaveType,
                Reason = leave.Reason,
                Status = leave.Status
            };
        }

        public async Task<IEnumerable<LeaveResponseDTO>> GetLeavesByEmployeeIdAsync(int employeeId)
        {
            var leaves = await _leaveRepository.GetLeavesByEmployeeIdAsync(employeeId);
            return leaves.Select(leave => new LeaveResponseDTO
            {
                LeaveId = leave.LeaveId,
                EmployeeId = leave.EmployeeId,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                LeaveType = leave.LeaveType,
                Reason = leave.Reason,
                Status = leave.Status
            });
        }

        public async Task<bool> UpdateLeaveStatusAsync(int leaveId, string status)
        {
            return await _leaveRepository.UpdateLeaveStatusAsync(leaveId, status);
        }

        public async Task<bool> DeleteLeaveAsync(int leaveId)
        {
            return await _leaveRepository.DeleteLeaveAsync(leaveId);
        }

        public async Task<IEnumerable<LeaveResponseDTO>> GetPendingLeavesAsync(int pageNumber, int pageSize)
        {
            var leaves = await _leaveRepository.GetPendingLeavesPaginatedAsync(pageNumber, pageSize);

            return leaves.Select(l => new LeaveResponseDTO
            {
                LeaveId = l.LeaveId,
                EmployeeId = l.EmployeeId,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                LeaveType = l.LeaveType,
                Reason = l.Reason,
                Status = l.Status
            }).ToList();
        }


    }
}
