using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Device.Location;
using System.IO;
using System.Web.Security;

namespace PimsApp
{
    public partial class RegisterComplaint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Fetch and populate employee details
                string email = Session["Email"] as string;
                if (string.IsNullOrEmpty(email))
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    PopulateEmployeeDetails(email);
                }

                lblWelcome.Text = $"Welcome, {email}!";
            }


        }

        private void PopulateEmployeeDetails(string email)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT FirstName, LastName, EmpId, Email, ContactNumber FROM EmpDetails WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFirstName.Text = reader["FirstName"].ToString();
                        txtLastName.Text = reader["LastName"].ToString();
                        txtEmpId.Text = reader["EmpId"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtContactNumber.Text = reader["ContactNumber"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred while fetching employee details: " + ex.Message;
                }
            }
        }

        protected void btnAddDateTime_Click(object sender, EventArgs e)
        {
            txtDateTimeCapture.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string empId = txtEmpId.Text.Trim();
                string email = txtEmail.Text.Trim();
                string contactNumber = txtContactNumber.Text.Trim();
                string dateTimeCapture = txtDateTimeCapture.Text.Trim();
                //string location = txtLocation.Text.Trim();
                string comments = txtComments.Text.Trim();
                string picturePaths = UploadPictures();

                string complaintId = GenerateUniqueComplaintId();

                string streetaddress1 = txtStreetAddress1.Text.Trim();
                string streetAddress2 = txtStreetAddress2.Text.Trim();
                string city = txtCity.Text.Trim();
                string state = txtState.Text.Trim();
                string zip = txtZipcode.Text.Trim();

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"
                INSERT INTO Complaints (ComplaintId, FirstName, LastName, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation, PictureUpload, Comments, Status,StreetAddress1,StreetAddress2,City,Zip,State,CurrentStatus)
                VALUES (@ComplaintId, @FirstName, @LastName, @EmpId, @Email, @ContactNumber, @DateTimeCapture, @PictureCaptureLocation, @PictureUpload, @Comments, @Status,@StreetAddress1,@StreetAddress2,@City,@Zip,@State,@CurrentStatus)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ComplaintId", complaintId);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                    cmd.Parameters.AddWithValue("@DateTimeCapture", dateTimeCapture);
                    cmd.Parameters.AddWithValue("@PictureCaptureLocation", "");
                    cmd.Parameters.AddWithValue("@PictureUpload", picturePaths);
                    cmd.Parameters.AddWithValue("@Comments", comments);
                    cmd.Parameters.AddWithValue("@Status", "Not Started");
                    cmd.Parameters.AddWithValue("@StreetAddress1", streetaddress1);
                    cmd.Parameters.AddWithValue("@StreetAddress2", streetAddress2);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Zip", zip);
                    cmd.Parameters.AddWithValue("@State", state);
                    cmd.Parameters.AddWithValue("@CurrentStatus", "Not Started");

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Construct success message
                        string successMessage = $"Thank you for the submission. Your Complaint registered successfully. Your complaint ID is {complaintId}.";

                        // Set the success message in the session
                        Session["SuccessMessage"] = successMessage;

                        //// Log out the user
                        //FormsAuthentication.SignOut(); // Use FormsAuthentication to log out if you are using Forms Authentication

                        // Redirect to EmpHome with a flag indicating that a message should be displayed
                        //Response.Redirect("EmpHome.aspx?showMessage=true");
                        Response.Redirect("Home.aspx");
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "An error occurred: " + ex.Message;
                    }
                }
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please fill out either the Picture Capture Location or the Address fields.";
            }

            
        }

        private bool IsFormValid()
        {
            // Check if Picture Capture Location is filled
            //bool isLocationFilled = !string.IsNullOrEmpty(txtLocation.Text.Trim());

            // Check if Address fields are filled
            bool isAddressFilled = !string.IsNullOrEmpty(txtStreetAddress1.Text.Trim()) &&
                                   !string.IsNullOrEmpty(txtCity.Text.Trim()) &&
                                   !string.IsNullOrEmpty(txtZipcode.Text.Trim()) &&
                                   !string.IsNullOrEmpty(txtState.Text.Trim());

            // Return true if either the location or the address is filled
            //return isLocationFilled || isAddressFilled;
            return isAddressFilled;
        }

        private string GenerateUniqueComplaintId()
        {
            return "CMP" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private string UploadPictures()
        {
            List<string> imagePaths = new List<string>();

            // Path to the Desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Path to the UploadImages folder on the Desktop
            string uploadDirectory = Path.Combine(desktopPath, "UploadImages");

            // Ensure the UploadImages directory exists
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            if (fileUpload.HasFiles)
            {
                foreach (HttpPostedFile uploadedFile in fileUpload.PostedFiles)
                {
                    // Generate a unique file name
                    string uniqueId = Guid.NewGuid().ToString();
                    string fileName = Path.GetFileName(uploadedFile.FileName);
                    string uniqueFileName = uniqueId + "_" + fileName;
                    string filePath = Path.Combine(uploadDirectory, uniqueFileName);

                    // Save the file to the local directory
                    uploadedFile.SaveAs(filePath);

                    imagePaths.Add(filePath);
                }
            }

            // Return a comma-separated string of image paths
            return string.Join(",", imagePaths);
        }


    }
}