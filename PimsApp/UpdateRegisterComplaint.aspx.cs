using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace PimsApp
{
    public partial class UpdateRegisterComplaint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the ComplaintId from the session
                string complaintId = Session["ComplaintId"] as string;

                lblComplaintId.Text = complaintId;

                if (!string.IsNullOrEmpty(complaintId))
                {
                    // Prefetch complaint details and populate the form fields
                    PrefetchComplaintDetails(complaintId);
                }
                else
                {
                    lblMessage.Text = "Complaint ID is missing. Please try again.";
                }
                string email = Session["UpdateEmail"] as string;
                lblWelcome.Text = $"Welcome, {email}!";
            }
        }
        private void PrefetchComplaintDetails(string complaintId)
        {
            // Database connection string
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT * FROM Complaints WHERE ComplaintId = @ComplaintId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtFirstName.Text = reader["FirstName"].ToString();
                            txtLastName.Text = reader["LastName"].ToString();
                            txtEmpId.Text = reader["EmpId"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtContactNumber.Text = reader["ContactNumber"].ToString();
                            txtComments.Text = reader["Comments"].ToString();
                            txtStreetAddress1.Text = reader["StreetAddress1"].ToString();
                            txtStreetAddress2.Text = reader["StreetAddress2"].ToString();
                            txtCity.Text = reader["City"].ToString();
                            txtState.Text = reader["State"].ToString();
                            txtZipCode.Text = reader["Zip"].ToString();
                            txtStatus.Text = reader["Status"].ToString();
                            txtDateTimeCapture.Text = reader["DateTimeCapture"].ToString();
                            //txtLocation.Text = reader["PictureCaptureLocation"].ToString();
                           

                            hiddenImagePaths.Value = reader["PictureUpload"].ToString();

                            // Populate existing images
                            LoadExistingData();

                            hiddenImagePaths.Value = reader["PictureUpload"].ToString();
                        }
                        else
                        {
                            lblMessage.Text = "Complaint not found. Please check the Complaint ID.";
                        }
                    }
                }
            }
        }

        private void LoadExistingData()
        {
            string imagePaths = hiddenImagePaths.Value;           
            StringBuilder imagesHtml = new StringBuilder();
            if (!string.IsNullOrEmpty(imagePaths))
            {
                string[] existingImages = imagePaths.Split(',');

                foreach (string imagePath in existingImages)
                {
                    string imgURL = Path.GetFileName(imagePath);
                    imagesHtml.AppendFormat("<img src='{0}' alt='Complaint Image' class='img-thumbnail' style='max-width: 200px; max-height: 150px;'/>", "imagehandler.ashx?imageName=" + imgURL);
                }
                litImages.Text = imagesHtml.ToString();
            }            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Get the ComplaintId from the session
            string complaintId = Session["ComplaintId"] as string;

            if (!string.IsNullOrEmpty(complaintId))
            {
                // Database connection string
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Delete old images
                    //DeleteOldImages(hiddenImagePaths.Value);

                    string queryGetOriginal = "SELECT FirstName, LastName, EmpId, Email, ContactNumber, Comments, StreetAddress1, StreetAddress2, City, State, Zip, Status, DateTimeCapture, PictureUpload FROM Complaints WHERE ComplaintId = @ComplaintId";

                    using (SqlCommand cmdGetOriginal = new SqlCommand(queryGetOriginal, conn))
                    {
                        cmdGetOriginal.Parameters.AddWithValue("@ComplaintId", complaintId);

                        using (SqlDataReader reader = cmdGetOriginal.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Fetch original values from the database
                                string originalFirstName = reader["FirstName"].ToString();
                                string originalLastName = reader["LastName"].ToString();
                                string originalEmpId = reader["EmpId"].ToString();
                                string originalEmail = reader["Email"].ToString();
                                string originalContactNumber = reader["ContactNumber"].ToString();
                                string originalComments = reader["Comments"].ToString();
                                string originalStreetAddress1 = reader["StreetAddress1"].ToString();
                                string originalStreetAddress2 = reader["StreetAddress2"].ToString();
                                string originalCity = reader["City"].ToString();
                                string originalState = reader["State"].ToString();
                                string originalZip = reader["Zip"].ToString();
                                string originalStatus = reader["Status"].ToString();
                                string originalDateTimeCapture = reader["DateTimeCapture"].ToString();
                                string originalPictureUpload = reader["PictureUpload"].ToString();

                                // Check if any changes were made
                                bool hasChanges = false;

                                if (txtFirstName.Text.Trim() != originalFirstName ||
                                    txtLastName.Text.Trim() != originalLastName ||
                                    txtEmpId.Text.Trim() != originalEmpId ||
                                    txtEmail.Text.Trim() != originalEmail ||
                                    txtContactNumber.Text.Trim() != originalContactNumber ||
                                    txtComments.Text.Trim() != originalComments ||
                                    txtStreetAddress1.Text.Trim() != originalStreetAddress1 ||
                                    txtStreetAddress2.Text.Trim() != originalStreetAddress2 ||
                                    txtCity.Text.Trim() != originalCity ||
                                    txtState.Text.Trim() != originalState ||
                                    txtZipCode.Text.Trim() != originalZip ||
                                    txtStatus.Text.Trim() != originalStatus ||
                                    txtDateTimeCapture.Text.Trim() != originalDateTimeCapture ||
                                    hiddenImagePaths.Value != originalPictureUpload) // Include image comparison if needed
                                {
                                    hasChanges = true;
                                }

                                if (!hasChanges)
                                {
                                    // No changes were made
                                    lblMessage.Visible = true;
                                    lblMessage.Text = "No changes were made.";
                                    Response.Redirect("Home.aspx");
                                }
                            }
                        }
                    }


                    string newPicturePaths = UploadNewPictures();

                    if (string.IsNullOrEmpty(newPicturePaths))
                    {
                        newPicturePaths = hiddenImagePaths.Value;
                    }

                    // SQL query to update the complaint
                    string query = @"
                    UPDATE Complaints
                    SET 
                        FirstName = @FirstName,
                        LastName = @LastName,
                        EmpId = @EmpId,
                        Email = @Email,
                        ContactNumber = @ContactNumber,
                        Comments = @Comments,
                        StreetAddress1 = @StreetAddress1,
                        StreetAddress2 = @StreetAddress2,
                        City = @City,
                        State = @State,
                        Zip = @Zip,
                        Status = @Status,
                        DateTimeCapture = @DateTimeCapture,
                        PictureUpload = @PictureUpload -- Update with new image paths
                    WHERE 
                        ComplaintId = @ComplaintId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to the query
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@EmpId", txtEmpId.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@Comments", txtComments.Text.Trim());
                        cmd.Parameters.AddWithValue("@StreetAddress1", txtStreetAddress1.Text.Trim());
                        cmd.Parameters.AddWithValue("@StreetAddress2", txtStreetAddress2.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                        cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                        cmd.Parameters.AddWithValue("@Zip", txtZipCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@DateTimeCapture", txtDateTimeCapture.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", txtStatus.Text.Trim());
                        cmd.Parameters.AddWithValue("@PictureUpload", newPicturePaths); // Update image paths
                        cmd.Parameters.AddWithValue("@ComplaintId", complaintId);
                        cmd.Parameters.AddWithValue("@PictureCaptureLocation", "");
                        //cmd.Parameters.AddWithValue("@PictureCaptureLocation", txtLocation.Text.Trim());

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();
                        try
                        {
                            cmd.ExecuteNonQuery();
                            

                            // Construct success message
                            string successMessage = $"Your complaint has been updated.";

                            // Set the success message in the session
                            Session["SuccessMessage"] = successMessage;

                            // Log out the user
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
            }
            else
            {
                lblMessage.Text = "Complaint ID is missing. Please try again.";
                lblMessage.CssClass = "text-danger"; // Bootstrap danger class for red text
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect to the home page
            Response.Redirect("Home.aspx");
        }

        private void DeleteOldImages(string imagePaths)
        {
            if (!string.IsNullOrEmpty(imagePaths))
            {
                string[] paths = imagePaths.Split(',');

                foreach (string path in paths)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
        }

        private string UploadNewPictures()
        {
            List<string> imagePaths = new List<string>();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string uploadDirectory = Path.Combine(desktopPath, "UploadImages");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            if (fileUpload.HasFiles)
            {
                foreach (HttpPostedFile uploadedFile in fileUpload.PostedFiles)
                {
                    string uniqueId = Guid.NewGuid().ToString();
                    string fileName = Path.GetFileName(uploadedFile.FileName);
                    string uniqueFileName = uniqueId + "_" + fileName;
                    string filePath = Path.Combine(uploadDirectory, uniqueFileName);

                    uploadedFile.SaveAs(filePath);
                    imagePaths.Add(filePath);
                }
            }

            return imagePaths.Count > 0 ? string.Join(",", imagePaths) : string.Empty;
        }
    }
}