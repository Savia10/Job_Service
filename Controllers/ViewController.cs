using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Job_Service.Services;
using Job_Service.Models;

namespace Job_Service.Controllers
{
    [Route("api/v1/")]
    [ApiController]

    public class ViewController : ControllerBase
    {
        private readonly IActionContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        private readonly ICVRepository _cvRepository;
        public static string strMethodName = string.Empty;
        public static string finalfilterstring = string.Empty;

        public ViewController(IActionContextAccessor accessor, IConfiguration configuration, ICVRepository cvRepository)
        {
            _accessor = accessor;
            _configuration = configuration;
            _cvRepository = cvRepository;
        }


        [HttpGet("jobs/{id?}")]
        public async Task Get(int id, [FromQuery] string filter, [FromQuery] string since, [FromQuery] string attributes)
        {
            var response = HttpContext.Response;

            try
            {

                string commandtext = "";
                if (id > 0)
                {
                    commandtext = "Select * from  JobDataTable where Id=" + id;

                }
                else { commandtext = "Select * from  JobDataTable  "; }


                var dtForStream = new DataTable();
                dtForStream = await _cvRepository.GetCVData(commandtext);


                int chunkSize = 5000000;
                bool isChunkSizeInt = false;
                if (Request.Headers.ContainsKey("X-Chunk-Size".ToLower()))
                {
                    isChunkSizeInt = int.TryParse(Request.Headers["X-Chunk-Size".ToLower()], out chunkSize);
                    if (!isChunkSizeInt || (isChunkSizeInt && chunkSize <= 0))
                    {
                        Error errorObj = new Error();
                        errorObj.ErrorMessage = "Bad Request";

                        response.StatusCode = 400;
                        response.ContentType = "application/json";
                        return;
                    }
                }
                string responseContent;
                if (Request.Headers["Accept"] == "application/xml")
                {
                    string xmlString = string.Empty;
                    using (TextWriter writer = new StringWriter())
                    {
                        dtForStream.WriteXml(writer);
                        xmlString = writer.ToString();
                    }
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlString);

                    XmlDocument docNew = new XmlDocument();
                    XmlElement newRoot = docNew.CreateElement("records");
                    docNew.AppendChild(newRoot);
                    newRoot.InnerXml = xDoc.DocumentElement.InnerXml;
                    responseContent = docNew.OuterXml;
                    response.ContentType = "application/xml";
                }
                else
                {
                    responseContent = JsonConvert.SerializeObject(dtForStream);
                    response.ContentType = "application/json";
                }


                int byteSize = System.Text.ASCIIEncoding.ASCII.GetByteCount(responseContent);

                int recordCount = dtForStream.Rows.Count;
                string contentRange = "records 0";
                if (recordCount > 0)
                {
                    contentRange = "records 0-" + (recordCount - 1) + "/" + recordCount;
                }
                byte[] JSbytes = Encoding.ASCII.GetBytes(responseContent);

                response.Headers.Add("Content-Range", contentRange);
                response.Headers.Add(HeaderNames.TransferEncoding, "chunked");

                string responseHeaderString = "";
                foreach (var header in response.Headers)
                {
                    responseHeaderString += (" " + header.Key + " : " + header.Value + " ");
                }

                

                for (var j = 0; j < JSbytes.Length / chunkSize + 1; j++)
                {
                    string res = Encoding.ASCII.GetString(JSbytes.Skip(j * chunkSize).Take(chunkSize).ToArray());

                    string len = res.Length.ToString("X");
                    await response.WriteAsync($"{len}\r\n{res}\r\n");
                    response.Body.Flush();
                }

                await response.WriteAsync("0\r\n\r\n");


                await response.Body.FlushAsync();


            }
            catch (Exception)
            {
               
                Error errorObj = new Error();

                errorObj.ErrorMessage = "Internal Server Error";
                response.StatusCode = 500;
                response.ContentType = "application/json";
                return;

            }

        }

        [HttpPost("jobs")]
        public async Task<ActionResult<User>> CreateEmployee([FromBody] User emp)
        {
            string title = emp.title;
            string description = emp.description;
            int locationId = emp.locationId;
            int departmentId = emp.departmentId;
            DateTime closingDate = emp.closingDate;

            try
            {
                string content = emp.ToString();
                var createdEmployee = await _cvRepository.AddEmployee(title, description, locationId, departmentId, closingDate);

                return StatusCode(StatusCodes.Status201Created,
                                "Created new employee record");


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error creating new employee record");
            }
        }

        [HttpPut("jobs/{id}")]
        public async Task<ActionResult<User>> UpdateEmployee(int id, [FromBody] User emp)
        {
            string title = emp.title;
            string description = emp.description;
            int locationId = emp.locationId;
            int departmentId = emp.departmentId;
            DateTime closingDate = emp.closingDate;

            try
            {
                string content = emp.ToString();
                var createdEmployee = await _cvRepository.UpdateEmployee(id,title, description, locationId, departmentId, closingDate);

                return StatusCode(StatusCodes.Status200OK,
                                  "Updated new employee details");


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error updating new employee details");
            }

        }

        [HttpGet("departments/{id?}")]
        public async Task GetDepartments(int id, [FromQuery] string filter, [FromQuery] string since, [FromQuery] string attributes)
        {
            var response = HttpContext.Response;

            try
            {

                string commandtext = "";
                if (id == 0)
                {
                    commandtext = "Select * from  JobDataTable where department is not null";

                }
                else { commandtext = "select * from JobDataTable where department like '%" + id + "%'"; }


                var dtForStream = new DataTable();
                dtForStream = await _cvRepository.GetCVData(commandtext);


                int chunkSize = 5000000;
                bool isChunkSizeInt = false;
                if (Request.Headers.ContainsKey("X-Chunk-Size".ToLower()))
                {
                    isChunkSizeInt = int.TryParse(Request.Headers["X-Chunk-Size".ToLower()], out chunkSize);
                    if (!isChunkSizeInt || (isChunkSizeInt && chunkSize <= 0))
                    {
                        Error errorObj = new Error();
                        errorObj.ErrorMessage = "Bad Request";

                        response.StatusCode = 400;
                        response.ContentType = "application/json";
                        return;
                    }
                }

                string responseContent;
                if (Request.Headers["Accept"] == "application/xml")
                {
                    string xmlString = string.Empty;
                    using (TextWriter writer = new StringWriter())
                    {
                        dtForStream.WriteXml(writer);
                        xmlString = writer.ToString();
                    }
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlString);

                    XmlDocument docNew = new XmlDocument();
                    XmlElement newRoot = docNew.CreateElement("records");
                    docNew.AppendChild(newRoot);
                    newRoot.InnerXml = xDoc.DocumentElement.InnerXml;
                    responseContent = docNew.OuterXml;
                    response.ContentType = "application/xml";
                }
                else
                {
                    responseContent = JsonConvert.SerializeObject(dtForStream);
                    response.ContentType = "application/json";
                }


                int byteSize = System.Text.ASCIIEncoding.ASCII.GetByteCount(responseContent);

                int recordCount = dtForStream.Rows.Count;
                string contentRange = "records 0";
                if (recordCount > 0)
                {
                    contentRange = "records 0-" + (recordCount - 1) + "/" + recordCount;
                }
                byte[] JSbytes = Encoding.ASCII.GetBytes(responseContent);

                response.Headers.Add("Content-Range", contentRange);
                response.Headers.Add(HeaderNames.TransferEncoding, "chunked");

                string responseHeaderString = "";
                foreach (var header in response.Headers)
                {
                    responseHeaderString += (" " + header.Key + " : " + header.Value + " ");
                }



                for (var j = 0; j < JSbytes.Length / chunkSize + 1; j++)
                {
                    string res = Encoding.ASCII.GetString(JSbytes.Skip(j * chunkSize).Take(chunkSize).ToArray());

                    string len = res.Length.ToString("X");
                    await response.WriteAsync($"{len}\r\n{res}\r\n");
                    response.Body.Flush();
                }

                await response.WriteAsync("0\r\n\r\n");


                await response.Body.FlushAsync();


            }
            catch (Exception)
            {

                Error errorObj = new Error();

                errorObj.ErrorMessage = "Internal Server Error";
                response.StatusCode = 500;
                response.ContentType = "application/json";
                return;

            }

        }

        [HttpGet("locations/{id?}")]
        public async Task GetLocations(int id, [FromQuery] string filter, [FromQuery] string since, [FromQuery] string attributes)
        {
            var response = HttpContext.Response;
            try
            {

                string commandtext = "";
                if (id == 0)
                {
                    commandtext = "Select * from  JobDataTable where location is not null";

                }
                else { commandtext = "select * from JobDataTable where location like '%" + id + "%'"; }

                var dtForStream = new DataTable();
                dtForStream = await _cvRepository.GetCVData(commandtext);


                int chunkSize = 5000000;
                bool isChunkSizeInt = false;
                if (Request.Headers.ContainsKey("X-Chunk-Size".ToLower()))
                {
                    isChunkSizeInt = int.TryParse(Request.Headers["X-Chunk-Size".ToLower()], out chunkSize);
                    if (!isChunkSizeInt || (isChunkSizeInt && chunkSize <= 0))
                    {
                        Error errorObj = new Error();
                        errorObj.ErrorMessage = "Bad Request";

                        response.StatusCode = 400;
                        response.ContentType = "application/json";
                        return;
                    }
                }

                string responseContent;
                if (Request.Headers["Accept"] == "application/xml")
                {
                    string xmlString = string.Empty;
                    using (TextWriter writer = new StringWriter())
                    {
                        dtForStream.WriteXml(writer);
                        xmlString = writer.ToString();
                    }
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlString);

                    XmlDocument docNew = new XmlDocument();
                    XmlElement newRoot = docNew.CreateElement("records");
                    docNew.AppendChild(newRoot);
                    newRoot.InnerXml = xDoc.DocumentElement.InnerXml;
                    responseContent = docNew.OuterXml;
                    response.ContentType = "application/xml";
                }
                else
                {
                    responseContent = JsonConvert.SerializeObject(dtForStream);
                    response.ContentType = "application/json";
                }


                int byteSize = System.Text.ASCIIEncoding.ASCII.GetByteCount(responseContent);

                int recordCount = dtForStream.Rows.Count;
                string contentRange = "records 0";
                if (recordCount > 0)
                {
                    contentRange = "records 0-" + (recordCount - 1) + "/" + recordCount;
                }
                byte[] JSbytes = Encoding.ASCII.GetBytes(responseContent);

                response.Headers.Add("Content-Range", contentRange);
                response.Headers.Add(HeaderNames.TransferEncoding, "chunked");

                string responseHeaderString = "";
                foreach (var header in response.Headers)
                {
                    responseHeaderString += (" " + header.Key + " : " + header.Value + " ");
                }



                for (var j = 0; j < JSbytes.Length / chunkSize + 1; j++)
                {
                    string res = Encoding.ASCII.GetString(JSbytes.Skip(j * chunkSize).Take(chunkSize).ToArray());

                    string len = res.Length.ToString("X");
                    await response.WriteAsync($"{len}\r\n{res}\r\n");
                    response.Body.Flush();
                }

                await response.WriteAsync("0\r\n\r\n");


                await response.Body.FlushAsync();


            }
            catch (Exception)
            {

                Error errorObj = new Error();

                errorObj.ErrorMessage = "Internal Server Error";
                response.StatusCode = 500;
                response.ContentType = "application/json";
                return;

            }

        }



    }
}
        

    

    
    
    
