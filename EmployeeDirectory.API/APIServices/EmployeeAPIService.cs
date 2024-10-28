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
using EmployeeDirectory.BLL;

namespace EmployeeDirectory.API.APIServices
{
    public class EmployeeAPIService
    {
        private readonly HttpClient _httpClient;
        private IEnumerable<EmployeeDTO> _employees;
        private IEnumerable<EmployeeDTO> _subordinates;
        private IEnumerable<EmployeeDTO> _higherEmployees;
        private IEnumerable<EmployeeDTO> _nextSubordinates;
        private IEnumerable<int> _allManagerIds;
        private EmployeeTreeNode _employeeTreeNode;
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
        public async Task<IEnumerable<int>> GetAllManagerIds()
        {
            var _response = await _httpClient.GetStringAsync("api/Employee/AllManagerIds");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };
            _allManagerIds = JsonConvert.DeserializeObject<List<int>>(_response, settings);
            return _allManagerIds;

        }
        public async Task<IEnumerable<EmployeeDTO>> GetAllSubordinates(int managerId)
        {
            var _response = await _httpClient.GetStringAsync($"api/Employee/GetSubordinates/{managerId}");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };
            _nextSubordinates = JsonConvert.DeserializeObject<List<EmployeeDTO>>(_response, settings);
            return _nextSubordinates;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetNextSubordinates(int managerId)
        {
            var _response = await _httpClient.GetStringAsync($"api/Employee/GetNextSubordinates/{managerId}");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };
            _subordinates = JsonConvert.DeserializeObject<List<EmployeeDTO>>(_response, settings);
            return _subordinates;
        }

        public async Task<EmployeeTreeNode> BuildEmployeeTree(int id)
        {
            var _response = await _httpClient.GetStringAsync($"api/Employee/EmployeeTree/{id}");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };
            _employeeTreeNode= JsonConvert.DeserializeObject<EmployeeTreeNode>(_response, settings);
            return _employeeTreeNode;

        }
        public async Task<IEnumerable<EmployeeDTO>> GetHigherAuthorities(int managerId)
        {
            var _response = await _httpClient.GetStringAsync($"api/Employee/GetHigherAuthorities/{managerId}");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };
            _higherEmployees = JsonConvert.DeserializeObject<List<EmployeeDTO>>(_response, settings);
            return _higherEmployees;
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
