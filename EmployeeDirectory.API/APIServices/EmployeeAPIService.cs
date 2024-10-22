using EmployeeDirectory.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.BLL.APIServices
{
    public class EmployeeAPIService
    {
        private readonly HttpClient _httpClient;

        public EmployeeAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<EmployeeDTO> AddEmployee(EmployeeDTO employeeDTO)
        {
            var _response = await _httpClient.PostAsJsonAsync("api/Employee/Add", employeeDTO);
            _response.EnsureSuccessStatusCode();
            return await _response.Content.ReadFromJsonAsync<EmployeeDTO>();
        }
    }
}
