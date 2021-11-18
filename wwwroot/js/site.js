// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const apiActionURI = "/api/ApiActions/";

function addFriend(id) {
    console.log(window.location.origin + apiActionURI + "addFriend/" + id)
    let xhr = new XMLHttpRequest();
    xhr.open("POST", window.location.origin + apiActionURI + "addFriend/" + id);
    xhr.send(id);
    xhr.onload = () => {
        if (xhr.status === 200) {
            console.log(xhr.response);
            location.reload();
        } else {
            console.log(xhr.status, xhr.response)
        }
    };

    xhr.onerror = () => {
        console.log("ошибка запроса")
    }
}

function deleteFriend(id) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", window.location.origin + "/user/deleteFriend/" + id);
    xhr.send(id);
    xhr.onload = () => {
        if (xhr.status === 200) {
            console.log(xhr.response);
            location.reload();
        } else {
            console.log(xhr.status, xhr.response)
        }
    };

    xhr.onerror = () => {
        console.log("ошибка запроса")
    }
}

function cancelAddFriendRequest(id) {
    console.log(id);
    let xhr = new XMLHttpRequest();
    xhr.open("POST", window.location.origin + "/user/cancelAddFriendRequest/" + id);
    xhr.send(id);
    xhr.onload = () => {
        if (xhr.status === 200) {
            console.log(xhr.response);
            location.reload();
        } else {
            console.log(xhr.status, xhr.response)
        }
    };

    xhr.onerror = () => {
        console.log("ошибка запроса")
    }
}

function acceptAddFriendRequest(id) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", window.location.origin + "/user/acceptAddFriendRequest/" + id);
    xhr.send(id);
    xhr.onload = () => {
        if (xhr.status === 200) {
            console.log(xhr.response);
            location.reload();
        } else {
            console.log(xhr.status, xhr.response)
        }
    };

    xhr.onerror = () => {
        console.log("ошибка запроса")
    }
}

function declineAddFriendRequest(id) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", window.location.origin + "/user/declineAddFriendRequest/" + id);
    xhr.send(id);
    xhr.onload = () => {
        if (xhr.status === 200) {
            console.log(xhr.response);
            location.reload();
        } else {
            console.log(xhr.status, xhr.response)
        }
    };

    xhr.onerror = () => {
        console.log("ошибка запроса")
    }
}