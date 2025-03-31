import { base_api_url, DisplayError} from './common.js';

document.getElementById('loginButton').addEventListener('click', async (event) => {
    event.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const urlParams = new URLSearchParams(window.location.search);
    const referralCode = urlParams.get('referralCode'); 

    try {

        const response = await fetch(`${base_api_url}/auth/register`, {
            method: 'post',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify({ username, password, referralCode })
        });

        const parsedResponse = await response.json();

        if (!response.ok) {

            DisplayError(parsedResponse.message);
            return;
        }

        alert('User created successfully!');
        window.location.href = 'index.html';

    } catch (e) {
        DisplayError('An error occured. Please try again later...');
    }

});
