using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PimsApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
       


        private List<string> GetUserRoles(string email)
        {
            List<string> roles = new List<string>();
            string connString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Role FROM EmpDetails WHERE Email = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", email);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Split roles in case they are stored as comma-separated
                        roles.AddRange(reader["Role"].ToString().Split(','));
                    }
                }
            }
            return roles;
        }

        // Redirect to the Register Complaint page
        protected void RegisterComplaint_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegisterComplaint.aspx");
        }

        protected void btnLoginUser_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            List<string> roles = GetUserRoles(email, password);           

            if (roles.Contains("Admin") || roles.Contains("NormalUser") || roles.Contains("BothRoles"))
            {
                if (AuthenticateUser(email, password, roles))
                {
                    Session["Email"] = email;
                    Session["Roles"] = roles;
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Invalid username or password.";
                }
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Invalid username or password.";
            }

        }

        

        private bool AuthenticateUser(string username, string password, List<string> roles)
        {
            string connString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Building the WHERE clause to check for any matching roles
                string roleConditions = string.Join(" OR ", roles.Select(role => $"Role LIKE '%' + @role{role} + '%'"));

                string query = $@"
            SELECT COUNT(1)
            FROM EmpDetails
            WHERE Email = @username
              AND Password = @password
              AND ({roleConditions})"; // Checking if any role matches

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password); // Ensure this is hashed in production

                // Add parameters for each role
                for (int i = 0; i < roles.Count; i++)
                {
                    cmd.Parameters.AddWithValue($"@role{roles[i]}", roles[i]);
                }

                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    // Log or display the exception message for debugging
                    lblMessage.Text = "An error occurred: " + ex.Message;
                    return false;
                }
            }
        }


        private List<string> GetUserRoles(string username, string password)
        {
            List<string> roles = new List<string>();
            string connString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Role FROM EmpDetails WHERE Email = @username AND Password = @password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Split the roles string into individual roles
                        string[] roleArray = reader["Role"].ToString().Split(',');
                        roles.AddRange(roleArray.Select(r => r.Trim()));
                    }
                }
            }
            return roles;
        }

        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}