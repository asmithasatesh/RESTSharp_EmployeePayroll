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
    }
}
