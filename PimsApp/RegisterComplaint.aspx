<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterComplaint.aspx.cs" Inherits="PimsApp.RegisterComplaint" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Complaint Registration</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    
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
        // Function to get the current location
        function getCurrentLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            } else {
                alert("Geolocation is not supported by this browser.");
            }
        }

        // Function to display the position (latitude and longitude)
        function showPosition(position) {
            document.getElementById("txtLocation").value = position.coords.latitude + ", " + position.coords.longitude;
        }

        // Function to handle errors in retrieving the location
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

        // Function to handle file upload and preview
        function handleFileUpload() {
            var files = document.getElementById('fileUpload').files;
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
                    img.style.width = '100%'; // Ensure the image fits within the container
                    container.appendChild(img);
                };
                reader.readAsDataURL(file);
            });

            // Restore any previously displayed images
            existingImagesArray.forEach(src => {
                var img = document.createElement('img');
                img.src = src;
                img.classList.add('img-thumbnail', 'mb-2');
                img.style.width = '100%'; // Ensure the image fits within the container
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

            var dateTimeInput = document.getElementById('txtDateTimeCapture');
            dateTimeInput.value = formattedDate;           
        }

        document.addEventListener('DOMContentLoaded', function () {

            $('#txtDateTimeCapture').datetimepicker({
                format: 'YYYY-MM-DD HH:mm',
                autoclose: true,
                todayHighlight: true,
                todayBtn: 'linked',
                minuteStep: 1,
                showMeridian: false,
                maxDate:new Date()
            });

        });

        function addCurrentDateTime() {
            var currentDateTime = new Date();
            var formattedDateTime = currentDateTime.getFullYear() + "-" +
                ("0" + (currentDateTime.getMonth() + 1)).slice(-2) + "-" +
                ("0" + currentDateTime.getDate()).slice(-2) + " " +
                ("0" + currentDateTime.getHours()).slice(-2) + ":" +
                ("0" + currentDateTime.getMinutes()).slice(-2);

            // Get the TextBox control by its ID
            var textBox = document.getElementById('<%= txtDateTimeCapture.ClientID %>');
            textBox.value = formattedDateTime;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            
            
            <div class="collapse navbar-collapse">
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item">
                        <asp:Label ID="lblWelcome" runat="server" Text="Welcome!" Font-Bold="true" CssClass="navbar-text" />
                    </li>
                </ul>
            </div>
        </nav>
        <div class="banner-container">
        <div class="banner">
            <h3 id="pageTitle">EcoSight: Ecological Incident Reporting & Monitoring</h3>
        </div>
            <h5 class="complaint-heading">Complaint Registration</h5>
    </div>
        <div class="container form-container border p-4">
            <!-- Message Display -->
            <div class="form-group">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"/>
            </div>
            <div class="row">
                <!-- Form Section (70%) -->
                <div class="col-md-9">
                    <asp:Panel ID="pnlComplaint" runat="server">
                        <div class="row">
                            <!-- First Name and Last Name -->
                            <div class="col-md-6">
                                <label for="txtFirstName">First Name:</label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="col-md-6">
                                <label for="txtLastName">Last Name:</label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <!-- Emp Id and Email -->
                            <div class="col-md-6">
                                <label for="txtEmpId">Emp Id:</label>
                                <asp:TextBox ID="txtEmpId" runat="server" CssClass="form-control" ReadOnly="true"/>
                            </div>
                            <div class="col-md-6">
                                <label for="txtEmail">Email:</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"/>
                            </div>

                            <!-- Contact Number -->
                            <div class="col-md-6">
                                <label for="txtContactNumber">Contact Number:</label>
                                <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" ReadOnly="true"/>
                            </div>

                            <!-- Date Time -->
                            <div class="col-md-12">
                                <label for="txtDateTimeCapture">Date and Time of Capture:
                                    <asp:RequiredFieldValidator ID="ref_date" runat="server" ForeColor="Red" ControlToValidate="txtDateTimeCapture" ErrorMessage="Enter Date and Time of Capture" SetFocusOnError="true" ValidationGroup="reg_valid"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group date" id="datepicker">
                                    <asp:TextBox ID="txtDateTimeCapture" runat="server" CssClass="form-control" />
                                    
                                </div>
                                <span id="addDateTimeSpan" class="btn btn-info mt-2" style="cursor:pointer;" onclick="addCurrentDateTime()">Add Date & Time</span>
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

                                <label for="txtZipcode">Zipcode:</label>

                                <asp:TextBox ID="txtZipcode" runat="server" CssClass="form-control" />

                            </div>

                            <div class="col-md-4">

                                <label for="txtState">State:</label>

                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" />

                            </div>

                            <!-- Image Upload -->
                            <div class="col-md-12">
                                <label for="fileUpload">Image Upload:</label>
                                <asp:RequiredFieldValidator ID="req_image" runat="server" ForeColor="Red" ControlToValidate="fileUpload" ErrorMessage="Please upload Image" SetFocusOnError="true" ValidationGroup="reg_valid"></asp:RequiredFieldValidator>
                                <div class="input-group">
                                    <asp:FileUpload ID="fileUpload" runat="server" AllowMultiple="true" CssClass="form-control" OnChange="handleFileUpload();" />
                                </div>
                            </div>

                            <!-- Comments -->
                            <div class="col-md-12">
                                <label for="txtComments">Comments:</label>
                                <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                            </div>

                            <!-- Submit and Cancel Buttons -->
                            <div class="col-md-12">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="reg_valid" CssClass="btn btn-primary mr-2" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <!-- Image Preview Section (30%) -->
                <div class="col-md-3">
                    <div id="imagePreviewContainer" class="border p-2 bg-light rounded">
                        <!-- Image previews will be inserted here dynamically -->
                    </div>
                </div>
            </div>
             
            
        </div>
    </form>

</body>
    <script type="text/javascript">
        //document.getElementById('txtLocation').addEventListener('input', validateForm);
        document.getElementById('txtStreetAddress1').addEventListener('input', validateForm);
        document.getElementById('txtCity').addEventListener('input', validateForm);
        document.getElementById('txtZipcode').addEventListener('input', validateForm);
        document.getElementById('txtState').addEventListener('input', validateForm);

        document.getElementById('btnSubmit').addEventListener('click', function (event) {
            //var location = document.getElementById('txtLocation').value.trim();
            var streetAddress1 = document.getElementById('txtStreetAddress1').value.trim();
            var city = document.getElementById('txtCity').value.trim();
            var zipcode = document.getElementById('txtZipcode').value.trim();
            var state = document.getElementById('txtState').value.trim();

            //if (location === "" && (streetAddress1 === "" || city === "" || zipcode === "" || state === "")) {
            //    alert("Please fill out either the Picture Capture Location or the Address fields.");
            //    event.preventDefault(); // Prevent form submission
            //}
            if (streetAddress1 === "" || city === "" || zipcode === "" || state === "") {
                alert("Please fill out Address fields.");
                event.preventDefault(); 
            }
        });

        function validateForm() {
            var location = document.getElementById('txtLocation').value.trim();
            var streetAddress1 = document.getElementById('txtStreetAddress1').value.trim();
            var city = document.getElementById('txtCity').value.trim();
            var zipcode = document.getElementById('txtZipcode').value.trim();
            var state = document.getElementById('txtState').value.trim();

            // Implement logic to dynamically enable/disable validation or change appearance based on input
        }
    </script>
</html>




