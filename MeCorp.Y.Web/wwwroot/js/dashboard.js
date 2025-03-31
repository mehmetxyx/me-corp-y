import { base_api_url, DisplayError } from './common.js';

document.getElementById('logout').addEventListener('click', event => {
    event.preventDefault();
    localStorage.setItem('token', '');
    window.location.href = 'index.html';
});

document.addEventListener('DOMContentLoaded', async (event) => {
    event.preventDefault();

    const token = localStorage.getItem('token');

    if (!token) {
        alert('You are not logged in!');
        window.location.href = 'index.html';
    }

    try {

        const urlParams = new URLSearchParams(window.location.search);
        const userId = urlParams.get('userId'); 

        const response = await fetch(`${base_api_url}/users/${userId}`, {
            method: 'get',
            headers: {
                'content-type': 'application/json',
                'authorization': `Bearer ${token}`
            }
        });

        const parsedResponse = await response.json();

        if (!response.ok) {

            DisplayError(parsedResponse.message);
            return;
        }

        document.getElementById('userInfo').innerHTML = `
            <p>Welcome, ${parsedResponse.data.username}</p>
            <p>Role: ${parsedResponse.data.role}</p>
        `;

        if (parsedResponse.data.role != 'Admin')
            return;

        const adminSummaryResponse = await fetch(`${base_api_url}/users/admin-summary`, {
            method: 'get',
            headers: {
                'content-type': 'application/json',
                'authorization': `Bearer ${token}`
            }
        });

        const parsedAdminSummaryResponse = await adminSummaryResponse.json();

        if (!response.ok) {
            DisplayError(parsedAdminSummaryResponse.message);
            return;
        }

        document.getElementById('adminSummary').innerHTML = `
            <h2>Admin Summary</h2>
            <p>Customers: ${parsedAdminSummaryResponse.data.registeredCustomerCount}</p>
            <p>Managers: ${parsedAdminSummaryResponse.data.registeredManagerCount}</p>
        `;

    } catch (e) {
        DisplayError('An error occured. Please try again later...');
    }

});
