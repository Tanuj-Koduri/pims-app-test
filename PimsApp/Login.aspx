<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PimsApp.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page - PIMSapp</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />

    <style>
        body {
            margin: 0;
            padding: 0;
            height: 100%;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f5f5f5;
        }

        .container {
            max-width: 900px;
            margin: 60px auto;
            padding: 40px;
            background: #ffffff;
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
            border-radius: 15px;
            background-image: linear-gradient(to right, #e0f7fa, #80deea);
        }

        .menu-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 15px 20px;
            background-color: #ffffff;
            color: #333;
            border-radius: 8px;
            margin-bottom: 30px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            text-align: center;
        }

        .menu-bar label {
            font-size: 24px;
            font-weight: bold;
            color: #007bff;
            margin: auto;
        }

        .btn-admin {
            padding: 10px 20px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .btn-admin:hover {
            background-color: #0056b3;
        }

        .content {
            text-align: center;
        }

        .content h3 {
            font-size: 36px;
            color: #333;
            margin-bottom: 30px;
            font-weight: bold;
            text-shadow: 1px 1px 4px rgba(0, 0, 0, 0.2);
        }

        .card-container {
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            padding: 40px;
            max-width: 400px;
            margin: auto;
            text-align: center;
        }

        .form-icon {
            color: #007bff;
            font-size: 50px;
            margin-bottom: 20px;
        }

        .title {
            color: #333;
            font-size: 28px;
            font-weight: bold;
            margin-bottom: 25px;
        }

        .form-horizontal .form-group {
            margin-bottom: 20px;
            text-align: left;
        }

        .form-horizontal .form-control {
            color: #333;
            background: #f9f9f9;
            font-size: 15px;
            height: 45px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-shadow: inset 2px 2px 4px rgba(0, 0, 0, 0.1);
            transition: all 0.3s ease;
        }

        .form-horizontal .form-control:focus {
            outline: none;
            box-shadow: 0 0 8px rgba(0, 123, 255, 0.5);
        }

        .form-horizontal .btn {
            background-color: #007bff;
            color: #fff;
            font-size: 16px;
            font-weight: bold;
            padding: 12px 20px;
            border-radius: 25px;
            border: none;
            width: 100%;
            cursor: pointer;
            transition: background-color 0.3s;
            margin-top: 10px;
        }

        .form-horizontal .btn:hover {
            background-color: #0056b3;
        }

        .message {
            font-size: 18px;
            color: red;
            margin-top: 20px;
            display: block;
        }

        .messagesucess {
            font-size: 18px;
            color: forestgreen;
            margin-top: 20px;
            display: block;
        }
        .forgot-password {
            margin-top: 15px;
            font-size: 14px;
        }

        .forgot-password a {
            color: #007bff;
            text-decoration: none;
        }

        .forgot-password a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="menu-bar">
                <label>Welcome to EcoSight: Ecological Incident Reporting & Monitoring</label>
            </div>

            <div class="content">
                <h3 class="display-4">Citizen Repair: Report Public Issues Here</h3>
                <div class="card-container">
                    <div class="form-icon"><i class="fas fa-user"></i></div>
                    <h3 class="title">Login</h3>

                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="txtUsername">Username</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Username"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPassword">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Password"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnLoginUser" runat="server" CssClass="btn" Text="Login" OnClick="btnLoginUser_Click" />
                        <div class="forgot-password">
                        Forgot Password? <a href="Login.aspx">Click here</a>
                    </div>
                    </div>
                </div>
                <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>





