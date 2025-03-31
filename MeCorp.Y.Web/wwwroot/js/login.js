import { base_api_url, DisplayError} from './common.js';

document.getElementById('loginButton').addEventListener('click', async (event) => {
    event.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {

        const response = await fetch(`${base_api_url}/auth/login`, {
            method: 'post',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        const parsedResponse = await response.json();

        if (!response.ok) {

            DisplayError(parsedResponse.message);
            return;
        }

        localStorage.setItem('token', parsedResponse.data.token);
        window.location.href = `dashboard.html?userId=${parsedResponse.data.id}`;

    } catch (e) {
        DisplayError('An error occured. Please try again later...');
    }
});
