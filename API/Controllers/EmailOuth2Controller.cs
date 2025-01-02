using System.ComponentModel.DataAnnotations;
using emService.AppCode;
using Microsoft.AspNetCore.Mvc;

namespace emService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController :ControllerBase
{
    private readonly EmailHelperOuth2 _emailHelperOuth2;

    public EmailController(EmailHelperOuth2 emailHelperOuth2)
    {
        _emailHelperOuth2 = emailHelperOuth2;
    }
    
    [HttpPost]
    public IActionResult SendEmailRequest([FromBody] EmailRequest emailRequest)
    {
        if (emailRequest == null || string.IsNullOrWhiteSpace(emailRequest.To))
        {
            return BadRequest("Invalid email request. 'To' field is required.");
        }

        try
        {
            _emailHelperOuth2.SendMailOuth2(emailRequest);
            return Ok("Email sent successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception (using a logger, e.g., ILogger)
            return StatusCode(500, $"An error occurred while sending the email: {ex.Message}");
        }
    }

    // For File Convert into bytes
[HttpGet]
public byte [] StreamFile()
{
   FileStream fs = new FileStream("C:Users/shubhamt/Desktop/Shubham.jpg", FileMode.Open,FileAccess.Read);

   // Create a byte array of file stream length
   byte[] ImageData = new byte[fs.Length];

   //Read block of bytes from stream into the byte array
   fs.Read(ImageData,0,System.Convert.ToInt32(fs.Length));

   //Close the File Stream
   fs.Close();
   return ImageData; //return the byte data
}

}

	





 