export { base_api_url, DisplayError };

const base_api_url = 'http://localhost:5280/api';

function DisplayError(message) {
    document.getElementById('errorMessage').textContent = message;
}