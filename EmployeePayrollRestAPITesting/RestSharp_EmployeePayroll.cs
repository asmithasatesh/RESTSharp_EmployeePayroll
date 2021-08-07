using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollRestAPITesting
{
    [TestClass]
    public class RestSharp_EmployeePayroll
    {
        //Initializing the restclient as null
        RestClient client = null;
        EmployeeModel employee;
        [TestInitialize]
        //Setup base 
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }

        public IRestResponse GetAllEmployees()
        {
            //Define method Type
            RestRequest request = new RestRequest("/Employees", Method.GET);
            //Eexcute request
            IRestResponse response = client.Execute(request);
            //Return the response
            return response;
        }
        //Usecase 1: Getting all the employee details from json server
        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployees()
        {
            IRestResponse response = GetAllEmployees();

            //Deserialize json object to List
            var jsonObject = JsonConvert.DeserializeObject<List<EmployeeModel>>(response.Content);
            Assert.AreEqual(3, jsonObject.Count);
            foreach (var element in jsonObject)
            {
                Console.WriteLine("Id: {0} || Name: {1} || Salary :{2} ", element.id, element.firstName + " " + element.lastName, element.salary);
            }

            //Check the status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //Usecase 2: Add an employee to Json server
        [TestMethod]
        public void OnCallingPOSTAPI_ReturnEmployees()
        {
            RestRequest request = new RestRequest("/Employees", Method.POST);
            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("firstName", "Rakesh");
            jsonObject.Add("lastName", "Kumar");
            jsonObject.Add("salary", 3000000);

            //Adds a parameter to request 
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<EmployeeModel>(response.Content);
            Console.WriteLine("Id: {0} || Name: {1} || Salary :{2} ", result.id, result.firstName + " " + result.lastName, result.salary);
            Assert.AreEqual("Rakesh Kumar", result.firstName+" "+result.lastName);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        }
        //add data to json server
        public IRestResponse AddingInJsonServer(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/Employees", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;

        }
        //Usecase 3: 
        [TestMethod]
        public void OnCallingPostAPI_Adding_MultipleData()
        {
            //Create Json object for employee one
            JsonObject employeeOne = new JsonObject();
            employeeOne.Add("firstName", "Dhana");
            employeeOne.Add("lastName", "Lakshmi");
            employeeOne.Add("salary", 700000);
            //Call Function to Add
            HttpStatusCode responseOne = AddingInJsonServer(employeeOne).StatusCode;

            //Create Json object for employee Two
            JsonObject employeeTwo = new JsonObject();
            employeeTwo.Add("firstName", "Sunita");
            employeeTwo.Add("lastName", "Biswas");
            employeeTwo.Add("salary", 750000);
            //Call Function to Add
            HttpStatusCode responseTwo = AddingInJsonServer(employeeOne).StatusCode;

            Assert.AreEqual(responseOne, HttpStatusCode.Created);
            Assert.AreEqual(responseTwo, HttpStatusCode.Created);
        }
    }
}
