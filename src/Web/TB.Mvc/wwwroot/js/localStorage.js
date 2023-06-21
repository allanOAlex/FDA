// Save data to local storage
function saveToLocalStorage(key, value) {
    localStorage.setItem(key, value);
}

function saveTokenToLocalStorage(token) {
    localStorage.setItem('authToken', token);
}

// Retrieve data from local storage
function getFromLocalStorage(key) {
    return localStorage.getItem(key);
}

// Remove data from local storage
function removeFromLocalStorage(key) {
    localStorage.removeItem(key);
}