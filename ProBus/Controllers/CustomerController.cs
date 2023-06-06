using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProBus.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [Route("MyTest")]
        [HttpGet]

        public string Timekeeper()

            {
            string date_str = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
            return(date_str);
        }


        /// <summary>
        /// เรียกข้อมูลลูกค้าคนเดียว
        /// </summary>
        /// <returns></returns>

        [Route("GetCustomer")]
        [HttpGet]

        public ActionResult<string> GetCustomer(int personId)
        {
            try
            {
                CustomerV1Model person = new CustomerV1Model();
                string query = $"SELECT * FROM Persons WHERE PersonID = {personId} ";
                using (SqlConnection con = new SqlConnection(Startup.ConnectionString02))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                person = new CustomerV1Model();
                                person.PersonID = (read["PersonID"] != DBNull.Value) ? int.Parse(read["PersonID"].ToString()) : 0;
                                person.FirstName = (read["FirstName"] != DBNull.Value) ? read["FirstName"].ToString() : string.Empty;
                                person.LastName = (read["LastName"] != DBNull.Value) ? read["LastName"].ToString() : string.Empty;
                                person.Age = (read["Age"] != DBNull.Value) ? int.Parse(read["Age"].ToString()) : 0;
                                person.City = (read["City"] != DBNull.Value) ? read["City"].ToString() : "";
                                person.Phone = (read["Phone"] != DBNull.Value) ? int.Parse(read["Phone"].ToString()) : 0;
                            }
                        }
                        con.Close();
                    }

                    var result = person.PersonID != 0 ? StatusCode(200, person) : StatusCode(404, "Data Not found.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }  
        /// <summary>
        /// เรียกข้อมูลลูกค้ามากกว่าหนึ่งคนขึันไป
        /// </summary>
        /// <returns></returns>
        [Route ("GetCustomers")]
        [HttpGet]
        public ActionResult<string> GetCustomers()
        {
            try
            {
                List<CustomerV1Model> persons = new List<CustomerV1Model>();
                string query = "SELECT * FROM Persons ";
                using (SqlConnection con = new SqlConnection(Startup.ConnectionString02))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                CustomerV1Model person = new CustomerV1Model();
                                person.PersonID = (read["PersonID"] != DBNull.Value) ? int.Parse(read["PersonID"].ToString()) : 0;
                                person.FirstName = (read["FirstName"] != DBNull.Value) ? read["FirstName"].ToString() : string.Empty;
                                person.LastName = (read["LastName"] != DBNull.Value) ? read["LastName"].ToString() : string.Empty;
                                person.Age = (read["Age"] != DBNull.Value) ? int.Parse(read["Age"].ToString()) : 0;
                                person.City = (read["City"] != DBNull.Value) ? read["City"].ToString() : "";
                                person.Phone = (read["Phone"] != DBNull.Value) ? int.Parse(read["Phone"].ToString()) : 0;
                                persons.Add(person);
                            }
                        }
                        con.Close();
                    }

                    if (persons.Count > 0)
                    {
                        return StatusCode(200, persons);
                    }
                    else
                    {
                        return StatusCode(404, "Data Not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// เพิ่มชุดข้อมูลลูกค้า
        /// </summary>
        /// <param name="customer">ชุดข้อมูลลูกค้า</param>
        /// <returns></returns>
        [Route("InsertCustomer")]
        [HttpPost]
        public ActionResult<string> InsertCustomer([FromBody]CustomerV1Model customer)
        {
            try
            {
                if (!customer.PersonID.Equals(0))
                {
                    throw new ArgumentException("PersonID Not Equals 0");
                }
                var id = 0;
                string query = "INSERT INTO Persons(FirstName,LastName,Age,City,Phone) VALUES(@FirstName, @LastName, @Age, @City ,@Phone)";
                query += "SELECT SCOPE_IDENTITY()";
                using (SqlConnection con = new SqlConnection(Startup.ConnectionString02))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@Age", customer.Age);
                        cmd.Parameters.AddWithValue("@City", customer.City);
                        cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                        cmd.Connection = con;
                        con.Open();
                        id = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
                if (!id.Equals(0))
                {
                    return StatusCode(200,$"Insert Sucessfully.");
                }
                else
                {
                    return StatusCode(400,$"Insert Failure.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// แก้ไขข้อมูลลูกค้า
        /// </summary>
        /// <returns></returns>
        [Route("EditCustomer")]
        [HttpPut]
        public ActionResult<string> EditCustomer([FromBody] CustomerV1Model customer)
        {
            try
            {
                if (customer.PersonID.Equals(0))
                {
                    throw new ArgumentException("PersonID Not Equals 0");
                }
                var cmd = 0;
                string query = "UPDATE Persons SET FirstName=@FirstName, LastName=@LastName, Age=@Age ,City=@City, Phone=@Phone WHERE PersonID=@PersonID";
                using (SqlConnection con = new SqlConnection(Startup.ConnectionString02))
                {
                    using (SqlCommand bbb = new SqlCommand(query))
                    {
                        bbb.Parameters.AddWithValue("@PersonID", customer.PersonID);
                        bbb.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        bbb.Parameters.AddWithValue("@LastName", customer.LastName);
                        bbb.Parameters.AddWithValue("@Age", customer.Age);
                        bbb.Parameters.AddWithValue("@City", customer.City);
                        bbb.Parameters.AddWithValue("@Phone", customer.Phone);

                        bbb.Connection = con;
                        con.Open();
                        cmd = Convert.ToInt32(bbb.ExecuteNonQuery());
                        con.Close();
                    }   
                }
                if (cmd!=0)
                {
                    return StatusCode(200, $"Update Sucessfull.");
                }
                else
                {
                    return StatusCode(400, $"Update Failure.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// ลบข้อมูลลูกค้า
        /// </summary>
        /// <returns></returns>
        [Route("DeleteCustomer")]
        [HttpDelete]
        public ActionResult<string> DeleteCustomer(int PersonID)
        {
            try
            {
                var person = 0; 
                string query = "DELETE FROM Persons WHERE PersonID=@PersonID";
                using (SqlConnection con = new SqlConnection(Startup.ConnectionString02))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@PersonID", PersonID);
                        cmd.Connection = con;
                        con.Open();
                        person = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                    if (!person.Equals(0))
                    {
                        return StatusCode(200, $"Delete Sucessfully.");
                    }
                    else
                    {
                        return StatusCode(400, $"Delete Failure.");
                    }
                }            
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
