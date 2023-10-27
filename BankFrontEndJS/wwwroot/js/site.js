// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function loadView(status) {
    var apiUrl = '/login/defaultview';
    if (status === "authview")
        apiUrl = '/login/authview';
    if (status === "error")
        apiUrl = '/login/error';
    if (status === "about")
        apiUrl = '/about/view';
    if (status === "logout")
        apiUrl = '/logout';
    if (status === "account")
        apiUrl = '/account';
    if (status === "transactions")
        apiUrl = '/transactions';
    if (status === "transfer")
        apiUrl = '/transfer';
    if (status === "profile")
        apiUrl = '/profile';

    console.log("Hello " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            document.getElementById('main').innerHTML = data;
            if (status === "logout") {
                document.getElementById('LogoutButton').style.display = "none";
                document.getElementById('ProfileButton').style.display = "none";
                document.getElementById('AccountButton').style.display = "none";
                document.getElementById('TransactionsButton').style.display = "none";
                document.getElementById('TransferButton').style.display = "none";
            } else if (status == "profile") {

            }
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}


function performAuth() {

    var name = document.getElementById('SName').value;
    var password = document.getElementById('SPass').value;
    var data = {
        UserName: name,
        PassWord: password
    };
    console.error(data);
    const apiUrl = '/login/auth';

    const headers = {
        'Content-Type': 'application/json', // Specify the content type as JSON if you're sending JSON data
        // Add any other headers you need here
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data) // Convert the data object to a JSON string
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Handle the data from the API
            const jsonObject = data;
            if (jsonObject.login) {
                loadView("authview");
                document.getElementById('LogoutButton').style.display = "block";
                document.getElementById('ProfileButton').style.display = "block";
                document.getElementById('AccountButton').style.display = "block";
                document.getElementById('TransactionsButton').style.display = "block";
                document.getElementById('TransferButton').style.display = "block";

            }
            else {
                loadView("error");
            }

        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}


document.addEventListener("DOMContentLoaded", loadView);
/*
const loginButton = document.getElementById('LoginButton');
loginButton.addEventListener('click', loadView);

const aboutButton = document.getElementById('AboutButton');
aboutButton.addEventListener('click', loadView("about"));

const logoutButton = document.getElementById('LogoutButton');
logoutButton.addEventListener('click', loadView("logout"));*/




