<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateRegisterComplaint.aspx.cs" Inherits="PimsApp.UpdateRegisterComplaint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update your complaint/Know Resolution Status</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
    <!-- jQuery and Bootstrap 5 JS -->
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.11.6/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <style>
        .image-preview-container {
            position: sticky;
            top: 0;
            max-width: 30%;
            padding: 15px;
            background-color: #f8f9fa;
            border-left: 1px solid #dee2e6;
            height: calc(100vh - 30px); /* Adjust to fit the remaining height */
            overflow-y: auto;
        }

        .image-preview {
            width: 100%;
            height: auto;
            margin-bottom: 15px;
        }

        .form-group {
            position: relative;
        }
        .navbar-nav {

            margin-left: auto;

        }
                .banner-container {
    max-width: 1400px; /* Adjust max-width as needed */
    margin: 20px auto 0 auto; /* Add margin-top to create space above the banner */
    text-align: center; /* Center the text inside the banner container */
}

.banner {
    background-color: #E0FFFF; /* Light Cyan color */
    padding: 20px; /* Padding for the banner */
    border-radius: 5px; /* Rounded corners */
    box-shadow: 0 2px 4px rgba(0,0,0,0.1); /* Subtle shadow */
    text-align: center;
}

#pageTitle {
    font-size: 25px; /* Adjust font size as needed */
    margin-bottom: 0;
}

.complaint-heading {
    margin-top: 20px; /* Space between banner and heading */
    font-size: 21px; /* Adjust font size as needed */
    margin-bottom: 0;
}
.container.form-container {
    margin-top: 30px; /* Adjust this value to increase or decrease the gap */
}
    </style>
    <script>
        function getCurrentLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            } else {
                alert("Geolocation is not supported by this browser.");
            }
        }

        <%--function showPosition(position) {
            document.getElementById("<%= txtLocation.ClientID %>").value = position.coords.latitude + ", " + position.coords.longitude;
        }--%>

        function showError(error) {
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    alert("User denied the request for Geolocation.");
                    break;
                case error.POSITION_UNAVAILABLE:
                    alert("Location information is unavailable.");
                    break;
                case error.TIMEOUT:
                    alert("The request to get user location timed out.");
                    break;
                case error.UNKNOWN_ERROR:
                    alert("An unknown error occurred.");
                    break;
            }
        }

        function handleFileUpload() {
            var files = document.getElementById('<%= fileUpload.ClientID %>').files;
            var container = document.getElementById('imagePreviewContainer');
            var existingImages = container.querySelectorAll('img');

            // Preserve existing previews
            var existingImagesArray = Array.from(existingImages).map(img => img.src);

            // Clear existing previews
            container.innerHTML = '';

            Array.from(files).forEach(file => {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = document.createElement('img');
                    img.src = e.target.result;
                    img.classList.add('img-thumbnail', 'mb-2');
                    img.style.width = '100%';
                    container.appendChild(img);
                };
                reader.readAsDataURL(file);
            });

            existingImagesArray.forEach(src => {
                var img = document.createElement('img');
                img.src = src;
                img.classList.add('img-thumbnail', 'mb-2');
                img.style.width = '100%';
                container.appendChild(img);
            });
        }

        function addCurrentDateTime() {
            var now = new Date();
            var formattedDate = now.getFullYear() + '-' +
                ('0' + (now.getMonth() + 1)).slice(-2) + '-' +
                ('0' + now.getDate()).slice(-2) + ' ' +
                ('0' + now.getHours()).slice(-2) + ':' +
                ('0' + now.getMinutes()).slice(-2);

            document.getElementById('<%= txtDateTimeCapture.ClientID %>').value = formattedDate;
        }

        $(document).ready(function () {
            $('#txtDateTimeCapture').datetimepicker({
                format: 'YYYY-MM-DD HH:mm',
                autoclose: true,
                todayHighlight: true,
                todayBtn: 'linked',
                minuteStep: 1,
                showMeridian: false,
                maxDate: new Date()
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="container-fluid">
        <div class="d-flex justify-content-between w-100">
            <div></div> <!-- Empty div to push content to the center -->
            <%--<h5 class="navbar-brand text-center flex-fill">Update Complaint - <asp:Label ID="lblComplaintId" runat="server" Text="" CssClass="ml-2" /></h5>--%>
            <div class="navbar-nav">
                <asp:Label ID="lblWelcome" runat="server" Text="Welcome!" Font-Bold="true" CssClass="navbar-text" />
            </div>
        </div>
    </div>
</nav>
            <div class="banner-container">
    <div class="banner">
        <h3 id="pageTitle">EcoSight: Ecological Incident Reporting & Monitoring</h3>
    </div>
        <h5 class="complaint-heading">Update Complaint  - <asp:Label ID="lblComplaintId" runat="server" Text=""/></h5>
</div>

        <div class="container form-container border p-4">
            <div class="form-group">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false" />
            </div>
            <div class="row">
                <!-- Form Section (70%) -->
                <div class="col-md-9">
                    <asp:Panel ID="pnlComplaint" runat="server">
                        <div class="row">
                            <!-- First Name and Last Name -->
                            <div class="col-md-6">
                                <label for="txtFirstName">First Name:</label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control text-dark" ReadOnly="true" />
                            </div>
                            <div class="col-md-6">
                                <label for="txtLastName">Last Name:</label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <!-- Emp Id and Email -->
                            <div class="col-md-6">
                                <label for="txtEmpId">Emp Id:</label>
                                <asp:TextBox ID="txtEmpId" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="col-md-6">
                                <label for="txtEmail">Email:</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="col-md-6">
                                <label for="txtContactNumber">Contact Number:</label>
                                <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="col-md-12">
                                <label for="txtDateTimeCapture">
                                    Date and Time of Capture:
                                   
                                    <asp:RequiredFieldValidator ID="ref_date" runat="server" ForeColor="Red" ControlToValidate="txtDateTimeCapture" ErrorMessage="Enter Date and Time of Capture" SetFocusOnError="true" ValidationGroup="reg_valid"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group date" id="datepicker">
                                    <asp:TextBox ID="txtDateTimeCapture" runat="server" CssClass="form-control" />
                                </div>
                                <span id="addDateTimeSpan" class="btn btn-info mt-2" style="cursor: pointer;" onclick="addCurrentDateTime()">Add Date & Time</span>
                            </div>

                            <div class="col-md-6">
                                <label for="txtStreetAddress1">Street Address 1:</label>
                                <asp:TextBox ID="txtStreetAddress1" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <label for="txtStreetAddress2">Street Address 2 (optional):</label>
                                <asp:TextBox ID="txtStreetAddress2" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-md-4">
                                <label for="txtCity">City:</label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-4">
                                <label for="txtState">State:</label>
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-4">
                                <label for="txtZipCode">Zip Code:</label>
                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" />
                            </div>

                            <!-- Add New Images -->
                            <div class="col-md-12">
                                <label for="fileUpload">Add New Images:</label>
                                <asp:FileUpload ID="fileUpload" runat="server" AllowMultiple="true" OnChange="handleFileUpload();" />
                            </div>
                            <asp:HiddenField ID="hiddenImagePaths" runat="server" />
                            <asp:Literal ID="litImages" runat="server" />

                            <div class="col-md-12">
                                <label for="txtComments">Comments:</label>
                                <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblStatus" runat="server" Text="Status:" CssClass="control-label"></asp:Label>
                                <asp:TextBox ID="txtStatus" runat="server" Rows="4" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </div>

                            <!-- Submit -->
                            <div class="col-md-12 mt-3">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="reg_valid" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary" Text="Cancel" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <!-- Preview Section (30%) -->
                <div class="col-md-3 image-preview-container" id="imagePreviewContainer">
                    <!-- This section will dynamically populate image previews -->
                </div>
            </div>
        </div>
    </form>
</body>
</html>
