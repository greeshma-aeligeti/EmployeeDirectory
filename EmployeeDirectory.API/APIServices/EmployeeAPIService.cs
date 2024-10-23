using Azure;
using EmployeeDirectory.BLL.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using System;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EmployeeDirectory.API.APIServices
{
    public class EmployeeAPIService
    {
        private readonly HttpClient _httpClient;
        private IEnumerable<EmployeeDTO> _employees;
        public EmployeeAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            var _response = await _httpClient.GetStringAsync("api/Employee/AllEmployees");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };
            _employees= JsonConvert.DeserializeObject<List<EmployeeDTO>>(_response, settings);
            return _employees;

        }
        public async Task<EmployeeDTO> AddEmployee(EmployeeDTO employeeDTO)

        {
            employeeDTO.Id = 1;
            employeeDTO.Path = "x";

            var _response = await _httpClient.PostAsJsonAsync("api/Employee/Add", employeeDTO);
            _response.EnsureSuccessStatusCode();
            return await _response.Content.ReadFromJsonAsync<EmployeeDTO>();
        }
    }
}
