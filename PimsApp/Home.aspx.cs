using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace PimsApp
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the roles from session as a List<string>
            List<string> roles = Session["Roles"] as List<string>;

            if (!IsPostBack)
            {
                
                // Check if roles list is not null and contains at least one of the required roles
                if (roles != null && (roles.Contains("Admin") || roles.Contains("NormalUser") || roles.Contains("BothRoles")))
                {                   
                    bool IsAdmin = roles.Contains("Admin");
                    bool IsBoth = roles.Contains("BothRoles");

                    TemplateField actionTakenField = gvComplaints.Columns
                            .OfType<TemplateField>()
                            .FirstOrDefault(f => f.HeaderText == "Action Taken");

                    if (actionTakenField != null)
                    {
                        // Change the HeaderText based on the role
                        actionTakenField.HeaderText = IsAdmin ? "UpdateProgress" : "Current Status";
                        actionTakenField.HeaderText = IsBoth ? "UpdateProgress" : "Current Status";
                    }

                    pageTitle.InnerText = IsAdmin || IsBoth ? "Admin Dashboard - Complaints Management" : "My Complaints";

                    if (roles.Contains("Admin") || roles.Contains("BothRoles"))
                    {
                        gvComplaints.Columns[9].Visible = true; // Show "Current Status" column
                    }
                    else
                    {
                        gvComplaints.Columns[9].Visible = false; // Hide "Current Status" column
                    }

                    string email = Session["Email"] as string;
                    lblWelcome.Text = $"Welcome, {email}!";
                    BindComplaints();
                    DisplaySuccessMessage();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
                
            }
        }

        private void DisplaySuccessMessage()
        {

                // Get the success message from the session
                string successMessage = Session["SuccessMessage"] as string;

                if (!string.IsNullOrEmpty(successMessage))
                {
                    lblSucessMessage.Text = successMessage;
                    lblSucessMessage.Visible = true;

                    // Clear the success message from the session
                    Session["SuccessMessage"] = null;
                }
            
        }

       


        private void BindComplaints()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

            List<string> roles = Session["Roles"] as List<string>;
            string email = Session["Email"] as string;

            //string query = "SELECT Id, FirstName + ' ' + LastName AS Name,EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' +StreetAddress1 + ' ' + City + ',' + ' ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload ,ComplaintId ,Status FROM Complaints ORDER BY Id DESC";            

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = roles.Contains("Admin") || roles.Contains("BothRoles")
                ? "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId,CurrentStatus ,Status FROM Complaints ORDER BY Id DESC"
                : "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId,CurrentStatus ,Status FROM Complaints WHERE Email = @Email ORDER BY Id DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    
                    if (roles.Contains("NormalUser"))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                    }
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ComplaintViewModel> complaints = new List<ComplaintViewModel>();

                    while (reader.Read())
                    {
                        ComplaintViewModel complaint = new ComplaintViewModel
                        {
                            Id = reader["Id"].ToString(),
                            ComplaintId = reader["ComplaintId"].ToString(),
                            Name = reader["Name"].ToString(),
                            EmpId = reader["EmpId"].ToString(),
                            Email = reader["Email"].ToString(),
                            ContactNumber = reader["ContactNumber"].ToString(),
                            DateTimeCapture = Convert.ToDateTime(reader["DateTimeCapture"]),
                            PictureCaptureLocation = reader["PictureCaptureLocation"].ToString(),
                            Comments = reader["Comments"].ToString(),
                            Status = reader["Status"].ToString(),
                            PictureUploads = Array.ConvertAll(
                            reader["PictureUpload"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                            path => Path.GetFileName(path)),
                            CurrentStatus = reader["CurrentStatus"].ToString(),
                        };

                        complaints.Add(complaint);
                    }

                    gvComplaints.DataSource = complaints;
                    gvComplaints.DataBind();
                }
            }
        }                          


        public class ComplaintViewModel
        {
            public string Id { get; set; }
            public string ComplaintId { get; set; }
            public string Name { get; set; }
            public string EmpId { get; set; }
            public string Email { get; set; }
            public string ContactNumber { get; set; }
            public DateTime DateTimeCapture { get; set; }
            public string PictureCaptureLocation { get; set; }
            public string Comments { get; set; }
            public string[] PictureUploads { get; set; }// Comma-separated image paths
            public string Status { get; set; }
            public string CurrentStatus { get; set; }

        }

        private string GetImageBasePath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ImageBasePath"];
        }

        protected void gvComplaints_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ComplaintViewModel complaint = (ComplaintViewModel)e.Row.DataItem;
                Literal litImages = (Literal)e.Row.FindControl("litImages");
                string email = Session["Email"] as string;

                if (litImages != null)
                {
                    litImages.Text = string.Empty;
                    StringBuilder imagesHtml = new StringBuilder();
                    //string basePath = ResolveUrl(GetImageBasePath()); // Get base path relative to the application root

                    if (complaint.PictureUploads != null)
                    {
                        foreach (string imageFileName in complaint.PictureUploads)
                        {
                            string imageUrl = imageFileName.Trim(); // Combine base path with filename                            
                            imagesHtml.AppendFormat("<img src='{0}' alt='Complaint Image' class='img-thumbnail' style='width: 250px; height: 200px;'/>", "imagehandler.ashx?imageName="+ imageFileName);
                        }
                    }

                    litImages.Text = imagesHtml.ToString();
                }

                List<string> roles = Session["Roles"] as List<string>;

                var txtStatus = (TextBox)e.Row.FindControl("txtStatus");
                var btnEdit = (Button)e.Row.FindControl("btnEdit");
                var ddlCurrentStatus = (DropDownList)e.Row.FindControl("ddlCurrentStatus");
                Button btnUpdateStatus = e.Row.FindControl("btnUpdateStatus") as Button;
                HiddenField hfComplaintId = e.Row.FindControl("hfComplaintId") as HiddenField;
                Label lblheader = e.Row.FindControl("lblheader") as Label;
                Label lblStatus =  e.Row.FindControl("lblStatus") as Label;

                if (lblheader != null)
                {
                    lblheader.Text = "Status :";
                }

                txtStatus.Text = complaint.Status;

                if (lblStatus != null)
                {
                    lblStatus.Visible = false;
                }


                if (roles.Contains("NormalUser") && complaint.Email == email)
                {
                    //lblheader.Visible = false;
                    //lblheader.Text = "";
                    if (complaint.CurrentStatus == "Resolved")
                    {
                        // Hide the Edit button if the complaint is resolved
                        if (btnEdit != null)
                        {                           
                            btnEdit.Visible = false;
                        }
                        // Show Current Status for resolved complaints only
                        if (lblStatus != null)
                        {
                            lblStatus.Text = complaint.CurrentStatus;
                            lblStatus.Visible = true;
                        }
                    }
                    else
                    {
                        // Enable Edit button if the complaint is not resolved
                        if (btnEdit != null)
                        {
                            btnEdit.Visible = true;
                        }
                        lblStatus.Visible = true;
                        lblStatus.Text = complaint.Status;
                    }
                    if (ddlCurrentStatus != null)
                    {
                        ddlCurrentStatus.Visible = false;
                    }
                    if (txtStatus != null)
                    {
                        txtStatus.Visible = false;
                    }
                    if (btnUpdateStatus != null)
                    {
                        btnUpdateStatus.Visible = false;
                    }
                    //if (lblheader != null)
                    //{
                    //    lblheader.Visible = false;
                    //}
                    
                }
                else if (roles.Contains("Admin") || roles.Contains("BothRoles"))
                {
                    if (btnEdit != null)
                    {
                        btnEdit.Visible = complaint.Email == email;
                    }
                    if (ddlCurrentStatus != null)
                    {
                        ddlCurrentStatus.Enabled = true;
                        ddlCurrentStatus.SelectedValue = complaint.CurrentStatus;
                    }
                    if (txtStatus != null)
                    {
                        txtStatus.Enabled = true;
                    }
                    if (btnUpdateStatus != null)
                    {
                        btnUpdateStatus.Enabled = true;
                    }
                    if (lblheader != null)
                    {
                        lblheader.Enabled = true;
                    }
                }

                if (complaint.CurrentStatus == "Resolved")
                {
                    lblheader.Visible = false;
                    if (txtStatus != null)
                    {
                        txtStatus.Visible = false;
                    }

                    if (btnUpdateStatus != null)
                    {
                        btnUpdateStatus.Visible = false;
                    }

                    if (btnEdit != null)
                    {
                        btnEdit.Visible = false;
                    }
                    e.Row.CssClass = "not-editable";
                    if (lblStatus != null)
                    {
                        lblStatus.Text = complaint.CurrentStatus;
                        lblStatus.Visible = true;
                    }
                }
                else if (complaint.CurrentStatus == "Re-opened")
                {
                    lblheader.Visible = true;
                    if (txtStatus != null)
                    {
                        txtStatus.Enabled = true;
                    }

                    if (btnUpdateStatus != null)
                    {
                        btnUpdateStatus.Enabled = true;
                    }

                    if (btnEdit != null)
                    {
                        btnEdit.Visible = complaint.Email == email;


                    }
                    e.Row.CssClass = "";
                }
            }
        }



        protected void gvComplaints_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                // Find the row that triggered the command
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                // Get the ComplaintId from the HiddenField
                HiddenField hfComplaintId = row.FindControl("hfComplaintId") as HiddenField;

                if (hfComplaintId != null)
                {
                    string complaintId = hfComplaintId.Value;

                    if (string.IsNullOrEmpty(complaintId))
                    {
                        Console.WriteLine("Error: ComplaintId is null or empty.");
                        return;
                    }

                    var txtStatus = (TextBox)row.FindControl("txtStatus");
                    DropDownList ddlCurrentStatus = row.FindControl("ddlCurrentStatus") as DropDownList;

                    // Handle the command based on CommandName
                    if (e.CommandName == "UpdateStatus")
                    {
                        // Update complaint status only
                        string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            string query = "UPDATE Complaints SET [Status] = @Status WHERE [ComplaintId] = @ComplaintId";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Status", txtStatus.Text.Trim());
                                cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }

                        // Make the row uneditable if the status is "Resolved"
                        if (ddlCurrentStatus.SelectedValue == "Not Started")
                        {
                            UpdateComplaintCurrentStatus(complaintId, "In Progress");
                        }

                        BindComplaints(); // Refresh the GridView
                    }
                    else
                    {
                        Console.WriteLine("Error: DropDownList or TextBox for status not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: HiddenField for ComplaintId not found.");
                }
            }
        }

        protected void ddlCurrentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {     

            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;

            string complaintId = ((HiddenField)row.FindControl("hfComplaintId")).Value;
            TextBox txtStatus = row.FindControl("txtStatus") as TextBox;
            Button btnUpdateStatus = row.FindControl("btnUpdateStatus") as Button;
            Button btnEdit = row.FindControl("btnEdit") as Button;
            Label lblheader = row.FindControl("lblheader") as Label;

            var lblCurrentStatus = (Label)row.FindControl("lblCurrentStatus");

            string status = ddl.SelectedValue;

            string email = Session["Email"] as string;
            string UIemail = string.Empty;
            if (row.RowType == DataControlRowType.DataRow)
            {
                UIemail = row.Cells[3].Text; // Adjust index based on the column order
            }

            UpdateComplaintCurrentStatus(complaintId, status);

            if (status == "Resolved")
            {
                lblheader.Visible = false;
                //lblCurrentStatus.Text = ddl.SelectedValue;
                if (txtStatus != null)
                {
                    txtStatus.Visible = false;
                }
                
                if (btnUpdateStatus != null)
                {
                    btnUpdateStatus.Visible = false;
                }
                
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                }
                row.CssClass = "not-editable";
            }
            else if (status == "Re-opened")
            {
                lblheader.Visible = true;
                if (txtStatus != null)
                {
                    txtStatus.Enabled = true;
                }

                if (btnUpdateStatus != null)
                {
                    btnUpdateStatus.Enabled = true;
                }

                if (btnEdit != null)
                {
                    btnEdit.Visible = UIemail == email; 


                }
                row.CssClass = "";
            }
            BindComplaints();
        }

        private void UpdateComplaintCurrentStatus(string complaintId, string status)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            string query = "UPDATE Complaints SET CurrentStatus = @CurrentStatus WHERE ComplaintId = @ComplaintId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CurrentStatus", status);
                    cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateComplaintStatus(string complaintId, string status)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            string query = "UPDATE Complaints SET Status = @Status WHERE ComplaintId = @ComplaintId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        protected void btnRegisterComplaint_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegisterComplaint.aspx");
        }

        protected void btnEditComplaint_Click(object sender, EventArgs e)
        {

            // Get the row that contains the button clicked
            GridViewRow row = (GridViewRow)((Control)sender).NamingContainer;

            // Find the HiddenField control in the row and get the ComplaintId
            HiddenField hfComplaintId = (HiddenField)row.FindControl("hfComplaintId");
            if (hfComplaintId != null)
            {
                string complaintId = hfComplaintId.Value;

                // Store the ComplaintId in session
                Session["ComplaintId"] = complaintId;
                string email = Session["Email"] as string;
                Session["UpdateEmail"] = email;

                // Redirect to the edit complaint page
                Response.Redirect("UpdateRegisterComplaint.aspx");
            }
            else
            {
                // Handle the error if HiddenField is not found
                Console.WriteLine("Error: HiddenField for ComplaintId not found.");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {

            // End the current session and sign out
            Session.Abandon();
            FormsAuthentication.SignOut();

            // Redirect to the login page or home page
            Response.Redirect("Login.aspx"); // Change to your login page

        }

        protected string GetUserRoleClass()
        {
            // Logic to check if the user is an Admin or has BothRoles
            if (User.IsInRole("Admin") || User.IsInRole("BothRoles"))
            {
                return "admin";
            }
            return "";
            
        }

    }
}