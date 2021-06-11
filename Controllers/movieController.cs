using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mnshTheatreApp.Data;
using mnshTheatreApp.Models;
using QRCoder;
using System.Net;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using static mnshTheatreApp.Models.movieModel;


namespace mnshTheatreApp.Controllers
{
    public class movieController : Controller
    {
        private readonly IConfiguration _configuration;


        public movieController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: movie
        public IActionResult Index()
        {
            writeLog(DateTime.Now.ToString() + " : " + "Rendering HomePage");
            DataTable dtbl = new DataTable();
            movieModel obj = new movieModel();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlcmd = new SqlDataAdapter("GetMovies", sqlConnection);
                    sqlcmd.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlcmd.SelectCommand.Parameters.AddWithValue("Location", Location);
                    sqlcmd.Fill(dtbl);

                    List<movieModel> indexList = new List<movieModel>();
                    indexList = (from DataRow dr in dtbl.Rows
                                 select new movieModel()
                                 {
                                     MovieId = dr["MovieId"].ToString(),
                                     MovieName = dr["MovieName"].ToString(),
                                     Language = dr["Language"].ToString(),
                                     AverageRating = dr["AverageRating"].ToString(),
                                     Description = dr["Description"].ToString(),
                                     PosterURL = dr["PosterURL"].ToString(),
                                     Location = dr["Location"].ToString()
                                 }).ToList();
                    obj.indexList = indexList;
                }
            }
            catch (Exception e)
            {
                writeLog(DateTime.Now.ToString() + " : " + e.ToString());
                throw;
            }


            return View(obj);
        }


        // GET: movie/Edit/5
        public IActionResult BookMovie(string id)
        {
            movieModel movieModel = new movieModel();
            if (id!=null)
            {
                movieModel = FetchMovieByID(id);
            }

            return View(movieModel);
        }

        // POST: movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookMovie(string id, [Bind("MovieId,PosterURL,Name,EmailID,SeatNo,Location,AverageRating,Description,Language,MovieName")] movieModel movieModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                    {
                        sqlConnection.Open();
                        SqlCommand sqlcmd = new SqlCommand("BookTickets", sqlConnection);
                        sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("MovieId", movieModel.MovieId);
                        sqlcmd.Parameters.AddWithValue("Name", movieModel.Name);
                        sqlcmd.Parameters.AddWithValue("EmailID", movieModel.EmailID);
                        sqlcmd.Parameters.AddWithValue("NoOfSeats", movieModel.SeatNo);
                        sqlcmd.ExecuteNonQuery();
                    }
                    writeLog(DateTime.Now.ToString() + " : " + "Booking Successful");

                    return RedirectToAction("BookingConfirmation", movieModel);
                }
            }
            catch (Exception e)
            {
                writeLog(DateTime.Now.ToString() + " : " + e.ToString());
                throw;
            }        

            return View(movieModel);
        }

      [NonAction]
      public movieModel FetchMovieByID(string id)
      {
            movieModel model = new movieModel();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    DataTable dtbl = new DataTable();
                    sqlConnection.Open();
                    SqlDataAdapter sqlcmd = new SqlDataAdapter("GetMovieByID", sqlConnection);
                    sqlcmd.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcmd.SelectCommand.Parameters.AddWithValue("MovieID", id);
                    sqlcmd.Fill(dtbl);
                    if (dtbl.Rows.Count == 1)
                    {
                        model.MovieId = dtbl.Rows[0]["MovieID"].ToString();
                        model.MovieName = dtbl.Rows[0]["MovieName"].ToString();
                        model.PosterURL = dtbl.Rows[0]["PosterURL"].ToString();
                        model.Location = dtbl.Rows[0]["Location"].ToString();
                        model.AverageRating = dtbl.Rows[0]["AverageRating"].ToString();
                        model.Description = dtbl.Rows[0]["Description"].ToString();
                        model.Language = dtbl.Rows[0]["Language"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                writeLog(DateTime.Now.ToString() + " : " + e.ToString());
                throw;
            }
           
            return model;

        }

        public IActionResult BookingConfirmation(movieModel obj)
        {
            writeLog(DateTime.Now.ToString() + " : " + "Rendering Confirmation Screen");
            try
            {
                Credential c = new Credential();
                c.QRCodePath = _configuration.GetValue<string>("Credential:QRCodePath");

                string TicketContent = " Ticket Details: # Movie: " + obj.MovieName + "# Genre: " + obj.Description + "# Language: " + obj.Language + "# Location: " + obj.Location + "# Name: " + obj.Name + "# No of Seats: " + obj.SeatNo + "# Email: " + obj.EmailID;
                TicketContent = TicketContent.Replace("#", System.Environment.NewLine);
                System.IO.File.Delete(c.QRCodePath + "\\qrcode.jpg");
                //Generate QR Code
                using (MemoryStream ms = new MemoryStream())
                {

                    QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(TicketContent, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qRCodeData);

                    using (Bitmap bitmap = qrCode.GetGraphic(20))
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        bitmap.Save(c.QRCodePath + "\\qrcode.jpg");
                        obj.QRCodeImage = "data:Image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        writeLog(DateTime.Now.ToString() + " : " + "QRCode Generated");
                        //AppContext.BaseDirectory

                    }



                }
            }
            catch (Exception e)
            {
                writeLog(DateTime.Now.ToString() + " : " + e.ToString());
                throw;
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult SendEmail(string EmailId, string MovieName, string Genre, string Language, string Location, string Name, int SeatNo, string QRCode)
        {
            try
            {
                Credential c = new Credential();
                c.Email = _configuration.GetValue<string>("Credential:Email");
                c.Password = _configuration.GetValue<string>("Credential:Password");
                c.QRCodePath = _configuration.GetValue<string>("Credential:QRCodePath");

                string TicketContent = " Ticket Details: Hi, Here is your ticket details. # # Movie: " + MovieName + "# Genre: " + Genre + "# Language: " + Language + "# Location: " + Location + "# Name: " + Name + "# No of Seats: " + SeatNo + "# Email: " + EmailId;
                TicketContent = TicketContent.Replace("#", System.Environment.NewLine);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(c.Email);
                mail.To.Add(EmailId);
                mail.Subject = "Ticket Booking Confirmation";
                mail.Body = TicketContent;
                mail.IsBodyHtml = true;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(c.QRCodePath + "\\qrcode.jpg");
                mail.Attachments.Add(attachment);
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(c.Email, c.Password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                writeLog(DateTime.Now.ToString() + " : " + "Ticket Details sent via Email");
            }
            catch (Exception e)
            {
                writeLog(DateTime.Now.ToString() + " : " + e.ToString());
                throw;
            }

            return RedirectToAction(nameof(Index));

        }
        
        public void writeLog(string strValue)
        {
            Credential c = new Credential();
            c.LogPath = _configuration.GetValue<string>("Credential:LogPath");
            try
            {
                //AppContext.BaseDirectory + "\\TheatreApplog.txt"
                string path = c.LogPath;
                StreamWriter sw;
                if(!System.IO.File.Exists(path))
                {
                    sw = System.IO.File.CreateText(path);
                }
                else
                {
                    sw = System.IO.File.AppendText(path);
                }

                LogWrite(strValue, sw);

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private static void LogWrite(string logMessage, StreamWriter w)
        {
            w.WriteLine("{0}", logMessage);
            w.WriteLine("---------------------------------------");
        }
    
    }
}
