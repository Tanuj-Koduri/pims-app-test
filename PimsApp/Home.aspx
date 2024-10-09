<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PimsApp.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Page - Dashboard</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous" />
    <style>
        .image-container img {
            width: 250px; /* Increased width for better visibility */
            height: auto;
            margin-right: 10px;
            margin-bottom: 10px;
            border: 1px solid #ccc;
            padding: 5px;
            border-radius: 5px;
        }

        .navbar-nav {
            flex-direction: row;
        }

        .nav-item {
            margin-left: 15px;
        }

        .navbar-brand {
            flex: 1;
            text-align: center;
        }

        .table th, .table td {
            text-align: center;
            vertical-align: middle;
        }

        .container {
            max-width: 1400px; /* Increase the max-width for a larger form */
        }

        .btn-primary {
            background-color: #007bff;
            border: none;
        }

            .btn-primary:hover {
                background-color: #0056b3;
            }

        .btn-danger {
            background-color: #dc3545;
            border: none;
        }

            .btn-danger:hover {
                background-color: #c82333;
            }

        .messagesucess {
            font-size: 18px;
            color: forestgreen;
            margin-top: 20px;
            display: block;
        }        

        .nowrap-header, .nowrap-item {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
         .no-wrap-text {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        display: inline-block;
        max-width: 100%;
    }
          .banner {
            background-color: #E0FFFF; /* Light background color */
            padding: 20px; /* Padding for the banner */
            border-radius: 5px; /* Rounded corners */
            box-shadow: 0 4px 8px rgba(0,0,0,0.2); /* Subtle shadow */
            margin-bottom: 20px; /* Space below the banner */
            text-align: center; /* Center the text */
            font-size: 26px; /* Font size for the banner text */
            color: #333; /* Dark text color */
            font-weight: bold;
        }

    /* Style for h3 element under the banner */
    h5.mb-0 {
        text-align: center; /* Center the text */
        margin-top: 10px; /* Add some space above the h3 */
        font-size: 25px; /* Adjust font size if necessary */
        color: #333; /* Text color for h3 */
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <div class="collapse navbar-collapse">
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item">
                        <asp:Label ID="lblWelcome" runat="server" Text="Welcome!" Font-Bold="true" CssClass="navbar-text" />
                    </li>
                    <li class="nav-item">
                        <asp:Button ID="btnLogout" runat="server" CssClass="btn btn-danger" Text="Logout" OnClick="btnLogout_Click" />
                    </li>
                </ul>
            </div>
        </nav>

        <div class="container mt-5">
            <div class="banner">
                EcoSight: Ecological Incident Reporting & Monitoring
            </div>
            <h5 id="pageTitle" runat="server" class="mb-0"></h5>
            <asp:Label ID="lblSucessMessage" runat="server" CssClass="messagesucess" Visible="false"></asp:Label>
            <div class="row mb-4 align-items-center">
                <div class="text-right mb-4">
                    <asp:Button ID="btnRegisterComplaint" runat="server" CssClass="btn btn-primary" Text="Register Complaint" OnClick="btnRegisterComplaint_Click" />
                </div>
                <hr />
                <asp:GridView ID="gvComplaints" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvComplaints_RowDataBound" OnRowCommand="gvComplaints_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ComplaintId" HeaderText="Complaint Id" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="EmpId" HeaderText="Emp Id">
    <HeaderStyle CssClass="nowrap-header" />
    <ItemStyle CssClass="nowrap-item" />
</asp:BoundField>
                        <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-CssClass="email-column" />
                        <asp:BoundField DataField="ContactNumber" HeaderText="Number" />
                        <asp:BoundField DataField="DateTimeCapture" HeaderText="Date/Time of Capture" DataFormatString="{0:dd-MM-yyyy HH:mm}" />
                        <asp:BoundField DataField="PictureCaptureLocation" HeaderText="Location" />
                        <asp:BoundField DataField="Comments" HeaderText="Description" />                     

                        <asp:TemplateField HeaderText="Images/Pictures">
                            <HeaderStyle Width="400px" />
                            <ItemStyle Width="400px" />
                            <ItemTemplate>
                                <asp:Literal ID="litImages" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Current Status" Visible="True">
                            <ItemTemplate>
                                <!-- Container div with increased width -->
                                <div class="form-group" style="width: 150px;">
                                    <!-- Adjust width as needed -->
                                    <asp:DropDownList ID="ddlCurrentStatus" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrentStatus_SelectedIndexChanged">
                                        <asp:ListItem Text="Not Started" Value="Not Started" />
                                        <asp:ListItem Text="In Progress" Value="In Progress" />
                                        <asp:ListItem Text="Resolved" Value="Resolved" />
                                        <asp:ListItem Text="Re-opened" Value="Re-opened" />
                                    </asp:DropDownList>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action Taken">
                            <HeaderStyle Width="400px" />
                            <ItemStyle Width="400px" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hfComplaintId" runat="server" Value='<%# Eval("ComplaintId") %>' />

                                 
                                <asp:Label ID="lblheader" runat="server" Text='<%# Eval("Status") %>' CssClass="d-block mb-2" />
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' CssClass="d-block mb-2" />

                                <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control mb-2" TextMode="MultiLine" Rows="2" Style="width: 100%;"></asp:TextBox>

                                <div class="button-group">
                                    <asp:Button ID="btnUpdateStatus" runat="server" Text="      Update     " CssClass="btn btn-primary mb-2" CommandName="UpdateStatus" CommandArgument="<%# Container.DataItemIndex %>" />

                                    <asp:Button ID="btnEdit" runat="server" Text="     Edit     " CssClass="btn btn-secondary mb-2" CommandName="Edit"
                                        OnClick="btnEditComplaint_Click" CommandArgument="<%# Container.DataItemIndex %>" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK3t38cC1W7H4F9j9xF2az10M3qG3ErJ3U3eZ2kBlOW/ZD" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UOa6z7m2A1e+F9FxVjW2Qib7k7AoIogL9tK1dH9F9yyiF5mKb3+gG9I5k5hE5QF0" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js" integrity="sha384-XnSDiBF7UBExI/WRZxQ6A6D8pmtBYZ8Tk+8B8bprI1D4MDPvN2/GFhOxHDFj0H9P" crossorigin="anonymous"></script>
</body>
</html>
